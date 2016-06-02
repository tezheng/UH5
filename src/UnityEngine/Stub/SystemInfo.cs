
namespace UnityEngine
{
// Access system information.
public class SystemInfo
{

	public static int graphicsShaderLevel;
/*
	// Operating system name with version (RO).
	CUSTOM_PROP static string operatingSystem { return scripting_string_new( systeminfo::GetOperatingSystem() ); }

	// Processor name (RO).
	CUSTOM_PROP static string processorType { return scripting_string_new( systeminfo::GetProcessorType() ); }

	// Number of processors present (RO).
	CUSTOM_PROP static int processorCount { return systeminfo::GetProcessorCount(); }

	// Amount of system memory present (RO).
	CUSTOM_PROP static int systemMemorySize { return systeminfo::GetPhysicalMemoryMB(); }


	// Amount of video memory present (RO).
	CUSTOM_PROP static int graphicsMemorySize { return (int)gGraphicsCaps.videoMemoryMB; }


	// The name of the graphics device (RO).
	CUSTOM_PROP static string graphicsDeviceName { return scripting_string_new(gGraphicsCaps.rendererString.c_str()); }


	// The vendor of the graphics device (RO).
	CUSTOM_PROP static string graphicsDeviceVendor { return scripting_string_new(gGraphicsCaps.vendorString.c_str()); }


	// The identifier code of the graphics device (RO).
	CUSTOM_PROP static int graphicsDeviceID { return gGraphicsCaps.rendererID; }


	// The identifier code of the graphics device vendor (RO).
	CUSTOM_PROP static int graphicsDeviceVendorID { return gGraphicsCaps.vendorID; }


	// The graphics API version supported by the graphics device (RO).
	CUSTOM_PROP static string graphicsDeviceVersion { return scripting_string_new(gGraphicsCaps.fixedVersionString.c_str()); }



	// Graphics device shader capability level (RO).
	CUSTOM_PROP static int graphicsShaderLevel { return gGraphicsCaps.shaderCaps; }


	OBSOLETE warning graphicsPixelFillrate is no longer supported in Unity 5.0+.
	CSRAW public static int graphicsPixelFillrate { get {
		return -1; // was already indicating "unknown GPU/platform" back when we had some support for it
	} }

	OBSOLETE warning Vertex program support is required in Unity 5.0+
	CSRAW public static bool supportsVertexPrograms { get { return true; } }

	CUSTOM_PROP static bool graphicsMultiThreaded {
		#if ENABLE_MULTITHREADED_CODE
		return GetGfxThreadingMode() == kGfxThreadingModeThreaded;
		#else
		return false;
		#endif
	}

	// Are built-in shadows supported? (RO)
	CUSTOM_PROP static bool supportsShadows {
		return RenderTexture::IsEnabled() && GetBuildSettings().hasShadows && CheckPlatformSupportsShadows();
	}

	// Are render textures supported? (RO)
	CUSTOM_PROP static bool supportsRenderTextures {
		return RenderTexture::IsEnabled();
	}
	CUSTOM_PROP static bool supportsRenderToCubemap {
		return RenderTexture::IsEnabled() && (gGraphicsCaps.hasRenderToCubemap);
	}
	// Are image effects supported? (RO)
	CUSTOM_PROP static bool supportsImageEffects {
		return RenderTexture::IsEnabled() && (gGraphicsCaps.npotRT >= kNPOTRestricted);
	}

	// Are 3D (volume) textures supported? (RO)
	CUSTOM_PROP static bool supports3DTextures {
		return gGraphicsCaps.has3DTexture;
	}

	// Are compute shaders supported? (RO)
	CUSTOM_PROP static bool supportsComputeShaders {
		return gGraphicsCaps.hasComputeShader;
	}

	// Is GPU draw call instancing supported? (RO)
	CUSTOM_PROP static bool supportsInstancing {
		return gGraphicsCaps.hasInstancing;
	}

	CUSTOM_PROP static bool supportsSparseTextures {
		return (gGraphicsCaps.sparseTextures != kSparseTextureNone) && GetBuildSettings().hasAdvancedVersion;
	}

	// How many simultaneous render targets (MRTs) are supported? (RO)
	CUSTOM_PROP static int supportedRenderTargetCount {
		return RenderTexture::IsEnabled() ? gGraphicsCaps.maxMRTs : 0;
	}

	// Is the stencil buffer supported? (RO)
	CUSTOM_PROP static int supportsStencil {
		return gGraphicsCaps.hasStencil && GetBuildSettings ().hasAdvancedVersion;
	}

	// Is render texture format supported?
	CUSTOM static bool SupportsRenderTextureFormat (RenderTextureFormat format) {
		if(format < 0 || format >= kRTFormatCount)
		{
			Scripting::RaiseArgumentException("Failed SupportsRenderTextureFormat; format is not a valid RenderTextureFormat");
			return false;
		}

		return RenderTexture::IsEnabled() && gGraphicsCaps.supportsRenderTextureFormat[format];
	}

	CUSTOM static bool SupportsTextureFormat (TextureFormat format) {
		if(format <= 0 || format >= kTexFormatTotalCount)
		{
			Scripting::RaiseArgumentException("Failed SupportsTextureFormat; format is not a valid TextureFormat");
			return false;
		}

		return gGraphicsCaps.supportsTextureFormat[format];
	}

	/// What [[NPOT|NPOTSupport]] support does GPU provide? (RO)
	///
	/// SA: [[NPOTSupport]] enum.
	CUSTOM_PROP static NPOTSupport npotSupport { return gGraphicsCaps.npot; }

	//A unique device identifier. It is guaranteed to be unique for every device (RO).
	CUSTOM_PROP static string deviceUniqueIdentifier {
		return scripting_string_new (systeminfo::GetDeviceUniqueIdentifier ());
	}

	// The user defined name of the device (RO).
	CUSTOM_PROP static string deviceName {
		return scripting_string_new (systeminfo::GetDeviceName ());
	}

	// The model of the device (RO).
	CUSTOM_PROP static string deviceModel {
		return scripting_string_new (systeminfo::GetDeviceModel ());
	}

	// Returns a boolean value that indicates whether an accelerometer is
	CUSTOM_PROP static bool supportsAccelerometer {
		return systeminfo::SupportsAccelerometer ();
	}

	// Returns a boolean value that indicates whether a gyroscope is available
	CUSTOM_PROP static bool supportsGyroscope {
		return IsGyroAvailable ();
	}

	// Returns a boolean value that indicates whether the device is capable to
	CUSTOM_PROP static bool supportsLocationService {
		return systeminfo::SupportsLocationService ();
	}

	// Returns a boolean value that indicates whether the device is capable to
	CUSTOM_PROP static bool supportsVibration {
		return systeminfo::SupportsVibration ();
	}

	// Returns the kind of device the application is running on. See [[DeviceType]] enumeration for possible values.
	CUSTOM_PROP static DeviceType deviceType
	{
		return systeminfo::GetDeviceType ();
	}

	CUSTOM_PROP static int maxTextureSize
	{
		return gGraphicsCaps.maxTextureSize;
	}
*/

}
}