
namespace UnityEngine
{


// A UnityGUI event.

public class Event
{

	public static Event current
	{
		get {return null;}
	}
	public EventType rawType;
	public EventModifiers modifiers;
	public KeyCode keyCode;
	public void Use() {}
/*
CSRAW
	// *undocumented
	public Event () {
		Init ();
	}
	THREAD_SAFE
	CUSTOM private void Init () {
		InputEvent* newEvent = new InputEvent ();
		self.SetPtr(newEvent,CleanupInputEvent);
		newEvent->Init();
	}

	// Copy an event
	CSRAW public Event (Event other) {
		if (other == null)
			throw new ArgumentException ("Event to copy from is null.");
		InitCopy (other);
	}
	
	// *undocumented
	CSRAW private Event (IntPtr ptr) {
		InitPtr (ptr);
	}
	
	// *undocumented
	CSRAW ~Event ()
	{
		Cleanup ();
	}
	THREAD_SAFE
	CUSTOM private void Cleanup ()
	{
		CleanupInputEvent(self.GetPtr());
	}
	
	THREAD_SAFE
	CUSTOM private void InitCopy (Event other)
	{
		self.SetPtr(new InputEvent (*other), CleanupInputEvent);
	}
	
	THREAD_SAFE
	CUSTOM private void InitPtr (IntPtr ptr)
	{
		self.SetPtr((InputEvent*)ptr, CleanupInputEvent);
	}

	
CSRAW
	[System.NonSerialized]
	internal IntPtr m_Ptr;

	CUSTOM_PROP EventType rawType
		{ return self->type; }


	//@TODO: This is not useful when accessed in the new GUI. Should we do something about it?
	// The type of event.
	CUSTOM_PROP EventType type
		{ return IMGUI::GetEventType (GetGUIState(), *self); }
		{ self->type = value; }

	CUSTOM EventType GetTypeForControl (int controlID)
		{ return IMGUI::GetEventTypeForControl (GetGUIState(), *self, controlID); }


	// The mouse position.
	CSRAW public Vector2   mousePosition { get {
			Vector2 tmp; Internal_GetMousePosition(out tmp); return tmp; }
		set { Internal_SetMousePosition (value); }
	}
	CUSTOM private void Internal_SetMousePosition (Vector2 value)
	{
		self->mousePosition = value;
	}
	CUSTOM private void Internal_GetMousePosition (out Vector2 value)
	{
		*value = self->mousePosition;
	}


	// The relative movement of the mouse compared to last event.
	CSRAW public Vector2 delta { get {
			Vector2 tmp; Internal_GetMouseDelta(out tmp); return tmp; }
		set { Internal_SetMouseDelta (value); }
	}
	CUSTOM private void Internal_SetMouseDelta (Vector2 value)
	{
		self->delta = value;
	}
	CUSTOM private void Internal_GetMouseDelta (out Vector2 value)
	{
		*value = self->delta;
	}
	
	OBSOLETE error Use HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
	CSRAW public Ray mouseRay { get { return new Ray (Vector3.up, Vector3.up); } set {  }}
	
	// Which mouse button was pressed.
	CUSTOM_PROP int button
		{ return self->button; }
		{ self->button = value; }
	
	// Which modifier keys are held down.
	CUSTOM_PROP EventModifiers modifiers
		{ return self->modifiers; }
		{ self->modifiers = value; }
	
	// *undocumented*
	CUSTOM_PROP float pressure
		{ return self->pressure; }
		{ self->pressure = value; }

	// How many consecutive mouse clicks have we received.
	// Detects consecutive clicks and prints them.
	//
	CUSTOM_PROP int clickCount
		{ return self->clickCount; }
		{ self->clickCount = value; }
	
	// The character typed.
	// Detects characters pressed and prints them.
	//
	CUSTOM_PROP char character
		{ return self->character; }
		{ self->character = value; }
	
	// The name of an ExecuteCommand or ValidateCommand Event
	CUSTOM_PROP string commandName
		{
			char* commandString = self->commandString;
			return scripting_string_new(commandString == NULL ? "" : commandString);
		}
		{
#if ENABLE_MONO
			char* oldPtr = reinterpret_cast<char*>(self->commandString);
			delete[] oldPtr;
			char *str = mono_string_to_utf8 (value.GetScriptingString());
			self->commandString = new char[strlen (str) + 1];
			strncpy (self->commandString, str, strlen(str)+1);
#endif
		}

	// The raw key code for keyboard events.
	CUSTOM_PROP KeyCode keyCode
		{ return self->keycode; }
		{ self->keycode = value; }

	// Is Shift held down? (RO)
	CSRAW public bool shift { get {
			return (modifiers & EventModifiers.Shift)  != 0; }
		set { if (!value) modifiers &= ~EventModifiers.Shift; else modifiers |= EventModifiers.Shift; }
	}
	
	// Is Control key held down? (RO)
	CSRAW public bool control { get {
			return (modifiers & EventModifiers.Control)  != 0; }
		set { if (!value) modifiers &= ~EventModifiers.Control; else modifiers |= EventModifiers.Control; }
	}

	// Is Alt/Option key held down? (RO)
	CSRAW public bool alt { get {
			return (modifiers & EventModifiers.Alt)  != 0; }
		set { if (!value) modifiers &= ~EventModifiers.Alt; else modifiers |= EventModifiers.Alt; } }


	// Is Command/Windows key held down? (RO)
	CSRAW public bool command { get {
			return (modifiers & EventModifiers.Command)  != 0; }
		set { if (!value) modifiers &= ~EventModifiers.Command; else modifiers |= EventModifiers.Command; }
	}

	// Is Caps Lock on? (RO)
	CSRAW public bool capsLock { get {
			return (modifiers & EventModifiers.CapsLock)  != 0; }
		set { if (!value) modifiers &= ~EventModifiers.CapsLock; else modifiers |= EventModifiers.CapsLock; }
	}

	// Is the current keypress on the numeric keyboard? (RO)
	CSRAW public bool numeric { get {
			return (modifiers & EventModifiers.Numeric)  != 0; }
		set { if (!value) modifiers &= ~EventModifiers.Shift; else modifiers |= EventModifiers.Shift; }
	}

	// Is the current keypress a function key? (RO)
	CSRAW public bool functionKey { get { return (modifiers & EventModifiers.FunctionKey)  != 0; } }

	// The current event that's being processed right now.
	// TODO: set this to null outside the event loop.
	//
	CSRAW public static Event current { get {
			// return null if Event.current is queried outside OnGUI
			// Only in editor because of backwards compat.
			#if UNITY_EDITOR
				if (GUIUtility.Internal_GetGUIDepth () > 0)
					return s_Current;
				else
					return null;
			#else
				return s_Current;
			#endif
		}
		set
		{
			if (value != null)
				s_Current = value;
			else
				s_Current = s_MasterEvent;
			Internal_SetNativeEvent (s_Current.m_Ptr);
		}
	}
	CSRAW static Event s_Current;
	CSRAW static Event s_MasterEvent;

	CUSTOM static private void Internal_SetNativeEvent (IntPtr ptr)
	{
		GetGUIState().Internal_SetManagedEvent (ptr);
	}

	CSRAW static private void Internal_MakeMasterEventCurrent ()
	{
		if (s_MasterEvent == null)
			s_MasterEvent = new Event ();
		s_Current = s_MasterEvent;
		Internal_SetNativeEvent (s_MasterEvent.m_Ptr);
	}

	// Use this event.
	CUSTOM void Use () { self->Use(); }

	CUSTOM static bool PopEvent (Event outEvent)
	{
		return GetGUIEventManager().PopEvent(*outEvent);
	}

	CUSTOM static int GetEventCount ()
	{
		return GetGUIEventManager().GetEventCount();
	}
	
	// Is this event a keyboard event? (RO)
	CSRAW public bool isKey { get {
		EventType t = type; return t == EventType.KeyDown || t == EventType.KeyUp; }
	}

	// Is this event a mouse event? (RO)
	CSRAW public bool isMouse { get {
		EventType t = type; return t == EventType.MouseMove || t == EventType.MouseDown  || t == EventType.MouseUp || t == EventType.MouseDrag; }
	}
	
	// Create a keyboard event.
	CSRAW public static Event KeyboardEvent (string key) {
		return evt;
	}
	
	// Calculate the hash code
	CSRAW public override int GetHashCode( ) {
		int hc = 1;
		if (isKey)
			hc =  (ushort)keyCode;
		if (isMouse)
			hc = mousePosition.GetHashCode ();
		hc = hc*37 | (int)modifiers;
//		Debug.Log (hc + "  GetHashCode of " + ToString());
		return hc;
	}


	CSRAW public override bool Equals (object obj) {
		if (obj == null)
			return false;
		if (Object.ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != GetType())
			return false;

		Event rhs = (Event)obj;
		// We are ignoring Caps Lock for modifiers, so that Key Combinations will still work when Caps Lock is down.
		if (type != rhs.type || (modifiers & ~EventModifiers.CapsLock) != (rhs.modifiers & ~EventModifiers.CapsLock))
			return false;
		if (isKey)
			return keyCode == rhs.keyCode;
		if (isMouse)
			return mousePosition == rhs.mousePosition;
		return false;
	}
	
	
	
	CSRAW public override string ToString( )
	{
		if (isKey) {
			if ((int)character == 0)
				return UnityString.Format ("Event:{0}   Character:\\0   Modifiers:{1}   KeyCode:{2}", type, modifiers, keyCode);
			else {
				return UnityString.Format ("Event:" +type + "   Character:" + (int)(character) + "   Modifiers:" + modifiers + "   KeyCode:" + keyCode);
			}
		}
		if (isMouse)
			return UnityString.Format ("Event: {0}   Position: {1} Modifiers: {2}", type, mousePosition, modifiers);

		if (type == EventType.ExecuteCommand || type == EventType.ValidateCommand)
			return UnityString.Format ("Event: {0}  \"{1}\"", type, commandName);

		return "" + type;
	}

*/
}

}
