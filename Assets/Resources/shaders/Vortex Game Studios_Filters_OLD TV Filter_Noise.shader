Shader "Vortex Game Studios/Filters/OLD TV Filter/Noise" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PatternTex ("Pattern (RGB)", 2D) = "white" {}
		_Magnitude ("Noise Magnitude", Range(-1, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}