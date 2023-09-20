using UnityEngine;

public class RoomTeleporter : Teleporter
{    
    [SerializeField] private Loadable[] toUnload;
    [SerializeField] private Loadable[] toLoad;

    protected override void InvokeBeforeTeleport()
    {
        foreach (Loadable loadable in toUnload)
            loadable.Unload();

        foreach (Loadable loadable in toLoad)
            loadable.Load();
    }
}
