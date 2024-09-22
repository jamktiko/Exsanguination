// Made with Amplify Shader Editor v1.9.0.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DawnShader/SkinShader_V2"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_TattoMask("Tatto Mask", 2D) = "white" {}
		_TattooColor("Tattoo Color", Color) = (0.1422659,0.1889629,0.2169811,0)
		_TattooBoots("Tattoo Boots", Range( 0 , 1)) = 0
		_Dirt("Dirt", 2D) = "white" {}
		_DirtBoots("Dirt Boots", Range( 0 , 1)) = 0
		_DirtRoughness_Power("DirtRoughness_Power", Float) = 0
		_SkinNormal("Skin Normal", 2D) = "bump" {}
		_SkinColor("Skin Color", 2D) = "white" {}
		_ORC("ORC", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_SSSMask("SSS Mask", 2D) = "white" {}
		_SSSColor("SSS Color", Color) = (1,0.7457517,0.6839622,0)
		_SkinRounghnessBoots("Skin Rounghness Boots", Range( 0 , 1)) = 0
		_RoughnessMod_R("RoughnessMod_R", 2D) = "white" {}
		_SkinPore("Skin Pore", 2D) = "bump" {}
		_SkinPore_Tilling("SkinPore_Tilling", Float) = 35
		_SkinPore_Power("SkinPore_Power", Range( 0 , 1)) = 0.25
		_BaseColorMul("BaseColorMul", Color) = (1,1,1,0)
		_ColorPow("Color Pow", Range( 0 , 10)) = 0
		_LipNailColor("Lip/Nail Color", Color) = (1,1,1,0)
		_LipNailColorPower("Lip/Nail Color Power", Range( 0 , 1)) = 0
		_LipsNailRoughnessBoost("Lips/Nail Roughness Boost", Range( 0 , 1)) = 0.75
		_FreaklesColor("Freakles Color", Color) = (1,1,1,0)
		_FreaklesColorPower("Freakles Color Power", Range( 0 , 20)) = 0
		_HairCapColor("HairCap Color", Color) = (1,1,1,0)
		_HairCapColorPower("HairCap Color Power", Range( 0 , 5)) = 0
		_HairCapRoughness("HairCap Roughness", Range( 0 , 1)) = 0.6
		_EmissiveMask("Emissive Mask", 2D) = "white" {}
		_Emissivepowerboost("Emissive power boost", Float) = 100
		_EmissiveColor1("Emissive Color 1", Color) = (0,0,0,0)
		_EmissivePower1("Emissive Power 1", Range( 0 , 2000)) = 0
		_EmissiveColor2("Emissive Color 2", Color) = (0,0,0,0)
		_EmissivePower2("Emissive Power 2", Range( 0 , 2000)) = 248.8943
		_EmissiveColor3("Emissive Color 3", Color) = (0,0,0,0)
		_EmissivePower3("Emissive Power 3", Range( 0 , 2000)) = 0
		_EmissiveColor4("Emissive Color 4", Color) = (0,0,0,0)
		_EmissivePower4("Emissive Power 4", Range( 0 , 2000)) = 0
		_EmissivePannerMap("Emissive Panner Map", 2D) = "white" {}
		_EmissivePannerTilling("Emissive Panner Tilling", Float) = 1
		_PannerProperty("Panner Property", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input {
			half filler;
		};
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float2 uv2_texcoord2;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform sampler2D _SkinNormal;
		uniform float4 _SkinNormal_ST;
		uniform sampler2D _SkinPore;
		uniform float _SkinPore_Tilling;
		uniform float _SkinPore_Power;
		uniform float4 _BaseColorMul;
		uniform float _ColorPow;
		uniform sampler2D _SkinColor;
		uniform float4 _SkinColor_ST;
		uniform float4 _LipNailColor;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float4 _FreaklesColor;
		uniform float4 _HairCapColor;
		uniform float _LipNailColorPower;
		uniform float _FreaklesColorPower;
		uniform float _HairCapColorPower;
		uniform sampler2D _Dirt;
		uniform float4 _Dirt_ST;
		uniform float _DirtBoots;
		uniform float4 _TattooColor;
		uniform sampler2D _TattoMask;
		uniform float4 _TattoMask_ST;
		uniform float _TattooBoots;
		uniform float _Emissivepowerboost;
		uniform float4 _EmissiveColor1;
		uniform float _EmissivePower1;
		uniform sampler2D _EmissiveMask;
		uniform float4 _EmissiveMask_ST;
		uniform sampler2D _EmissivePannerMap;
		uniform float2 _PannerProperty;
		uniform float _EmissivePannerTilling;
		uniform float4 _EmissiveColor2;
		uniform float _EmissivePower2;
		uniform float4 _EmissiveColor3;
		uniform float _EmissivePower3;
		uniform float4 _EmissiveColor4;
		uniform float _EmissivePower4;
		uniform sampler2D _ORC;
		uniform float4 _ORC_ST;
		uniform float _SkinRounghnessBoots;
		uniform sampler2D _RoughnessMod_R;
		uniform float4 _RoughnessMod_R_ST;
		uniform float _DirtRoughness_Power;
		uniform float _HairCapRoughness;
		uniform float _LipsNailRoughnessBoost;
		uniform float4 _SSSColor;
		uniform sampler2D _SSSMask;
		uniform float4 _SSSMask_ST;

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_SkinNormal = i.uv_texcoord * _SkinNormal_ST.xy + _SkinNormal_ST.zw;
			float2 temp_cast_0 = (_SkinPore_Tilling).xx;
			float2 uv2_TexCoord94 = i.uv2_texcoord2 * temp_cast_0;
			o.Normal = BlendNormals( UnpackNormal( tex2D( _SkinNormal, uv_SkinNormal ) ) , UnpackScaleNormal( tex2D( _SkinPore, uv2_TexCoord94 ), _SkinPore_Power ) );
			float4 saferPower136 = abs( _BaseColorMul );
			float4 temp_cast_1 = (_ColorPow).xxxx;
			float2 uv_SkinColor = i.uv_texcoord * _SkinColor_ST.xy + _SkinColor_ST.zw;
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode11 = tex2D( _Mask, uv_Mask );
			float4 lerpResult124 = lerp( float4( 0,0,0,0 ) , _LipNailColor , tex2DNode11.r);
			float4 lerpResult125 = lerp( float4( 0,0,0,0 ) , _FreaklesColor , tex2DNode11.g);
			float4 lerpResult126 = lerp( float4( 0,0,0,0 ) , _HairCapColor , tex2DNode11.b);
			float clampResult165 = clamp( ( ( _LipNailColorPower * tex2DNode11.r ) + ( _FreaklesColorPower * tex2DNode11.g ) + ( _HairCapColorPower * tex2DNode11.b ) ) , 0.0 , 1.0 );
			float4 lerpResult140 = lerp( ( pow( saferPower136 , temp_cast_1 ) * tex2D( _SkinColor, uv_SkinColor ) ) , ( lerpResult124 + lerpResult125 + lerpResult126 ) , clampResult165);
			float2 uv_Dirt = i.uv_texcoord * _Dirt_ST.xy + _Dirt_ST.zw;
			float4 tex2DNode63 = tex2D( _Dirt, uv_Dirt );
			float4 lerpResult70 = lerp( lerpResult140 , ( tex2DNode63 * _DirtBoots ) , ( _DirtBoots * tex2DNode63.a ));
			float2 uv_TattoMask = i.uv_texcoord * _TattoMask_ST.xy + _TattoMask_ST.zw;
			float4 lerpResult187 = lerp( lerpResult70 , _TattooColor , ( tex2D( _TattoMask, uv_TattoMask ).r * _TattooBoots ));
			float4 clampResult188 = clamp( lerpResult187 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Albedo = clampResult188.rgb;
			float2 uv_EmissiveMask = i.uv_texcoord * _EmissiveMask_ST.xy + _EmissiveMask_ST.zw;
			float4 tex2DNode163 = tex2D( _EmissiveMask, uv_EmissiveMask );
			float2 temp_cast_3 = (_EmissivePannerTilling).xx;
			float2 uv_TexCoord142 = i.uv_texcoord * temp_cast_3;
			float2 panner145 = ( _Time.y * _PannerProperty + uv_TexCoord142);
			float4 tex2DNode153 = tex2D( _EmissivePannerMap, panner145 );
			o.Emission = ( _Emissivepowerboost * ( ( _EmissiveColor1 * _EmissivePower1 * tex2DNode163.r * tex2DNode153 ) + ( _EmissiveColor2 * _EmissivePower2 * tex2DNode163.g * tex2DNode153 ) + ( _EmissiveColor3 * _EmissivePower3 * tex2DNode163.b * tex2DNode153 ) + ( _EmissiveColor4 * _EmissivePower4 * tex2DNode163.a * tex2DNode153 ) ) ).rgb;
			float2 uv_ORC = i.uv_texcoord * _ORC_ST.xy + _ORC_ST.zw;
			float4 tex2DNode173 = tex2D( _ORC, uv_ORC );
			float2 uv_RoughnessMod_R = i.uv_texcoord * _RoughnessMod_R_ST.xy + _RoughnessMod_R_ST.zw;
			float lerpResult174 = lerp( ( tex2DNode173.g * ( _SkinRounghnessBoots * tex2D( _RoughnessMod_R, uv_RoughnessMod_R ).a ) ) , tex2DNode173.b , 0.0);
			float lerpResult71 = lerp( lerpResult174 , ( _DirtRoughness_Power * _DirtBoots ) , ( _DirtBoots * tex2DNode63.a ));
			float lerpResult170 = lerp( lerpResult71 , _HairCapRoughness , tex2DNode11.b);
			float lerpResult141 = lerp( lerpResult170 , _LipsNailRoughnessBoost , tex2DNode11.r);
			float clampResult171 = clamp( lerpResult141 , 0.0 , 1.0 );
			o.Smoothness = clampResult171;
			o.Occlusion = tex2DNode173.r;
			float2 uv_SSSMask = i.uv_texcoord * _SSSMask_ST.xy + _SSSMask_ST.zw;
			o.Transmission = ( _SSSColor * ( 1.0 - tex2D( _SSSMask, uv_SSSMask ).r ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19001
2560;1;2560;1058;578.6995;1127.365;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;26;-1414.051,168.8026;Float;False;Property;_SkinRounghnessBoots;Skin Rounghness Boots;12;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1417.679,276.184;Inherit;True;Property;_RoughnessMod_R;RoughnessMod_R;13;0;Create;True;0;0;0;False;0;False;-1;c0b7b18616e3d4c4d9f007d6681e9331;c0b7b18616e3d4c4d9f007d6681e9331;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-1946.72,-534.3867;Inherit;False;Property;_HairCapColorPower;HairCap Color Power;25;0;Create;True;0;0;0;False;0;False;0;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-1951.928,-817.153;Inherit;False;Property;_LipNailColorPower;Lip/Nail Color Power;20;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-1951.748,-684.711;Inherit;False;Property;_FreaklesColorPower;Freakles Color Power;23;0;Create;True;0;0;0;False;0;False;0;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1439.844,-932.1219;Inherit;True;Property;_Mask;Mask;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;173;-1656.62,-72.0534;Inherit;True;Property;_ORC;ORC;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1050.327,259.7152;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-4191.182,-571.95;Inherit;False;Property;_EmissivePannerTilling;Emissive Panner Tilling;38;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-940.8261,-1256.057;Float;False;Property;_FreaklesColor;Freakles Color;22;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-1342.003,-1245.533;Float;False;Property;_LipNailColor;Lip/Nail Color;19;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.5566037,0.107645,0.107645,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-1616.038,-674.923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-1657.896,-922.996;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1410.582,-202.3145;Float;False;Property;_ColorPow;Color Pow;18;0;Create;True;0;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-663.8802,-1247.952;Float;False;Property;_HairCapColor;HairCap Color;24;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;-1391.19,-623.9757;Float;False;Property;_BaseColorMul;BaseColorMul;17;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.372549,0.4821379,0.6509804,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-1637.466,-536.3641;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1431.925,-404.8359;Inherit;True;Property;_SkinColor;Skin Color;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;167;-872.2632,-488.2551;Inherit;False;Constant;_max;max;40;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;124;-1056.878,-1033.162;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;126;-620.8776,-1036.162;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;166;-876.2632,-561.2551;Inherit;False;Constant;_min;min;40;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;136;-1058.79,-307.5454;Inherit;False;True;2;0;COLOR;2,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TimeNode;144;-3905.697,-282.8923;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;125;-826.8776,-1033.162;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-3921.697,-570.8915;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;63;-1276.916,-1681.338;Inherit;True;Property;_Dirt;Dirt;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-903.1882,148.0897;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1256.622,-1791.901;Float;False;Property;_DirtBoots;Dirt Boots;4;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;143;-3889.697,-442.8915;Float;False;Property;_PannerProperty;Panner Property;39;0;Create;True;0;0;0;False;0;False;0,0;0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;97;-1232.994,-1483.387;Float;False;Property;_DirtRoughness_Power;DirtRoughness_Power;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;123;-986.5225,-784.5419;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-864.1537,-1542.7;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-864.1537,-1413.515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;174;-724.6627,-30.15831;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-650.0494,-819.539;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-868.0598,-418.015;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;145;-3601.696,-410.8915;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;165;-705.032,-586.2251;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-2924.894,216.7375;Float;False;Property;_EmissivePower3;Emissive Power 3;34;0;Create;True;0;0;0;False;0;False;0;0;0;2000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;163;-3304.605,-876.3577;Inherit;True;Property;_EmissiveMask;Emissive Mask;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;148;-2852.855,-212.5536;Float;False;Property;_EmissiveColor2;Emissive Color 2;31;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;147;-2922.544,-300.5449;Float;False;Property;_EmissivePower1;Emissive Power 1;30;0;Create;True;0;0;0;False;0;False;0;0;0;2000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;153;-3329.696,-410.8915;Inherit;True;Property;_EmissivePannerMap;Emissive Panner Map;37;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;151;-2853.01,300.7601;Float;False;Property;_EmissiveColor4;Emissive Color 4;35;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;154;-2856.544,-472.545;Float;False;Property;_EmissiveColor1;Emissive Color 1;29;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;95;-1907.972,854.9802;Inherit;False;Property;_SkinPore_Tilling;SkinPore_Tilling;15;0;Create;True;0;0;0;False;0;False;35;35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;152;-2853.01,42.5166;Float;False;Property;_EmissiveColor3;Emissive Color 3;33;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;184;-607.2698,-2066.642;Inherit;True;Property;_TattoMask;Tatto Mask;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-867.3045,-1667.159;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-185.2703,-1619.385;Inherit;False;Property;_HairCapRoughness;HairCap Roughness;26;0;Create;True;0;0;0;False;0;False;0.6;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;140;-485.7545,-523.3894;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-2926.115,474.9807;Float;False;Property;_EmissivePower4;Emissive Power 4;36;0;Create;True;0;0;0;False;0;False;0;0;0;2000;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-2919.855,-39.55336;Float;False;Property;_EmissivePower2;Emissive Power 2;32;0;Create;True;0;0;0;False;0;False;248.8943;0;0;2000;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;183;-586.9757,-2177.205;Float;False;Property;_TattooBoots;Tattoo Boots;2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-864.1538,-1793.193;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;71;-409.5221,-1575.355;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-2255.959,-220.9686;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;70;-475.3391,-1760.756;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;88;-202.9217,-701.5978;Inherit;True;Property;_SSSMask;SSS Mask;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-2251.683,51.29242;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;-2250.205,317.4468;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1970.994,945.6127;Inherit;False;Property;_SkinPore_Power;SkinPore_Power;16;0;Create;True;0;0;0;False;0;False;0.25;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-2269.648,-493.9601;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-1701.967,852.9305;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;170;105.5297,-1563.984;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;185;-528.5568,-2361.842;Inherit;False;Property;_TattooColor;Tattoo Color;1;0;Create;True;0;0;0;False;0;False;0.1422659,0.1889629,0.2169811,0;0.1509434,0.0874304,0.05339979,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;128;-176.1403,-1744.974;Inherit;False;Property;_LipsNailRoughnessBoost;Lips/Nail Roughness Boost;21;0;Create;True;0;0;0;False;0;False;0.75;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-232.059,-2149.464;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;177;99.1234,-944.0643;Inherit;False;Property;_SSSColor;SSS Color;11;0;Create;True;0;0;0;False;0;False;1,0.7457517,0.6839622,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;141;307.8163,-1605.319;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;113;132.2692,-668.0559;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-1405.436,652.8618;Inherit;True;Property;_SkinNormal;Skin Normal;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;160;-2075.068,289.3349;Inherit;False;Property;_Emissivepowerboost;Emissive power boost;28;0;Create;True;0;0;0;False;0;False;100;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;159;-2039.662,-169.42;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;187;118.4369,-2065.039;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;93;-1403.721,858.0552;Inherit;True;Property;_SkinPore;Skin Pore;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;-1861.381,150.4048;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;188;434.2753,-975.8932;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;171;488.1423,-1257.04;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;451.3824,-745.3231;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;92;-1060.379,651.025;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;172;790.2903,-848.5927;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;DawnShader/SkinShader_V2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;18;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;True;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;44;0;26;0
WireConnection;44;1;3;4
WireConnection;120;0;117;0
WireConnection;120;1;11;2
WireConnection;119;0;116;0
WireConnection;119;1;11;1
WireConnection;121;0;115;0
WireConnection;121;1;11;3
WireConnection;124;1;12;0
WireConnection;124;2;11;1
WireConnection;126;1;21;0
WireConnection;126;2;11;3
WireConnection;136;0;7;0
WireConnection;136;1;9;0
WireConnection;125;1;20;0
WireConnection;125;2;11;2
WireConnection;142;0;162;0
WireConnection;43;0;173;2
WireConnection;43;1;44;0
WireConnection;123;0;119;0
WireConnection;123;1;120;0
WireConnection;123;2;121;0
WireConnection;69;0;97;0
WireConnection;69;1;65;0
WireConnection;66;0;65;0
WireConnection;66;1;63;4
WireConnection;174;0;43;0
WireConnection;174;1;173;3
WireConnection;57;0;124;0
WireConnection;57;1;125;0
WireConnection;57;2;126;0
WireConnection;137;0;136;0
WireConnection;137;1;4;0
WireConnection;145;0;142;0
WireConnection;145;2;143;0
WireConnection;145;1;144;2
WireConnection;165;0;123;0
WireConnection;165;1;166;0
WireConnection;165;2;167;0
WireConnection;153;1;145;0
WireConnection;67;0;65;0
WireConnection;67;1;63;4
WireConnection;140;0;137;0
WireConnection;140;1;57;0
WireConnection;140;2;165;0
WireConnection;68;0;63;0
WireConnection;68;1;65;0
WireConnection;71;0;174;0
WireConnection;71;1;69;0
WireConnection;71;2;66;0
WireConnection;157;0;148;0
WireConnection;157;1;149;0
WireConnection;157;2;163;2
WireConnection;157;3;153;0
WireConnection;70;0;140;0
WireConnection;70;1;68;0
WireConnection;70;2;67;0
WireConnection;158;0;152;0
WireConnection;158;1;150;0
WireConnection;158;2;163;3
WireConnection;158;3;153;0
WireConnection;156;0;151;0
WireConnection;156;1;146;0
WireConnection;156;2;163;4
WireConnection;156;3;153;0
WireConnection;155;0;154;0
WireConnection;155;1;147;0
WireConnection;155;2;163;1
WireConnection;155;3;153;0
WireConnection;94;0;95;0
WireConnection;170;0;71;0
WireConnection;170;1;169;0
WireConnection;170;2;11;3
WireConnection;186;0;184;1
WireConnection;186;1;183;0
WireConnection;141;0;170;0
WireConnection;141;1;128;0
WireConnection;141;2;11;1
WireConnection;113;0;88;1
WireConnection;159;0;155;0
WireConnection;159;1;157;0
WireConnection;159;2;158;0
WireConnection;159;3;156;0
WireConnection;187;0;70;0
WireConnection;187;1;185;0
WireConnection;187;2;186;0
WireConnection;93;1;94;0
WireConnection;93;5;96;0
WireConnection;161;0;160;0
WireConnection;161;1;159;0
WireConnection;188;0;187;0
WireConnection;171;0;141;0
WireConnection;171;1;166;0
WireConnection;171;2;167;0
WireConnection;176;0;177;0
WireConnection;176;1;113;0
WireConnection;92;0;38;0
WireConnection;92;1;93;0
WireConnection;172;0;188;0
WireConnection;172;1;92;0
WireConnection;172;2;161;0
WireConnection;172;4;171;0
WireConnection;172;5;173;1
WireConnection;172;6;176;0
ASEEND*/
//CHKSM=4916A1DB5800B732C74DE187501D348BA95FDC18