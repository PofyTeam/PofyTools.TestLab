Shader "PofyTools/Standard/Texture Color Blend" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _Blend ("Blend (0-Color,1-Texture)", Range(0,1)) = 1
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM
    #pragma surface surf Standard fullforwardshadows
    #pragma target 3.0
    sampler2D _MainTex;

    struct Input {
      float2 uv_MainTex;
    };
    fixed4 _Color;
    float _Blend;

    void surf (Input IN, inout SurfaceOutputStandard o) {
    	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = lerp(_Color.rgb,c,_Blend);
    }
    ENDCG
  } 
  FallBack "Diffuse"
}