
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Respawn : UdonSharpBehaviour
{
    [SerializeField] private Room[] loadOnRespawn;
    [SerializeField] private Room[] unloadOnRespawn;

    public override void OnPlayerRespawn(VRCPlayerApi player)
    {
        base.OnPlayerRespawn(player);
        
        foreach (Room room in unloadOnRespawn)
        {
            room.Unload();
        }
        
        foreach (Room room in loadOnRespawn)
        {
            room.Load();
        }
    }
}
