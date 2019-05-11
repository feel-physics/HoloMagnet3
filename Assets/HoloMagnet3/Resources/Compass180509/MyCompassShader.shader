// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyCompassShader" {
	Properties{
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ScrollX("Scroll X", float) = 0
		_ScrollY("Scroll Y", float) = 0
		//磁石の位置　これを利用して明るさを調整する
		_CenterPos("Center", Vector) = (0,0,0,0)
		_StartDistance("Start Distance", Float) = 0.2
		_EndDistance("End Distance", Float) = 0.1

		_Emission("Emission", Color) = (1, 1, 1, 1)
	}
		SubShader{
			Tags
			{
			//"RenderType" = "Opaque"
			"RenderType" = "Transparent"
		}
		LOD 200

		CGPROGRAM

		#pragma surface surf Lambert
		#include "UnityCG.cginc"

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};
		float _ScrollX, _ScrollY;
		float4 _Emission;
		float4 _CenterPos;
		float _StartDistance;
		float _EndDistance;

		void surf(Input IN, inout SurfaceOutput o)
		{
			float3 center;
			center.x = _CenterPos.x;
			center.y = _CenterPos.y;
			center.z = _CenterPos.z;


			float3 look = center - IN.worldPos;

			float dist = length(look);

			float2 scroll = float2(_ScrollX, _ScrollY) * (-2) *_Time.y;
			//o.Albedo = tex2D(_MainTex, IN.uv_MainTex + scroll);
			fixed t = (_StartDistance - dist) / (_StartDistance - _EndDistance);
			half4 color = tex2D(_MainTex, IN.uv_MainTex + scroll) + _Emission * t;

			if (_Emission.r < 0.02) {
				discard;
			}

			o.Emission = color.rgb;// *step(0.02, _Emission.r);
		}

		ENDCG
		}
			FallBack "Diffuse"
}
