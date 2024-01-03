
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ConveyorBelt : UdonSharpBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float force;
    [SerializeField] private bool isOn = true;

    private void OnTriggerStay(Collider other)
    {
        if (isOn)
            other.attachedRigidbody.AddForce(direction * force * Time.deltaTime, ForceMode.Acceleration);
    }

    public void Activated(bool state)
    {
        isOn = state;
    }
}
