using DMT;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SphereII_MainMenu
{
    public class SphereII_MainMenuHooks : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + this.GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(XUiC_MainMenu))]
    [HarmonyPatch("OnOpen")]
    public class SphereII_MainMenu_Init
    {
        // Attach the script to the center button.
        public static void Postfix()
        {

            //    GameObject.Find("btnConnectToServer").AddComponent<SphereII_MenuScript>();


            GameObject myMenuObject = new GameObject();
            //ParticleSystem myParticleSystem;

            String strMyAssetBundle = "#@modfolder(SphereII Winter Project Main Menu):Resources/MenuParticles.unity3d?SnowSoftNoGround";
            Debug.Log(" Asset Bundle: " + strMyAssetBundle);
            if (strMyAssetBundle.IndexOf('#') == 0 && strMyAssetBundle.IndexOf('?') > 0)
            {
                GameObject temp = DataLoader.LoadAsset<GameObject>(strMyAssetBundle);
                if (temp != null)
                {
                    Debug.Log("Game Object loaded");
                    // We need to instantiate the object to have it come into effect. We'll attach it to the button as a focus point.
                    myMenuObject = UnityEngine.Object.Instantiate<GameObject>(temp, GameObject.Find("btnConnectToServer").transform);
                    // In order to display on the UI, the layer of the game object must be 12.
                    myMenuObject.layer = 12;
                    myMenuObject.SetActive(true);
                    //myParticleSystem = myMenuObject.GetComponent<ParticleSystem>();
                    //ParticleSystem.MainModule temp2 = myParticleSystem.main;
                    //temp2.maxParticles = 10000000;
                    //temp2.loop = true;
                }
            }


        }
    }
}



