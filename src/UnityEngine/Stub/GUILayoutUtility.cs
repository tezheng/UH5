using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine 
{

/*
// Utility functions for implementing and extending the GUILayout class.
NONSEALED_CLASS GUILayoutUtility 
CSRAW
	
	CLASS internal  LayoutCache 
		CSRAW
		//*undocumented*
		internal GUILayoutGroup topLevel = new GUILayoutGroup ();
		//*undocumented*
		internal UnityEngineInternal.GenericStack layoutGroups = new UnityEngineInternal.GenericStack ();
		//*undocumented*
		internal GUILayoutGroup windows = new GUILayoutGroup ();
		
		//*undocumented*
		internal LayoutCache () {
			layoutGroups.Push (topLevel);
		}
		
		internal LayoutCache (LayoutCache other)
		{
			topLevel = other.topLevel;
			layoutGroups = other.layoutGroups;
			windows = other.windows;
		}
	END
	
	// TODO: Clean these up after a while
	static Dictionary<int, LayoutCache> storedLayouts = new Dictionary<int, LayoutCache> ();
	static Dictionary<int, LayoutCache> storedWindows = new Dictionary<int, LayoutCache> ();

	static internal LayoutCache current = new LayoutCache ();

	static Rect kDummyRect = new Rect (0, 0, 1, 1);

	CSRAW static internal LayoutCache SelectIDList (int instanceID, bool isWindow) {
		Dictionary<int, LayoutCache> store = isWindow ? storedWindows : storedLayouts;
		LayoutCache cache;
		if (store.TryGetValue(instanceID, out cache) == false) {
//			Debug.Log ("Creating ID " +instanceID + " " + Event.current.type);
			cache = new LayoutCache();
			store[instanceID] = cache;
		} else {
//			Debug.Log ("reusing ID " +instanceID + " " + Event.current.type);
		}
		current.topLevel = cache.topLevel;
		current.layoutGroups = cache.layoutGroups;
		current.windows = cache.windows;
		return cache;
	}

	// Set up the internal GUILayouting
	// Called by the main GUI class automatically (from GUI.Begin)
	CSRAW static internal void Begin (int instanceID) {
		LayoutCache cache = SelectIDList (instanceID, false);
		// Make a vertical group to encompass the whole thing
		if (Event.current.type == EventType.Layout) {
			current.topLevel = cache.topLevel = new GUILayoutGroup ();
			current.layoutGroups.Clear ();
			current.layoutGroups.Push (current.topLevel);
			current.windows = cache.windows = new GUILayoutGroup ();
		} else {
			current.topLevel = cache.topLevel;
			current.layoutGroups = cache.layoutGroups;
			current.windows = cache.windows;
		}
	}
	
	CSRAW static internal void BeginWindow (int windowID, GUIStyle style, GUILayoutOption[] options) {
		LayoutCache cache = SelectIDList (windowID, true);
		// Make a vertical group to encompass the whole thing
		if (Event.current.type == EventType.Layout) {
			current.topLevel = cache.topLevel = new GUILayoutGroup ();
			current.topLevel.style = style;
			current.topLevel.windowID = windowID;
			if (options != null)
				current.topLevel.ApplyOptions (options);
			current.layoutGroups.Clear ();
			current.layoutGroups.Push (current.topLevel);
			current.windows = cache.windows = new GUILayoutGroup ();
		} else {
			current.topLevel = cache.topLevel;
			current.layoutGroups = cache. layoutGroups;
			current.windows = cache.windows;
		}
	}

	// TODO: actually make these check...
	// *undocumented*
	CSRAW static public void BeginGroup (string GroupName) {}
	// *undocumented*
	CSRAW static public void EndGroup (string groupName) {}


	static internal void Layout () {
		if (current.topLevel.windowID == -1) {
			// Normal GUILayout.whatever -outside beginArea calls.
			// Here we go over all entries and calculate their sizes
			current.topLevel.CalcWidth ();
			current.topLevel.SetHorizontal (0, Mathf.Min (Screen.width, current.topLevel.maxWidth));
			current.topLevel.CalcHeight ();
			current.topLevel.SetVertical (0, Mathf.Min (Screen.height, current.topLevel.maxHeight));

// UNCOMMENT ME TO DEBUG THE ROOT LAYOUT RESULTS
//			Debug.Log ("ROOT: " + current.topLevel);
			// Layout all beginarea parts...
			LayoutFreeGroup (current.windows);
		} else {
			LayoutSingleGroup (current.topLevel);
			LayoutFreeGroup (current.windows);
//			Debug.Log ("Windows: " + current.windows);
		}
	}
	
	// Global fayout function. Called from EditorWindows (differs from game view in that they use the full window size and try to stretch GUI
	// *undocumented*
	static internal void LayoutFromEditorWindow () {
		current.topLevel.CalcWidth ();
		current.topLevel.SetHorizontal (0, Screen.width);
		current.topLevel.CalcHeight ();
		current.topLevel.SetVertical (0, Screen.height);

// UNCOMMENT ME TO DEBUG THE EditorWindow ROOT LAYOUT RESULTS
//			Debug.Log (current.topLevel);
		// Layout all beginarea parts...
		LayoutFreeGroup (current.windows);
	}


	// Global layout function. Calculates all sizes of all windows etc & assigns.
	// After this call everything has a properly calculated size
	// Called by Unity automatically.
	// Is public so we can access it from editor inspectors, but not supported by public stuff
	// *undocumented*
	static internal float LayoutFromInspector (float width) {
		if (current.topLevel != null && current.topLevel.windowID == -1) {
			// Here we go over all entries and calculate their sizes
			current.topLevel.CalcWidth ();
			current.topLevel.SetHorizontal (0, width);
			current.topLevel.CalcHeight ();
			current.topLevel.SetVertical (0, Mathf.Min (Screen.height, current.topLevel.maxHeight));
// UNCOMMENT ME TO DEBUG THE INSPECTOR
//			Debug.Log (current.topLevel);
			float height = ((GUILayoutGroup)current.topLevel).minHeight;
			// Layout all beginarea parts...
			// TODO: NOT SURE HOW THIS WORKS IN AN INSPECTOR
			LayoutFreeGroup (current.windows);
			return height;
		}
		else {
			if (current.topLevel != null)
				LayoutSingleGroup (current.topLevel);
			return 0;
		}
	}
	
	static internal void LayoutFreeGroup (GUILayoutGroup toplevel) {
		foreach (GUILayoutGroup i in toplevel.entries) {
			LayoutSingleGroup (i);
		}
		toplevel.ResetCursor ();
	}
	
	static void LayoutSingleGroup (GUILayoutGroup i) {
		if (!i.isWindow) {
			// CalcWidth knocks out minWidth with the calculated sizes from its children. Normally, this is fine, but since we're in a fixed-size area,
			// we want to maintain that (godammit)
			float origMinWidth = i.minWidth;
			float origMaxWidth = i.maxWidth;

			// Figure out the group's min & maxWidth.
			i.CalcWidth ();

			// Make it as wide as possible, but the Rect supplied takes precedence...
			i.SetHorizontal (i.rect.x, Mathf.Clamp (i.maxWidth, origMinWidth, origMaxWidth));

			// Do the same preservation for CalcHeight...
			float origMinHeight = i.minHeight;
			float origMaxHeight = i.maxHeight;
			
			i.CalcHeight ();
			// Make it as high as possible, but the Rect supplied takes precedence...
			i.SetVertical (i.rect.y, Mathf.Clamp (i.maxHeight, origMinHeight, origMaxHeight));
			
// UNCOMMENT ME TO SEE BEGINAREA/ENDAREA BLOCKS			
//			Debug.Log (i);
		} else {
			// Figure out the group's min & maxWidth.
			i.CalcWidth ();

			Rect winRect = Internal_GetWindowRect (i.windowID);

			// Make it as wide as possible, but the Rect supplied takes precedence...
			i.SetHorizontal (winRect.x, Mathf.Clamp (winRect.width, i.minWidth, i.maxWidth));

			i.CalcHeight ();
			
			// Make it as high as possible, but the Rect supplied takes precedence...
			i.SetVertical (winRect.y, Mathf.Clamp (winRect.height, i.minHeight, i.maxHeight));

			// If GUILayout did any resizing, make sure the window reflects this.
			
			Internal_MoveWindow (i.windowID, i.rect);
		}
			}
	
	CUSTOM static private Rect Internal_GetWindowRect (int windowID)
	{
		return IMGUI::GetWindowRect (GetGUIState (), windowID);
	}
	
	CUSTOM static private void Internal_MoveWindow (int windowID, Rect r)
	{
		IMGUI::MoveWindowFromLayout (GetGUIState (), windowID, r);
	}

	CUSTOM static internal Rect GetWindowsBounds ()
	{
		return IMGUI::GetWindowsBounds (GetGUIState ());
	}
	
	CSRAW 
	[System.Security.SecuritySafeCritical]
	static GUILayoutGroup CreateGUILayoutGroupInstanceOfType(System.Type LayoutType)
	{
		if (!typeof(GUILayoutGroup).IsAssignableFrom(LayoutType))
			throw new ArgumentException("LayoutType needs to be of type GUILayoutGroup");
		return (GUILayoutGroup)System.Activator.CreateInstance (LayoutType);
	}
	
	// Generic helper - use this when creating a layoutgroup. It will make sure everything is wired up correctly.
	internal static GUILayoutGroup BeginLayoutGroup (GUIStyle style, GUILayoutOption[] options, System.Type LayoutType) {
		GUILayoutGroup g;
		switch (Event.current.type) {
		case EventType.Used:
		case EventType.Layout:
			g = CreateGUILayoutGroupInstanceOfType (LayoutType);
			g.style = style;
			if (options != null)
				g.ApplyOptions (options);
			current.topLevel.Add (g);
			break;
		default:
			g = current.topLevel.GetNext() as GUILayoutGroup;
			if (g == null)
				throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
			g.ResetCursor ();
			break;
		}
		current.layoutGroups.Push (g);
		current.topLevel = g;
		return g;
	}	

	// The matching end for BeginLayoutGroup
	internal static void EndLayoutGroup () {
		switch (Event.current.type) {
		default:		
			current.layoutGroups.Pop ();
			current.topLevel = (GUILayoutGroup)current.layoutGroups.Peek ();
			return;
		}
	}

	// Generic helper - use this when creating a layoutgroup. It will make sure everything is wired up correctly.
	internal static GUILayoutGroup BeginLayoutArea (GUIStyle style, System.Type LayoutType) {
		GUILayoutGroup g;
		switch (Event.current.type) {
		case EventType.Used:
		case EventType.Layout:
			g = CreateGUILayoutGroupInstanceOfType (LayoutType);
			g.style = style;
			current.windows.Add (g);
			break;
		default:
			g = current.windows.GetNext() as GUILayoutGroup;
			if (g == null)
				throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
			g.ResetCursor ();
			break;
		}
		current.layoutGroups.Push (g);
		current.topLevel = g;
		return g;
	}

	// Trampoline for Editor stuff
	//*undocumented*
	internal static GUILayoutGroup DoBeginLayoutArea (GUIStyle style, System.Type LayoutType) {
		return BeginLayoutArea (style, LayoutType);
	}
	internal static GUILayoutGroup topLevel {
		get { return current.topLevel; }
	}


	/// *listonly*
	CSRAW public static Rect GetRect (GUIContent content, GUIStyle style)									{ return DoGetRect (content, style, null); }
	// Reserve layout space for a rectangle for displaying some contents with a specific style.
	CSRAW public static Rect GetRect (GUIContent content, GUIStyle style, params GUILayoutOption[] options)		{ return DoGetRect (content, style, options); }
	CSRAW static Rect DoGetRect (GUIContent content, GUIStyle style, GUILayoutOption[] options) {
		GUIUtility.CheckOnGUI();
		
		switch (Event.current.type) {
		case EventType.Layout: {
			if (style.isHeightDependantOnWidth) {
				current.topLevel.Add (new GUIWordWrapSizer (style, content, options));
			} else {
				Vector2 size = style.CalcSize (content);
				current.topLevel.Add (new GUILayoutEntry (size.x, size.x, size.y, size.y, style, options));
			}
			return kDummyRect;
		}
		case EventType.Used:
			return kDummyRect;
		default: 
			return current.topLevel.GetNext ().rect;
		}
	}

	/// *listonly*
	CSRAW public static Rect GetRect (float width, float height)								{ return DoGetRect (width, width, height, height, GUIStyle.none, null);}
	/// *listonly*
	CSRAW public static Rect GetRect (float width, float height, GUIStyle style)					{return DoGetRect (width, width, height, height, style, null);}
	/// *listonly*
	CSRAW public static Rect GetRect (float width, float height, params GUILayoutOption[] options)	{return DoGetRect (width, width, height, height, GUIStyle.none, options);}
	// Reserve layout space for a rectangle with a fixed content area.
	CSRAW public static Rect GetRect (float width, float height, GUIStyle style, params GUILayoutOption[] options)			
		{return DoGetRect (width, width, height, height, style, options);}



	/// *listonly*
	CSRAW public static Rect GetRect (float minWidth, float maxWidth, float minHeight, float maxHeight) 
		{ return DoGetRect (minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, null); }
	/// *listonly*
	CSRAW public static Rect GetRect (float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style) 
		{ return DoGetRect (minWidth, maxWidth, minHeight, maxHeight, style, null); }
	/// *listonly*
	CSRAW public static Rect GetRect (float minWidth, float maxWidth, float minHeight, float maxHeight, params GUILayoutOption[] options) 
		{ return DoGetRect (minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options); }
	// Reserve layout space for a flexible rect.
	CSRAW public static Rect GetRect (float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, params GUILayoutOption[] options) 
		{ return DoGetRect (minWidth, maxWidth, minHeight, maxHeight, style, options); }
	CSRAW static Rect DoGetRect (float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, GUILayoutOption[] options) {
		switch (Event.current.type) {
		case EventType.Layout:
			current.topLevel.Add (new GUILayoutEntry (minWidth, maxWidth, minHeight, maxHeight, style, options));
			return kDummyRect;
		case EventType.Used:
			return kDummyRect;
		default:
			return current.topLevel.GetNext ().rect;
		}
	}
	
	// Get the rectangle last used by GUILayout for a control.
	CSRAW public static Rect GetLastRect () {
		switch (Event.current.type) {
		case EventType.Layout:
			return kDummyRect;
		case EventType.Used:
			return kDummyRect;
		default:
			return current.topLevel.GetLast ();
		}
	}
	
	/// *listonly*
	CSRAW public static Rect GetAspectRect (float aspect)											{ return DoGetAspectRect (aspect, GUIStyle.none, null); }
	/// *listonly*
	CSRAW public static Rect GetAspectRect (float aspect, GUIStyle style)								{ return DoGetAspectRect (aspect, style, null); }
	/// *listonly*
	CSRAW public static Rect GetAspectRect (float aspect, params GUILayoutOption[] options)				{  return DoGetAspectRect (aspect, GUIStyle.none, options); }
	// Reserve layout space for a rectangle with a specific aspect ratio.
	CSRAW public static Rect GetAspectRect (float aspect, GUIStyle style, params GUILayoutOption[] options)	{  return DoGetAspectRect (aspect, GUIStyle.none, options); }
	static Rect DoGetAspectRect (float aspect, GUIStyle style, GUILayoutOption[] options) {
	switch (Event.current.type) {
		case EventType.Layout: 
			current.topLevel.Add (new GUIAspectSizer (aspect, options));
			return kDummyRect;
		case EventType.Used:
			return kDummyRect;			
		default: 
			return current.topLevel.GetNext ().rect;
		}
	}
	
	// Style used by space elements so we can do special handling of spaces.
	CSRAW internal static GUIStyle spaceStyle 
	{ 
		get 
		{ 
			if (s_SpaceStyle == null) s_SpaceStyle = new GUIStyle();
			s_SpaceStyle.stretchWidth = false;
			return s_SpaceStyle; 
		} 
	}
	static GUIStyle s_SpaceStyle;
END


// Basic layout element
NONSEALED_CLASS internal GUILayoutEntry	
	CSRAW
	// The min and max sizes. Used during calculations...
	CSRAW public float minWidth, maxWidth, minHeight, maxHeight;
	
	// The rectangle that this element ends up having
	CSRAW public Rect rect = new Rect (0,0,0,0);

	// Can this element stretch?
	CSRAW public int stretchWidth, stretchHeight;
	
	// The style to use.
	CSRAW GUIStyle m_Style = GUIStyle.none;
	CSRAW public GUIStyle style { get { return m_Style; } set { m_Style = value; ApplyStyleSettings (value); } }
	
	CSRAW internal static Rect kDummyRect = new Rect (0,0,1,1);
	
	// The margins of this element.
	public virtual RectOffset margin { get {
		return style.margin;
	}}
	
	public GUILayoutEntry (float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style) {
		minWidth = _minWidth;
		maxWidth = _maxWidth;
		minHeight = _minHeight;
		maxHeight = _maxHeight;
		if (_style == null)
			_style = GUIStyle.none;
		style = _style;
	}

	public GUILayoutEntry (float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style, GUILayoutOption[] options) {
		minWidth = _minWidth;
		maxWidth = _maxWidth;
		minHeight = _minHeight;
		maxHeight = _maxHeight;
		style = _style;
		ApplyOptions (options);
	}
	
	public virtual void CalcWidth () {}
	public virtual void CalcHeight () {}
	public virtual void SetHorizontal (float x, float width) { rect.x = x; rect.width = width; }
	public virtual void SetVertical (float y, float height) { rect.y = y; rect.height = height; }

	protected virtual void ApplyStyleSettings (GUIStyle style) {
		stretchWidth = (style.fixedWidth == 0 && style.stretchWidth) ? 1 : 0;
		stretchHeight = (style.fixedHeight == 0  && style.stretchHeight) ? 1 : 0;
		m_Style = style;
	}

	public virtual void ApplyOptions (GUILayoutOption[] options) {
		if (options == null)
			return;
		foreach (GUILayoutOption i in options) {
			switch (i.type) {
				case GUILayoutOption.Type.fixedWidth:			minWidth = maxWidth = (float)i.value; stretchWidth = 0; break;
				case GUILayoutOption.Type.fixedHeight:			minHeight = maxHeight = (float)i.value; stretchHeight = 0; break;
				case GUILayoutOption.Type.minWidth:			minWidth = (float)i.value; if (maxWidth < minWidth) maxWidth = minWidth; break;
				case GUILayoutOption.Type.maxWidth:			maxWidth = (float)i.value; if (minWidth > maxWidth) minWidth = maxWidth; stretchWidth = 0; break;
				case GUILayoutOption.Type.minHeight:			minHeight = (float)i.value; if (maxHeight < minHeight) maxHeight = minHeight; break;
				case GUILayoutOption.Type.maxHeight:			maxHeight = (float)i.value; if (minHeight > maxHeight) minHeight = maxHeight; stretchHeight = 0; break;
				case GUILayoutOption.Type.stretchWidth:		stretchWidth = (int)i.value; break;
				case GUILayoutOption.Type.stretchHeight:		stretchHeight = (int)i.value; break;
			}
		}
		if (maxWidth != 0 && maxWidth < minWidth)
			maxWidth = minWidth;
		if (maxHeight != 0 && maxHeight < minHeight)
			maxHeight = minHeight;
		
	}
	
	protected static int indent = 0;
	public override string ToString () {
		string space = "";
		for (int i = 0; i < indent; i++) 
			space += " ";
		return space + UnityString.Format ("{1}-{0} (x:{2}-{3}, y:{4}-{5})", style != null ? style.name : "NULL", GetType(), rect.x, rect.xMax, rect.y, rect.yMax) +  
			  "   -   W: " + minWidth + "-" + maxWidth + (stretchWidth != 0 ? "+" : "") + ", H: " + minHeight + "-" + maxHeight + (stretchHeight  != 0 ? "+" : "");
	}
END

// *undocumented*
NONSEALED_CLASS internal GUILayoutGroup : GUILayoutEntry 
	CSRAW
	public List<GUILayoutEntry> entries = new List<GUILayoutEntry>();
	public bool isVertical = true;					// Is this group vertical
	public bool resetCoords = false;				// Reset coordinate for GetRect. Used for groups that are part of a window
	public float spacing = 0;						// Spacing between the elements contained within
	public bool sameSize = true;					// Are all subelements the same size
	public bool isWindow = false;					// Is this a window at all?
	public int windowID = -1;							// Optional window ID for toplevel windows. Used by Layout to tell GUI.Window of size changes...
	int cursor = 0;
	protected int stretchableCountX = 100, stretchableCountY = 100;
	protected bool userSpecifiedWidth = false, userSpecifiedHeight = false; 
	// Should all elements be the same size?
	// TODO: implement
//	CSRAW bool equalSize = false;

	// The summed sizes of the children. This is used to determine whether or not the children should be stretched
	CSRAW protected float childMinWidth = 100, childMaxWidth = 100, childMinHeight = 100, childMaxHeight = 100;
	
	// How are subelements justified along the minor direction?
	// TODO: implement
//	CSRAW enum Align { start, middle, end, justify }
//	CSRAW Align align;

	RectOffset m_Margin = new RectOffset();
	public override RectOffset margin { get {
		return m_Margin;
	}}


	public GUILayoutGroup () : base (0,0,0,0,GUIStyle.none) {}

#if !UNITY_FLASH && !UNITY_WEBGL
	public GUILayoutGroup (GUIStyle _style, GUILayoutOption[] options) : base (0,0,0,0, _style) {
		if (options != null)
			ApplyOptions (options);
		m_Margin.left  = _style.margin.left;
		m_Margin.right  = _style.margin.right;
		m_Margin.top  = _style.margin.top;
		m_Margin.bottom  = _style.margin.bottom;
	}
#endif		

	public override void ApplyOptions (GUILayoutOption[] options) {
		if (options == null)
			return;
		base.ApplyOptions (options);
		foreach (GUILayoutOption i in options) {
			switch (i.type) {
				case GUILayoutOption.Type.fixedWidth:		
				case GUILayoutOption.Type.minWidth:		
				case GUILayoutOption.Type.maxWidth:		
					userSpecifiedHeight = true;
					break;
				case GUILayoutOption.Type.fixedHeight:		
				case GUILayoutOption.Type.minHeight:		
				case GUILayoutOption.Type.maxHeight:		
					userSpecifiedWidth = true;
					break;
// TODO:
//				case GUILayoutOption.Type.alignStart: 		align = Align.start; break;
//				case GUILayoutOption.Type.alignMiddle:		align = Align.middle; break;
//				case GUILayoutOption.Type.alignEnd:		align = Align.end; break;
//				case GUILayoutOption.Type.alignJustify:		align = Align.justify; break;
//				case GUILayoutOption.Type.equalSize:		equalSize = true; break;
				case GUILayoutOption.Type.spacing:		spacing = (int)i.value; break;
			}
		}
	}

	protected override void ApplyStyleSettings (GUIStyle style) {
		base.ApplyStyleSettings (style);
		RectOffset mar = style.margin;
		m_Margin.left = mar.left;
		m_Margin.right = mar.right;
		m_Margin.top = mar.top;
		m_Margin.bottom = mar.bottom;
	}

	public void ResetCursor () { cursor = 0; }

	public Rect PeekNext () {
		if(cursor < entries.Count) {
			GUILayoutEntry e = (GUILayoutEntry)entries[cursor];
			return e.rect;
		} else {
			throw new ArgumentException("Getting control " + cursor + "'s position in a group with only " + entries.Count + " controls when doing " + Event.current.rawType + "\nAborting");
		}
	}
	
	public GUILayoutEntry GetNext () {
		if(cursor < entries.Count) {
			GUILayoutEntry e = (GUILayoutEntry)entries[cursor];
			cursor++;
			return e;
		} else {
			throw new ArgumentException("Getting control " + cursor + "'s position in a group with only " + entries.Count + " controls when doing " + Event.current.rawType + "\nAborting");
		}
	}

	//* undocumented
	public Rect GetLast () {
		if (cursor == 0) {
			Debug.LogError ("You cannot call GetLast immediately after beginning a group.");
			return kDummyRect;
		}
		if(cursor <= entries.Count) {
			GUILayoutEntry e = (GUILayoutEntry)entries[cursor - 1];
			return e.rect;
		} else {
			Debug.LogError ("Getting control " + cursor + "'s position in a group with only " + entries.Count + " controls when doing " + Event.current.type);
			return kDummyRect;
		}
	}

	
	public void Add (GUILayoutEntry e) {
		entries.Add (e);
	}
	
	CSRAW public override void CalcWidth () {
		if (entries.Count == 0) {
			maxWidth = minWidth = style.padding.horizontal;
			return;			
		}

		childMinWidth = 0; childMaxWidth = 0;
		int _leftMarginMin = 0, _rightMarginMin = 0;
		stretchableCountX = 0;
		bool first = true;
		if (isVertical) {
			foreach (GUILayoutEntry i in entries) {
				i.CalcWidth ();
				RectOffset margins = i.margin;
				if (i.style != GUILayoutUtility.spaceStyle) {
					if (!first) {
						_leftMarginMin = Mathf.Min (margins.left, _leftMarginMin);
						_rightMarginMin = Mathf.Min (margins.right, _rightMarginMin);
					} else {
						_leftMarginMin = margins.left;
						_rightMarginMin = margins.right;
						first = false;
					}
					childMinWidth = Mathf.Max (i.minWidth + margins.horizontal, childMinWidth);
					childMaxWidth = Mathf.Max (i.maxWidth + margins.horizontal, childMaxWidth);
				}
				stretchableCountX += i.stretchWidth;
			}
			// Before, we added the margins to the width, now we want to suptract them again.
			childMinWidth -= _leftMarginMin + _rightMarginMin;
			childMaxWidth -= _leftMarginMin + _rightMarginMin;

		} else {
			int lastMargin = 0;
			foreach (GUILayoutEntry i in entries) {
				i.CalcWidth ();
				RectOffset m = i.margin;
				int margin;
				
				// Specialcase spaceStyle - instead of handling margins normally, we just want to insert the size...
				// This ensure that Space(1) adds ONE space, and doesn't prevent margin collapses
				if (i.style != GUILayoutUtility.spaceStyle) {
					if (!first)
						margin = lastMargin > m.left ? lastMargin : m.left;
					else {
						// the first element's margins are handles _leftMarginMin and should not be added to the children's sizes
						margin = 0;  
						first = false;
					}
					childMinWidth += i.minWidth + spacing + margin;
					childMaxWidth += i.maxWidth + spacing + margin;
					lastMargin = m.right;
					stretchableCountX += i.stretchWidth;
				} else {
					childMinWidth += i.minWidth;
					childMaxWidth += i.maxWidth;
					stretchableCountX += i.stretchWidth;
				}
			}
			childMinWidth -= spacing;
			childMaxWidth -= spacing;
			if (entries.Count != 0) {
				_leftMarginMin = ((GUILayoutEntry)entries[0]).margin.left;
				_rightMarginMin = lastMargin;
			} else {
				_leftMarginMin = _rightMarginMin = 0;
			}
		}
		// Catch the cases where we have ONLY space elements in a group

		// calculated padding values.
		float leftPadding = 0, rightPadding = 0;

		// If we have a style, the margins are handled i.r.t. padding.
		if (style != GUIStyle.none || userSpecifiedWidth) {
			// Add the padding of this group to the total min & max widths
			leftPadding = Mathf.Max (style.padding.left, _leftMarginMin);
			rightPadding = Mathf.Max (style.padding.right, _rightMarginMin);
		} 
		else {
			// If we don't have a GUIStyle, we pop the min of margins outward from children on to us.
			m_Margin.left = _leftMarginMin;
			m_Margin.right = _rightMarginMin;
			leftPadding = rightPadding = 0;			
		}		

		// If we have a specified minwidth, take that into account...
		minWidth = Mathf.Max (minWidth, childMinWidth + leftPadding + rightPadding);
		
		if (maxWidth == 0) {		// if we don't have a max width, take the one that was calculated
			stretchWidth += stretchableCountX + (style.stretchWidth ? 1 : 0);
			maxWidth = childMaxWidth + leftPadding + rightPadding;
		} else {
			// Since we have a maximum width, this element can't stretch width.
			stretchWidth = 0;
		}
		// Finally, if our minimum width is greater than our maximum width, minWidth wins
		maxWidth = Mathf.Max (maxWidth, minWidth);
		
		// If the style sets us to be a fixed width that wins completely
		if (style.fixedWidth != 0) {
			maxWidth = minWidth = style.fixedWidth;
			stretchWidth = 0;
		}
	}

	public override void SetHorizontal (float x, float width) {
		base.SetHorizontal (x, width);

		if (resetCoords) 
			x = 0;

		RectOffset padding = style.padding;
		
		if (isVertical) {
			// If we have a GUIStyle here, spacing from our edges to children are max (our padding, their margins)
			if (style != GUIStyle.none) {
				foreach (GUILayoutEntry i in entries) {
					// NOTE: we can't use .horizontal here (As that could make things like right button margin getting eaten by large left padding - so we need to split up in left and right
					float leftMar = Mathf.Max (i.margin.left, padding.left);		
					float thisX = x + leftMar;
					float thisWidth = width - Mathf.Max (i.margin.right, padding.right) - leftMar;
					if (i.stretchWidth != 0)
						i.SetHorizontal (thisX, thisWidth);
					else 
						i.SetHorizontal (thisX, Mathf.Clamp (thisWidth, i.minWidth, i.maxWidth));
				}
			} else {
				// If not, PART of the subelements' margins have already been propagated upwards to this group, so we need to subtract that  from what we apply
				float thisX = x - margin.left;
				float thisWidth = width + margin.horizontal;
				foreach (GUILayoutEntry i in entries) {
					if (i.stretchWidth != 0) {
						i.SetHorizontal (thisX + i.margin.left, thisWidth - i.margin.horizontal);
					} else 
						i.SetHorizontal (thisX + i.margin.left, Mathf.Clamp (thisWidth - i.margin.horizontal, i.minWidth, i.maxWidth));
				}
			}
		} else {  // we're horizontally laid out:
			// apply margins/padding here
			// If we have a style, adjust the sizing to take care of padding (if we don't the horizontal margins have been propagated fully up the hierarchy)...
			if (style != GUIStyle.none) {
				float leftMar = padding.left, rightMar = padding.right;
				if (entries.Count != 0) {
					leftMar = Mathf.Max (leftMar, ((GUILayoutEntry)entries[0]).margin.left);
					rightMar = Mathf.Max (rightMar, ((GUILayoutEntry)entries[entries.Count - 1]).margin.right);
				}
				x += leftMar;
				width -= rightMar + leftMar;
			} 

			// Find out how much leftover width we should distribute.
			float widthToDistribute = width - spacing * (entries.Count - 1);
			// Where to place us in height between min and max
			float minMaxScale = 0;
			// How much height to add to stretchable elements
			if (childMinWidth != childMaxWidth)
				minMaxScale = Mathf.Clamp ((widthToDistribute - childMinWidth) / (childMaxWidth - childMinWidth), 0, 1);

			// Handle stretching
			float perItemStretch = 0;
			if (widthToDistribute > childMaxWidth) {			// If we have too much space, we need to distribute it.
				if (stretchableCountX > 0) {		
					perItemStretch = (widthToDistribute - childMaxWidth) / (float)stretchableCountX;
				} 
			}
			
			// Set the positions
			int lastMargin = 0;
			bool firstMargin = true;
//			Debug.Log ("" + x + ", " + width + "   perItemStretch:" + perItemStretch);
//			Debug.Log ("MinMaxScale"+ minMaxScale);
			foreach (GUILayoutEntry i in entries) {				
				float thisWidth = Mathf.Lerp (i.minWidth, i.maxWidth, minMaxScale);
//				Debug.Log (i.minWidth);
				thisWidth += perItemStretch * i.stretchWidth;

				if (i.style != GUILayoutUtility.spaceStyle) {	// Skip margins on spaces.
					int leftMargin = i.margin.left;
					if (firstMargin) {
						leftMargin = 0;
						firstMargin = false;
					}
					int margin = lastMargin > leftMargin ? lastMargin : leftMargin;
					x += margin;
					lastMargin = i.margin.right;
				}

				i.SetHorizontal (Mathf.Round (x), Mathf.Round(thisWidth));
				x += thisWidth + spacing;
			}
		}
	}

	public override void CalcHeight () {
		if (entries.Count == 0) {
			maxHeight = minHeight = style.padding.vertical;
			return;			
		}
	
		childMinHeight = childMaxHeight = 0;
		int _topMarginMin = 0,  _bottomMarginMin = 0;
		stretchableCountY = 0;
		if (isVertical) {
			int lastMargin = 0;
			bool first = true;
			foreach (GUILayoutEntry i in entries) {
				i.CalcHeight ();
				RectOffset m = i.margin;
				int margin;

				// Specialcase spaces - it's a space, so instead of handling margins normally, we just want to insert the size...
				// This ensure that Space(1) adds ONE space, and doesn't prevent margin collapses
				if (i.style != GUILayoutUtility.spaceStyle) {
					if (!first) 
						margin = Mathf.Max(lastMargin, m.top);
					else {
						margin = 0;
						first = false;
					}
						
					childMinHeight += i.minHeight + spacing + margin;
					childMaxHeight += i.maxHeight + spacing + margin;
					lastMargin = m.bottom;
					stretchableCountY += i.stretchHeight;
				} else {
					childMinHeight += i.minHeight;
					childMaxHeight += i.maxHeight;
					stretchableCountY += i.stretchHeight;				
				}
			}

			childMinHeight -= spacing;
			childMaxHeight -= spacing;
			if (entries.Count != 0) {
				_topMarginMin = ((GUILayoutEntry)entries[0]).margin.top;
				_bottomMarginMin = lastMargin;
			} else {
				_bottomMarginMin = _topMarginMin = 0;
			}
		} else {
			bool first = true;
			foreach (GUILayoutEntry i in entries) {
				i.CalcHeight ();
				RectOffset margins = i.margin;
				if (i.style != GUILayoutUtility.spaceStyle) {
					if (!first) {
						_topMarginMin = Mathf.Min (margins.top, _topMarginMin);
						_bottomMarginMin = Mathf.Min (margins.bottom, _bottomMarginMin);
					} else {
						_topMarginMin = margins.top;
						_bottomMarginMin = margins.bottom;
						first = false;					
					}
					childMinHeight = Mathf.Max (i.minHeight, childMinHeight);
					childMaxHeight = Mathf.Max (i.maxHeight, childMaxHeight);
				}
				stretchableCountY += i.stretchHeight;
			}
		}
		float firstPadding = 0, lastPadding = 0;

		// If we have a style, the margins are handled i.r.t. padding.
		if (style != GUIStyle.none || userSpecifiedHeight) {
			// Add the padding of this group to the total min & max widths
			firstPadding = Mathf.Max (style.padding.top, _topMarginMin);
			lastPadding = Mathf.Max (style.padding.bottom, _bottomMarginMin);
		} 
		else {
			// If we don't have a GUIStyle, we bubble the margins outward from children on to us.
			m_Margin.top = _topMarginMin;
			m_Margin.bottom = _bottomMarginMin;
			firstPadding = lastPadding = 0;
		}
		//Debug.Log ("Margins: " + _topMarginMin + ", " + _bottomMarginMin + "          childHeights:" + childMinHeight + ", " + childMaxHeight);
		// If we have a specified minheight, take that into account...		
		minHeight = Mathf.Max (minHeight, childMinHeight + firstPadding + lastPadding);
		
		if (maxHeight == 0) {		// if we don't have a max height, take the one that was calculated
			stretchHeight += stretchableCountY + (style.stretchHeight ? 1 : 0);
			maxHeight = childMaxHeight + firstPadding + lastPadding;
		} else {
			// Since we have a maximum height, this element can't stretch height.
			stretchHeight = 0;
		}
		// Finally, if out minimum height is greater than our maximum height, minHeight wins
		maxHeight = Mathf.Max (maxHeight, minHeight);
		
		// If the style sets us to be a fixed height
		if (style.fixedHeight != 0) {
			maxHeight = minHeight = style.fixedHeight;
			stretchHeight = 0;
		}
	}

	public override void SetVertical (float y, float height) {		
		base.SetVertical (y, height);

		if (entries.Count == 0)
			return;

		RectOffset padding = style.padding;

		if (resetCoords) 
			y = 0;
		
		if (isVertical) {
			// If we have a skin, adjust the sizing to take care of padding (if we don't have a skin the vertical margins have been propagated fully up the hierarchy)...
			if (style != GUIStyle.none) {
				float topMar = padding.top, bottomMar = padding.bottom;
				if (entries.Count != 0) {
					topMar = Mathf.Max (topMar, ((GUILayoutEntry)entries[0]).margin.top);
					bottomMar = Mathf.Max (bottomMar, ((GUILayoutEntry)entries[entries.Count - 1]).margin.bottom);
				}
				y += topMar;
				height -= bottomMar + topMar;
			} 

			// Find out how much leftover height we should distribute.
			float heightToDistribute = height - spacing * (entries.Count - 1);
			// Where to place us in height between min and max
			float minMaxScale = 0;
			// How much height to add to stretchable elements
			if (childMinHeight != childMaxHeight)
				minMaxScale = Mathf.Clamp ((heightToDistribute - childMinHeight) / (childMaxHeight - childMinHeight), 0, 1);

			// Handle stretching
			float perItemStretch = 0;
			if (heightToDistribute > childMaxHeight) {			// If we have too much space - stretch any stretchable children
				if (stretchableCountY > 0) 
					perItemStretch = (heightToDistribute - childMaxHeight) / (float)stretchableCountY;
			}
			
			// Set the positions
			int lastMargin = 0;
			bool firstMargin = true;
			foreach (GUILayoutEntry i in entries) {
				float thisHeight = Mathf.Lerp (i.minHeight, i.maxHeight, minMaxScale);
				thisHeight += perItemStretch * i.stretchHeight;

				if (i.style != GUILayoutUtility.spaceStyle) {	// Skip margins on spaces.				
					int topMargin = i.margin.top;
					if (firstMargin) {
						topMargin = 0;
						firstMargin = false;
					} 
					int margin = lastMargin > topMargin ? lastMargin : topMargin;
					y += margin;
					lastMargin = i.margin.bottom;
				}
				i.SetVertical (Mathf.Round (y), Mathf.Round(thisHeight));
				y += thisHeight + spacing;
			}
		} else {
			// If we have a GUIStyle here, we need to respect the subelements' margins
			if (style != GUIStyle.none) {
				foreach (GUILayoutEntry i in entries) {
					float topMar = Mathf.Max (i.margin.top, padding.top);		
					float thisY = y + topMar;
					float thisHeight = height - Mathf.Max (i.margin.bottom, padding.bottom) - topMar;
					
					if (i.stretchHeight != 0)
						i.SetVertical (thisY, thisHeight);
					else {
						i.SetVertical (thisY, Mathf.Clamp (thisHeight, i.minHeight, i.maxHeight));
					}
				}
			} else {
				// If not, the subelements' margins have already been propagated upwards to this group, so we can safely ignore them
				float thisY = y - margin.top;
				float thisHeight = height + margin.vertical;
				foreach (GUILayoutEntry i in entries) {
					if (i.stretchHeight != 0)
						i.SetVertical (thisY +i.margin.top, thisHeight - i.margin.vertical);
					else {
						i.SetVertical (thisY + i.margin.top, Mathf.Clamp (thisHeight - i.margin.vertical, i.minHeight, i.maxHeight));
					}
				}			
			
			}
		}
	}

	public override string ToString () {
		string str = "", space = "";
		for (int i = 0; i < indent; i++) 
			space += " ";
		str +=  base.ToString () + " Margins: " + childMinHeight + " {\n";
		indent += 4;
		foreach (GUILayoutEntry i in entries) {
			str += i.ToString() + "\n";
		}
		str += space + "}";
		indent -= 4;
		return str;
	}
END
	
// Layout controller for content inside scroll views
CLASS internal GUIScrollGroup : GUILayoutGroup 
	CSRAW
	public float calcMinWidth, calcMaxWidth, calcMinHeight, calcMaxHeight;
	public float clientWidth, clientHeight;
	public bool allowHorizontalScroll = true;
	public bool allowVerticalScroll = true;
	public bool needsHorizontalScrollbar, needsVerticalScrollbar;
	public GUIStyle horizontalScrollbar, verticalScrollbar;
	
	public override void CalcWidth () {	
		// Save the size values & reset so we calc the sizes of children without any contraints
		float _minWidth = minWidth;
		float _maxWidth = maxWidth;
		if (allowHorizontalScroll) {
			minWidth = 0;
			maxWidth = 0;
		}
		
		base.CalcWidth();
		calcMinWidth = minWidth;
		calcMaxWidth = maxWidth;

		// restore the stored constraints for our parent's sizing
		if (allowHorizontalScroll) {
			
		// Set an explicit small minWidth so it will correctly scroll when place inside horizontal groups
		if (minWidth > 32)
			minWidth = 32;
		
			if (_minWidth != 0)
				minWidth = _minWidth;
			if (_maxWidth != 0) {
				maxWidth = _maxWidth;
				stretchWidth = 0;
			}
		}
	}

	public override void SetHorizontal (float x, float width) { 
		float _cWidth = needsVerticalScrollbar ? width - verticalScrollbar.fixedWidth - verticalScrollbar.margin.left : width;
		//if (allowVerticalScroll == false)
		//	Debug.Log ("width " + width);
		// If we get a vertical scrollbar, the width changes, so we need to do a recalculation with the new width.
		if (allowHorizontalScroll && _cWidth < calcMinWidth) {
			// We're too small horizontally, so we need a horizontal scrollbar. 
			needsHorizontalScrollbar = true;

			// set the min and max width we calculated for the children so SetHorizontal works correctly
			minWidth = calcMinWidth;
			maxWidth = calcMaxWidth;
			base.SetHorizontal (x, calcMinWidth);
			
			// SetHorizontal also sets our width, but we know better
			rect.width = width;
			
			clientWidth = calcMinWidth;
		} else {
			// Got enough space.
			needsHorizontalScrollbar = false;

			// set the min and max width we calculated for the children so SetHorizontal works correctly
			if (allowHorizontalScroll) {
				minWidth = calcMinWidth;
				maxWidth = calcMaxWidth;
			}
			base.SetHorizontal (x, _cWidth);
			rect.width = width;

			// Store the client width
			clientWidth = _cWidth;
		}
	}

	public override void CalcHeight () {	
		// Save the values & reset so we calc the sizes of children without any contraints
		float _minHeight = minHeight;
		float _maxHeight = maxHeight;
		if (allowVerticalScroll)
		{
			minHeight = 0;
			maxHeight = 0;
		}
		
		base.CalcHeight();

		calcMinHeight = minHeight;
		calcMaxHeight = maxHeight;

		// if we KNOW we need a horizontal scrollbar, claim space for it now
		// otherwise we get a vertical scrollbar and leftover space beneath the scrollview.
		if (needsHorizontalScrollbar) {
			float scrollerSize = horizontalScrollbar.fixedHeight + horizontalScrollbar.margin.top;
			minHeight += scrollerSize;
			maxHeight += scrollerSize;
		}
		
		// restore the stored constraints from user SetHeight calls.
		if (allowVerticalScroll)
		{
			if (minHeight > 32)
				minHeight = 32;
			
			if (_minHeight != 0)
				minHeight = _minHeight;
			if (_maxHeight != 0)
			{
				maxHeight = _maxHeight;
				stretchHeight = 0;
			}
		}
	}

	public override void SetVertical (float y, float height) { 
		// if we have a horizontal scrollbar, we have less space than we thought
		float availableHeight = height;
		if (needsHorizontalScrollbar)
			availableHeight -= horizontalScrollbar.fixedHeight + horizontalScrollbar.margin.top;

		// Now we know how much height we have, and hence how much vertical space to distribute.
		// If we get a vertical scrollbar, the width changes, so we need to do a recalculation with the new width.
		if (allowVerticalScroll && availableHeight < calcMinHeight)
		{
			// We're too small vertically, so we need a vertical scrollbar. 
			// This means that we have less horizontal space, which can change the vertical size.
			if (!needsHorizontalScrollbar && !needsVerticalScrollbar) {			
				// Subtract scrollbar width from the size...
				clientWidth = rect.width - verticalScrollbar.fixedWidth - verticalScrollbar.margin.left;

				// ...But make sure we never get too small.
				if (clientWidth < calcMinWidth) 
					clientWidth = calcMinWidth;
					
				// Set the new (smaller) size.
				float outsideWidth = rect.width;		// store a backup of our own width
				SetHorizontal (rect.x, clientWidth);

				// This can have caused a reflow, so we need to recalclate from here on down
				// (we already know we need a vertical scrollbar, so this size change cannot bubble upwards.
				CalcHeight();

				rect.width = outsideWidth;
			}


			// set the min and max height we calculated for the children so SetVertical works correctly
			float origMinHeight = minHeight, origMaxHeight = maxHeight;
			minHeight = calcMinHeight;
			maxHeight = calcMaxHeight;
			base.SetVertical (y, calcMinHeight);
			minHeight = origMinHeight;
			maxHeight = origMaxHeight;
			
			rect.height = height;
			clientHeight = calcMinHeight;
		} else {

			// set the min and max height we calculated for the children so SetVertical works correctly
			if (allowVerticalScroll)
			{
				minHeight = calcMinHeight;
				maxHeight = calcMaxHeight;
			}
			base.SetVertical (y, availableHeight);
			rect.height = height;
			clientHeight = availableHeight;
		}
	}
END

// Layouter that makes elements which sizes will always conform to a specific aspect ratio.
CLASS internal GUIAspectSizer : GUILayoutEntry 
	CSRAW float aspect;

	public GUIAspectSizer (float aspect, GUILayoutOption[] options) : base (0,0,0,0,GUIStyle.none) {
		this.aspect = aspect;
		ApplyOptions (options);
	}
	
	public override void CalcHeight () {
		minHeight = maxHeight = rect.width / aspect;
	}
END

// Will layout a button grid so it can fit within the given rect.
// *undocumented*
CLASS internal GUIGridSizer : GUILayoutEntry 
	CSRAW
	// Helper: Create the layout group and scale it to fit
	public static Rect GetRect (GUIContent[] contents, int xCount, GUIStyle style, GUILayoutOption[] options) {
		Rect r = new Rect (0,0,0,0);
		switch (Event.current.type) {
		case EventType.Layout: {
			GUILayoutUtility.current.topLevel.Add (new GUIGridSizer (contents, xCount, style, options));
			break;
		}
		case EventType.Used:
			return kDummyRect;
		default: 
			r = GUILayoutUtility.current.topLevel.GetNext ().rect;
			break;
		}
		return r;		
	}
	int count;
	int xCount;
	float minButtonWidth = -1, maxButtonWidth =  -1, minButtonHeight =  -1, maxButtonHeight =  -1;

	// Cache of the content for wordwrapping.
	//GUIContent[] cachedContent;

	private GUIGridSizer (GUIContent[] contents, int _xCount, GUIStyle buttonStyle, GUILayoutOption[] options) : base (0,0,0,0,GUIStyle.none) {
		count = contents.Length;
		xCount = _xCount;

		// Most settings comes from the button style (can we stretch, etc). Hence, I apply the style here
		ApplyStyleSettings (buttonStyle);
		
		// We can have custom options coming from userland. We apply this last so it overrides
		ApplyOptions (options);

		if (_xCount == 0 || contents.Length == 0)
			return;

		// if we don't have wordwrap, there is no funky width<->height relationship. Hence, we can calculate everything here:
		// TODO: Actually make it work with wordwrapping stuff (that is pretty hard)
//		if (!buttonStyle.wordWrap) {

			// internal horizontal spacing
			float totalHorizSpacing = Mathf.Max (buttonStyle.margin.left,buttonStyle.margin.right) * (xCount - 1);
//			Debug.Log (String.Format ("margins: {0}, {1}   totalHoriz: {2}", buttonStyle.margin.left, buttonStyle.margin.right, totalHorizSpacing));
			// internal horizontal margins
			float totalVerticalSpacing = Mathf.Max (buttonStyle.margin.top, buttonStyle.margin.bottom) * (rows - 1);


			// Handle fixedSize buttons
			if (buttonStyle.fixedWidth != 0)
				minButtonWidth = maxButtonWidth = buttonStyle.fixedWidth;
//			Debug.Log ("buttonStyle.fixedHeight " + buttonStyle.fixedHeight);
			if (buttonStyle.fixedHeight != 0)
				minButtonHeight = maxButtonHeight = buttonStyle.fixedHeight;
			
			// Apply GUILayout.Width/Height/whatever properties.
			if (minButtonWidth == -1) {
				if (minWidth != 0) 
					minButtonWidth = (minWidth - totalHorizSpacing) / xCount;
				if (maxWidth != 0)
					maxButtonWidth = (maxWidth - totalHorizSpacing) / xCount;
			}

			if (minButtonHeight == -1) {
				if (minHeight != 0) 
					minButtonHeight = (minHeight - totalVerticalSpacing) / rows;
				if (maxHeight != 0)
					maxButtonHeight = (maxHeight - totalVerticalSpacing) / rows;
			}
//			Debug.Log (String.Format ("minButtonWidth {0}, maxButtonWidth {1}, minButtonHeight {2}, maxButtonHeight{3}", minButtonWidth, maxButtonWidth, minButtonHeight, maxButtonHeight));

			// if anything is left unknown, we need to iterate over all elements and figure out the sizes.
			if (minButtonHeight == -1 || maxButtonHeight == -1 || minButtonWidth == -1 || maxButtonWidth == -1) {
				// figure out the max size. Since the buttons are in a grid, the max size determines stuff.
				float calcHeight = 0, calcWidth = 0;
				foreach (GUIContent i in contents) {
					Vector2 size = buttonStyle.CalcSize (i);
					calcWidth = Mathf.Max (calcWidth, size.x);
					calcHeight = Mathf.Max (calcHeight, size.y);
				}
				
				// If the user didn't supply minWidth, we need to calculate that
				if (minButtonWidth == -1) {
					// if the user has supplied a maxButtonWidth, the buttons can never get larger.
					if (maxButtonWidth != -1)
						minButtonWidth = Mathf.Min (calcWidth, maxButtonWidth);
					else 
						minButtonWidth = calcWidth;
				}

				// If the user didn't supply maxWidth, we need to calculate that
				if (maxButtonWidth == -1) {
					// if the user has supplied a minButtonWidth, the buttons can never get smaler.
					if (minButtonWidth != -1)
						maxButtonWidth = Mathf.Max (calcWidth, minButtonWidth);
					else
						maxButtonWidth = calcWidth;
				}

				// If the user didn't supply minWidth, we need to calculate that
				if (minButtonHeight == -1) {
					// if the user has supplied a maxButtonWidth, the buttons can never get larger.
					if (maxButtonHeight != -1)
						minButtonHeight = Mathf.Min (calcHeight, maxButtonHeight);
					else 
						minButtonHeight = calcHeight;
				}

				// If the user didn't supply maxWidth, we need to calculate that
				if (maxButtonHeight == -1) {
					// if the user has supplied a minButtonWidth, the buttons can never get smaler.
					if (minButtonHeight != -1)
						maxHeight = Mathf.Max (maxHeight, minButtonHeight);
					maxButtonHeight = maxHeight;
				}
				
			}
			// We now know the button sizes. Calculate min & max values from that
			minWidth = minButtonWidth * xCount + totalHorizSpacing;
			maxWidth = maxButtonWidth * xCount + totalHorizSpacing;
			minHeight = minButtonHeight * rows + totalVerticalSpacing;
			maxHeight = maxButtonHeight * rows + totalVerticalSpacing;
//			Debug.Log (String.Format ("minWidth {0}, maxWidth {1}, minHeight {2}, maxHeight{3}", minWidth, maxWidth, minHeight, maxHeight));

	}
	
	int rows { 
		get { 		
			int rows = count / xCount;
			if (count % xCount != 0)
				rows++;
			return rows;
		}
	}
END	

// Class that can handle word-wrap sizing. this is specialcased as setting width can make the text wordwrap, which would then increase height...
CLASS internal  GUIWordWrapSizer : GUILayoutEntry 
	CSRAW
	GUIContent content;
	// We need to differentiate between min & maxHeight we calculate for ourselves and one that is forced by the user
	// (When inside a scrollview, we can be told to layout twice, so we need to know the difference)
	float forcedMinHeight, forcedMaxHeight;
	public GUIWordWrapSizer (GUIStyle _style, GUIContent _content, GUILayoutOption[] options) : base (0,0,0,0, _style) {
		content = new GUIContent (_content);
		base.ApplyOptions (options);
		forcedMinHeight = minHeight;
		forcedMaxHeight = maxHeight;
	}
	
	public override void CalcWidth () { 
		if (minWidth == 0 || maxWidth == 0) {
			float _minWidth, _maxWidth;
			style.CalcMinMaxWidth (content, out _minWidth, out _maxWidth);
			if (minWidth == 0)
				minWidth = _minWidth;
			if (maxWidth == 0)
				maxWidth = _maxWidth;
		}
	}

	public override void CalcHeight () {
		// When inside a scrollview, this can get called twice (as vertical scrollbar reduces width, which causes a reflow).
		// Hence, we need to use the separately cached values for min & maxHeight coming from the user...
		if (forcedMinHeight == 0 || forcedMaxHeight == 0) {
			float height = style.CalcHeight (content, rect.width);
			if (forcedMinHeight == 0) 
				minHeight = height;
			else
				minHeight = forcedMinHeight;
			if (forcedMaxHeight == 0)
				maxHeight = height;
			else
				maxHeight = forcedMaxHeight;
		}
	}
END
*/
	
// Class internally used to pass layout options into [[GUILayout]] functions. You don't use these directly, but construct them with the layouting functions in the [[GUILayout]] class.
	public class GUILayoutOption
	{
		internal enum Type {
			fixedWidth, fixedHeight, minWidth, maxWidth, minHeight, maxHeight, stretchWidth, stretchHeight,
			// These are just for the spacing variables
			alignStart, alignMiddle, alignEnd, alignJustify, equalSize, spacing
		}
		// *undocumented*
		internal Type type;
		// *undocumented*
		internal object value;
		// *undocumented*
		internal GUILayoutOption (Type type, object value) {
			this.type = type;
			this.value = value;
		}
	}

}
