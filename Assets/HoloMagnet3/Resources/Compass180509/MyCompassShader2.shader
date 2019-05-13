// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyCompassShader2" {
	Properties{
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ScrollX("Scroll X", float) = 0
		_ScrollY("Scroll Y", float) = 0
			//磁石の位置　これを利用して明るさを調整する
			_NorthPolePos("North Pole", Vector) = (0,0,0,0)
			_SouthPolePos("South Pole", Vector) = (0,0,0,0)
			_DarkDistance("Distance to Dark", Float) = 0.2
			_HideDistance("Distance to Hide", Float) = 0.1

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
		float _DarkDistance;
		float _HideDistance;

		void surf(Input IN, inout SurfaceOutput o)
		{
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

			/*
			//磁石の位置を取得
			float3 center;
			center.x = _NorthPolePos.x;
			center.y = _NorthPolePos.y;
			center.z = _NorthPolePos.z;
			*/

			//コンパスと磁石のベクトルを取得
			//IN.worldPos　が、現在のコンパスの座標
			float3 look = IN.worldPos - vecS;

			//距離を計算
			float dist = length(look);

			//ストライプ模様を時間でスクロールさせる
			float2 scroll = float2(_ScrollX, _ScrollY) * (-2) *_Time.y;

			//色の減衰量を計算
			float t = saturate((_HideDistance - dist) / _HideDistance);// (_HideDistance - dist) / (_HideDistance - _DarkDistance);


			half4 color = tex2D(_MainTex, IN.uv_MainTex + scroll) + _Emission * t;
			//if (_HideDistance < dist) {
			if (t < _DarkDistance) {
				discard;
			}

			o.Emission = color;// *step(0.02, _Emission.r);
			//o.Emission.a = t.x;
		}

		ENDCG
		}
			FallBack "Diffuse"
}
