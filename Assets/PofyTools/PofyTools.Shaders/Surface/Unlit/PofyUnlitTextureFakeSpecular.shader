﻿Shader "PofyTools/Surface/Unlit/Texture Fake Specular" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _DotProduct("Spec Rolloff", Range(20,200)) = 20
    //_RimColor ("Rim Color", Color) = (0,0,0,0)
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM

	#pragma surface surf NoLighting noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
    fixed4 _Color;
    float _DotProduct;
    //fixed4 _RimColor;

    struct Input {
      float2 uv_MainTex;
      float3 worldNormal;
      float3 viewDir;
    };

             fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
     {
         fixed4 c;
         c.rgb = s.Albedo; 
//         c.a = s.Alpha;
         return c;
     }

    
    void surf (Input IN, inout SurfaceOutput o) {
    	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = _Color.rgb * c;

      	float ratio = (_DotProduct-20)/180;
      	float border = 1-(abs(dot(IN.viewDir,IN.worldNormal)));

      	float blackBorder = clamp(border * ratio,0,1);
      	float blackBorder2 = 1-blackBorder;

      	float spot = clamp(10*border * (ratio),0,1);
      	float spot2 = 1-(spot*spot);

      	fixed4 highlight = clamp(((fixed4(1,1,1,1) *c.a)*spot2),0,1);
      	highlight *= (ratio);
      o.Albedo = clamp(highlight + o.Albedo*(blackBorder2),0,1); 
    }

    ENDCG
  } 
  FallBack "Unlit"
}