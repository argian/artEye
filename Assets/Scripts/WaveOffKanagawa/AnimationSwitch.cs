﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimationSwitch : UdonSharpBehaviour
{
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string triggerName;

    public override void Interact()
    {
        base.Interact();
        targetAnimator.SetTrigger(triggerName);
    }
}
