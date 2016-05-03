Shader "Sandr/LaserBeamShader"
{
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha//, SrcAlpha DstFactor
		
		Pass{
			CGPROGRAM
				//pragmas
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
				
				//user defined variables
				uniform float4 _Color;
				
				float4 _MainTex_ST;
				
				//base input structs
				struct vertexInput{
					
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};
				
				struct vertexOutput{
					
					float4 pos : SV_POSITION;
					float4 col : COLOR;
					float2 uv : TEXCOORD0;
				};
				
				//vertex function
				vertexOutput vert(vertexInput v){
					vertexOutput o;			
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
									
					return o;
				}
				
				//fragment function
				float4 frag(vertexOutput i) : COLOR{
					
					float y = i.uv.y;
					float a = sin(2*y*3.1415 - 0.5* 3.1415);
					
					return float4((8 - 16*y)*(8 - 16*y), 0.2, 0.2, a);
				}
				
			ENDCG
		}
	}
	
	//Fallback "Diffuse"
}
