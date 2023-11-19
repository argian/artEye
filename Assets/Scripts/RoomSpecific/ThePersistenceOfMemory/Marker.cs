
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Marker : UdonSharpBehaviour
{
    [Range(0f, 1f)]
    public float weight;
}
