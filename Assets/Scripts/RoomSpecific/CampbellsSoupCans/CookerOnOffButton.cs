
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CookerOnOffButton : UdonSharpBehaviour
{
    [SerializeField] private Cooker cooker;

    public override void Interact()
    {
        base.Interact();
        cooker.CookerToggle();
    }
}
