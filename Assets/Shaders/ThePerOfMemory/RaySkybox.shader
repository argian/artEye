Shader "Custom/RaySkybox"
{
    Properties
    {

    }
    SubShader
    {
        // Draw after all opaque geometry
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Geometry+1"
            "LightMode" = "Always"
            "IgnoreProjector" = "True"
            "DisableBatching" = "True"
            "ForceNoShadowCasting" = "True"
            "PreviewType" = "Plane"
            "VRCFallback" = "Hidden"
        }

        ZWrite Off
        ZTest Always
        Cull Off

        // Grab the screen behind where object is rendered
        GrabPass
        {
            "_GrabTex"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                //this magic gets screen uv
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

sampler2D _GrabTex;
vector CameraPos;
vector CameraDir;

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.grabPos.xy / i.grabPos.w;
                half4 col = tex2D(_GrabTex, uv);
                
                //funny test rainbow (actually these are normals but shhh)
                col.xyz = CameraDir.xyz;
                return col;
            }
            ENDCG
        }
    }
}
