﻿Shader "PofyTools/Standard/Texture Rim" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _DotProduct("Rim Effect", Range(-1,1)) = 0.25
    _RimColor ("Rim Color", Color) = (0,0,0,0)
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM
    #pragma surface surf Standard fullforwardshadows
    #pragma target 3.0
    sampler2D _MainTex;
    fixed4 _Color;
    float _DotProduct;
    fixed4 _RimColor;

    struct Input {
      float2 uv_MainTex;
      float3 worldNormal;
      float3 viewDir;
    };


    void surf (Input IN, inout SurfaceOutputStandard o) {
    	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = _Color.rgb * c;

      	float border = 1-(abs(dot(IN.viewDir,IN.worldNormal)));
      	float alpha = clamp(border * (1-_DotProduct) + _DotProduct,0,1);
      	fixed4 rimColor = _RimColor * alpha;
      	o.Albedo = clamp(rimColor+o.Albedo,0,1); 
    }
    ENDCG
  } 
  FallBack "Diffuse"
}