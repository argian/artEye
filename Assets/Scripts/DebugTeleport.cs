
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// TODO remove script in full version
public class DebugTeleport : UdonSharpBehaviour
{
    [SerializeField] private Teleporter[] teleporters;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (teleporters[0])
                teleporters[0].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (teleporters[1])
                teleporters[1].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (teleporters[2])
                teleporters[2].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (teleporters[3])
                teleporters[3].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            if (teleporters[4])
                teleporters[4].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            if (teleporters[5])
                teleporters[5].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            if (teleporters[6])
                teleporters[6].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            if (teleporters[7])
                teleporters[7].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            if (teleporters[8])
                teleporters[8].Interact();
        } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            if (teleporters[9])
                teleporters[9].Interact();
        }
    }
}
