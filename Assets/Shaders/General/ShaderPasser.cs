﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ShaderPasser : UdonSharpBehaviour
{
    //I know I know, it's in shader folder, but this is the base class
    //for EVERY script that passes data to shaders,
    //so that's why here

    public Material MainMaterial;

    protected string[] PropertyNames;
    protected int[] PropertyIDs;
    void Start()
    {
        BakePropertyNames();
        BakePropertyIDs();
        FakeStart();
    }
    
    protected virtual void FakeStart()
	{

	}

    //trust me it is that 1% scenario when hardcoding values is better then setting in inspector, as they are shader based, not material based
    protected virtual void BakePropertyNames()
	{

	}

    //we need to do that, it's just how shaders works
    void BakePropertyIDs()
	{
        //Shader shader = MainMaterial.shader;
        for (int i = 0; i < PropertyNames.Length; ++i)
		{
            PropertyIDs[i] = VRCShader.PropertyToID(PropertyNames[i]);
        }
	}

	void Update()
	{
        PassToRender();
	}

    //this thing has to be custom-made every time because of different data formats
    protected virtual void PassToRender()
	{
        
	}
}
