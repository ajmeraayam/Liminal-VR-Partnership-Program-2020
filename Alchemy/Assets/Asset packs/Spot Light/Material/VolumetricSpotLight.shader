Shader "Volumetric Spot Light/Geometry Based/Volumetric Spot Light" {
	Properties {
		_ConeSlopeCosSin ("Cone Slope Cos Sin", Vector) = (0, 0, 0, 0)
		_ConeRadius      ("Cone Radius", Vector) = (0, 0, 0, 0)

		[HDR] _Color  ("Color", Color) = (1, 1, 1, 1)
		_AlphaInside  ("Alpha Inside", Range(0, 1)) = 1
		_AlphaOutside ("Alpha Outside", Range(0, 1)) = 1

		_DistanceFadeStart  ("Distance Fade Start", Float) = 0
		_DistanceFadeEnd    ("Distance Fade End", Float) = 1

		_DepthBlendDistance ("Depth Blend Distance", Range(0, 1)) = 0.4

		_FresnelPow         ("Fresnel Pow", Range(0, 15)) = 1

		[Toggle(NOISE)] _NOISE ("Noise", Float) = 1
		_NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.2
		_NoiseParam     ("Noise(xyz:speed, w:scale)", Vector) = (0, 0, 0, 1)

		[Toggle(COLORBEAM)] _COLORBEAM ("ColorBeam", Float) = 1
		[NoScaleOffset]_BeamColorTex ("Beam Color", 2D) = "white" {}
		_BeamIntensity ("Beam Intensity", Float) = 1
		_BeamMove      ("Beam Move", Float) = 0.2
	}
	CGINCLUDE
		#include "UnityCG.cginc"

		inline float3 UnityWorldToObjectPos (in float3 pos)      { return mul(unity_WorldToObject, float4(pos, 1.0)).xyz; }
		inline float invLerp (float a, float b, float t)         { return (t - a) / (b - a); }
		inline float invLerpSaturate (float a, float b, float t) { return saturate(invLerp(a, b, t)); }

		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
		inline float SampleSceneZ (float4 projPos)
		{
			return LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(projPos)));
		}

		struct v2f
		{
			float4 clipPos : SV_POSITION;
			float4 localPos : TEXCOORD0;   // object space position
			float4 worldPos : TEXCOORD1;   // world space position
			float4 viewPos : TEXCOORD2;    // view space position
			float4 projPos : TEXCOORD3;    // project space position
#if COLORBEAM
			float2 uvSphereMapping : TEXCOORD4;   // sphere mapping
#endif
			UNITY_FOG_COORDS(5)
		};

		half4 _Color, _NoiseParam;
		float2 _ConeRadius, _ConeSlopeCosSin;
		float _AlphaInside, _AlphaOutside, _DistanceFadeEnd, _DistanceFadeStart, _FresnelPow, _DepthBlendDistance, _NoiseIntensity;
		sampler3D _NoiseTex;
		sampler2D _BeamColorTex;
		float _BeamIntensity, _BeamMove;

		v2f vert (appdata_base v)
		{
			v2f o;

			// build cone shape
			float4 posLocal = v.vertex;
			posLocal.xy *= lerp(_ConeRadius.x, _ConeRadius.y, posLocal.z);
			posLocal.z *= _DistanceFadeEnd;

			o.clipPos = UnityObjectToClipPos(posLocal);
			o.worldPos = mul(unity_ObjectToWorld, posLocal);
			o.localPos = posLocal;

			float3 posView = UnityObjectToViewPos(posLocal);
			float isCap = v.texcoord.x > 0.5;
			o.viewPos = float4(posView, isCap);

			float4 projPos = ComputeScreenPos(o.clipPos);
			projPos.z = -posView.z;
			o.projPos = projPos;

#if COLORBEAM
			float3 vn = mul(UNITY_MATRIX_MV, SCALED_NORMAL);
			vn = normalize(vn);
			posView = normalize(posView);
			float3 r = reflect(posView, vn);
			r.z += 1.0;
			float m = 0.5 * rsqrt(dot(r, r));
			o.uvSphereMapping = r.xy * m + 0.5;
#endif

			UNITY_TRANSFER_FOG(o, o.clipPos);
			return o;
		}
		half4 fragVSL (v2f i, float outside)
		{
			float3 viewPos = i.viewPos.xyz;
			float isCap = i.viewPos.w;
			float3 cam2pix = normalize(i.localPos.xyz - UnityWorldToObjectPos(_WorldSpaceCameraPos));

			float distFromSource = length(i.localPos.z);

			// the virutal normal of cone mesh
			float2 cosSinFlat = normalize(i.localPos.xy);
			float3 objnrm = normalize(float3(cosSinFlat.x * _ConeSlopeCosSin.x, cosSinFlat.y * _ConeSlopeCosSin.x, -_ConeSlopeCosSin.y));
			objnrm *= (outside * 2 - 1);
			objnrm = lerp(objnrm, float3(0, 0, -1), isCap);

			// near camera fade
			float nearPlane = _ProjectionParams.y;
			float fadeCloseCamera = smoothstep(0, 1, invLerpSaturate(nearPlane, nearPlane + 0.2, abs(viewPos.z)));

			// depth fade
			float depthBlendDist = _DepthBlendDistance * invLerpSaturate(0, _DepthBlendDistance, distFromSource);
			float sceneZ = max(0, SampleSceneZ(i.projPos) - _ProjectionParams.y);
			float partZ = max(0, i.projPos.z - _ProjectionParams.y);
			float fadeDepth = saturate((sceneZ - partZ) / depthBlendDist);
			fadeDepth = lerp(fadeDepth, 1, step(_DepthBlendDistance, 0));

			// attenuation
			float dist = invLerpSaturate(_DistanceFadeStart, _DistanceFadeEnd, distFromSource);
			float attQuad = 1.0 / (1.0 + 25.0 * dist * dist);
			attQuad *= saturate(smoothstep(1.0, 0.8, dist));
			float attenuation = attQuad;

			// fresnel
			float fsl = dot(objnrm, -cam2pix);
			fsl = smoothstep(0, 1, saturate(fsl));
			fsl = saturate(pow(fsl, _FresnelPow));
			fsl = lerp(fsl, outside, isCap);

			// dynamic 3d noise
#if NOISE
			float3 velocity = _NoiseParam.xyz;
			float nis = tex3D(_NoiseTex, frac(i.worldPos.xyz * _NoiseParam.w + (_Time.y * velocity))).r;
			nis = lerp(1, nis, _NoiseIntensity);
#else
			float nis = 1;
#endif

#if COLORBEAM
			float2 uv = i.uvSphereMapping;
			uv.x += _Time.y * _BeamMove;
			float4 bc = tex2D(_BeamColorTex, uv);
//			inten *= bc.r;
			_Color.rgb *= (bc.rgb * _BeamIntensity);
#endif

			float intensity = attenuation * fsl * fadeCloseCamera * fadeDepth * nis;

			half4 c = _Color * intensity;
			c.rgb *= _Color.a;
			c.rgb *= lerp(_AlphaInside, _AlphaOutside, outside);
			UNITY_APPLY_FOG_COLOR(i.fogCoord, c, 0);
			return c;
		}
	ENDCG

	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		ZWrite Off Blend One One

		Pass {   // INSIDE
			Cull Front

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma shader_feature NOISE
			#pragma shader_feature COLORBEAM

			half4 frag (v2f i) : SV_Target
			{
				return fragVSL(i, 0);
			}
			ENDCG
		}
		Pass {   // OUTSIDE
			Cull Back

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma shader_feature NOISE
			#pragma shader_feature COLORBEAM

			half4 frag (v2f i) : SV_Target
			{
				return fragVSL(i, 1);
			}
			ENDCG
		}
	}
	FallBack Off
}
