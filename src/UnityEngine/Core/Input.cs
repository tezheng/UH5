using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;

namespace UnityEngine
{


// Describes phase of a finger touch.
public enum TouchPhase
{
	// A finger touched the screen.
	Began = 0,
	// A finger moved on the screen.
	Moved = 1,
	// A finger is touching the screen but hasn't moved.
	Stationary = 2,
	// A finger was lifted from the screen. This is the final phase of a touch.
	Ended = 3,
	// The system cancelled tracking for the touch, as when (for example) the user puts the device to her face or more than five touches happened simultaneously. This is the final phase of a touch.
	Canceled = 4
}

// Controls IME input
public enum IMECompositionMode
{
	// Enable IME input only when a text field is selected (default).
	Auto = 0,
	// Enable IME input.
	On = 1,
	// Disable IME input.
	Off = 2
}
/*
enum {
	kMaxJoySticks = 12,
	kMaxJoyStickButtons = 20,
	kMaxJoyStickAxis = 20
}

enum {
	kKeyCount = 323,
	kMouseButtonCount = 7,
	kKeyAndMouseButtonCount = kKeyCount + kMouseButtonCount,
	kKeyAndJoyButtonCount = kKeyAndMouseButtonCount + kMaxJoySticks * kMaxJoyStickButtons
}
*/
// Structure describing status of a finger touching the screen.
public struct Touch
{
	// The unique index for touch.
	public int fingerId {get; internal set;}

	// The position of the touch.
	public Vector2 position {get; internal set;}

	// The position delta since last change.
	public Vector2 deltaPosition {get; internal set;}

	// Amount of time passed since last change.
	public float deltaTime {get; internal set;}

	// Number of taps.
	public int tapCount {get; internal set;}

	// Describes the phase of the touch.
	public TouchPhase phase {get; internal set;}

	internal void Start(float x, float y)
	{
		phase = TouchPhase.Began;
		startPosition = new Vector2(x, y);
		UpdatePosition(x, y);
		deltaPosition = new Vector2(0, 0);
	}

	internal void Move(float x, float y)
	{
		if (startPosition.x == x && startPosition.y == y && phase != TouchPhase.Moved)
		{
			phase = TouchPhase.Stationary;
		}
		else
		{
			phase = TouchPhase.Moved;
		}
		UpdatePosition(x, y);
	}

	internal void Cancel(float x, float y)
	{
		phase = TouchPhase.Canceled;
		UpdatePosition(x, y);
	}

	internal void End(float x, float y)
	{
		phase = TouchPhase.Ended;
		UpdatePosition(x, y);
	}

	void UpdatePosition(float x, float y)
	{
		time = Time.time;
		if (x == -1000 && y == -1000) return;
		if (position.x != x || position.y != y)
		{
			deltaPosition = new Vector2(x, y) - position;
			position = new Vector2(x, y);
			if (lastChangedTime == 0) deltaTime = 0;
			else deltaTime = Time.time - lastChangedTime;
			lastChangedTime = Time.time;
		}
	}

	internal float time;
	Vector2 startPosition;
	float lastChangedTime;
}

// Interface into the Input system.
public partial class Input
{
	const int kMouseButtonCount = 7;
	// Returns whether the given mouse button is held down.
	public static bool GetMouseButton (int button)
	{
		if (button >= 0 && button < kMouseButtonCount)
		{
			return Application.GetInputManager().GetMouseButton(button);
			//return GetInputManager ().GetMouseButton (button);
		}
		else
		{
			return false;
		}
	}

	// Returns true during the frame the user pressed the given mouse button.
	public static bool GetMouseButtonDown (int button)
	{
		if (button >= 0 && button < kMouseButtonCount)
			return Application.GetInputManager().GetMouseButtonDown(button);
		else
		{
			return false;
		}
	}

	// Returns true during the frame the user releases the given mouse button.
	public static bool GetMouseButtonUp (int button)
	{
		if (button >= 0 && button < kMouseButtonCount)
			return Application.GetInputManager().GetMouseButtonUp(button);
		else
		{
			return false;
		}
	}

	// The current mouse position in pixel coordinates. (RO)
	public static Vector3 mousePosition
	{
		get {
			return Application.GetInputManager().GetMousePosition();
		}
	}

	public static Touch[] touches { get {
			int count = touchCount;
			Touch[] touches = new Touch[count];
			for (int q = 0; q < count; ++q)
				touches[q] = GetTouch (q);
			return touches;
		}
	}

	// Returns object representing status of a specific touch. (Does not allocate temporary variables)
	public static Touch GetTouch (int index)
	{
		if (index >= 0 && index < touchCount)
		{
			return Application.GetInputManager().GetTouch(index);
		}
		else
			return Application.GetInputManager().GetTouch(0);
	}

	// Number of touches. Guaranteed not to change throughout the frame. (RO)
	public static int touchCount {
		get {
			return Application.GetInputManager().GetTouchCount();
		}
	}


	// Hack.
	public static bool touchSupported
	{
		get
		{
			return Application.GetInputManager().GetTouchSupported();
		}
	}

	// Property indicating whether the system handles multiple touches.
	public static bool multiTouchEnabled { get {return false;} }

	public static float GetAxis(string name)
	{
//		Debug.LogError("Input.GetAxis not implemented.");
		return 0;
	}

	public static bool GetKeyDown (KeyCode key)
	{
//		Debug.LogError("Input.GetKey not implemented.");
		return false;
	}

	public static bool GetKeyUp (KeyCode key)
	{
//		Debug.LogError("Input.GetKey not implemented.");
		return false;
	}

	public static bool GetKey (KeyCode key)
	{
//		Debug.LogError("Input.GetKey not implemented.");
		return false;
	}

	public static IMECompositionMode imeCompositionMode;
	public static Vector2 compositionCursorPos;
	public static string compositionString;
	public static string inputString;
	/*
	// Controls enabling and disabling of IME input composition.
	CUSTOM_PROP static IMECompositionMode imeCompositionMode
	{ return GetInputManager().GetIMECompositionMode(); }
	{ GetInputManager().SetIMECompositionMode (value); }

	// The current IME composition string being typed by the user.
	CUSTOM_PROP static string compositionString { return scripting_string_new(GetInputManager ().GetCompositionString ()); }

	// Indicates if the user has an IME keyboard input source selected.
	CUSTOM_PROP static bool imeIsSelected { return (GetInputManager().GetIMEIsSelected()); }

	// The current text input position used by IMEs to open windows.
	CUSTOM_PROP static Vector2 compositionCursorPos
	{ return GetInputManager().GetTextFieldCursorPos (); }
	{ GetInputManager().GetTextFieldCursorPos () = value; }
	*/
}

}
