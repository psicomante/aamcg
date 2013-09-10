Shader "Custom/shaderPhong" {
	
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
		_CameraDirection ("View Vector", Vector) = (0, 0, 1, 0)

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
			uniform float4 _CameraDirection;
			uniform float _SpecularIntensity;
			uniform float _SpecularHardness;


			// The vertex shader output (and fragment shader input)
			struct v2f {
				// the transformed vertex position			
				float4 pos : SV_POSITION;

				// the diffuse vertex color
				fixed4 ambient : COLOR;

				// the transformed vertex normal
				fixed4 normal : TEXCOORD0;
			};
			
			// The vertex shader
			v2f vert (appdata_base v)
			{
				v2f o;

				// Transforms the position, for rasterization
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				// Transforms the physics normal
				o.normal = mul(float4(v.normal,0), _World2Object);
				
				// Set ambient color on vertex
				o.ambient = _Color * _AmbientIntensity;
				
				return o;
			}
			
			// The fragment shader
			fixed4 frag (v2f i) : COLOR0 {
				// ligh/view norm vector
				float4 lightDirection = -normalize(_LightDirection);
				float4 view = normalize(_CameraDirection);


				fixed4 diffuse = max(0,dot(i.normal, lightDirection)) * _Color * _DiffuseIntensity;
				 
				float4 reflection = lightDirection - 2 * dot(lightDirection, i.normal) * i.normal;
				//float4 reflection = normalize(lightDirection + view);
				//float4 reflection = reflect(lightDirection, i.normal);
				float refDotView = max(dot(reflection, view), 0);
				fixed4 specular = _SpecularIntensity * pow(refDotView, _SpecularHardness);
				
				return saturate(diffuse + i.ambient + specular);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}