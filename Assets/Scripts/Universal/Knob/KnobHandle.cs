
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class KnobHandle : UdonSharpBehaviour
{
    private Knob knob;
    private VRCObjectSync vrcObjectSync;
    private VRCPlayerApi vrcPlayerApi;
    private bool isHeld;
    [SerializeField] private Transform model;

    private void Start()
    {
        knob = GetComponentInParent<Knob>();
        vrcObjectSync = GetComponent<VRCObjectSync>();
        vrcPlayerApi = Networking.LocalPlayer;
    }

    private void Update()
    {
        if (!isHeld)
            return;
        
        Vector3 rotation = model.rotation.eulerAngles;
        rotation.z = transform.rotation.eulerAngles.z;
        model.rotation = Quaternion.Euler(rotation);
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
