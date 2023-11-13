Shader "Vortex Game Studios/Materials/OLD TV Filter/Standard" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
		_ScreenWidth ("Screen Width", Float) = 320
		_ScreenHeight ("Screen Height", Float) = 240
		_Brightness ("Brightness", Range(-1, 1)) = 0
		_Contrast ("Contrast", Range(-1, 1)) = 0
		_Saturation ("Saturation", Range(0, 1)) = 0
		_CompositeDistortion ("Composite Distortion", Range(0, 1)) = 0.125
		_NoiseTex ("Noise (RGB)", 2D) = "white" {}
		_NoiseMagnitude ("Noise Magniture", Range(-1, 1)) = 0.25
		_StaticMaskTex ("Static Mask (RGB)", 2D) = "black" {}
		_StaticTex ("Static (RGB)", 2D) = "black" {}
		_StaticDistortionMagnitude ("Static Distortion Magniture", Range(0, 1)) = 0.16
		_StaticDirtMagnitude ("Static Dirt Magniture", Range(0, 1)) = 0.64
		_ScanlineTex ("Scanline (RGB)", 2D) = "white" {}
		_ScanlineMagnitude ("Scanline Magniture", Range(0, 1)) = 0.75
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}