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

		// The camera.transform.forward (to update at runtime)
		_CameraPosition ("View Vector", Vector) = (0, 0, 1, 0)

		// Indicates the intensity of the specular shine (range from 0 to 2)
		// The value can exceed 1, because tipically the specular shine saturates the output color
		_SpecularIntensity ("Specular Intensity", Float) = 1

		// Indicates the size and smoothness of the specular shine.
		_SpecularHardness ("Specular Hardness", Float) = 25

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
			uniform float4 _CameraPosition;
			uniform float _SpecularIntensity;
			uniform float _SpecularHardness;


			// The vertex shader output (and fragment shader input)
			struct v2f {
				// the transformed vertex position			
				float4 pos : SV_POSITION;

				// the diffuse vertex color
				fixed4 diffuse : COLOR;

				// the transformed vertex normal
				fixed4 normal : TEXCOORD0;

				// the transformed position
				fixed4 position : TEXCOORD1;
			};
			
			// The vertex shader
			v2f vert (appdata_base v)
			{
				v2f o;

				// Transforms the position
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

				// Transforms the normal
				float4 normal = mul(float4(v.normal,1), _Object2World);
				o.normal = normal;

				// Transforms the position
				o.position = mul(_Object2World, float4(v.vertex));

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
				
				float3 light = normalize(_LightDirection.xyz);
				float3 normal = normalize(i.normal.xyz);
				float3 r = normalize(2 * dot(light, normal) * normal - light);
				float3 v = normalize(mul((_CameraPosition - i.position), _Object2World));

				float dotProduct = dot(r, v);
				float4 specular = _SpecularIntensity * float4(1,1,1,1) * max(pow(dotProduct, _SpecularHardness), 0) * length(i.diffuse);
				
				//return saturate(max(i.diffuse, _Color * _AmbientIntensity) + specular);
				//return saturate(max(i.diffuse, _Color * _AmbientIntensity));
				return i.normal;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}