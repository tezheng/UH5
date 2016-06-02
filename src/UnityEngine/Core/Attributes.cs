using System;

namespace UnityEngine
{
	[AttributeUsage (AttributeTargets.Class, Inherited = false)]
	public sealed class DisallowMultipleComponentsAttribute : Attribute {}

	// The RequireComponent attribute lets automatically add required component as a dependency.
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public sealed class RequireComponentAttribute : Attribute
	{
		//*undocumented*
		public Type m_Type0;
		//*undocumented*
		public Type m_Type1;
		//*undocumented*
		public Type m_Type2;

		// Require a single component
		public RequireComponentAttribute (Type requiredComponent) :
			this (requiredComponent, null, null) {}

		// Require a two components
		public RequireComponentAttribute (Type requiredComponent, Type requiredComponent2) :
			this (requiredComponent, requiredComponent2, null) {}

		// Require three components
		public RequireComponentAttribute (Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
			m_Type2 = requiredComponent3;
		}
	}

	// The AddComponentMenu attribute allows you to place a script anywhere in the "Component" menu, instead of just the "Component->Scripts" menu.
	public sealed class AddComponentMenuAttribute : Attribute
	{
		private string m_AddComponentMenu;
		private int m_Ordering;

		// The script will be placed in the component menu according to /menuName/. /menuName/ is the path to the component
		public AddComponentMenuAttribute (string menuName) : this (menuName, 0) {}
		// same as above, but also specify a custom Ordering.
		public AddComponentMenuAttribute (string menuName, int order)
		{
			m_AddComponentMenu = menuName;
			m_Ordering = order;
		}

		//* undocumented
		public string componentMenu { get {return m_AddComponentMenu; } }

		//* undocumented
		public int componentOrder { get {return m_Ordering; } }
	}

	// The ContextMenu attribute allows you to add commands to the context menu
	public sealed class ContextMenuAttribute : Attribute
	{
		// Adds the function to the context menu of the component.
		public ContextMenuAttribute (string name)
		{
			m_ItemName = name;
		}

		private string m_ItemName;

		//* undocumented
		public string menuItem { get { return m_ItemName; } }
	}

	// Makes a script execute in edit mode.
	public sealed class ExecuteInEditModeAttribute : Attribute {}

	// Makes a variable not show up in the inspector but be serialized.
	public sealed class HideInInspectorAttribute : Attribute {}

	public class SerializeFieldAttribute : Attribute {}
}
