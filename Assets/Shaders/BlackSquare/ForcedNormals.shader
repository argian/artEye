Shader "Custom/ForcedNormals"
{

    Properties{
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader{
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
          #pragma surface surf SimpleLambert noambient

          half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
        //simply replace normals with whatever you like:
        half3 ambientLight = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
        //half NdotL = max(0, dot(s.Normal, lightDir));
        half NdotL = max(0, dot(float3(0, 0, -1), lightDir));
        half4 c;
        c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
        //c.rgb = lightDir;
        //c.rgb = ambientLight;
        //c.rgb = s.Normal;
        //c.rgb = s.Albedo;
        //c.rgb = atten;
        //c.rgb = half3(1, 1, 1);
        //c.a = NdotL;
        c.a = 1;
        return c;
        }

        struct Input
        {
        float2 uv_MainTex;
        };

sampler2D _MainTex;

void surf(Input IN, inout SurfaceOutput o)
{
fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
o.Albedo = c.rgb;
//o.Alpha = c.a;

}
ENDCG
    }
        Fallback "Diffuse"
}
