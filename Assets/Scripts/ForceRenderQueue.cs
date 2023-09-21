
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ForceRenderQueue : UdonSharpBehaviour
{
    public int RenderQueue;
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.renderQueue = RenderQueue;
    }
}
