// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader"PofyTools/Fragment/Unlit/Texture Fake Specular Rim"{
	Properties{
		_Color("Tint",Color) =(1.0,1.0,1.0,1.0)
		_MainTex("Albedo (RGB)",2D) = "white"{}

		_SpecColor("Specular Color",Color) = (1.0,1.0,1.0,1.0)
//		_SpecPower("Specular Power", Range(0.0,1.0)) = 0.5
		_SpecPower("Specular Power", Range(0.1,20)) = 3.0
		_RimColor ("RimColor",Color) = (1.0,1.0,1.0,1.0)
		_RimPower("Rim Power", Range(0,1)) = 0
		}

	SubShader{

		Pass{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			//user defined variables
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

			uniform float4 _Color;
			uniform float4 _SpecColor;
			uniform float4 _RimColor;

			uniform float _SpecPower;
			uniform float _RimPower;

//			//unity defined variables
//			uniform float4 _LightColor0;
//
			//base input structs
			struct vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct vertexOutput{
				float4 pos : SV_POSITION;
				float3 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float2 uv : TEXCOORD0;
			};

			//vertex function
			vertexOutput vert(vertexInput v){
				vertexOutput o;

				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize( mul(float4(v.normal,0.0), unity_WorldToObject).xyz);

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			//fragment function

			float4 frag(vertexOutput i) : SV_Target{

				//sample color from texture
				float4 c = tex2D(_MainTex,i.uv) * float4(_Color.rgb,1);

				//calculate view direction (vector AB = vector B - vector A)
				float3 viewDirection = normalize(i.posWorld.xyz - _WorldSpaceCameraPos.xyz);

				//facing ratio returns 0 if normal is parallel to view direction (0 degree) and 1 if perpendicular (90 degree)
				float facingRatio = 1-(abs(dot(viewDirection,i.normalDir)));

				float specRatio = _SpecPower/20;

		      	//specular
		      	float specMask =  specRatio * saturate(1 - pow( facingRatio * (_SpecPower*1), _SpecPower));
		      	float4 highlight = c.a * (specMask  * _SpecColor);

				//the more glossy the surface the less light is coming from perpendicular 
		      	float blackBorder = 1-saturate(facingRatio * specRatio);

				//rim lighting
		      	float rimMask = pow(saturate(facingRatio * (_RimPower*2) + ((_RimPower-1)*(1-_RimColor.a))),1-facingRatio);
		      	float4 rimColor = (_RimColor+rimMask) * rimMask;

		      	c = (  c * blackBorder + highlight + rimColor + UNITY_LIGHTMODEL_AMBIENT); 

//				//lighting
////				float3 finalLight = atten * _LightColor0.rgb;
//				float3 diffuseReflection = saturate(dot(i.normalDir,viewDirection)) * tex2D(_MainTex,i.uv).rgb;
//				float3 specularReflection = saturate(dot(i.normalDir,viewDirection)) * pow(saturate(dot(reflect(-viewDirection,i.normalDir),viewDirection)),_SpecPower) * _SpecColor;
//
//				//rim lighting
//				float rimMask = 1-saturate(dot(viewDirection,i.normalDir));
//				float3 rimLighting = _RimColor * saturate(dot(i.normalDir,viewDirection) * pow(rimMask,_RimPower)) * _RimColor;
//
//				c = float4(diffuseReflection + specularReflection + rimLighting + UNITY_LIGHTMODEL_AMBIENT.rgb,1);
//				c = c* _Color;
////				return float4(c,1.0);
				return c;
			}

			ENDCG
		}
	}
	//Fallback "Specular"
}