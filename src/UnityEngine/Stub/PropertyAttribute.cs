using System;

namespace System
{
[System.AttributeUsage (AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class NonSerialized : Attribute
{
}
}

namespace UnityEngine
{

// Base class to derive custom property attributes from. Use this to create custom attributes for script variables.
[System.AttributeUsage (AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public abstract class PropertyAttribute : Attribute
{
}

// Attribute used to make a float or int variable in a script be restricted to a specific range.
[System.AttributeUsage (AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class RangeAttribute : PropertyAttribute
{
	public readonly float min;
	public readonly float max;
	
	// Attribute used to make a float or int variable in a script be restricted to a specific range.
	public RangeAttribute (float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}

// Attribute to make a string be edited with a multi-line textfield
[System.AttributeUsage (AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MultilineAttribute : PropertyAttribute
{
	public readonly int lines;
	
	/// *listonly*
	public MultilineAttribute ()
	{
		this.lines = 3;
	}
	// Attribute used to make a string value be shown in a multiline textarea.
	public MultilineAttribute (int lines)
	{
		this.lines = lines;
	}
	
}

}
