// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyRotateShader" {
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

		#pragma surface surf Lambert vertex:vertex_program
		#include "UnityCG.cginc"

		sampler2D _MainTex;

		float4x4 rotate(float3 r, float4 d)  // r=rotations axes
		{
			float cx, cy, cz, sx, sy, sz;
			sincos(r.x, sx, cx);
			sincos(r.y, sy, cy);
			sincos(r.z, sz, cz);
			return float4x4(
				cy*cz,      -cy*sz,     sy, d.x,
            	cx*sz+cz*sx*sy, cx*cz-sx*sy*sz, -cy*sx, d.y,
            	sx*sz-cx*cz*sy, cx*sy*sz+cz*sx, cx*cy,  d.z,
            	0,              0,      0,  1
			);
		}

		void vertex_program(inout appdata_full v) 
		{
			float4x4 rot_mat;

			rot_mat = rotate(float3(0, _Time.y/3, _Time.x), float4(0, 0, 0, 0.1));

			float4 a = v.vertex;
			float4 b = mul(rot_mat, a);
			v.vertex = b;
		}

		struct Input 
		{
			float2 uv_MainTex;
		};
		float _ScrollX, _ScrollY;
		float4 _Emission;

		void surf( Input IN, inout SurfaceOutput o) 
		{
			float2 scroll = float2(_ScrollX, _ScrollY) * _Time.y;
            //o.Albedo = tex2D(_MainTex, IN.uv_MainTex + scroll);

            half4 color = tex2D(_MainTex, IN.uv_MainTex + scroll) + _Emission;
            o.Emission = color.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
