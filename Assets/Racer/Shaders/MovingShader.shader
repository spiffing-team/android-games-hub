Shader "Custom/Road Shader"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "grey" {}
		[PerRendererData]_offsetY("offsetY", float) = 0
	}

	SubShader
	{
		CGPROGRAM

		#pragma surface surf Lambert

		sampler2D _MainTex;

		float _offsetY;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(in Input IN, inout SurfaceOutput OUT)
		{
			IN.uv_MainTex.y += _offsetY;

			OUT.Emission = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG
	}

	Fallback "Diffuse"
}