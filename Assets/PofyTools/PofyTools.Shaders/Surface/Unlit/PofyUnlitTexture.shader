Shader "PofyTools/Surface/Unlit/Texture" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM

	#pragma surface surf NoLighting noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
    half4 _Color;

    struct Input {
      float2 uv_MainTex;
    };

    half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten)
     {
         half4 c;
         c.rgb = s.Albedo; 
         return c;
     }

    
    void surf (Input IN, inout SurfaceOutput o) {
    	half4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = _Color.rgb * c;
    }

    ENDCG
  } 
  FallBack "Unlit"
}