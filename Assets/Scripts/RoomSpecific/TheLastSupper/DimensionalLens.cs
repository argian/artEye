
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DimensionalLens : UdonSharpBehaviour
{
    [SerializeField] private MeshRenderer portalBaker;
    
    public MeshRenderer GetRenderer()
    {
        return portalBaker;
    } 
}
