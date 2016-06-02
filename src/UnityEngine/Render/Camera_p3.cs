//partial class Camera
//{
	/*


	// Rendering path.
	AUTO_PROP RenderingPath renderingPath GetRenderingPath SetRenderingPath

	// Actually used rendering path (RO).
	CUSTOM_PROP RenderingPath actualRenderingPath { return self->CalculateRenderingPath(); }

	// High dynamic range rendering
	AUTO_PROP bool hdr CalculateUsingHDR SetHDR

	// Transparent object sorting mode.
	AUTO_PROP TransparencySortMode transparencySortMode GetSortMode SetSortMode

	// This is used to render parts of the scene selectively.
	AUTO_PROP int cullingMask GetCullingMask SetCullingMask

	CUSTOM_PROP static internal int PreviewCullingLayer
	{
		return Camera::kPreviewLayer;
	}

	// The event mask used by the camera.
	int eventMask GetEventMask SetEventMask

	// Where on the screen is the camera rendered in normalized coordinates.
	public Rect rect {
		get {return GetNormalizedViewportRect();} set {SetNormalizedViewportRect(value);}
	}

	// Where on the screen is the camera rendered in pixel coordinates.
	public Rect pixelRect {
		get {return GetScreenViewportRect();} set {SetScreenViewportRect(value);}
		}

	// Destination render texture __(Unity Pro only)__.
	AUTO_PTR_PROP RenderTexture targetTexture GetTargetTexture SetTargetTexture


	CUSTOM private void SetTargetBuffersImpl(out RenderBuffer color, out RenderBuffer depth)
	{
		self->SetTargetBuffersScript(1, color, depth);
	}

	CUSTOM private void SetTargetBuffersMRTImpl(RenderBuffer[] color, out RenderBuffer depth)
	{
		int count = GetScriptingArraySize(color);
		if (count < 1 || count > kMaxSupportedRenderTargets)
		{
			ErrorString ("Invalid color buffer count for SetTargetBuffers");
			return;
		}

		self->SetTargetBuffersScript(count, Scripting::GetScriptingArrayStart<ScriptingRenderBuffer>(color), depth);
	}

	CSRAW public void SetTargetBuffers(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
	{
		SetTargetBuffersImpl(out colorBuffer, out depthBuffer);
	}

	CSRAW public void SetTargetBuffers(RenderBuffer[] colorBuffer, RenderBuffer depthBuffer)
	{
		SetTargetBuffersMRTImpl(colorBuffer, out depthBuffer);
	}

	// How wide is the camera in pixels (RO).
	CUSTOM_PROP int pixelWidth { return self->GetScreenViewportRectInt ().Width(); }
	// How tall is the camera in pixels (RO).
	CUSTOM_PROP int pixelHeight { return self->GetScreenViewportRectInt ().Height(); }

	// Set a custom projection matrix.
	AUTO_PROP Matrix4x4 projectionMatrix GetProjectionMatrix SetProjectionMatrix

	// Get the world-space speed of the camera (RO).
	AUTO_PROP Vector3 velocity GetVelocity

	// Matrix that transforms from camera space to world space (RO).
	public Matrix4x4 cameraToWorldMatrix {
		get {return GetCameraToWorldMatrix();}
	}

	// Matrix that transforms from world to camera space.
	public Matrix4x4 worldToCameraMatrix {
		get {return GetWorldToCameraMatrix();} set {SetWorldToCameraMatrix(value);}
	}

	// Make the rendering position reflect the camera's position in the scene.
	public void ResetWorldToCameraMatrix ()
	{
	}

	// How the camera clears the background.
	AUTO_PROP CameraClearFlags clearFlags GetClearFlags SetClearFlags

	// Does this camera render stereoscopic 3d view from two virtual "eyes" ?
	AUTO_PROP bool stereoEnabled GetStereoEnabled

	// Distance between eyes
	AUTO_PROP float stereoSeparation GetStereoSeparation SetStereoSeparation

	// Distance to a point where eyes converge
	AUTO_PROP float stereoConvergence GetStereoConvergence SetStereoConvergence

	CONDITIONAL ENABLE_MULTIPLE_DISPLAYS
	// Auto Prop
	AUTO_PROP int targetDisplay GetTargetDisplay SetTargetDisplay

		// The camera we are currently rendering with, for low-level render control only (Read Only).
	CUSTOM_PROP static Camera current { return Scripting::ScriptingWrapperFor (GetCurrentCameraPtr()); }

	C++RAW static int GetAllCamerasCountInternal()
	{
		return GetRenderManager ().GetOffscreenCameras (). size() + GetRenderManager ().GetOnscreenCameras (). size();
	}

	C++RAW static int GetAllCamerasInternal(ScriptingArrayPtr array)
	{
		int numFilledOnScreen = FillScriptingArrayFromUnityObjects (array, GetRenderManager ().GetOnscreenCameras (), 0);
		int numFilledOffScreen = FillScriptingArrayFromUnityObjects (array, GetRenderManager ().GetOffscreenCameras (), numFilledOnScreen);

		return numFilledOnScreen + numFilledOffScreen;
	}

	// Returns all enabled cameras in the scene.
	CUSTOM_PROP static Camera[] allCameras
	{
		ScriptingArrayPtr scriptingCams = CreateScriptingArray<ScriptingObjectPtr>(GetScriptingManager ().GetCommonClasses ().camera, GetAllCamerasCountInternal());
		
		GetAllCamerasInternal(scriptingCams);

		return scriptingCams;
	}

	// Returns the size of the array that Camera.allCameras returns and the amount of cameras that Camera.GetAllCameras will fill.
	CUSTOM_PROP static int allCamerasCount
	{
		return GetAllCamerasCountInternal();
	}

	// Fills an array of Camera with the current allCameras, without allocating a new array.
	// The passed in array needs to be of minimum size according to allCamerasCount
	// When the array size is larger then the allCamerasCount value, only the first elements up to allCamerasCount will be filled up.
	// When the array size is smaller then the allCamerasCount value, an argument exception is thrown.
	// When the array argument passed in is null, this call will throw a nullreference exception.
	CUSTOM static int GetAllCameras(Camera[] cameras)
	{
		Scripting::RaiseIfNull(cameras);

		if(GetScriptingArraySize(cameras) < GetAllCamerasCountInternal())
			Scripting::RaiseArgumentException("Passed in array to fill with cameras is to small to hold the number of cameras. Use Camera.allCamerasCount to get the needed size.");
		
		return GetAllCamerasInternal(cameras);
	}


	// OnPreCull is called before a camera culls the scene.
	CSNONE void OnPreCull ();

	// OnPreRender is called before a camera starts rendering the scene.
	CSNONE void OnPreRender ();

	// OnPostRender is called after a camera has finished rendering the scene.
	CSNONE void OnPostRender ();

	// OnRenderImage is called after all rendering is complete to render image
	CSNONE void OnRenderImage (RenderTexture source, RenderTexture destination);

	// OnRenderObject is called after camera has rendered the scene.
	CSNONE void OnRenderObject ();

	// OnWillRenderObject is called once for each camera if the object is visible.

	CONVERTEXAMPLE
	BEGIN EX
	function OnWillRenderObject() {
		// Tint the object red for identification if it is
		// being shown on the overhead mini-map view.
		if (Camera.current.name == "MiniMapcam") {
			renderer.material.color = Color.red;
		} else {
			renderer.material.color = Color.white;
		}
	}
	END EX
	///
	CSNONE void OnWillRenderObject();


	CSRAW public delegate void CameraCallback (Camera cam);

	CSRAW public static CameraCallback onPreCull;
	CSRAW public static CameraCallback onPreRender;
	CSRAW public static CameraCallback onPostRender;

	CSRAW private static void FireOnPreCull (Camera cam)
	{
		if (onPreCull != null)
			onPreCull (cam);
	}
	CSRAW private static void FireOnPreRender (Camera cam)
	{
		if (onPreRender != null)
			onPreRender (cam);
	}
	CSRAW private static void FireOnPostRender (Camera cam)
	{
		if (onPostRender != null)
			onPostRender (cam);
	}


	// Render the camera manually.
	CUSTOM void Render () {
		self->StandaloneRender( Camera::kRenderFlagSetRenderTarget, NULL, "" );
	}

	// Render the camera with shader replacement.
	CUSTOM void RenderWithShader (Shader shader, string replacementTag) {
		self->StandaloneRender( Camera::kRenderFlagSetRenderTarget, shader, replacementTag );
	}

	// Make the camera render with shader replacement.
	CUSTOM void SetReplacementShader (Shader shader, string replacementTag) {
		self->SetReplacementShader( shader, replacementTag );
	}
	// Remove shader replacement from camera.
	AUTO void ResetReplacementShader ();

	AUTO_PROP bool useOcclusionCulling GetUseOcclusionCulling SetUseOcclusionCulling

	// These are only used by terrain engine impostor rendering and should be used with care!
	//*undoc*
	CUSTOM void RenderDontRestore() {
		self->StandaloneRender( Camera::kRenderFlagDontRestoreRenderState | Camera::kRenderFlagSetRenderTarget, NULL, "" );
	}

		//*undoc*
	CUSTOM static void SetupCurrent (Camera cur)
	{
		if (cur)
		{
			cur->StandaloneSetup(ShaderLab::GetDefaultPassContext());
		}
		else
		{
			GetRenderManager ().SetCurrentCamera (NULL);
			RenderTexture::SetActive(NULL);
		}
	}

	// Render into a static cubemap from this camera.
	CSRAW public bool RenderToCubemap (Cubemap cubemap, int faceMask = 63) {
		return Internal_RenderToCubemapTexture( cubemap, faceMask );
	}

	// Render into a cubemap from this camera.
	CSRAW public bool RenderToCubemap (RenderTexture cubemap, int faceMask = 63) {
		return Internal_RenderToCubemapRT ( cubemap, faceMask );
	}


	CUSTOM private bool Internal_RenderToCubemapRT( RenderTexture cubemap, int faceMask )
	{
		RenderTexture* rt = cubemap;
		if( !rt )
		{
			ErrorString( "Cubemap must not be null" );
			return false;
		}
		return self->StandaloneRenderToCubemap( rt, faceMask );
	}
	CUSTOM private bool Internal_RenderToCubemapTexture( Cubemap cubemap, int faceMask )
	{
		Cubemap* cube = cubemap;
		if( !cube )
		{
			ErrorString( "Cubemap must not be null" );
			return false;
		}
		return self->StandaloneRenderToCubemap( cube, faceMask );
	}


	// Per-layer culling distances.
	CUSTOM_PROP float[] layerCullDistances
	{
		return CreateScriptingArray(self->GetLayerCullDistances(), 32, GetScriptingManager().GetCommonClasses().floatSingle);
	}
	{
		Scripting::RaiseIfNull(value);
		if(GetScriptingArraySize(value) != 32)
		{
			Scripting::RaiseMonoException(" Array needs to contain exactly 32 floats for layerCullDistances.");
			return;
		}
		self->SetLayerCullDistances(Scripting::GetScriptingArrayStart<float> (value));
	}

	// How to perform per-layer culling for a Camera.
	CUSTOM_PROP bool layerCullSpherical { return self->GetLayerCullSpherical(); } { self->SetLayerCullSpherical(value); }
	
	// How and if camera generates a depth texture.
	CUSTOM_PROP DepthTextureMode depthTextureMode { return self->GetDepthTextureMode(); } { self->SetDepthTextureMode (value); }

	// Should the camera clear the stencil buffer after the lighting stage of the deferred rendering path?
	CUSTOM_PROP bool clearStencilAfterLightingPass { return self->GetClearStencilAfterLightingPass(); } { self->SetClearStencilAfterLightingPass (value); }

	CUSTOM internal bool IsFiltered (GameObject go) {
	#if UNITY_EDITOR
			return true;
//@TODO
//		return self->GetCuller().IsFiltered(*go);
	#else
		return true;
	#endif
	}

	
	// Adjusts the given projection matrix so that near plane is the given clipPlane
	// clipPlane is given in camera space. See article in Game Programming Gems 5 and
	// http://aras-p.info/texts/obliqueortho.html
	CUSTOM public Matrix4x4 CalculateObliqueMatrix(Vector4 clipPlane)
	{
		Matrix4x4f projection = self->GetProjectionMatrix();
		Matrix4x4f inversion = projection;	// Make a copy - inverse operation is in-place;
		
		#if UNITY_WP8 && !UNITY_EDITOR
		void UnRotateScreenIfNeeded(Matrix4x4f& projection);
		UnRotateScreenIfNeeded(inversion);
		#endif
		
		inversion.Invert_Full();
		
		Vector4f q = inversion.MultiplyVector4(Vector4f(
			(clipPlane.x > 0) - (clipPlane.x < 0),
			(clipPlane.y > 0) - (clipPlane.y < 0),
			1.0f,
			1.0f
		));
		
		Vector4f c = clipPlane * (2.0F / Dot(clipPlane, q));
		projection[2] = c.x - projection[3];
		projection[6] = c.y - projection[7];
		projection[10] = c.z - projection[11];
		projection[14] = c.w - projection[15];
		
		return projection;
	}
	*/
//}