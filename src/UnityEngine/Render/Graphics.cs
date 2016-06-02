using System;

namespace UnityEngine
{
public enum CameraClearFlags
{
	// Clear with the skybox.
	Skybox = 1,

	Color = 2,

	// Clear with a background color.
	SolidColor = 2,

	// Clear only the depth buffer.
	Depth = 3,

	// Don't clear anything.
	Nothing = 4
}

// The type of a [[Light]].
public enum LightType
{
	// The light is a spot light.
	Spot = 0,

	// The light is a directional light.
	Directional = 1,

	// The light is a point light.
	Point = 2,

	// The light is an area light. It affects only lightmaps and lightprobes.
	Area = 3
}

// How the [[Light]] is rendered.
public enum LightRenderMode
{
	// Automatically choose the render mode.
	Auto = 0,
	// Force the [[Light]] to be a pixel light.
	ForcePixel = 1,
	// Force the [[Light]] to be a vertex light.
	ForceVertex = 2
}

// Shadow casting options for a [[Light]].
public enum LightShadows
{
	// Do not cast shadows (default).
	None = 0,
	// Cast "hard" shadows (with no shadow filtering).
	Hard = 1,
	// Cast "soft" shadows (with 4x PCF filtering).
	Soft = 2,
}

// Format of a [[Texture2D|texture]]. Used when creating textures from scripts.
public enum TextureFormat
{
	// Alpha-only texture format.
	Alpha8 = 1,

	// A 16 bits/pixel texture format. Texture stores color with an alpha channel.
	ARGB4444 = 2,

	// A color texture format.
	RGB24 = 3,

	//*undocumented*
	RGBA32 = 4,

	// Color with an alpha channel texture format.
	ARGB32 = 5,

	// A 16 bit color texture format.
	RGB565 = 7,

	// Compressed color texture format.
	DXT1 = 10,

	// Compressed color with alpha channel texture format.
	DXT5 = 12,

	// A 16 bits/pixel texture format. Texture stores color with an alpha channel.
	RGBA4444 = 13,

	// Format returned by iPhone camera
	BGRA32 = 14,

	// PowerVR (iOS) 2 bits/pixel compressed color texture format.
	PVRTC_RGB2 = 30,

	// PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
	PVRTC_RGBA2 = 31,

	// PowerVR (iOS) 4 bits/pixel compressed color texture format.
	PVRTC_RGB4 = 32,

	// PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
	PVRTC_RGBA4 = 33,

	// ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
	ETC_RGB4 = 34,

	// ATC (ATITC) 4 bits/pixel compressed RGB texture format.
	ATC_RGB4 = 35,

	// ATC (ATITC) 8 bits/pixel compressed RGB texture format.
	ATC_RGBA8 = 36,

	// Flash-specific RGB DXT1 compressed color texture format.
	ATF_RGB_DXT1 = 38,

	// Flash-specific RGBA JPG-compressed color texture format.
	ATF_RGBA_JPG = 39,

	// Flash-specific RGB JPG-compressed color texture format.
	ATF_RGB_JPG = 40,
}

}