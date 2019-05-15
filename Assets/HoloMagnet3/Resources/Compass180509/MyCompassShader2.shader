// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyCompassShader2" {
	Properties{
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ScrollX("Scroll X", float) = 0
		_ScrollY("Scroll Y", float) = 0
			// 磁石の位置
			_NorthPolePos("North Pole", Vector) = (0,0,0,0)
			_SouthPolePos("South Pole", Vector) = (0,0,0,0)
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
		float4 _NorthPolePos;
		float4 _SouthPolePos;
		float _BrightnessCoefficient;
		float _BrightnessLowerLimit;
		float _DarkDistance;
		float _HideDistance;

		void surf(Input IN, inout SurfaceOutput o)
		{
			// 自身（方位磁針）の位置ベクトルvecPを作成
			float3 vecP;
			vecP = IN.worldPos;

			// N極の位置ベクトルvecNを作成
			float3 vecN;
			vecN.x = _NorthPolePos.x;
			vecN.y = _NorthPolePos.y;
			vecN.z = _NorthPolePos.z;

			// S極の位置ベクトルvecNを作成
			float3 vecS;
			vecS.x = _SouthPolePos.x;
			vecS.y = _SouthPolePos.y;
			vecS.z = _SouthPolePos.z;

			// 自身から棒磁石に対する変位ベクトルvecDisN、vecDisSを作成
			float3 vecDisN, vecDisS;
			vecDisN = vecP - vecN;
			vecDisS = vecP - vecS;

			// 極からの磁力ベクトルvecF_N, vecF_Sを求める
			float3 vecF_N, vecF_S;
			vecF_N =        vecDisN / pow(length(vecDisN), 3);
			vecF_S = -1.0 * vecDisS / pow(length(vecDisS), 3);

			// 磁力の合力ベクトルvecFを求める
			float3 vecF;
			vecF = vecF_N + vecF_S;

			// 方位磁針の明るさを求める
			float brightness;
			brightness = _BrightnessCoefficient * length(vecF);

			// 暗すぎる方位磁針は消す
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
