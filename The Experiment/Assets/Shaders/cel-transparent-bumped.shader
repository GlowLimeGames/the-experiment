// By Tim Volp
// Updated 02/04/15

Shader "Cel/Transparent/Bumped Diffuse" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Float) = 1.4
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Cel alpha

		sampler2D _Ramp;

		// Custom lighting model
		inline half4 LightingCel(SurfaceOutput s, half3 lightDir, half atten) {
			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

			// Calculate lighting from angle between normal and light direction
			half NdotL = saturate(dot(s.Normal, lightDir));
			// New lighting based on texture ramp
			half3 ramp = tex2D(_Ramp, half2(NdotL, 0.5));

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb  * (atten * 2) * ramp;
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		sampler2D _BumpMap;
		half4 _Color;
		half4 _RimColor;
		half _RimPower;

		struct Input {
			half2 uv_MainTex;
			half2 uv_BumpMap;
			half3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			// Calculate rim lighting from angle between normal and view direction
			half NdotV = saturate(dot(o.Normal, normalize(IN.viewDir)));
			// New lighting based on texture ramp
			half ramp = tex2D(_Ramp, half2(1.0 - NdotV, 0.5));

			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a * _Color.a;
			o.Emission = _RimColor.rgb * pow(ramp, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}