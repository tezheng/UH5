using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;

namespace UnityEngine
{

// Behaviours are Components that can be enabled or disabled.
public abstract class Behaviour : Component
{
	// Enabled Behaviours are Updated, disabled Behaviours are not.
	public bool enabled 
	{
		get {return GetEnabled();} set {SetEnabled(value);}
	}

	internal bool GetEnabled()
	{
		return m_Enabled;
	}

	internal void SetEnabled(bool value)
	{
		if (m_Enabled == value)
			return;
		m_Enabled = value;
		UpdateEnabledState (IsActive ());
		SetDirty ();
	}

	internal override void Deactivate()
	{
		UpdateEnabledState (false);
		base.Deactivate ();
	}

	bool m_Enabled = true;
	bool m_IsAdded;
	bool IsAddedToManager () { return m_IsAdded; }

	abstract internal void AddToManager ();
	abstract internal void RemoveFromManager ();
	void UpdateEnabledState (bool active)
	{
		bool shouldBeAdded = active && m_Enabled;
		if (shouldBeAdded == m_IsAdded)
			return;

		// Set IsAdded flag before adding/removing from manager. Otherwise if we get enabled update
		// from inside of AddToManager/RemoveFromManager, we'll early out in the check above because
		// flag is not set yet!
		if (shouldBeAdded)
		{
			m_IsAdded = true;
			AddToManager ();
		}
		else
		{
			m_IsAdded = false;
			RemoveFromManager ();
		}
	}

	internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
	{
		base.AwakeFromLoad (awakeMode);
		UpdateEnabledState (IsActive ());
	}

	internal virtual void __Update () {}
	internal virtual void __FixedUpdate () {}
	internal virtual void __LateUpdate () {}
}

}
