﻿//https://forum.unity.com/threads/normal-map-from-tangent-space-to-world.1221684/

Shader "Unlit/NormalsToColor"
{
    Properties
    {
        _NormalMap("NormalMap", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque"
                "Queue" = "Overlay-2002"
            }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv_normal : TEXCOORD0;
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                // these three vectors will hold a 3x3 rotation matrix
                // that transforms from tangent to world space
                half3 tspace0 : TEXCOORD1; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD2; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD3; // tangent.z, bitangent.z, normal.z
                // texture coordinate for the normal map
                float2 uv_normal : TEXCOORD4;
                float3 worldNormal : TEXCOORD5;
                float4 pos : SV_POSITION;
                //float eyePos : 
            };

            sampler2D _NormalMap;
            float4 _NormalMap_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                half3 wNormal = UnityObjectToWorldNormal(v.normal);
                half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
                // compute bitangent from cross product of normal and tangent
                half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                o.uv_normal = v.uv_normal;
                o.worldNormal = wNormal;

                //o.eyePos = mul(UNITY_MATRIX_MV, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
            fixed4 col = fixed4(1, 1, 1, 1);

            // sample the normal map, and decode from the Unity encoding
            half3 tnormal = UnpackNormal(tex2D(_NormalMap, i.uv_normal));
            // transform normal from tangent to world space
            half3 worldNormal;
            worldNormal.x = dot(i.tspace0, tnormal);
            worldNormal.y = dot(i.tspace1, tnormal);
            worldNormal.z = dot(i.tspace2, tnormal);

            //enchance normals:
            worldNormal = round(worldNormal);
            //convert normals to color space
            col.rgb = worldNormal * 0.5 + 0.5;
            //convert gamma to linear result
            col.rgb = pow(col.rgb, 2.2);
            //i.worldNormal
            return col;
        }
        ENDCG
    }
    }
}