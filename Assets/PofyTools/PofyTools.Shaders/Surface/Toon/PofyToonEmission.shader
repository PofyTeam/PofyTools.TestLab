// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PofyTools/Toon/Emission" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmissionTex("Emission (RGB)",2D) = "black"{}

		_SpecularPower("Specular Power", Range(0,20)) = 1

		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 1.0)) = .005

		_RampTex ("Ramp", 2D) = "white"{}

		_Blend ("Blend (0-Color,1-Texture)", Range(0,1)) = 1
	}

  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

	Pass {
		Name "OUTLINE"
		Tags { "LightMode" = "Always"}
		Cull Front
		ZWrite Off
		CGPROGRAM
		#pragma vertex VertexOutline
		#pragma fragment FragmentOutline
		#pragma target 3.0
		 
		#include "UnityCG.cginc"

		fixed _Outline;
		fixed4 _OutlineColor;

		struct VertexInput {
			float4 vertex : POSITION;
			half3 normal : NORMAL;
		};

		struct VertexOutput {
			float4 pos : SV_Position;
			half4 color : SV_Target;
		};

		VertexOutput VertexOutline(VertexInput v) {
			VertexOutput o;
			o.pos = UnityObjectToClipPos(v.vertex);

			//half3 normal = normalize(mul ((half3x3)UNITY_MATRIX_IT_MV, v.normal));
			half3 normal = UnityObjectToWorldNormal(v.normal);

			half2 offset = TransformViewToProjection(normal.xy);

			//o.pos.xy += offset * o.pos.z * _Outline;
			o.pos.xy += offset *_Outline;
			o.pos.z = 0;
			o.color = _OutlineColor;
			return o;
		}

		fixed4 FragmentOutline(VertexOutput o) :SV_Target {
			return o.color;
		}
		ENDCG
	}

	Pass {
		Name "FORWARD"
		Tags { "RenderType" = "Opaque"
				"LightMode" = "ForwardBase"
		 }
		Cull Back
		
		CGPROGRAM
		#pragma vertex VertexBase
		#pragma fragment FragmentBase
		#pragma target 3.0
		#include "UnityStandardBRDF.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _EmissionTex;
		fixed _Outline;
		half4 _Color;
		half _SpecularPower;
		
		half _Blend;
		sampler2D _RampTex;

		struct VertexInput {
			fixed4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			half3 normal : NORMAL;
		};

		struct VertexOutput {
			fixed4 pos : POSITION;
			float2 uv : TEXCOORD0;
			half3 normal : NORMAL;
		};

		VertexOutput VertexBase(VertexInput i){
			VertexOutput o;
			o.pos = UnityObjectToClipPos(i.vertex);
			//o.color.rgb = i.normal;
			o.uv = TRANSFORM_TEX(i.uv,_MainTex);

			// half3 normal = normalize(mul ((half3x3)UNITY_MATRIX_IT_MV, i.normal));
			// half3 normal = UnityObjectToWorldNormal(v.normal);

			// half2 offset = TransformViewToProjection(normal);

			//o.pos.xy += -offset *  /*o.pos.z **/ _Outline;

			o.normal = i.normal;
			return o;
		}
//				fixed4 FragmentBase(VertexOutput o) : SV_Target
//		{
//			// fixed4 color = saturate(tex2D(_MainTex,o.uv) + tex2D(_EmissionTex,o.uv));
//			fixed4 color = LightingToon(o); 
//			return color;
//		}
//
		half4 FragmentBase (VertexOutput o):SV_Target
		{
			fixed4 color = saturate(tex2D(_MainTex,o.uv) + tex2D(_EmissionTex,o.uv));
			float3 lightDir = _WorldSpaceLightPos0.xyz;
			half NdotL = dot(normalize(o.normal), lightDir); 
			NdotL = tex2D(_RampTex, half2(NdotL, 0.5));

			//half4 c;
			// s.Alpha =s.Alpha 
			 color.rgb = (color.rgb * _LightColor0.rgb * NdotL*10);// * color.a;// * (1-alpha);
			//s.Emission = 0;
			//color.a*= 1+_SpecularPower;;
			color.rgb = lerp(_Color.rgb,color,_Blend);
			return color;
		}
    
//   		void surf (Input IN, inout SurfaceOutput o) 
//		{
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = lerp(_Color.rgb,c,_Blend);
//			o.Alpha = _Color.a;
//   		}

    ENDCG
  }
  }
  FallBack "Unlit"
}