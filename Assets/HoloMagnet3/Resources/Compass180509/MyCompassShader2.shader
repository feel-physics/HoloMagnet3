// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyCompassShader2" {
	Properties{
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ScrollX("Scroll X", float) = 0
		_ScrollY("Scroll Y", float) = 0
			// 棒磁石1の位置
			_NorthPole1Pos("North Pole1", Vector) = (0,0,0,0)
			_SouthPole1Pos("South Pole1", Vector) = (0,0,0,0)
			// 棒磁石2の位置
			_NorthPole2Pos("North Pole2", Vector) = (0,0,0,0)
			_SouthPole2Pos("South Pole2", Vector) = (0,0,0,0)
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
		float4 _NorthPole2Pos;
		float4 _SouthPole2Pos;
		float _BrightnessCoefficient;
		float _BrightnessLowerLimit;
		float _DarkDistance;
		float _HideDistance;

		void surf(Input IN, inout SurfaceOutput o)
		{
			// Define position vector of self (compass) as vecP
			float3 vecP;
			vecP = IN.worldPos;

			/* BarMagnet01 */
			// Define position vector of NORTH Pole 1 as vecN1
			float3 vecN1;
			vecN1.x = _NorthPole1Pos.x;
			vecN1.y = _NorthPole1Pos.y;
			vecN1.z = _NorthPole1Pos.z;

			// Define position vector of SOUTH Pole 1 as vecS1
			float3 vecS1;
			vecS1.x = _SouthPole1Pos.x;
			vecS1.y = _SouthPole1Pos.y;
			vecS1.z = _SouthPole1Pos.z;

			// Define displacement vector from self to bar magnet 01 as vecDisN1, vecDisS1
			float3 vecDisN1, vecDisS1;
			vecDisN1 = vecP - vecN1;
			vecDisS1 = vecP - vecS1;

			// Get magnetic force vectors from two poles of bar magnet 01 as vecF_N1, vecF_S1
			float3 vecF_N1, vecF_S1;
			vecF_N1 =        vecDisN1 / pow(length(vecDisN1), 3);
			vecF_S1 = -1.0 * vecDisS1 / pow(length(vecDisS1), 3);

			/* BarMagnet02 */
			// Define position vector of NORTH Pole 2 as vecN2
			float3 vecN2;
			vecN2.x = _NorthPole2Pos.x;
			vecN2.y = _NorthPole2Pos.y;
			vecN2.z = _NorthPole2Pos.z;

			// Define position vector of SOUTH Pole 2 as vecS2
			float3 vecS2;
			vecS2.x = _SouthPole2Pos.x;
			vecS2.y = _SouthPole2Pos.y;
			vecS2.z = _SouthPole2Pos.z;

			// Define displacement vector from self to bar magnet 02 as vecDisN2, vecDisS2
			float3 vecDisN2, vecDisS2;
			vecDisN2 = vecP - vecN2;
			vecDisS2 = vecP - vecS2;

			// Get magnetic force vectors from two poles of bar magnet 02 as vecF_N2, vecF_S2
			float3 vecF_N2, vecF_S2;
			vecF_N2 = vecDisN2 / pow(length(vecDisN2), 3);
			vecF_S2 = -1.0 * vecDisS2 / pow(length(vecDisS2), 3);

			/* Resultant Force */
			// Get resultant magnetic force vector as vecF
			float3 vecF;
			vecF = vecF_N1 + vecF_S1 + vecF_N2 + vecF_S2;

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
