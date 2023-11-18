
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ConveyorBelt : UdonSharpBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;

    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.MovePosition(other.transform.position + direction * speed * Time.deltaTime);
    }
}
