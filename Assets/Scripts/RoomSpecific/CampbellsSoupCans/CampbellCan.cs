
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CampbellCan : UdonSharpBehaviour
{
    private Renderer r;
    
    private void Awake()
    {
        r = GetComponent<Renderer>();
    }

    public void SetTexture(Texture2D newTexture)
    {
        r.material.mainTexture = newTexture;
    }
}
