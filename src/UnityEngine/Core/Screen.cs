using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public class Screen
	{
		public static int width {
			get {
				return Application.width;
			}
		}

		public static int height {
			get {
				return Application.height;
			}
		}

		public static float dpi {
			get {
				return 1;
			}
		}

		// Access to display information.

/*
	// All fullscreen resolutions supported by the monitor (RO).

	CUSTOM_PROP static Resolution[] resolutions
	{
		ScriptingClassPtr klass = GetScriptingManager ().GetCommonClasses ().resolution;
		ScreenManager::Resolutions resolutions = GetScreenManager ().GetResolutions ();
		ScriptingArrayPtr array = CreateScriptingArray<ScreenManager::Resolution> (klass, resolutions.size ());
		for (size_t i=0;i<resolutions.size ();i++)
			Scripting::SetScriptingArrayElement (array, i, resolutions[i]);

		return array;
	}

	CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	OBSOLETE error Property GetResolution has been deprecated. Use resolutions instead (UnityUpgradable).
	CSRAW static public Resolution[] GetResolution { get { return null; } }

	CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	OBSOLETE error Property showCursor has been deprecated. Use Cursor.visible instead (UnityUpgradable).
	CSRAW static public bool showCursor { get; set; }

	CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	OBSOLETE warning Property lockCursor has been deprecated. Use Cursor.lockState and Cursor.visible instead.
	CSRAW static public bool lockCursor
	{
		get{ return CursorLockMode.None == Cursor.lockState; }
		set
		{
			if (value)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}

	// The current screen resolution (RO).
	CUSTOM_PROP static Resolution currentResolution { return GetScreenManager().GetCurrentResolution (); }

	// Switches the screen resolution.
	CUSTOM static void SetResolution (int width, int height, bool fullscreen, int preferredRefreshRate = 0)
	{
		#if WEBPLUG
		if (fullscreen)
		{
			if (!GetScreenManager ().GetAllowFullscreenSwitch())
			{
				ErrorString("Fullscreen mode can only be enabled in the web player after clicking on the content.");
				return;
			}
			// when going from windowed to fullscreen, show escape warning
			if( !GetScreenManager().IsFullScreen() )
				ShowFullscreenEscapeWarning();
		}
		#endif

		GetScreenManager ().RequestResolution (width, height, fullscreen, preferredRefreshRate);
	}

	// The current width of the screen window in pixels (RO).
	THREAD_SAFE
	CUSTOM_PROP static int width { return GetScreenManager ().GetWidth (); }

	// The current height of the screen window in pixels (RO).
	THREAD_SAFE
	CUSTOM_PROP static int height { return GetScreenManager ().GetHeight (); }

	// The current DPI of the screen / device (RO).
	CUSTOM_PROP static float dpi { return GetScreenManager ().GetDPI (); }

	// Is the game running fullscreen?
	CUSTOM_PROP static bool fullScreen
	{
		return GetScreenManager ().IsFullScreen ();
	}
	{
		ScreenManager& screen = GetScreenManager();
		bool goFullscreen = (bool)value;
		if (goFullscreen  == screen.IsFullScreen ())
		{
			return;
		}

		#if WEBPLUG
		if (goFullscreen)
		{
			if (!GetScreenManager().GetAllowFullscreenSwitch())
			{
				ErrorString("Fullscreen mode can only be enabled in the web player after clicking on the content.");
				return;
			}
			// when going from windowed to fullscreen, show escape warning
			if( !screen.IsFullScreen() )
				ShowFullscreenEscapeWarning();
		}
		#endif

		screen.RequestSetFullscreen (goFullscreen);
	}

	// Allow auto-rotation to portrait?
	CUSTOM_PROP static bool autorotateToPortrait {
		return GetScreenManager().GetIsOrientationEnabled(kAutorotateToPortrait);
	} {
		GetScreenManager().SetIsOrientationEnabled(kAutorotateToPortrait, value);
	}

	// Allow auto-rotation to portrait, upside down?
	CUSTOM_PROP static bool autorotateToPortraitUpsideDown {
		return GetScreenManager().GetIsOrientationEnabled(kAutorotateToPortraitUpsideDown);
	} {
		GetScreenManager().SetIsOrientationEnabled(kAutorotateToPortraitUpsideDown, value);
	}

	// Allow auto-rotation to landscape left?
	CUSTOM_PROP static bool autorotateToLandscapeLeft {
		return GetScreenManager().GetIsOrientationEnabled(kAutorotateToLandscapeLeft);
	} {
		GetScreenManager().SetIsOrientationEnabled(kAutorotateToLandscapeLeft, value);
	}

	// Allow auto-rotation to landscape right?
	CUSTOM_PROP static bool autorotateToLandscapeRight {
		return GetScreenManager().GetIsOrientationEnabled(kAutorotateToLandscapeRight);
	} {
		GetScreenManager().SetIsOrientationEnabled(kAutorotateToLandscapeRight, value);
	}


	// Specifies logical orientation of the screen.
	CUSTOM_PROP static ScreenOrientation orientation {
		return GetScreenManager ().GetScreenOrientation ();
	} {
		GetScreenManager ().RequestOrientation (value);
	}

	// A power saving setting, allowing the screen to dim some time after the
	CUSTOM_PROP static int sleepTimeout {
		return GetScreenManager ().GetScreenTimeout ();
	} {
		GetScreenManager ().SetScreenTimeout (value);
	}
*/
	}
}

