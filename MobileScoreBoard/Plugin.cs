using BepInEx;
using BepInEx.Configuration;
using GorillaLocomotion;
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
        private bool pressing;
        private float holdTimeCounter;
        private bool cooldown;
        private ConfigEntry<int> holdTime;
        private GorillaMetaReport gmet;

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }

        void OnGameInitialized()
        {
            holdTime = Config.Bind("Settings", "Hold Time", 4);
            gmet = FindObjectOfType<GorillaMetaReport>();
            gmet.reportScoreboard.transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        void Update()
        {
            pressing = ControllerInputPoller.instance.rightControllerSecondaryButton;

            if (pressing && !cooldown)
            {
                holdTimeCounter += Time.deltaTime;

                if (holdTimeCounter >= holdTime.Value)
                {
                    gmet.StartOverlay();
                    cooldown = true;
                    holdTimeCounter = 0;
                    StartCoroutine(Cooldown(1f));
                }
            }
            else
            {
                holdTimeCounter = 0;
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
