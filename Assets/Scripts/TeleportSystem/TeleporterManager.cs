
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TeleporterManager : UdonSharpBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private string shaderPropertyName;
    [SerializeField] private TeleporterStatus teleporterStatus;
    [SerializeField] private float fadeValue;
    [SerializeField] private Teleporter chosenTeleporter;

    private void Update()
    {
        switch (teleporterStatus)
        {
            case TeleporterStatus.None:
                break;
            case TeleporterStatus.TeleportIn:
                if (fadeValue < 1)
                    fadeValue += Time.deltaTime / fadeDuration;

                if (targetRenderer)
                    targetRenderer.material.SetFloat(shaderPropertyName, fadeValue);

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

                if (targetRenderer)
                    targetRenderer.material.SetFloat(shaderPropertyName, fadeValue);

                if (fadeValue <= 0)
                {
                    teleporterStatus = TeleporterStatus.None;
                    fadeValue = 0;
                    chosenTeleporter = null;
                }
                break;
        }
    }

    public void TeleportPlayerWithFade(Teleporter teleporter)
    {
        if (chosenTeleporter)
            return;
        chosenTeleporter = teleporter;
        teleporterStatus = TeleporterStatus.TeleportIn;
    }
}
