#ifndef POFY_TOOLS_BASE_CGINC
#define POFY_TOOLS_BASE_CGINC

////////////////////////////////////////////////////////////////////////////
//																		  //
//Variables 															  //
//																		  //
////////////////////////////////////////////////////////////////////////////

//#ifdef V_CW_GLOBAL_ON	
	uniform float _V_CW_X_Bend_Size_GLOBAL;
	uniform float _V_CW_Y_Bend_Size_GLOBAL;
	uniform float _V_CW_Z_Bend_Size_GLOBAL;
	uniform float _V_CW_Z_Bend_Bias_GLOBAL;
	uniform float _V_CW_Camera_Bend_Offset_GLOBAL;

	#ifdef V_CW_GLOBAL_FOG_ON
		uniform fixed4 _V_CW_Fog_Color_GLOBAL;
		uniform fixed _V_CW_Fog_Density_GLOBAL;
		uniform half _V_CW_Fog_Start_GLOBAL;
		uniform half _V_CW_Fog_End_GLOBAL;
	#endif


////////////////////////////////////////////////////////////////////////////
//																		  //
//Defines    															  //
//																		  //
////////////////////////////////////////////////////////////////////////////

//#ifdef V_CW_GLOBAL_ON
	#define V_CW_X_BEND_SIZE        _V_CW_X_Bend_Size_GLOBAL
	#define V_CW_Y_BEND_SIZE        _V_CW_Y_Bend_Size_GLOBAL
	#define V_CW_Z_BEND_SIZE        _V_CW_Z_Bend_Size_GLOBAL
	#define V_CW_Z_BEND_BIAS        _V_CW_Z_Bend_Bias_GLOBAL
	#define V_CW_CAMERA_BEND_OFFSET _V_CW_Camera_Bend_Offset_GLOBAL
	
	#ifdef V_CW_GLOBAL_FOG_ON
		#define V_CW_FOG_COLOR _V_CW_Fog_Color_GLOBAL
		#define V_CW_FOG_DENSITY _V_CW_Fog_Density_GLOBAL
		#define V_CW_FOG_START _V_CW_Fog_Start_GLOBAL
		#define V_CW_FOG_END _V_CW_Fog_End_GLOBAL

		#define V_CW_FOG saturate((_V_CW_Fog_End_GLOBAL - length(mv.xyz) * _V_CW_Fog_Density_GLOBAL) / (_V_CW_Fog_End_GLOBAL - _V_CW_Fog_Start_GLOBAL));
	#endif

#define V_CW_BEND(v)  float4 mv = normalize(mul(UNITY_MATRIX_MV, v)); \
				      float zOff = min(0, mv.z + V_CW_CAMERA_BEND_OFFSET); \
				      zOff = zOff * zOff * 0.001; \
				      float xOff = max(0, abs(mv.x) - V_CW_Z_BEND_BIAS) * sign(mv.x); \
				      float4 pos = mv + float4(V_CW_Y_BEND_SIZE * zOff, V_CW_X_BEND_SIZE * zOff + (xOff * xOff * V_CW_Z_BEND_SIZE) * 0.001, 0, 0);	o.pos = mul(UNITY_MATRIX_P, pos);

#endif 
