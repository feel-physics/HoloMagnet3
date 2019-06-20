// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyCompassShader2" {
	Properties{
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ScrollX("Scroll X", float) = 0
		_ScrollY("Scroll Y", float) = 0
			// 磁石の位置
			_NorthPole1Pos("North Pole", Vector) = (0,0,0,0)
			_SouthPole1Pos("South Pole", Vector) = (0,0,0,0)
			// 方位磁針の明るさの係数
			_BrightnessCoefficient("Brightness Coefficient", Float) = 0.005
			// 磁石を消し込むパラメータ
			_BrightnessLowerLimit("Brightness Lower Limit", Float) = 0.04

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
		float4 _NorthPole1Pos;
		float4 _SouthPole1Pos;
		float _BrightnessCoefficient;
		float _BrightnessLowerLimit;
		float _DarkDistance;
		float _HideDistance;

		void surf(Input IN, inout SurfaceOutput o)
		{
			// Define position vector of self (compass) as vecP
			float3 vecP;
			vecP = IN.worldPos;

			// Define position vector of NORTH Pole as vecN
			float3 vecN;
			vecN.x = _NorthPole1Pos.x;
			vecN.y = _NorthPole1Pos.y;
			vecN.z = _NorthPole1Pos.z;

			// Define position vector of SOUTH Pole as vecS
			float3 vecS;
			vecS.x = _SouthPole1Pos.x;
			vecS.y = _SouthPole1Pos.y;
			vecS.z = _SouthPole1Pos.z;

			// Define displacement vector from self to bar magnet as vecDisN, vecDisS
			float3 vecDisN, vecDisS;
			vecDisN = vecP - vecN;
			vecDisS = vecP - vecS;

			// Get magnetic force vectors from two poles as vecF_N, vecF_S
			float3 vecF_N, vecF_S;
			vecF_N =        vecDisN / pow(length(vecDisN), 3);
			vecF_S = -1.0 * vecDisS / pow(length(vecDisS), 3);

			// Get resultant magnetic force vector as vecF
			float3 vecF;
			vecF = vecF_N + vecF_S;

			// 方位磁針の明るさを求める
			float brightness;
			brightness = _BrightnessCoefficient * length(vecF);

			if (brightness < _BrightnessLowerLimit) {
				discard;
			}

			//ストライプ模様を時間でスクロールさせる
			float2 scroll = float2(_ScrollX, _ScrollY) * (-2) *_Time.y;

			half4 color = 
				tex2D(_MainTex, IN.uv_MainTex + scroll) + 
				_Emission * brightness;
			o.Emission = color;
		}

		ENDCG
		}
			FallBack "Diffuse"
}
