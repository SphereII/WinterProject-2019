using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SDX.Compiler;
using SDX.Core;

/// <summary>
/// TormentedEmu 2018 tormentedemu@gmail.com
/// </summary>
public class EmuXuiPatch : IPatcherMod
{
  public bool Patch(ModuleDefinition module)
  {
    return true;
  }

  public bool Link(ModuleDefinition gameModule, ModuleDefinition modModule)
  {
    if (!PatchHook(gameModule, modModule))
      throw new Exception("Failed to patch the assembly!");

    return true;
  }

  private bool PatchHook(ModuleDefinition gameModule, ModuleDefinition modModule)
  {
    var xUi = gameModule.Types.FirstOrDefault(c => c.Name == "XUi");
    if (xUi == null)
    {
      Logging.LogError("Failed to find class XUi.");
      return false;
    }

    var load = xUi.Methods.FirstOrDefault(m => m.Name == "Load");
    if (load == null)
    {
      Logging.LogError("XUi::Load method not found.  Aborting patch...");
      return false;
    }
    
    var emuLoadHook = gameModule.Import(modModule.Types.First(c => c.Name == "EmuGuiManager").Resolve().Methods.First(m => m.Name == "XuiLoadHook"));

    var first = load.Body.Instructions.First();
    var il = load.Body.GetILProcessor();

    il.InsertBefore(first, il.Create(OpCodes.Ldarg_0));
    il.InsertBefore(first, il.Create(OpCodes.Call, emuLoadHook));
    
    return true;
  }
}
