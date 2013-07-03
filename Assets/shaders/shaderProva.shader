Shader "Custom/shaderProva" {
	Properties {
		_Texture ("My Texture", 2D) = "white" { }
		_Color ("Color", Color) = (1,0,0,0)
		_AmbientIntensity ("Ambient Intensity", Float) = 0.2
	}
	SubShader {
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform fixed4 _Color;
			uniform fixed _AmbientIntensity;

			struct v2f {
			  float4 pos : SV_POSITION;
			  fixed4 ambient : COLOR0;
			  fixed4 diffuse : COLOR1;
			};
			
			v2f vert (appdata_base v)
			{
			  v2f o;
			  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			  o.ambient.xyz = fixed4(_Color.xyz * _AmbientIntensity, 1);
			  o.diffuse.xyz = fixed4(_Color.xyz, 1);
			  return o;
			}
			
			fixed4 frag (v2f i) : COLOR0 {
				return i.ambient;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}