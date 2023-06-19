using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Utilla;

namespace MobileScoreBoard
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly XRNode lNode = XRNode.LeftHand;
        bool pressing;
        int secHeld;
        bool cooldown;
        ConfigEntry<int> HoldTime;
        GorillaMetaReport gmet;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            HoldTime = Config.Bind("Settings", "Hold Time", 4);
            gmet = FindObjectOfType<GorillaMetaReport>();
            gmet.enabled = true;
        }

        void Update()
        {

            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(CommonUsages.primaryButton, out pressing);
            if(pressing && cooldown == false) 
            {
                cooldown = true;
                StartCoroutine(Cooldown(1));
                secHeld++;
            }
            if (!pressing)
            {
                secHeld = 0;
            }
            if (secHeld == HoldTime.Value)
            {
                FindObjectOfType<GorillaMetaReport>().StartOverlay();
            }
        }
        IEnumerator Cooldown(float sec)
        {
            yield return new WaitForSeconds(sec);
            cooldown = false;
        }
    }
}
