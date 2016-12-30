Shader "PofyTools/Surface/Toon/Texture Blend" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
	_Blend ("Blend (0-Color,1-Texture)", Range(0,1)) = 1
	_RampTex ("Ramp", 2D) = "white"{}
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM

	#pragma surface surf Toon noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
    half4 _Color;
    half _Blend;
	sampler2D _RampTex;

    struct Input {
      float2 uv_MainTex;
    };


	 half4 LightingToon (SurfaceOutput s, fixed3 lightDir, fixed atten)
	{
		half NdotL = dot(s.Normal, lightDir); 
		NdotL = tex2D(_RampTex, half2(NdotL, 0.5));

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;
		c.a = s.Alpha;

		return c;
	}
    
    void surf (Input IN, inout SurfaceOutput o) {
    	half4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = lerp(_Color.rgb, c,_Blend);
    }

    ENDCG
  } 
  FallBack "Unlit"
}