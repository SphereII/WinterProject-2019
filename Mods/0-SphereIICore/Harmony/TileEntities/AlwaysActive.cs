using Harmony;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
/*
 *    <!-- Allows the  Trigger to work -->
      <property name="AlwaysActive" value="true" />

      <!-- How far out the tile entity will re-scan to detect the player -->
      <property name="ActivationDistance" value="5" />

          <property name="ActivateOnLook" value="true" />

      <!-- Triggers the block if the buff buffCurseSamara is active on the player, or if the player has a cvar called "cvar" with a value of 4, or if myOtherCvar is available, regardless of value -->
      <property name="ActivationBuffs" value="buffCurseSamara,cvar(4),myOtherCvar" />
*/
public class SphereII_TileEntityAlwaysActive
{
    [HarmonyPatch(typeof(TileEntity))]
    [HarmonyPatch("IsActive")]
    public class SphereII_TileEntity_IsActive
    {
        public static bool Prefix(ref bool __result, TileEntity __instance, World world)
        {
            BlockValue block = GameManager.Instance.World.GetBlock(__instance.ToWorldPos());
            Block block2 = Block.list[block.type];
            bool isAlwaysActive = false;
            if (block2.Properties.Values.ContainsKey("AlwaysActive"))
            {
                isAlwaysActive = StringParsers.ParseBool(block2.Properties.Values["AlwaysActive"], 0, -1, true);
                if (isAlwaysActive)
                {
                    bool blCanTrigger = false;

                    // Scan for the player in the radius as defined by the Activation distance of the block
                    List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(null, new Bounds(__instance.ToWorldPos().ToVector3(), Vector3.one * ( block2.GetActivationDistanceSq() / block2.GetActivationDistanceSq() )));
                    if (entitiesInBounds.Count > 0)
                    {
                        for (int i = 0; i < entitiesInBounds.Count; i++)
                        {
                            EntityPlayer player = entitiesInBounds[i] as EntityPlayer;
                            if (player)
                            {
                                if (block2.Properties.Values.ContainsKey("ActivationBuffs"))
                                {
                                    foreach (String strbuff in block2.Properties.Values["ActivationBuffs"].Split(','))
                                    {
                                        String strBuffName = strbuff;
                                        float CheckValue = -1f;

                                        // Check if there's a ( ) at the end of the buff; this will be used as a cvar hook.
                                        int start = strbuff.IndexOf('(');
                                        int end = strbuff.IndexOf(')');
                                        if (start != -1 && end != -1 && end > start + 1)
                                        {
                                            CheckValue = StringParsers.ParseFloat(strbuff.Substring(start + 1, end - start - 1), 0, -1, NumberStyles.Any);
                                            strBuffName = strbuff.Substring(0, start);
                                        }

                                        // If the player has a buff by this name, trigger it.
                                        if (player.Buffs.HasBuff(strBuffName))
                                            blCanTrigger = true;

                                        // If the player has a cvar, then do some extra checks
                                        if (player.Buffs.HasCustomVar(strBuffName))
                                        {
                                            // If there's no cvar value specified, just allow it.
                                            if (CheckValue == -1)
                                                blCanTrigger = true;

                                            // If a cvar is set, then check to see if it matches
                                            if (CheckValue > -1)
                                            {
                                                if (player.Buffs.GetCustomVar(strBuffName) == CheckValue)
                                                    blCanTrigger = true;
                                            }

                                            // If any of these conditions match, trigger the Activate block
                                            if (blCanTrigger)
                                                break;
                                        }

                                    }
                                }
                            }

                        }
                    }

                    if (blCanTrigger)
                    {
                        Block.list[block.type].ActivateBlock(world, __instance.GetClrIdx(), __instance.ToWorldPos(), block, true, true);
                        return true;
                    }
                    else
                    {
                            Block.list[block.type].ActivateBlock(world, __instance.GetClrIdx(), __instance.ToWorldPos(), block, false, true);
                    }
                    
                }

            }

            return true;
        }
    }
}
