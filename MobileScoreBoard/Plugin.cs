using BepInEx;
using BepInEx.Configuration;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Utilla;
using Valve.VR;

namespace MobileScoreBoard
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool pressing;
        private bool pressingb;
        private float holdTimeCounter;
        private bool cooldown;
        private bool cooldownnew;
        private ConfigEntry<int> holdTime;
        private GorillaMetaReport gmet;
        private GorillaReportButton gmetb;

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }

        void OnGameInitialized()
        {
            holdTime = Config.Bind("Settings", "Hold Time", 4);
            gmet = FindObjectOfType<GorillaMetaReport>();
            gmetb = FindObjectOfType<GorillaReportButton>();
            cooldownnew = false;
        }

        void Update()
        {
            pressingb = ControllerInputPoller.instance.rightControllerSecondaryButton;
            pressing = ControllerInputPoller.instance.leftControllerSecondaryButton;

            if (pressing)
            {
                if (cooldownnew == false)
                {
                    gmet.StartOverlay();
                    cooldownnew = true;
                }
            }
            else
            {
                cooldownnew = false;
            }
            gmet.isMoving = false;

            if (pressingb)
            {
                gmetb.selected = true;
            }
            else
            {
                gmetb.selected = false;
            }
            gmet.isMoving = true;
        }

        IEnumerator Cooldown(float sec)
        {
            yield return new WaitForSeconds(sec);
            cooldown = false;
        }

        
    }
}
