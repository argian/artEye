
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class KnobHandle : UdonSharpBehaviour
{
    private VRCObjectSync vrcObjectSync;
    private VRCPlayerApi vrcPlayerApi;
    private bool isHeld;
    [SerializeField] private Knob knob;
    [SerializeField] private Transform model;
    [SerializeField] private float minAngleLock;
    [SerializeField] private float maxAngleLock;

    private void Start()
    {
        vrcObjectSync = GetComponent<VRCObjectSync>();
        vrcPlayerApi = Networking.LocalPlayer;
    }

    private void Update()
    {
        if (!isHeld)
            return;
        
        Vector3 rotation = model.localRotation.eulerAngles;
        rotation.z = Mathf.Clamp(transform.localRotation.eulerAngles.z, minAngleLock, maxAngleLock);
        Debug.Log(rotation);
        
        model.localRotation = Quaternion.Euler(rotation);
        knob.SetAngle(rotation.z);
    }

    public override void OnPickup()
    {
        base.OnPickup();
        if (!vrcPlayerApi.isLocal)
            return;

        isHeld = true;
    }

    public override void OnDrop()
    {
        base.OnDrop();
        if (!vrcPlayerApi.isLocal)
            return;
		
        isHeld = false;
        vrcObjectSync.TeleportTo(model);
    }
}
