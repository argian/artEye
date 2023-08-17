using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Teleporter : UdonSharpBehaviour
{
    public string teleporterName;
    [SerializeField] private Loadable[] toUnload;
    [SerializeField] private Loadable[] toLoad;
    public Teleporter linkedTeleporter;
    public Transform exit;
    private VRCPlayerApi vrcPlayerApi;

    void Start()
    {
        vrcPlayerApi = Networking.LocalPlayer;
        Debug.Log(vrcPlayerApi);
        if (!linkedTeleporter)
            Debug.LogError($"No teleporter linked to {name}");
    }

    public override void Interact()
    {
        base.Interact();
        if (!vrcPlayerApi.isLocal)
            return;
        
        if (!linkedTeleporter)
        {
            Debug.LogError("No teleporter linked!");
            return;
        }
        
        foreach (Loadable loadable in toUnload)
        {
            loadable.Unload();
        }
        
        foreach (Loadable loadable in toLoad)
        {
            loadable.Load();
        }
        
        vrcPlayerApi.TeleportTo(linkedTeleporter.exit.position, linkedTeleporter.exit.rotation);

        /*
        teleportedObject.transform.position = linkedTeleporter.exit.position;
        teleportedObject.transform.rotation = linkedTeleporter.exit.rotation;
        */
    }
}
