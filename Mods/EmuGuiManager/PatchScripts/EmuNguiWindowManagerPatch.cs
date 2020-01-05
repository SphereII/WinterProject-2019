using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SDX.Compiler;
using SDX.Core;

/// <summary>
/// TormentedEmu 2018 tormentedemu@gmail.com
/// </summary>
public class EmuNguiWindowManagerPatch : IPatcherMod
{
  public bool Patch(ModuleDefinition module)
  {
    Logging.LogInfo("Start patch process..." + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType);
    ModifyAssembly(module);
    Logging.LogInfo("Patch mod complete.");
    return true;
  }

  private void ModifyAssembly(ModuleDefinition module)
  {
    if (!ModifyNguiWindowManager(module))
      throw new Exception("Failed to find and modify the required method!");
  }

  private bool ModifyNguiWindowManager(ModuleDefinition module)
  {
    var nguiWM = module.Types.FirstOrDefault(c => c.Name == "NGUIWindowManager");
    if (nguiWM == null)
    {
      Logging.LogError("Failed to find class NGUIWindowManager.");
      return false;
    }

    var activateLabel = nguiWM.Fields.First(f => f.Name == "activateLabel");
    activateLabel.IsPrivate = false;
    activateLabel.IsPublic = true;

    var bGlobalShowFlag = nguiWM.Fields.First(f => f.Name == "bGlobalShowFlag");
    bGlobalShowFlag.IsPrivate = false;
    bGlobalShowFlag.IsPublic = true;

    var playerUI = nguiWM.Fields.First(f => f.Name == "playerUI");
    playerUI.IsPrivate = false;
    playerUI.IsPublic = true;

    var kbdFont = nguiWM.Fields.First(f => f.Name == "kbdFont");
    kbdFont.IsPrivate = false;
    kbdFont.IsPublic = true;

    return true;
  }

  public bool Link(ModuleDefinition gameModule, ModuleDefinition modModule)
  {
    if (!PatchAssembly(gameModule, modModule))
      throw new Exception("Failed to patch the assembly!");

    return true;
  }

  private bool PatchAssembly(ModuleDefinition gameModule, ModuleDefinition modModule)
  {
    var nguiWM = gameModule.Types.FirstOrDefault(c => c.Name == "NGUIWindowManager");
    if (nguiWM == null)
    {
      Logging.LogError("Failed to find class NGUIWindowManager.");
      return false;
    }

    var slt = nguiWM.Methods.FirstOrDefault(m => m.Name == "SetLabelText");
    if (slt == null)
    {
      Logging.LogError("NGUIWindowManager::SetLabelText method not found.  Aborting patch...");
      return false;
    }

    MethodDefinition setLabelText = null;

    foreach (var inst in slt.Body.Instructions)
    {
      if (inst.Operand != null && inst.OpCode == OpCodes.Call && inst.Operand.ToString().Contains("SetLabelText"))
      {
        setLabelText = inst.Operand as MethodDefinition;
        break;
      }
    }

    if (setLabelText == null)
    {
      Logging.LogError("NGUIWindowManager::SetLabelText(EnumNGUIWindow,string,bool) method not found.  Aborting patch...");
      return false;
    }
    
    var emuSetLabelText = gameModule.Import(modModule.Types.First(c => c.Name == "EmuGuiManager").Resolve().Methods.First(m => m.Name == "SetLabelTextOverride"));

    setLabelText.Body.Instructions.Clear();

    var il = setLabelText.Body.GetILProcessor();

    il.Emit(OpCodes.Ldarg_0);
    il.Emit(OpCodes.Ldarg_1);
    il.Emit(OpCodes.Ldarg_2);
    il.Emit(OpCodes.Ldarg_3);
    il.Emit(OpCodes.Call, emuSetLabelText);
    il.Emit(OpCodes.Ret);
    
    return true;
  }
}
