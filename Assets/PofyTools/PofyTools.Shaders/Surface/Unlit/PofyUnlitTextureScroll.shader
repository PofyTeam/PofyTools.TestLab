Shader "PofyTools/Surface/Unlit/Texture Scroll" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    		_ScrollXSpeed ("X Scroll Speed", Range(0,10)) = 2
		_ScrollYSpeed ("Y Scroll Speed", Range(0,10)) = 2
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM

	#pragma surface surf NoLighting noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
    half4 _Color;

    half _ScrollXSpeed;
	half _ScrollYSpeed;

    struct Input {
      half2 uv_MainTex;
    };

    half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten)
     {
         half4 c;
         c.rgb = s.Albedo; 
         return c;
     }

    
    void surf (Input IN, inout SurfaceOutput o) {
		half2 scrolledUV = IN.uv_MainTex;
		half xScroll = _ScrollXSpeed * _Time;
		half yScroll = _ScrollYSpeed * _Time;

		scrolledUV += half2(xScroll,yScroll);

    	half4 c = tex2D (_MainTex, scrolledUV);
      	o.Albedo = _Color.rgb * c;
    }

    ENDCG
  } 
  FallBack "Unlit"
}