
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Respawn : UdonSharpBehaviour
{
    [SerializeField] private Renderer r;
    
    [SerializeField] private Room[] loadOnRespawn;
    [SerializeField] private Room[] unloadOnRespawn;

    [SerializeField] private GameObject dummy;
    
    private void Update()
    {
        /*
        if (!dummy.activeSelf)
        {
            return;
        }

        Debug.Log("on respawn");
        r.material.color = Color.red;
        
        foreach (Room room in unloadOnRespawn)
        {
            room.Unload();
        }
        
        foreach (Room room in loadOnRespawn)
        {
            room.Load();
        }
        
        dummy.SetActive(false);
        */
    }
}
