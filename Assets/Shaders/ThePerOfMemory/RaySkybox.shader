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
        ZTest Less
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
            #include "UsefulCalculations.cginc"

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
float Fov;

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.grabPos.xy / i.grabPos.w;
                half4 col = tex2D(_GrabTex, uv);
                
                //funny test rainbow (actually these are normals but shhh)
                
                //calculate camera frustum (lazy)
                float2 uvFromCenter = (uv - 0.5) * 2;
                //float3 rayDir = Rotate3D(CameraDir.xyz, float3(uvFromCenter.x * Fov, 0, uvFromCenter.y * Fov));
                
                //another way:
                //float3 rayDir = Rotate3D(float3((uv - 0.5) * 2, 1), -CameraDir);
                float3 rayDir = Rotate3DMatrix(CameraDir, float3(-(uv - 0.5) * 2, 1));

                col.xyz = rayDir.xyz;

                //draw testing sphere:
                if (distance(float3(0, 0, 1), rayDir) < 0.5)
                {
                    col.xyz = float3(0.8, 0.8, 0.8);
                }

                return col;
            }
            ENDCG
        }
    }
}
