Shader "Custom/MyDirectionShaderFor3D" {
    Properties {
        _Taxture( "Texture", 2D ) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert
        struct Input {
            float2 uv_Texture;
            float customDataRed;
            float customDataBlue;
        };
        sampler2D _Taxture;

        void myvert( inout appdata_full v, out Input data ) {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            data.customDataRed = abs(sin(v.vertex.y * 1000 + _Time.x * 50));
            data.customDataBlue = abs(cos(v.vertex.y * 1000 + _Time.x * 50));
        }

        void mycolor(Input IN, SurfaceOutput o, inout fixed4 color) {
            color = float4(1.0, 0.0, 0.0, 1.0) * IN.customDataRed
            	+ float4(0.0, 0.0, 1.0, 1.0) * IN.customDataBlue;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_Taxture, IN.uv_Texture).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}