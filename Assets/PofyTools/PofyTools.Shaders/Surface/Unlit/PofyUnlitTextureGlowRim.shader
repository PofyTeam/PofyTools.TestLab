Shader "PofyTools/Surface/Unlit/Texture Glow Rim" {
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

	#pragma surface surf NoLighting noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
   half4 _Color;
    half _DotProduct;
   half4 _RimColor;

    struct Input {
      half2 uv_MainTex;
      half3 worldNormal;
      half3 viewDir;
    };

            half4 LightingNoLighting(SurfaceOutput s,half3 lightDir,half atten)
     {
        half4 c;
         c.rgb = s.Albedo; 
//         c.a = s.Alpha;
         return c;
     }

    
    void surf (Input IN, inout SurfaceOutput o) {
    	half4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = _Color.rgb * c;

      	half border = 1-(abs(dot(IN.viewDir,IN.worldNormal)));
      	half alpha = clamp(border * (1-_DotProduct) + _DotProduct,0,1);
      	alpha*=alpha;

      	half4 rimColor = (_RimColor+alpha) * alpha;
      	o.Albedo = clamp(rimColor+o.Albedo*(1-alpha),0,1); 
    }

    ENDCG
  } 
  FallBack "Unlit"
}