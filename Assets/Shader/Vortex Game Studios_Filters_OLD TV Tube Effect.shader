Shader "Vortex Game Studios/Filters/OLD TV Tube Effect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ScreenHorizontal ("Screen Horizontal Orientation", Float) = 1
		_ScanLine ("ScanLine (RGB)", 2D) = "white" {}
		_ScanLineCount ("ScanLine Count (Resolution Size)", Float) = 100
		_ScanLineMin ("ScanLine Min", Float) = 0
		_ScanLineMax ("ScanLine Max", Float) = 1
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
		_ReflexTex ("Reflex (RGB)", 2D) = "black" {}
		_ReflexMagnetude ("Reflex Magnetude", Range(0, 1)) = 0.5
		_Distortion ("Distortion", Range(-1, 1)) = 0.1
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