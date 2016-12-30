
Shader "PofyTools/Fragment/Toon/Advanced" {
  Properties {
    _Color ("Color (A - Ambience)", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
	_RampTex ("Ramp", 2D) = "white"{}
	 _Blend ("Blend (0-Texture,1-Color)", Range(0,1)) = 1
	 _ShadeTex ("Shade",2D) = "white"{}
  }

   CGINCLUDE
#include "UnityCG.cginc"

struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	//float4 worldPos : TEXCOORD0;
	float4 color : COLOR;
};
 
v2f vert(appdata v) {
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 	float4 worldPos = mul(unity_ObjectToWorld,v.vertex);
	half3 norm   = normalize(mul ((half3x3)UNITY_MATRIX_IT_MV, v.normal));
	half2 offset = TransformViewToProjection(norm.xy);

	float _Distance = distance(worldPos,_WorldSpaceCameraPos);
	o.pos.xy += offset * o.pos.z * 0.01 / _Distance;
	o.color = 0;
	return o;
}
ENDCG

  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

	Pass {
		Name "OUTLINE"
		Tags { "LightMode" = "Always" }
		Cull Front


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		 
		half4 frag(v2f i) :COLOR {
			return i.color;
		}
		ENDCG
	}

	Pass {
		Tags{"LightMode" = "ForwardBase"}

		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		//user defined variables
		uniform sampler2D _MainTex;
		uniform sampler2D _RampTex;
		uniform sampler2D _ShadeTex;

		uniform float4 _MainTex_ST;
		uniform float4 _RampTex_ST;
		uniform float4 _ShadeTex_ST;

		uniform float4 _Color;
		uniform float _Blend;

//		unity defined variables
		uniform float4 _LightColor0;

		//base input structs
		struct vertexInput{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 uv : TEXCOORD0;
		};

		struct vertexOutput{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;

			float4 posWorld : TEXCOORD1;
			float3 normalDir : TEXCOORD2;
			float4 screenPos : TEXCOORD3;

//			float distance : FLOAT;
		};

		vertexOutput vert(vertexInput v){
			vertexOutput o;
			//Basic
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);

			//World / Normal
			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.normalDir = normalize( mul(float4(v.normal,0.0), unity_WorldToObject).xyz);

			// Screen
			o.screenPos = o.pos;
			o.screenPos.xy = 0.5f * (o.screenPos.xy / o.pos.w + 1.0);

			//Distance ratio (near/far range)
//			o.distance = saturate(sqrt((distance(o.posWorld,_WorldSpaceCameraPos) - _ProjectionParams.y) / (_ProjectionParams.z*0.5f - _ProjectionParams.y)));

			return o;
		}

		float4 frag (vertexOutput i):SV_Target{

			//surf
			float4 c = tex2D(_MainTex,i.uv);

			//blend calculated from property and distance from camera
			float distanceRatio = saturate(sqrt((distance(i.posWorld,_WorldSpaceCameraPos) - _ProjectionParams.y) / (_ProjectionParams.z*0.5f - _ProjectionParams.y)));

			//light direction
			float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
			float diffuseRamp = dot(i.normalDir, lightDir);

			//view direction
			float3 viewDirection = normalize(i.posWorld.xyz - _WorldSpaceCameraPos.xyz);
			float softFacingRatio = abs ( dot ( viewDirection,i.normalDir) );

			//override mapped_diffuseRamp using only red channel for mapping
			float mapped_diffuseRamp = tex2D(_RampTex, float2(saturate (diffuseRamp+c.a*(1-distanceRatio)*0.3), 0)).r;

			//override facing ratio using only green channel for mapping
			float facingRatio = tex2D(_RampTex,float2(softFacingRatio,0)).g;

			//hard distance for shade
			float mapped_distanceRatio = tex2D(_RampTex,float2(distanceRatio,0)).b;

			//blend reminder is scaled by distance ratio
			float blendedDistance = _Blend + (1 - _Blend) * distanceRatio;
			//final blend
			float finalBlend = (1-facingRatio) + facingRatio*blendedDistance;

			//shade uv
			float4 uv = i.screenPos;
			uv.xy *= 8000 * mapped_diffuseRamp * mapped_distanceRatio;
//			uv *= mul(_WorldSpaceCameraPos,uv);
//			uv *= mapped_diffuseRamp;

			float4 shade = tex2D(_ShadeTex,uv.xy); 
			float shadeMask = saturate	(1 - (facingRatio * (1-diffuseRamp) * (finalBlend)));
//			float finalShade = shade.r * shadeMask;

			//final albedo
			float3 albedo = lerp(c,_Color.rgb,finalBlend) * _LightColor0.rgb;

//			float3 specularReflection =c.a *_LightColor0.rgb * saturate(dot(i.normalDir,lightDir)) * pow(saturate(dot(reflect(lightDir,i.normalDir),viewDirection)),5);
//			specularReflection *= mapped_diffuseRamp;
			float finalShade = saturate(mapped_diffuseRamp * (1 + _Color.a)) * saturate(shade.r + diffuseRamp + shadeMask + shade.g + _Color.a);
			c.rgb = albedo * finalShade;// + UNITY_LIGHTMODEL_AMBIENT*0.2;// + specularReflection;
			//c.rgb = albedo * // + UNITY_LIGHTMODEL_AMBIENT;
			c.a = _Color.a;
			return c;
		}
		 ENDCG
	}
  	
  }
   
  FallBack "Unlit"
}