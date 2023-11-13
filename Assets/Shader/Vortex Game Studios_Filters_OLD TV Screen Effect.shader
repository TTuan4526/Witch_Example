Shader "Vortex Game Studios/Filters/OLD TV Screen Effect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Saturation ("Saturation", Range(0, 1)) = 0
		_ChromaticAberrationTex ("Chromatic Aberration (RGB)", 2D) = "black" {}
		_ChromaticAberrationMagnetude ("Chromatic Aberration Magnetude", Range(-1, 1)) = 0
		_NoiseTex ("Noise (RGB)", 2D) = "black" {}
		_NoiseOffsetX ("Noise OffsetX", Float) = 0
		_NoiseOffsetY ("Noise OffsetY", Float) = 0
		_NoiseMagnetude ("Noise Magnetude", Range(-1, 1)) = 0.5
		_StaticTex ("Static (RGB)", 2D) = "black" {}
		_StaticMask ("Static Mask (RGB)", 2D) = "white" {}
		_StaticMagnetude ("Static Magnetude", Range(-1, 1)) = 0.5
		_StaticVertical ("Static Vertical", Float) = 0.5
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