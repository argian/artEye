
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FollowCamera : UdonSharpBehaviour
{
    private VRCPlayerApi LocalPlayer;

    private void Start()
    {
        LocalPlayer = Networking.LocalPlayer;
    }

    private void Update()
    {
        transform.position = LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        transform.rotation = LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
    }
}
