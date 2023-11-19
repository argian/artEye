Shader "Custom/RaySkybox"
{
    Properties
    {
        //spacing("spacing", vector) = (1,1,1,1)
        LightDir("LightDir", vector) = (0, 0, 0, 1)
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
            #include "Assets/Shaders/General/RayTracing.cginc"
            //#include "Assets/Shaders/General/UsefulCalculations.cginc"

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
uniform vector HeadPos;
uniform vector Spacing;
vector LightDir;

half4 frag(v2f i) : SV_Target
{
    float2 uv = i.grabPos.xy / i.grabPos.w;
    half4 col = tex2D(_GrabTex, uv); //we need that later

    //well that is here for testing
    col = half4(1, 1, 1, 1);

    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

    float3 rayDir = normalize((i.ray.xyz / i.ray.w).xyz);

    col.xyz = rayDir;
    float3 spacing = abs(float3(Spacing.x, 0.1, Spacing.z - 400));

    float4 hit = float4(0, 0, 0, 0); //w means if hits or not
    float3 normals = float3(0, 0, 0);
    //*
    if (SimplePlane(rayDir, hit, float3(0, -2, 0), float3(0, 1, 0), float3(10, 1, 10)))
    {
        if (SimpleCheckboard(hit, spacing.xyz))
        {
            //col.xyz = half4(0.8, 0.2, 0.6, 1);
            normals = float3(0, 1, 0);
            hit.w = 1;
            col.xyz = half4(0.8, 0.2, 0.2, 1);
        }
        else
        {
            //render cube in nearest possible object
            float3 nearestHit = float3((round(hit.x / spacing.x / 2) * 2 + 0.5 * sign(rayDir.x)) * spacing.x, hit.y - spacing.y / 2, (round(hit.z / spacing.z / 2) * 2 + 0.5 * sign(rayDir.z)) * spacing.z);
            if (SimpleCube(nearestHit, hit, rayDir, spacing.xyz / 2, normals))
            {
                hit.w = 1;
                col.xyz = half4(0.8, 0.2, 0.2, 1);
            }

            //okay, we actually have to also check 2 nearest object
            if (abs(rayDir.x) > abs(rayDir.z))
            {
                nearestHit.x += spacing.x * 2 * sign(rayDir.x);
            }
            else if (abs(rayDir.z) > abs(rayDir.x))
            {
                nearestHit.z += spacing.z * 2 * sign(rayDir.z);
            }
            if (SimpleCube(nearestHit, hit, rayDir, spacing.xyz / 2, normals))
            {
                hit.w = 1;
                col.xyz = half4(0.8, 0.2, 0.2, 1);
            }
        }
    }

    if (SimpleCube(float3(6, -3, 0), hit, rayDir, float3(2, 2, 2), normals))
    {
        hit.w = 1;
        col.xyz = half4(0.8, 0.2, 0.2, 1);
    }
    //shitty lighting
    if (hit.w != 0)
    {
        col *= dot(normals, normalize(-LightDir.xyz));
    }
    //calculate screen col and uv and proceed to do nothing
    return col;
}
            ENDCG
        }
    }
}