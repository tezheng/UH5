using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{

	public enum SendMessageOptions
	{
		// A receiver is required for SendMessage.
		RequireReceiver = 0,

		// No receiver is required for SendMessage.
		DontRequireReceiver = 1
	}

	public enum PrimitiveType
	{
		// A sphere primitive
		Sphere = 0,

		// A capsule primitive
		Capsule = 1,

		// A cylinder primitive
		Cylinder = 2,

		// A cube primitive
		Cube = 3,

		// A plane primitive
		Plane = 4,

		// A quad primitive
		Quad = 5
	}

	public enum Space
	{
		// Applies transformation relative to the world coordinate system
		World = 0,
		// Applies transformation relative to the local coordinate system
		Self = 1
	}

	// LayerMask allow you to display the LayerMask popup menu in the inspector
	public struct LayerMask
	{
		private int m_Mask;

		//*undocumented* TODO: make this actually work
		public static implicit operator int (LayerMask mask) { return mask.m_Mask; }

		// implicitly converts an integer to a LayerMask
		public static implicit operator LayerMask (int intVal) { LayerMask mask; mask.m_Mask = intVal; return mask; }

		// Converts a layer mask value to an integer value.
		public int value { get { return m_Mask; } set { m_Mask = value; } }

		// Given a set of layer names, returns the equivalent layer mask for all of them.
		public static int GetMask(params string[] layerNames)
		{
			int mask = 0;
			foreach (string name in layerNames)
			{
				int layer = NameToLayer (name);

				if (layer != 0)
					mask |= 1 << layer;
			}
			return mask;
		}

		internal static List<string> layernames = new List<string>();

		internal static void SetLayerSettings(List<System.Object> setting)
		{
			foreach (string layer in setting)
			{
				layernames.Add(layer);
			}
		}

		// TODO: Need to be implemented. Load from configs.
		// Will impl in TagManager.
		// Given a layer number, returns the name of the layer as defined in either a Builtin or a User Layer in the [[wiki:class-TagManager|Tag Manager]]
		public static string LayerToName (int layer) {
			return layernames[layer];
		}

		// Given a layer name, returns the layer index as defined by either a Builtin or a User Layer in the [[wiki:class-TagManager|Tag Manager]]
		public static int NameToLayer (string layerName)
		{
			for (var i = 0; i < 32; i++)
			{
				if (layernames[i] == layerName)
				{
					return i;
				}
			}
			return -1;
		}
	}

	public enum RuntimePlatform
	{
		// In the Unity editor on Mac OS X.
		OSXEditor = 0,

		// In the player on Mac OS X.
		OSXPlayer = 1,

		// In the player on Windows.
		WindowsPlayer = 2,

		// In the web player on Mac OS X.
		OSXWebPlayer = 3,

		// In the Dashboard widget on Mac OS X.
		OSXDashboardPlayer = 4,

		// In the web player on Windows.
		WindowsWebPlayer = 5,

		// In the Unity editor on Windows.
		WindowsEditor = 7,

		// In the player on the iPhone.
		IPhonePlayer = 8,

		// In the player on the XBOX360
		XBOX360 = 10,

		// In the player on the Play Station 3
		PS3 = 9,

		// In the player on Android devices.
		Android = 11,

		NaCl = 12,

		FlashPlayer = 15,

		//*undocumented*
		LinuxPlayer = 13,

		WebGLPlayer = 17,

		//*undocumented*
		MetroPlayerX86 = 18,
		WSAPlayerX86 = 18,

		//*undocumented*
		MetroPlayerX64 = 19,
		WSAPlayerX64 = 19,

		//*undocumented*
		MetroPlayerARM = 20,
		WSAPlayerARM = 20,

		//*undocumented*
		WP8Player = 21,
		BB10Player = 22,
		BlackBerryPlayer = 22,

		//*undocumented*
		TizenPlayer = 23,

		// In the player on PS Vita
		PSP2 = 24,

		// In the player on PS4
		PS4 = 25,

		// In the player on PSM
		PSM = 26,

		// In the player on XboxOne
		XboxOne = 27,

		//*undocumented*
		SamsungTVPlayer = 28,
	}

	// The language the user's operating system is running in. Returned by Application.systemLanguage.
	public enum SystemLanguage
	{
		//Afrikaans
		Afrikaans = 0,
		//Arabic
		Arabic = 1,
		//Basque
		Basque = 2,
		//Belarusian
		Belarusian = 3,
		//Bulgarian
		Bulgarian = 4,
		//Catalan
		Catalan = 5,
		//Chinese
		Chinese = 6,
		//Czech
		Czech = 7,
		//Danish
		Danish = 8,
		//Dutch
		Dutch = 9,
		//English
		English = 10,
		//Estonian
		Estonian = 11,
		//Faroese
		Faroese = 12,
		//Finnish
		Finnish = 13,
		//French
		French = 14,
		//German
		German = 15,
		//Greek
		Greek = 16,
		//Hebrew
		Hebrew = 17,
		//*undocumented*
		Hugarian = 18,
		//Icelandic
		Icelandic = 19,
		//Indonesian
		Indonesian = 20,
		//Italian
		Italian = 21,
		//Japanese
		Japanese = 22,
		//Korean
		Korean = 23,
		//Latvian
		Latvian = 24,
		//Lithuanian
		Lithuanian = 25,
		//Norwegian
		Norwegian = 26,
		//Polish
		Polish = 27,
		//Portuguese
		Portuguese = 28,
		//Romanian
		Romanian = 29,
		//Russian
		Russian = 30,
		//Serbo-Croatian
		SerboCroatian = 31,
		//Slovak
		Slovak = 32,
		//Slovenian
		Slovenian = 33,
		//Spanish
		Spanish = 34,
		//Swedish
		Swedish = 35,
		//Thai
		Thai = 36,
		//Turkish
		Turkish = 37,
		//Ukrainian
		Ukrainian = 38,
		//Vietnamese
		Vietnamese = 39,
		//Chinese-Simplified
		ChineseSimplified = 40,
		//Chinese-Traditional
		ChineseTraditional = 41,
		//Unknown
		Unknown = 42,
		//Hungarian
		Hungarian = 18
	}

	public enum LogType
	{
		// LogType used for Errors.
		Error = 0,
		// LogType used for Asserts. (These indicate an error inside Unity itself.)
		Assert = 1,
		// LogType used for Warnings.
		Warning = 2,
		// LogType used for regular log messages.
		Log = 3,
		// LogType used for Exceptions.
		Exception = 4
	}

	public enum DeviceType
	{
		// Device type is unknown. You should never see this in practice.
		Unknown = 0,

		// A handheld device like mobile phone or a tablet.
		Handheld = 1,

		// A stationary gaming console.
		Console = 2,

		// Desktop or laptop computer.
		Desktop = 3,
	}

	enum AwakeFromLoadMode
	{
		kDefaultAwakeFromLoad,
		kDidLoadFromDisk,
		kActivateAwakeFromLoad,
		kInstantiateOrCreateFromCodeAwakeFromLoad,
	}

/*
// Suspends the coroutine execution for the given amount of seconds.
CSRAW [StructLayout(LayoutKind.Sequential)]
CLASS WaitForSeconds : YieldInstruction
	//*undocumented*
	CSRAW internal float m_Seconds;

	// Creates a yield instruction to wait for a given number of seconds
	CSRAW public WaitForSeconds (float seconds) { m_Seconds = seconds; }
END

// Waits until next fixed frame rate update function. SA: MonoBehaviour::pref::FixedUpdate.
CLASS WaitForFixedUpdate : YieldInstruction
END

// Waits until the end of the frame after all cameras and GUI is rendered, just before displaying the frame on screen.
CLASS WaitForEndOfFrame : YieldInstruction
END


// MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this class are only used to reference these coroutines and do not hold any exposed properties or functions.
CSRAW [StructLayout (LayoutKind.Sequential)]

CLASS Coroutine : YieldInstruction
	CSRAW internal IntPtr m_Ptr;
	private Coroutine () { }

	THREAD_SAFE
	CUSTOM private void ReleaseCoroutine ()
	{
		Assert (self.GetPtr() != NULL);
		Coroutine::CleanupCoroutineGC (self);
	}

	CSRAW
	~Coroutine ()
	{
		ReleaseCoroutine ();
	}
END
*/
}

