// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/Diamond"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ReflectTex ("Reflection Texture", Cube) = ""
        _RefractTex ("Refraction Texture", Cube) = ""
    }
 
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
        }
 
        Pass
        {
            Cull Front
            ZWrite Off
     
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
     
            float4         _Color;
            samplerCUBE _ReflectTex;
            samplerCUBE _RefractTex;
     
            struct v2f
            {
                float4 pos     : SV_POSITION;
                float3 uv     : TEXCOORD0;
            };
     
            v2f vert (float4 v : POSITION, float3 n : NORMAL)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v);
         
                // TexGen CubeReflect: Reflect view direction along the normal, in view space
                float3 viewDir = normalize(ObjSpaceViewDir(v));
                o.uv = reflect(-viewDir, n);
                o.uv = mul(UNITY_MATRIX_MV, float4(o.uv, 0));
                return o;
            }
     
            half4 frag (v2f i) : SV_Target
            {
                float4 col = texCUBE(_RefractTex, i.uv) * _Color;          
                col.a = texCUBE(_ReflectTex, i.uv) - 0.5f;
                return col;
            }
            ENDCG
        } 
 
        Pass
        {
            Cull     Back
            ZWrite     On
            Blend     One One
     
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
     
            float4         _Color;
            samplerCUBE _ReflectTex;
            samplerCUBE _RefractTex;
     
            struct v2f
            {
                float4 pos     : SV_POSITION;
                float3 uv     : TEXCOORD0;
            };
     
            v2f vert (float4 v : POSITION, float3 n : NORMAL)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v);
         
                // TexGen CubeReflect: Reflect view direction along the normal, in view space
                float3 viewDir = normalize(ObjSpaceViewDir(v));
                o.uv = reflect(-viewDir, n);
                o.uv = mul(UNITY_MATRIX_MV, float4(o.uv, 0));
                return o;
            }
     
            half4 frag (v2f i) : SV_Target
            {
                float4 col = texCUBE(_RefractTex, i.uv) * _Color + texCUBE(_ReflectTex, i.uv);          
                col.a += texCUBE(_ReflectTex, i.uv) - 0.5f;
                return col;
            }
            ENDCG
        }
 
    }
    FallBack "Diffuse"
}