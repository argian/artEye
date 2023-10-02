using System;
using UnityEngine;

public class RoomTeleporter : Teleporter
{
    public Room room;

    private void OnValidate()
    {
        room = GetComponentInParent<Room>();
    }

    protected override void InvokeBeforeTeleport()
    {
        if (room)
            room.Unload();
        RoomTeleporter roomTeleporter = (RoomTeleporter)linkedTeleporter;
        if (roomTeleporter)
            roomTeleporter.room.Load();
    }
}
