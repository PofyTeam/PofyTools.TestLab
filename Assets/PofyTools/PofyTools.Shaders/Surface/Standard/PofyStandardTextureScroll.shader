Shader "PofyTools/Standard/Texture Scroll" {
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
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;

		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;

		struct Input {
		float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed2 scrolledUV = IN.uv_MainTex;
			fixed xScroll = _ScrollXSpeed * _Time;
			fixed yScroll = _ScrollYSpeed * _Time;

			scrolledUV += fixed2(xScroll,yScroll);

			fixed4 c = tex2D (_MainTex, scrolledUV);

			//Set output values
		  	o.Albedo = _Color.rgb * c;
		  	o.Alpha = c.a;
		}
		ENDCG	
	} 
	FallBack "Diffuse"
}