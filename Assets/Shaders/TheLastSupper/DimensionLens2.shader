Shader "Custom/DimensionLens2"
{
    Properties
    {
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque"  
            "ForceNoShadowCasting" = "True" 
            "Queue" = "Geometry-11"}
            ColorMask 0
            LOD 200
            ZWrite off

            Stencil
            {
                Ref 2
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            UNITY_INSTANCING_BUFFER_START(Props)

            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {

            }
            ENDCG
        }
            //FallBack "Diffuse"
}
