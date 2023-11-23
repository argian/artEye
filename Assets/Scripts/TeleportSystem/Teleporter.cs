using System;
using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class Teleporter : UdonSharpBehaviour
{
    public string teleporterName;
    public Teleporter linkedTeleporter;
    public Transform exit;

    [SerializeField] protected TeleporterFadeInOut teleporterFadeInOut;
    protected VRCPlayerApi vrcPlayerApi;

    protected virtual void Start()
    {
        vrcPlayerApi = Networking.LocalPlayer;
        if (!linkedTeleporter)
            Debug.LogWarning($"No teleporter linked to {teleporterName}");
    }

    public override void Interact()
    {
        if (teleporterFadeInOut)
            teleporterFadeInOut.TeleportPlayerWithFade(this);
        else
            TeleportPlayer();
    }

    public virtual void TeleportPlayer()
    {
        if (!vrcPlayerApi.isLocal)
            return;

        if (!linkedTeleporter)
        {
            Debug.LogError("No teleporter linked!");
            return;
        }

        InvokeBeforeTeleport();

        vrcPlayerApi.TeleportTo(linkedTeleporter.exit.position, linkedTeleporter.exit.rotation);

        InvokeAfterTeleport();
    }

    protected virtual void InvokeBeforeTeleport() { }

    protected virtual void InvokeAfterTeleport() { }
}

public enum TeleporterStatus
{
    None,
    TeleportIn,
    TeleportOut
}