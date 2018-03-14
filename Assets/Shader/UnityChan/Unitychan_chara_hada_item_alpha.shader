Shader "UnityChan/Skin - Item - Alpha"
{
	Properties
	{
		_MainTexAlpha("Diffuse Alpha", Range(0,1)) = 0.5

		_MainTex ("Diffuse", 2D) = "white" {}
		_FalloffSampler ("Falloff Control", 2D) = "white" {}
		_RimLightSampler ("RimLight Control", 2D) = "white" {}
	}

	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha, One One
		Tags
		{
			"Queue"="Geometry+1" // Transparent+1"
			"IgnoreProjector"="True"
			"RenderType"="Overlay"
			"LightMode"="ForwardBase"
		}

		Pass
		{
			Cull Back
			ZTest LEqual
CGPROGRAM
#pragma multi_compile_fwdbase
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "CharaSkinItem.cg"
ENDCG
		}

	}

	FallBack "Transparent/Cutout/Diffuse"
}
