Shader "Custom/background" {

	// The shader
	SubShader {
		// This is a single-pass shader
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			

			// The vertex shader output (and fragment shader input)
			struct v2f {
				// the transformed vertex position			
				float4 pos : SV_POSITION;

				float4 pos2 : TEXCOORD0;
			};
			
			// The vertex shader
			v2f vert (appdata_base v)
			{
				v2f o;

				// Transforms the position, for rasterization
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.pos2 = mul(_World2Object, float4(v.vertex));
				
				return o;
			}
			
			// The fragment shader
			fixed4 frag (v2f i) : COLOR0 {
				float4 outcolor;
				outcolor = abs(cos(i.pos2) * tan(i.pos2));

				return outcolor;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}