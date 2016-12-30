Shader "PofyTools/Surface/Toon/Texture" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
//    _DotProduct("Rim Effect", Range(-1,1)) = 0.25
//    _RimColor ("Rim Color", Color) = (0,0,0,0)
	_RampTex ("Ramp", 2D) = "white"{}
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM

	#pragma surface surf Toon noforwardadd exclude_path:deferred exclude_path:prepass noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa 
    #pragma target 3.0

    sampler2D _MainTex;
    fixed4 _Color;
    float _DotProduct;
//    fixed4 _RimColor;
	sampler2D _RampTex;

    struct Input {
      float2 uv_MainTex;
//      float3 worldNormal;
//      float3 viewDir;
    };

//             fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
//     {
//         fixed4 c;
//         c.rgb = s.Albedo; 
////         c.a = s.Alpha;
//         return c;
//     }

	 fixed4 LightingToon (SurfaceOutput s, fixed3 lightDir, fixed atten)
	{
		half NdotL = dot(s.Normal, lightDir); 
		NdotL = tex2D(_RampTex, fixed2(NdotL, 0.5));

		fixed4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;
		c.a = s.Alpha;

		return c;
	}
    
    void surf (Input IN, inout SurfaceOutput o) {
    	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
      	o.Albedo = _Color.rgb * c;

//      	float border = 1-(abs(dot(IN.viewDir,IN.worldNormal)));
//      	float alpha = clamp(border * (1-_DotProduct) + _DotProduct,0,1);
//      	fixed4 rimColor = _RimColor * alpha;
//      	o.Albedo = clamp(rimColor+o.Albedo,0,1); 
    }

    ENDCG
  } 
  FallBack "Unlit"
}