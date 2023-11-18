Shader "Unlit/DepthComparator"
{
    Properties
    {
        _Depth("Depth", float) = 98
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D_float _CameraDepthTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 screenPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
float _Depth;
            float frag(v2f i) : SV_Depth
            {

                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPos);
                depth = Linear01Depth(depth) * _ProjectionParams.z;
                // sample the texture
                clip(_Depth - depth);
                return _Depth;
            }
            ENDCG
        }
    }
}
