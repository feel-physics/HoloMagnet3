// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyDirectionShader" {
	Properties {
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ScrollX("Scroll X", float) = 0
        _ScrollY("Scroll Y", float) = 0

		_Emission("Emission", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags
		{
			"RenderType" = "Opaque"
		}
		LOD 200

		CGPROGRAM

		#pragma surface surf Lambert
		#include "UnityCG.cginc"

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};
		float _ScrollX, _ScrollY;
		float4 _Emission;

		void surf( Input IN, inout SurfaceOutput o) 
		{
			float2 scroll = float2(_ScrollX, _ScrollY) * (-2) *_Time.y;
            //o.Albedo = tex2D(_MainTex, IN.uv_MainTex + scroll);

            half4 color = tex2D(_MainTex, IN.uv_MainTex + scroll) + _Emission;
            o.Emission = color.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
