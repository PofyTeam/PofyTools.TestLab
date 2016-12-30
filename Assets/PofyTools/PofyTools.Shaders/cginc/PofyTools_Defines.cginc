#ifndef POFY_TOOLS_DEFINES_CGINC
#define POFY_TOOLS_DEFINES_CGINC


//#if defined(UNITY_PASS_FORWARDBASE) && !defined(LIGHTMAP_ON)
//	#define NEED_V_CALC_NORMAL_WS
//	
//	#if defined(V_CW_LIGHT_PER_PIXEL) || defined(V_CW_BUMP)
//		#define NEED_P_NORMAL_WS
//	#endif
//	#if defined(V_CW_BUMP)
//		#define NEED_V_CALC_ROTATION
//	#endif
//
//	#if defined(VERTEXLIGHT_ON) && defined(V_CW_UNITY_VERTEXLIGHT_ON)
//		#define NEED_V_CALC_POS_WS
//	#endif
//
//	#if !defined(V_CW_LIGHT_PER_PIXEL) || defined(V_CW_UNITY_VERTEXLIGHT_ON)
//		#define NEED_P_VERTEX_LIGHT
//	#endif
//
//	#ifdef V_CW_SPECULAR
//		#ifdef V_CW_BUMP
//			#define NEED_V_CALC_VIEWDIR_TS
//			#define NEED_P_VIEWDIR_TS
//		#else
//			#define NEED_V_CALC_VIEWDIR_WS
//			#define NEED_P_VIEWDIR_WS
//		#endif
//	#endif	
//#endif
//
//#if defined(UNITY_PASS_UNLIT) && !defined(LIGHTMAP_OFF) && !defined(V_CW_UNLIT_LIGHTMAP_ON)
//	#ifdef LIGHTMAP_ON
//	#undef LIGHTMAP_ON
//	#endif
//
//	#ifndef LIGHTMAP_OFF
//	#define LIGHTMAP_OFF
//	#endif
//#endif
//
//#ifdef V_CW_FRESNEL_ON
//	#define NEED_V_CALC_VIEWDIR_OS
//#endif
// 
//#ifdef V_CW_REFLECTION
//	#ifndef NEED_V_CALC_NORMAL_WS
//		#define NEED_V_CALC_NORMAL_WS
//	#endif
//
//	#ifndef NEED_V_CALC_VIEWDIR_WS
//		#define NEED_V_CALC_VIEWDIR_WS
//	#endif
//
//	#define NEED_P_REFLECTION_WS
//#endif
//
//#if defined(V_CW_IBL_ON) || defined(V_CW_GLOBAL_IBL_ON)
//	#ifndef NEED_V_CALC_NORMAL_WS
//		#define NEED_V_CALC_NORMAL_WS
//	#endif
//
//	#ifndef NEED_P_NORMAL_WS
//		#define NEED_P_NORMAL_WS
//	#endif
//#endif
//
//
//#if defined(V_CW_RIM_ON) || defined(V_CW_GLOBAL_RIM_ON)
//	#ifndef NEED_V_CALC_VIEWDIR_OS
//		#define NEED_V_CALC_VIEWDIR_OS
//	#endif 
//#endif
//
//
//#ifdef V_CW_BUMP
//	#define V_CW_LIGHTDIR i.lightDir
//#else
//	#define V_CW_LIGHTDIR _WorldSpaceLightPos0.xyz
//#endif
//
//#ifdef V_CW_TERRAIN
//	#define V_CW_LIGHTMAP_UV i.texcoord.zw
//#else
//	#define V_CW_LIGHTMAP_UV i.lmap.xy
//#endif
//
//
//
//	#define V_CW_X_BEND_SIZE        _V_CW_X_Bend_Size_GLOBAL
//	#define V_CW_Y_BEND_SIZE        _V_CW_Y_Bend_Size_GLOBAL
//	#define V_CW_Z_BEND_SIZE        _V_CW_Z_Bend_Size_GLOBAL
//	#define V_CW_Z_BEND_BIAS        _V_CW_Z_Bend_Bias_GLOBAL
//	#define V_CW_CAMERA_BEND_OFFSET _V_CW_Camera_Bend_Offset_GLOBAL
//	
//	#ifdef V_CW_GLOBAL_FOG_ON
//		#define V_CW_FOG_COLOR _V_CW_Fog_Color_GLOBAL
//		#define V_CW_FOG_DENSITY _V_CW_Fog_Density_GLOBAL
//		#define V_CW_FOG_START _V_CW_Fog_Start_GLOBAL
//		#define V_CW_FOG_END   _V_CW_Fog_End_GLOBAL
//	#endif
//
//
//
//#define V_CW_BEND(v)  float4 mv = normalize(mul(UNITY_MATRIX_MV, v)); \
//				      float zOff = min(0, mv.z + V_CW_CAMERA_BEND_OFFSET); \
//				      zOff = zOff * zOff * 0.001; \
//				      float xOff = max(0, abs(mv.x) - V_CW_Z_BEND_BIAS) * sign(mv.x); float4 pos = mv; \
//				      pos.xy += float2(V_CW_Y_BEND_SIZE * zOff, V_CW_X_BEND_SIZE * zOff + (xOff * xOff * V_CW_Z_BEND_SIZE) * 0.001);	o.pos = mul(UNITY_MATRIX_P, pos);
//					 
//
//#endif