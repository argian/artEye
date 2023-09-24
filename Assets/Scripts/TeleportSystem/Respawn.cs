
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Respawn : UdonSharpBehaviour
{
    public Room mainHub;
    public Room[] otherRooms;
    
    public override void OnPlayerRespawn(VRCPlayerApi player)
    {
        base.OnPlayerRespawn(player);
        mainHub.Load();
        for (int i = 0; i < otherRooms.Length; i++)
        {
            otherRooms[i].Unload();
        }
    }
}
