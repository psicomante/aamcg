Shader "Custom/shaderProva" {
	Properties {
		
	}
	SubShader {
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
			  float4 pos : SV_POSITION;
			  fixed4 color : COLOR;
			};
			
			v2f vert (appdata_base v)
			{
			  v2f o;
			  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			  o.color.xyz = v.texcoord;
			  o.color.w = 1;
			  return o;
			}
			
			fixed4 frag (v2f i) : COLOR0 { return normalize(i.color); }
			ENDCG
		}
	}
	FallBack "Diffuse"
}