// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PofyTools/Surface/Toon/Texture Blend Specular Outline" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _SpecularPower("Specular Power", Range(0,20)) = 1

	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Range (0.0, 0.03)) = .005

	_RampTex ("Ramp", 2D) = "white"{}

	 _Blend ("Blend (0-Color,1-Texture)", Range(0,1)) = 1
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
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
 
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
	#pragma debug
    #pragma target 3.0

    sampler2D _MainTex;
    half4 _Color;
    half _SpecularPower;
     half _Blend;
	sampler2D _RampTex;


    struct Input {
      half2 uv_MainTex;
    };

	 half4 LightingToon (SurfaceOutput s, half3 lightDir,half3 viewDir,half atten)
	{
		half NdotL = dot(s.Normal, lightDir); 
		NdotL = tex2D(_RampTex, half2(NdotL, 0.5));

		half4 c;
		s.Alpha =s.Alpha * 1+_SpecularPower;
		c.rgb = (s.Albedo * _LightColor0.rgb * NdotL * atten) * s.Alpha;// * (1-alpha);
		//s.Emission = 0;
		c.a = s.Alpha;

		return c;
	}
    
    void surf (Input IN, inout SurfaceOutput o) {
    	half4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = lerp(_Color.rgb,c,_Blend);
      	o.Alpha = _Color.a;
    }

    ENDCG
  }


  FallBack "Unlit"
}