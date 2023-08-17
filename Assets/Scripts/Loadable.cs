
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public abstract class Loadable : UdonSharpBehaviour
{
    public abstract void Load();
    public abstract void Unload();
}
