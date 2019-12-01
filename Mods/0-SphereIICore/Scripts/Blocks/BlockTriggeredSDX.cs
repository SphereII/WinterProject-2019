using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/*
     <property name="Class" value="TriggeredSDX, Mods"/>
     <property name="AlwaysActive" value="true" />

      <!-- How far out the tile entity will re-scan to detect the player -->
      <property name="ActivationDistance" value="5" />

       <!-- this triggers a SetTrigger("On") when looked at -->
      <property name="ActivateOnLook" value="true" />
      
      <!-- Triggers the block if the buff buffCurseSamara is active on the player, or if the player has a cvar called "cvar" with a value of 4, or if myOtherCvar is available, regardless of value -->
       <property name="ActivationBuffs" value="buffCurseSamara,cvar(4),myOtherCvar" />

*/
class BlockTriggeredSDX : BlockLoot
{
    private static string AdvFeatureClass = "AdvancedTileEntities";

    float timeOut = 5f;
    float NextCheck = 0f;
    bool TriggerOnly = false;

    public override string GetActivationText(WorldBase _world, BlockValue _blockValue, int _clrIdx, Vector3i _blockPos, EntityAlive _entityFocusing)
    {
        if (_blockValue.Block.Properties.Values.ContainsKey("ActivateOnLook"))
        {
            bool ActivateOnLook = StringParsers.ParseBool(_blockValue.Block.Properties.Values["ActivateOnLook"], 0, -1, true);
            if (ActivateOnLook)
            {
                AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Activating Block on GetActivationText");

                TriggerOnly = true;
                ActivateBlock(_world, _clrIdx, _blockPos, _blockValue, true, true);
                TriggerOnly = false;
            }

        }
        return "";
    }
   
    public override bool ActivateBlock(WorldBase _world, int _cIdx, Vector3i _blockPos, BlockValue _blockValue, bool isOn, bool isPowered)
    {
        // If there's no transform, no sense on keeping going for this class.
        BlockEntityData _ebcd = _world.GetChunkFromWorldPos(_blockPos).GetBlockEntity(_blockPos);
        if (_ebcd == null || _ebcd.transform == null)
            return false;

        //if (NextCheck > Time.time)
        //    return true;

        //NextCheck = Time.time + this.timeOut;

        Animator[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>();
        if (componentsInChildren != null)
        {
            for (int i = componentsInChildren.Length - 1; i >= 0; i--)
            {
                Animator animator = componentsInChildren[i];

                AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Animator: " + animator.name + " : Active: " + isOn);
                if (isOn)
                {
                    AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Starting Animator: " + animator.name);
                    animator.enabled = true;
                    if (!TriggerOnly)
                    {
                        AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Setting Bool for On: True " + animator.name);
                        animator.SetBool("On", true);
                    }
                    AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Trigger for On: " + animator.name);
                    animator.SetTrigger("TriggerOn");
                }

                if (isOn == false)
                {
                    AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Setting Bool for On: false" + animator.name);

                    animator.SetBool("On", false);
                    AdvLogging.DisplayLog(AdvFeatureClass, _blockValue.Block.GetBlockName() + ": Turning Off Animator " + animator.name);
                    animator.enabled = false;
                }
            }
        }
        return true;
    }
}

