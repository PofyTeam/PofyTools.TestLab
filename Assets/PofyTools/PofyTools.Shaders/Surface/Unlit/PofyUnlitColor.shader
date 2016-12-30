Shader "PofyTools/Surface/Unlit/Color" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM

	#pragma surface surf NoLighting noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    struct Input {
      half4 uv_MainTex;
    };
    half4 _Color;

    half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten)
     {
         half4 c;
         c.rgb = s.Albedo; 
         return c;
     }

    
    void surf (Input IN, inout SurfaceOutput o) {
      	o.Albedo = _Color.rgb;
    }

    ENDCG
  } 
  FallBack "Unlit"
}