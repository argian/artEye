﻿Shader "Custom/LocalMelt"
{
    Properties
    {
        _CenterPos("CenterPos", vector) = (0, 0, 0, 0)
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseScale ("_NoiseScale", vector) = (1, 1, 1, 1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert fullforwardshadows forcenoshadowcasting

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "Assets/Shaders/General/UsefulCalculations.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        uniform float4 _CenterPos;
        float4 _NoiseScale;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            //o.worldPos = float4(mul(unity_ObjectToWorld, v.vertex).xyz, 1);
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            float3 centeredPos = worldPos - _CenterPos;
            centeredPos.z += _Time * _NoiseScale.w;
            worldPos.y -= (GradientNoise(centeredPos / _NoiseScale.xyz) + 1) * -min(0, centeredPos.y);
            //world to vertex pos:
            v.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
