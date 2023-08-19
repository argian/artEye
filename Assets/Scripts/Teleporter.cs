using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class Teleporter : UdonSharpBehaviour
{
    public string teleporterName;
    public Teleporter linkedTeleporter;
    public Transform exit;
    
    [SerializeField] private Loadable[] toUnload;
    [SerializeField] private Loadable[] toLoad;
    private VRCPlayerApi vrcPlayerApi;

    private void Start()
    {
        vrcPlayerApi = Networking.LocalPlayer;
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
            loadable.Unload();

        foreach (Loadable loadable in toLoad)
            loadable.Load();

        vrcPlayerApi.TeleportTo(linkedTeleporter.exit.position, linkedTeleporter.exit.rotation);
    }
}
