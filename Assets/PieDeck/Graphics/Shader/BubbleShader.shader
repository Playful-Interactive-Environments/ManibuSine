// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "Custom/BubbleShader" {
Properties {
	_BumpAmt  ("Distortion", range (0,128)) = 10
	_MainColor("Tint Color", Color) = (1,1,1,1)
	_UseMainColor ("Use Tint Color", range(0,1)) = 0
	_UseTintEffect ("Use Tint Effect", range(0,1)) = 0
	_MainTex ("Tint Color (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_RimColor("Rim Color", Color) = (1,1,1,1)
	_RimPower("Rim Power", Range(0.1, 6.0)) = 3.0
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_SpecLightness ("Spec Lightness", Range (0, 1)) = 0.5
	_SpecMap ("Spec map (RGB)", 2D) = "white" {}
	_Emission ("Emission", Color) = (1,1,1,1)
}

Category {

	// We must be transparent, so other objects are drawn before this one.
	Tags { "Queue"="Transparent" "RenderType"="Opaque" }


	SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass {							
			Name "BASE"
			Tags { "LightMode" = "Always" }
 		}
 		
 		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
 		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 texcoord: TEXCOORD0;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
	float2 uvspec : TEXCOORD3;
	float4 color : COLOR;
    float3 viewDirT : VIEW_DIR;
    float3 lightDirT : LIGHT_DIR;
    float specularK : SHININESS;
    float3 normal : NORMAL;
    float rimValue : RIM_VALUE;
};

float _BumpAmt;
float4 _BumpMap_ST;
float4 _MainColor;
float4 _MainTex_ST;
float4 _SpecMap_ST;
float4 _RimColor;
float _RimPower;
float _Shininess;
float _SpecLightness;
sampler2D _GrabTexture;
float _UseMainColor;
float _UseTintEffect;

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	#if UNITY_UV_STARTS_AT_TOP
	float scale = -1.0;
	#else
	float scale = 1.0;
	#endif
	o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
	o.uvgrab.zw = o.vertex.zw;
	o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
	o.uvspec = TRANSFORM_TEX( v.texcoord, _SpecMap );
    o.specularK = _Shininess * 128;
    o.normal = v.normal;
	
	float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
    //o.rim = 1 - dot(v.normal, viewDir); // 1 - saturate(dot(normalize(viewDir), v.normal));
    float dotProduct = 1 - dot(v.normal, viewDir);
    float rimWidth = _RimPower; // 0.7;
    o.rimValue = smoothstep(1 - rimWidth, 1.0, dotProduct);
    
    float4 normalColor = float4(0,0,0,0);
    normalColor[0] = abs(v.normal)[0];
    normalColor[1] = abs(v.normal)[1];
    normalColor[2] = abs(v.normal)[2];

    //o.color = normalColor * _RimColor;
    //o.color = o.color * (1 - o.rimValue) + col * o.rimValue;
    
//    o.color = (o.rimValue) * (1 - normalColor) * (1 - _RimColor);
//    o.color = 1 - o.color;
    o.color = (o.rimValue) * (normalColor) * (_RimColor);

    //TANGENT_SPACE_ROTATION;
    o.lightDirT = normalize(ObjSpaceLightDir(v.vertex)); // mul( rotation, ObjSpaceLightDir( v.vertex ) );    
    o.viewDirT = normalize(ObjSpaceViewDir(v.vertex)); // mul( rotation, ObjSpaceViewDir( v.vertex ) );    

	return o;
}


float4 _GrabTexture_TexelSize;
sampler2D _BumpMap;
sampler2D _MainTex;
sampler2D _SpecMap;
float4 _Emission;

// Calculates Blinn-Phong (specular) lighting model
inline half4 SpecularColorLight( half3 lightDir, half3 viewDir, half3 normal, half4 color, half4 specColor, float specK, half atten )
{
    half4 c;
    #ifndef USING_DIRECTIONAL_LIGHT
    lightDir = normalize(lightDir);
    #endif
    viewDir = normalize(viewDir);
    half3 h = normalize( lightDir + viewDir );
    
    half diffuse = dot( normal, lightDir );
    
    float nh = saturate( dot( h, normal ) );
    float spec = pow( nh, specK ) * color.a;
    
    c.rgb = (color.rgb + specColor.rgb * spec * _SpecLightness) * (atten * 2);
    c.a = specColor.a * spec * atten; // specular passes by default put highlights to overbright

    return c;
}

half4 frag( v2f i ) : COLOR
{
	// calculate perturbed coordinates
	half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg; // we could optimize this by just reading the x & y without reconstructing the Z
	float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
	i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
	
	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
	half4 tint = _MainColor * _UseMainColor + tex2D( _MainTex, i.uvmain ) * (1 - _UseMainColor);
//	half4 materialColor = col * (1 - _UseTintEffect) + tint * _UseTintEffect;
	float tintAlpha = tint[3] * _UseTintEffect;
	half4 materialColor = col * (1 - tintAlpha) + tint * tintAlpha;


	materialColor = materialColor * (1 - i.rimValue) + (i.color + _Emission * 2) * i.rimValue;
	//if (i.color[3] > 0) materialColor *= i.color;	


	half4 speccol = tex2D( _SpecMap, i.uvspec );
	
	half4 c = SpecularColorLight( i.lightDirT, i.viewDirT, i.normal, materialColor, speccol, i.specularK, 0.5 );

	half4 finalColor = c + _Emission;

	finalColor = finalColor * (1 - i.rimValue / 2) + col * i.rimValue / 2;
	
	return finalColor;
}
ENDCG
		}
	}

	// ------------------------------------------------------------------
	// Fallback for older cards and Unity non-Pro
	
	SubShader {
		Blend DstColor Zero
		Pass {
			Name "BASE"
			SetTexture [_MainTex] {	combine texture }
		}
	}
}

}