
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimationOnOffSwitch : UdonSharpBehaviour
{
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string boolName;
    public bool isOn;

    private void Start()
    {
        targetAnimator.SetBool(boolName, isOn);
    }

    public override void Interact()
    {
        base.Interact();
        isOn = !isOn;
        targetAnimator.SetBool(boolName, isOn);
    }
}
