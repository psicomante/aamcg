Shader "Custom/shaderProva" {
	
	// Unity imported parameters
	Properties {
		//This color is used for both diffuse and ambient. (Specular is white)
		_Color ("Color", Color) = (1, 0, 0, 1)

		// The intensity of the ambient color (range from 0 to 1)
		_AmbientIntensity ("Ambient Intensity", Float) = 0.4

		// The intensity of the diffuse color (range from 0 to 1)
		_DiffuseIntensity ("Diffuse Intensity", Float) = 1

		// The direction of the directional light
		_LightDirection ("Light Direction", Vector) = (1, 0, 0, 0)
	}

	// The shader
	SubShader {
		// This is a single-pass shader
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			// Imported parameters matching 
			// (marked as uniform because they don't change during the entire shader computation)
			uniform fixed4 _Color;
			uniform fixed _AmbientIntensity;
			uniform float _DiffuseIntensity;
			uniform float4 _LightDirection;

			// The vertex shader output (and fragment shader input)
			struct v2f {
				// the transformed vertex position			
				float4 pos : SV_POSITION;

				// the diffuse vertex color
				fixed4 diffuse : COLOR;

				// the transformed vertex normal
				fixed4 normal : TEXCOORD0;
			};
			
			// The vertex shader
			v2f vert (appdata_base v)
			{
				v2f o;

				// Transforms the position
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

				// Transforms the normal
				float4 normal = mul(UNITY_MATRIX_IT_MV, float4(v.normal,1));
				o.normal = normal;

				// Light intensity increases when the angle between the light direction and the normal is smaller
				// Both normal and light direction (normalized) are versors, so dot product (a * b * cos(theta)) is the perfect operand
				float lightIntensity = dot(normal, normalize(_LightDirection));

				// In this case saturate is useless, because previous instructions avoid results > 1.
				// Anyway, saturate ensure that output diffuse components never exceed 1. (possible if wrong input parameters).
				o.diffuse = saturate(_Color * lightIntensity * _DiffuseIntensity);
				return o;
			}
			
			// The fragment shader
			fixed4 frag (v2f i) : COLOR0 {
				
				// Calculates Ambient + Diffuse colors 
				// (used max function instead of + to avoid saturation in case of intensity values too high)
				fixed4 ambientDiffuse = max(_AmbientIntensity * _Color, i.diffuse);
				return ambientDiffuse;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}