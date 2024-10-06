using BepInEx;
using BepInEx.Configuration;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Utilla;

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
        private bool menuOpen;
        private ConfigEntry<int> holdTime;
        private ConfigEntry<bool> shouldMove;
        private GorillaMetaReport gmet;
        private GorillaReportButton gmetb;

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }

        void OnGameInitialized()
        {
            holdTime = Config.Bind("Settings", "Hold Time", 4);
            shouldMove = Config.Bind("Settings", "Should Move", true);
            gmet = FindObjectOfType<GorillaMetaReport>();
            gmetb = FindObjectOfType<GorillaReportButton>();
            menuOpen = false;
        }

        void Update()
        {
            pressingb = ControllerInputPoller.instance.rightControllerSecondaryButton;
            pressing = ControllerInputPoller.instance.leftControllerSecondaryButton;

            if (pressing && !cooldown && !menuOpen)
            {
                holdTimeCounter += Time.deltaTime;

                if (holdTimeCounter >= holdTime.Value)
                {
                    gmet.StartOverlay();
                    menuOpen = true;
                    cooldown = true;
                    holdTimeCounter = 0;
                    StartCoroutine(Cooldown(1f));
                }
            }
            else
            {
                holdTimeCounter = 0;
            }
            gmet.isMoving = false;

            if (pressingb)
            {
                gmetb.selected = true;
                menuOpen = false;
            }
            else
            {
                gmetb.selected = false;
            }
            gmet.isMoving = shouldMove.Value;
        }

        IEnumerator Cooldown(float sec)
        {
            yield return new WaitForSeconds(sec);
            cooldown = false;
        }

        
    }
}
