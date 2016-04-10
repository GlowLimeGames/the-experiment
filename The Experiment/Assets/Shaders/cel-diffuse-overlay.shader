// By Tim Volp
// Updated 02/04/15

Shader "Cel/DiffuseOverlay" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Float) = 1.4
		_OverlayTex ("Overlay (RGB)", 2D) = "white" {}
		_OverlayAmt ("Overlay amount", Range(0, 1)) = 0.1
		_OverlayColor ("Overlay Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Cel

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
		sampler2D _OverlayTex;
		half4 _Color;
		half4 _RimColor;
		half _RimPower;
		half _OverlayAmt;
		half4 _OverlayColor;

		struct Input {
			half2 uv_MainTex;
			half3 viewDir;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			// Calculate rim lighting from angle between normal and view direction
			half NdotV = saturate(dot(o.Normal, normalize(IN.viewDir)));
			// New lighting based on texture ramp
			half ramp = tex2D(_Ramp, half2(1.0 - NdotV, 0.5));

			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			half4 overlay = tex2D(_OverlayTex, IN.screenPos.xy / IN.screenPos.w);
			o.Albedo = lerp(c.rgb * _Color.rgb, overlay * _OverlayColor, _OverlayAmt);
			o.Alpha = c.a;
			o.Emission = _RimColor.rgb * pow(ramp, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}