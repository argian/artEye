Shader "Custom/RaySkybox"
{
    Properties
    {
        //spacing("spacing", vector) = (1,1,1,1)
        LightDir("LightDir", vector) = (0, 0, 0, 1)
        FacingDir("FacingDir", vector) = (0, 0, 1, 1)
        MainGroundPos("MainGroundPos", vector) = (0, 0, 0, 0)
        CubeWallPos("CubeWallPos", vector) = (0, 0, 0, 1)
        CubeWallDir("CubeWallDir", vector) = (0, 0, 1, 1)
        CubeWallScale("CubeWallScale", vector) = (0, 0, 0, 1)
        CubeRoofPos("CubeRoofPos", vector) = (0, 0, 0, 1)
        CubeRoofDir("CubeRoofDir", vector) = (0, 0, 1, 1)
        CubeRoofScale("CubeRoofScale", vector) = (0, 0, 0, 1)
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
        ZTest LEqual
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
uniform vector Spacing1;
uniform vector Spacing2;
uniform vector Spacing3;

vector LightDir;
vector FacingDir;
vector MainGroundPos;
vector CubeWallPos;
vector CubeWallDir;
vector CubeWallScale;
vector CubeRoofPos;
vector CubeRoofDir;
vector CubeRoofScale;

//rayPos is not used, player is fixed as if he is in "center" of world
//NOTE TO SELF: CHANGE RESULT NORMALS BASED ON FACING DIR
bool CubePatternPlane(float3 rayDir, float3 planePos, float3 facingDir, float3 scale, float3 spacing, inout float4 Hit, inout float3 Normals)
{
    //space transformations to make several facing directions
    //planePos = Rotate3DMatrix(planePos, facingDir);
    rayDir = Rotate3DMatrix(rayDir, facingDir);

    //actual object sequence
    if (SimplePlane(rayDir, planePos, scale, Hit))
    {
        //if (SimpleCheckboard(Hit, spacing.xyz))
        if (false)
        {
            //col.xyz = half4(0.8, 0.2, 0.6, 1);
            Normals = Rotate3DMatrix(float3(1, 1, 0), facingDir);
            Hit.w = 1;
            return true;
        }
        else
        {
            //return false;
            //render cube in nearest possible object
            //            float3 nearestHit = float3((round(Hit.x / spacing.x / 2) * 2 + 0.5 * sign(rayDir.x)) * spacing.x, Hit.y - spacing.y / 2, (round(Hit.z / spacing.z / 2) * 2 + 0.5 * sign(rayDir.z)) * spacing.z);

            //float3 nearestHole = float3(round((Hit.x + spacing.x / 2) / spacing.x) * spacing.x - spacing.x / 2, Hit.y - spacing.y / 2, );
            //Hit.z -= planePos.z;
            //Hit.x -= planePos.x;
            Hit.xz += spacing.xz * sign(Hit.xz) / 2; //Falty, but will check for now
            float3 nearestHit = float3((round(Hit.x / spacing.x / 2) * 2 + 0.5 * sign(rayDir.x)) * spacing.x, Hit.y - spacing.y / 2, (round(Hit.z / spacing.z / 2) * 2 + 0.5 * sign(rayDir.z)) * spacing.z);
            nearestHit.xz -= spacing.xz * sign(Hit.xz) / 2; //+ planePos.xz
            //nearestHit.xz -= planePos.xz;
            /*
            if (nearestHit.x < 0)
            {
                nearestHit.x += spacing.x;
            }
            */
            //nearestHit.xz -= planePos.xz % spacing.xz;

            if (SimpleCube(nearestHit, Hit, rayDir, spacing.xyz / 2, Normals))
            {
                Hit.w = 1;
                return true;
            }

            //*
            //okay, we actually have to also check 2 nearest object
            if (abs(rayDir.x) > abs(rayDir.z))
            {
                nearestHit.x += spacing.x * 2 * sign(rayDir.x);
            }
            else if (abs(rayDir.z) > abs(rayDir.x))
            {
                nearestHit.z += spacing.z * 2 * sign(rayDir.z);
            }
            if (SimpleCube(nearestHit, Hit, rayDir, spacing.xyz / 2, Normals))
            {
                Hit.w = 1;
                return true;
            }
            //*/
        }
    }

    //cube for testing
    /*
    if (SimpleCube(float3(6, -3, 0), hit, rayDir, float3(2, 2, 2), normals))
    {
        Hit.w = 1;
        return true;
    }
    */

    return false;
}

half4 frag(v2f i) : SV_Target
{
    float2 uv = i.grabPos.xy / i.grabPos.w;
    half4 col = tex2D(_GrabTex, uv); //we need that later

    //well that is here for testing
    col = half4(1, 1, 1, 1);

    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

    float3 rayDir = normalize((i.ray.xyz / i.ray.w).xyz);

    col.xyz = rayDir;
    float3 spacing = abs(float3(Spacing1.x, 0.1, Spacing1.z - 400));

    float4 hit = float4(0, 0, 0, 0); //w means if hits or not
    float3 normals = float3(0, 0, 0);
    //main ground
    if (CubePatternPlane(rayDir, MainGroundPos.xyz, FacingDir.xyz, float3(1000, 1, 1000), Spacing1, hit, normals))
    {
        col.xyz = half4(0.8, 0.2, 0.2, 1);
        //normals = Rotate3DMatrix(rayDir, -FacingDir.xyz);
    }

    //shitty lighting
    if (hit.w != 0)
    {
        col *= dot(normals, normalize(-LightDir.xyz));
    }

    //*
    //partial Cube wall 
    if (CubePatternPlane(rayDir, CubeWallPos.xyz, CubeWallDir.xyz, CubeWallScale.xyz, Spacing2.xyz, hit, normals))
    {
        col.xyz = half4(0.8, 0.2, 0.2, 1);
        //normals = Rotate3DMatrix(rayDir, -CubeWallDir.xyz);
    }

    //shitty lighting
    if (hit.w != 0)
    {
        col *= dot(normals, normalize(-LightDir.xyz));
    }

    //partial Cube roof 
    if (CubePatternPlane(rayDir, CubeRoofPos.xyz, CubeRoofDir.xyz, CubeRoofScale.xyz, Spacing3.xyz, hit, normals))
    {
        col.xyz = half4(0.8, 0.2, 0.2, 1);
        //normals = Rotate3DMatrix(rayDir, -CubeRoofDir.xyz);
    }

    //shitty lighting
    if (hit.w != 0)
    {
        col *= dot(normals, normalize(-LightDir.xyz));
    }

    return col;
}
            ENDCG
        }
    }
}