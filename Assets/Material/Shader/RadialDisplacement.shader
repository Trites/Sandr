Shader "Sandr/RadialDisplacement"
{
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend Zero SrcAlpha //, SrcAlpha DstFactor

		// Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_MainTex"
        }
		
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
							
					o.pos.x = o.pos.x + 0.1;
									
					return o;
				}
				
				//fragment function
				float4 frag(vertexOutput i) : COLOR{
					
					//fixed4 col = text2D(_MainTex, i.uv);
					half4 bgcolor = tex2Dproj(_MainTex, i.grabPos);
					return float4(1.0, 1.0, 1.0, 0.5);
				}
				
			ENDCG
		}
	}
	
	//Fallback "Diffuse"
}
