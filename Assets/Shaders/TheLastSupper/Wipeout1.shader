Shader "Custom/Wipeout"
{
    Properties
    {
    }
            SubShader
        {
            // Draw after all opaque geometry
            //well not exacly
            Tags {
                "RenderType" = "Opaque"
                "Queue" = "Overlay-2001"
                "LightMode" = "Always"
                "IgnoreProjector" = "True"
                "DisableBatching" = "True"
                "ForceNoShadowCasting" = "True"
                "PreviewType" = "Plane"
                "VRCFallback" = "Hidden"
            }

            ZWrite On
            ZTest Always
            Cull Off

            // Grab the screen behind the object into _BackgroundTexture
            GrabPass
            {
                "_GrabTex"
            }

            // Render the object with the texture generated above
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

                o.pos = UnityObjectToClipPos(v.vertex);

                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
                }

            sampler2D _GrabTex;

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.grabPos.xy / i.grabPos.w;
                half4 col = tex2D(_GrabTex, uv);

                return half4(1, 1, 1, 1);
            }
            ENDCG
            }

        }
    }
