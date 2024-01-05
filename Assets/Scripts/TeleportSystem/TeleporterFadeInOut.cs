
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TeleporterFadeInOut : ShaderPasser
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private TeleporterStatus teleporterStatus;
    [SerializeField] private float fadeValue;
    [SerializeField] private Teleporter chosenTeleporter;
    [SerializeField] private Transform faceCube;
    VRCPlayerApi localPlayer;

    public void TeleportPlayerWithFade(Teleporter teleporter)
    {
        if (chosenTeleporter)
            return;
        chosenTeleporter = teleporter;
        
        if (!MainMaterial)
        {
            chosenTeleporter.TeleportPlayer();
            chosenTeleporter = null;
            return;
        }
        
        teleporterStatus = TeleporterStatus.TeleportIn;
    }

    protected override void FakeStart() {
        localPlayer = Networking.LocalPlayer;
    }

    protected override void BakePropertyNames()
    {
        PropertyNames = new string[1];
        PropertyIDs = new int[1];

        PropertyNames[0] = "Fade";
    }

    protected override void PassToRender()
    {
        switch (teleporterStatus)
        {
            case TeleporterStatus.None:
                return;
            case TeleporterStatus.TeleportIn:
                faceCube.gameObject.SetActive(true);

                if (fadeValue < 1)
                    fadeValue += Time.deltaTime / fadeDuration;

                if (fadeValue >= 1)
                {
                    teleporterStatus = TeleporterStatus.TeleportOut;
                    fadeValue = 1;
                    chosenTeleporter.TeleportPlayer();
                }

                break;
            case TeleporterStatus.TeleportOut:
                if (fadeValue > 0)
                    fadeValue -= Time.deltaTime / fadeDuration;

                if (fadeValue <= 0)
                {
                    teleporterStatus = TeleporterStatus.None;
                    fadeValue = 0;
                    chosenTeleporter = null;
                    faceCube.gameObject.SetActive(false);
                }
                break;
        }
        MainMaterial.SetFloat(PropertyIDs[0], fadeValue);
        faceCube.position = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    }
}
