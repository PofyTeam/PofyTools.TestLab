Shader "PofyTools/Standard/Texture Rim Scroll" {
  Properties {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
	_ScrollXSpeed ("X Scroll Speed", Range(0,10)) = 0
	_ScrollYSpeed ("Y Scroll Speed", Range(0,10)) = 0

    _DotProduct("Rim Effect", Range(-1,1)) = 0.25
    _RimColor ("Rim Color", Color) = (0,0,0,0)
    _RimTex ("Rim Mask(R)", 2D) = "white" {}

    _RimScrollXSpeed ("Rim X Scroll Speed", Range(0,10)) = 2
    _RimScrollYSpeed ("Rim Y Scroll Speed", Range(0,10)) = 2
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM
    #pragma surface surf Standard fullforwardshadows
    #pragma target 3.0
    sampler2D _MainTex;
    sampler2D _RimTex;
    fixed4 _Color;

     float _DotProduct;
    fixed4 _RimColor;

	fixed _ScrollXSpeed;
	fixed _ScrollYSpeed;

	fixed _RimScrollXSpeed;
	fixed _RimScrollYSpeed;

    struct Input {
      float2 uv_MainTex;
      float3 worldNormal;
      float3 viewDir;
    };


    void surf (Input IN, inout SurfaceOutputStandard o) {
		fixed2 scrolledUV = IN.uv_MainTex;
		fixed xScroll = _ScrollXSpeed * _Time;
		fixed yScroll = _ScrollYSpeed * _Time;

		scrolledUV += fixed2(xScroll,yScroll);

		fixed4 c = tex2D (_MainTex, scrolledUV);
	  	o.Albedo = _Color.rgb * c;
	  	o.Alpha = c.a;

	  	fixed2 rimScrolledUV = IN.uv_MainTex;
		fixed rimXScroll = _RimScrollXSpeed * _Time;
		fixed rimYScroll = _RimScrollYSpeed * _Time;

		rimScrolledUV += fixed2(rimXScroll,rimYScroll);

		fixed rimColorR = tex2D (_RimTex, rimScrolledUV).r;

      	float border = 1-(abs(dot(IN.viewDir,IN.worldNormal)));
      	float alpha = clamp(border * (1-_DotProduct) + _DotProduct,0,1);
      	alpha *= alpha;
      	fixed4 rimColor = (_RimColor+alpha) * rimColorR * alpha;

      	o.Albedo = clamp(rimColor + o.Albedo* (1 - alpha),0,1);
      	o.Emission = rimColor.rgb;
    }
    ENDCG
  } 
  FallBack "Diffuse"
}