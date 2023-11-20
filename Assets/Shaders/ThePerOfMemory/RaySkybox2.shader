Shader "Custom/RaySkybox2"
{
    Properties
    {
        Spacing("Spacing", vector) = (1,1,1,1)
    }
    SubShader
    {
        // Draw after all opaque geometry
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Geometry+1" //change it to correspond to scene!
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
            #include "RayTracing.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 clipPos : SV_POSITION;
                float4 ray : TEXCOORD1;
            };

            float3 get_camera_pos()
            {
                float3 worldCam;
                worldCam.x = unity_CameraToWorld[0][3];
                worldCam.y = unity_CameraToWorld[1][3];
                worldCam.z = unity_CameraToWorld[2][3];
                return worldCam;
            }
            // _WorldSpaceCameraPos is broken in VR (single pass stereo)
            static float3 camera_pos = get_camera_pos();

            v2f vert(appdata_base v)
            {
                v2f o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                //this magic gets screen uv
                o.clipPos = UnityObjectToClipPos(v.vertex);
                //yeah vr specialties stuff again
                o.grabPos = ComputeNonStereoScreenPos(o.clipPos);
                //this thing calculates rayDir accordingly in vr
                //(it is not normal way of doing that btw)
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.ray.xyz = worldPos.xyz - camera_pos.xyz;
                o.ray.w = o.clipPos.w;
                return o;
            }

sampler2D _GrabTex;
vector CameraPos;
vector Spacing;

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.grabPos.xy / i.grabPos.w;
                half4 col = tex2D(_GrabTex, uv); //we need that later

                //well that is here for testing
                col = half4(1, 1, 1, 1);

                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                float3 rayDir = normalize((i.ray.xyz / i.ray.w).xyz);

                col.xyz = rayDir;

                float4 hit = float4(0, 0, 0, 0);
                float3 normals = float3(0, 0, 0);
                //*
                if (SimplePlane(rayDir, hit, float3(0, -2, 0), float3(0, 1, 0), float3(10, 1, 10)))
                {
                    if (SimpleCheckboard(hit, Spacing.xyz))
                    {
                        //*
                        
                        //*/
                        //col.xz = abs(hit.xz) / 10;
                        //col.xyz = half4(0.8, 0.2, 0.2, 1);
                        col.xyz = half4(0.8, 0.2, 0.6, 1);
                    }
                    else
                    {
                        //render cube in nearest possible object
                        float3 nearestHit = float3((round(hit.x / Spacing.x / 2) * 2 + 0.5 * sign(rayDir.x)) * Spacing.x, hit.y - Spacing.y / 2, (round(hit.z / Spacing.z / 2) * 2 + 0.5 * sign(rayDir.z)) * Spacing.z);
                        if (SimpleCube(nearestHit, hit, rayDir, Spacing.xyz / 2, normals))
                        {
                            col.xyz = half4(0.8, 0.2, 0.2, 1);
                        }

                        //okay, we actually have to also check 2 nearest object
                        if (abs(rayDir.x) > abs(rayDir.z))
                        {
                            nearestHit.x += Spacing.x * 2 * sign(rayDir.x);
                        }
                        else if (abs(rayDir.z) > abs(rayDir.x))
                        {
                            nearestHit.z += Spacing.z * 2 * sign(rayDir.z);
                        }
                        if (SimpleCube(nearestHit, hit, rayDir, Spacing.xyz / 2, normals))
                        {
                            col.xyz = half4(0.8, 0.2, 0.2, 1);
                        }
                    }
                }

                if (SimpleCube(float3(6, 3, 0), hit, rayDir, float3(2, 2, 2), normals))
                {
                    col.xyz = half4(0.8, 0.2, 0.2, 1);
                }
                //calculate screen col and uv and proceed to do nothing
                return col;
            }
            ENDCG
        }
    }
}
