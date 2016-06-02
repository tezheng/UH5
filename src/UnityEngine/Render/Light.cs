using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{


public partial class Light : Behaviour
{
	// The type of the light.
	public LightType type;

	// The color of the light.
	public Color color;

	// The Intensity of a light is multiplied with the Light color.
	public float intensity;
	
	public float bounceIntensity;

	// How this light casts shadows?
	public LightShadows shadows;
	
	
	// Strength of light's shadows
	public float shadowStrength;
	
	// Shadow mapping bias
	public float shadowBias;

	// Softness of directional light's soft shadows
	public float shadowSoftness;
	
	// Fadeout speed of directional light's soft shadows
	public float shadowSoftnessFade;
	
	// The range of the light.
	public float range;

	// The angle of the light's spotlight cone in degrees.
	public float spotAngle;

	public float cookieSize;
	
	// The cookie texture projected by the light.
	//AUTO_PTR_PROP Texture cookie GetCookie SetCookie
	
	// The [[wiki:class-Flare|flare asset]] to use for this light.
	//AUTO_PTR_PROP Flare flare GetFlare SetFlare
	
	// How to render the light.
	public LightRenderMode renderMode;

	// Has the light been already lightmapped.
	public bool alreadyLightmapped;
	

	// This is used to light certain objects in the scene selectively.
	public int cullingMask;

//	CUSTOM_PROP static int pixelLightCount { return GetQualitySettings().GetCurrent().pixelLightCount; } { GetQualitySettings().SetPixelLightCount(value); }
	
	/*
	CUSTOM public static Light[] GetLights (LightType type, int layer)
	{
		UNITY_TEMP_VECTOR(Light*) lightsvector;
		
		layer = 1 << layer;
		LightManager::Lights& lights = GetLightManager().GetAllLights();
		for (LightManager::Lights::iterator i=lights.begin();i != lights.end();i++)
		{
			Light* light = &*i;
			if (!light)
				continue;
			if (light->GetType() == type && (light->GetCullingMask() & layer) != 0)
				lightsvector.push_back(light);
		}
		
		return CreateScriptingArrayFromUnityObjects(lightsvector,Scripting::ClassIDToScriptingType(ClassID(Light)));
	}
	*/

}

}
