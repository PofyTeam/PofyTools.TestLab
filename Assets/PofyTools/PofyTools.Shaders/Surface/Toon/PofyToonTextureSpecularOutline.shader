Shader "PofyTools/Surface/Toon/Texture Specular Outline" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _DotProduct("Outline Size", Range(0,10)) = 1

	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Range (0.0, 1)) = .005

	_RampTex ("Ramp", 2D) = "white"{}
  }

  CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	fixed4 vertex : POSITION;
	half3 normal : NORMAL;
};
 
struct v2f {
	fixed4 pos : POSITION;
	half4 color : COLOR;
};
 
fixed _Outline;
fixed4 _OutlineColor;
 
v2f vert(appdata v) {
	// just make a copy of incoming vertex data but scaled according to normal direction
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
	half3 norm   = normalize(mul ((half3x3)UNITY_MATRIX_IT_MV, v.normal));
	half2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Outline;
	o.color = _OutlineColor;
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
		//Blend One OneMinusDstColor // Soft Additive


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		 
		half4 frag(v2f i) :COLOR {
			return i.color;
		}
		ENDCG
	}

    CGPROGRAM

	#pragma surface surf Toon noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
    half4 _Color;
    half _DotProduct;
//    fixed4 _RimColor;
	sampler2D _RampTex;

//	uniform float _Outline;
//uniform float4 _OutlineColor;

    struct Input {
      half2 uv_MainTex;
//      float3 worldNormal;
//      float3 screenPos;
//      float3 viewDir;
//      float3 color;
    };

//      void vert (inout appdata_full v, out Input o) {
//      	UNITY_INITIALIZE_OUTPUT(Input,o);
//
//		o.screenPos = mul(UNITY_MATRIX_MVP, v.vertex);
// 
//		float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
//		float2 offsetT = TransformViewToProjection(norm.xy);
// 
//		o.screenPos.xy += offsetT * o.screenPos.z * _Outline;
//		o.color = _OutlineColor;
//
//      }


      

	 half4 LightingToon (SurfaceOutput s, half3 lightDir,half3 viewDir,half atten)
	{
		half NdotL = dot(s.Normal, lightDir); 
		NdotL = tex2D(_RampTex, half2(NdotL, 0.5));

//		float border = 1-(abs(dot(viewDir,s.Normal)));
//      	half alpha = _DotProduct * border;
//      	alpha = floor(alpha*alpha*alpha * 2) / (1.5); // Snap

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten * s.Alpha ;// * (1-alpha);
		//s.Emission = 0;
		c.a = s.Alpha;

		return c;
	}
    
    void surf (Input IN, inout SurfaceOutput o) {
    	half4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = _Color.rgb * c;
      	o.Alpha = _Color.a;
    }

    ENDCG
  }


  FallBack "Unlit"
}