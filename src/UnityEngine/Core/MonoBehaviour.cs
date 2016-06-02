using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{
// MonoBehaviour is the base class every script derives from.

public partial class MonoBehaviour : Behaviour
{
	public MonoBehaviour ()
	{
	}

	// Cancels all Invoke calls on this MonoBehaviour.
	public void CancelInvoke ()
	{
		Internal_CancelInvokeAll ();
	}

	// Cancels all Invoke calls with name /methodName/ on this behaviour.
	public void CancelInvoke (string methodName) {Internal_CancelInvoke(methodName);}

	// Is any invoke on /methodName/ pending?
	public bool IsInvoking (string methodName) { return Internal_IsInvoking(methodName); }
	
	// Is any invoke pending on this MonoBehaviour?
	public bool IsInvoking ()
	{
		return Internal_IsInvokingAll ();
	}
	
	// Stop a coroutine.
	public void StopCoroutine (IEnumerator routine)
	{
		StopCoroutineViaEnumerator_Auto(routine);
	}

	// Logs message to the Unity Console. This function is identical to [[Debug.Log]].
	public static void print (object message)
	{
		Debug.Log (message);
	}

	// Disabling this lets you skip the GUI layout phase.
	public bool useGUILayout;
}

partial class MonoBehaviour
{
	bool m_DidAwake;
	int _executionOrder;
	bool _orderQueried;
	internal int executionOrder {
		get {
			if (!_orderQueried)
			{
				_orderQueried = true;
				_executionOrder = Resources.GetExecutionOrder(GetType().Name);
			}
			return _executionOrder;
		}
	}

	internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
	{
		base.AwakeFromLoad(awakeMode);
		if (GetEnabled() && IsActive())
		{
			return;
		}
		// CallDelayed->Start must be called before Awake otherwise objects created inside of Awake will get their Start call
		// before the Start of the Behaviour that created them.
		bool willCallAddToManager = GetEnabled() && IsActive();
		if (willCallAddToManager)
		{
			base.AwakeFromLoad (awakeMode);
			return;
		}
		bool isMonoBehaviourRequiringAwake = !m_DidAwake && IsActive ();
		bool isScriptableObjectRequiringAwakeAndEnable = false;//!m_DidAwake;

		// Awake for monobehaviours and scriptable objects
		if (isMonoBehaviourRequiringAwake || isScriptableObjectRequiringAwakeAndEnable)
		{
			CallAwake();
		}

		// OnEnable for scriptable object
		if (isScriptableObjectRequiringAwakeAndEnable)
		{
			//CallEnable();
		}
		
		// Potential Call to AddToManager / RemoveFromManager with various C# callbacks
		base.AwakeFromLoad (awakeMode);
	}

	void DelayedStartCall(Object target, System.Object userdata)
	{
		__Start();
	}

	internal override void AddToManager()
	{	
		// call delayed start.

		// Behaviour callbacks are registered before OnEnable.
		// If an object is created in OnEnable, the order will be this script, then the created script.
		BehaviourManager.AddBehaviour(this, executionOrder);

		DelayedCallManager.CallDelayed(DelayedStartCall, this, 0, null, 0, 0);

		// We must call Awake here.
		// CallDelayed->Start must be called before Awake otherwise objects created inside of Awake will get their Start call
		// before the Start of the Behaviour that created them.
		if (!m_DidAwake)
		{
			CallAwake ();
		}

		CallEnable();
	}

	internal override void RemoveFromManager()
	{
		if (m_DidAwake)
		{
			CallDisable();
			BehaviourManager.RemoveBehaviour(this, executionOrder);
		}
	}

	internal bool IsDestroying() {return m_IsDestroying;}
	bool m_IsDestroying;
	internal override void WillDestroyComponent ()
	{
		base.WillDestroyComponent();

		if (m_IsDestroying)
		{
			Debug.LogError("DestroyImmediate should not be called on the same game object when destroying a MonoBehaviour");
			return;
		}

		m_IsDestroying = true;
		if (m_DidAwake)
		{
			CallDisable();
			CallOnDestroy();
		}
	}

	bool m_DidStart;
	void __Start()
	{
		if (m_DidStart)
			return;
		
		m_DidStart = true;
		CallStart();
	}

	internal override void __Update () {CallUpdate();}
	internal override void __LateUpdate () {CallLateUpdate();}

	void CallAwake()
	{
		m_DidAwake = true;
		PerformMessage (MonoBehaviour.C_Awake);
	}

	void CallEnable()
	{
			PerformMessage (MonoBehaviour.C_OnEnable);
	}

	void CallDisable()
	{
			PerformMessage (MonoBehaviour.C_OnDisable);
	}

	void CallStart()
	{
			PerformMessage (MonoBehaviour.C_Start);
	}

	void CallUpdate()
	{
		UpdateAllCoroutines (Time.frameCount, Time.time);
		__Start ();
			PerformMessage (MonoBehaviour.C_Update);
	}

	void CallLateUpdate()
	{
		__Start ();
			PerformMessage (MonoBehaviour.C_LateUpdate);
	}

	void CallOnDestroy()
	{
			PerformMessage (MonoBehaviour.C_OnDestroy);
	}


	// Coroutine related.
	private void Internal_CancelInvokeAll ()
	{

	}

	private bool Internal_IsInvokingAll ()
	{
		return false;
	}

	// Invokes the method /methodName/ in time seconds.
	public void Invoke (string methodName, float time)
	{
	}
	
	// Invokes the method /methodName/ in /time/ seconds.
	public void InvokeRepeating (string methodName, float time, float repeatRate)
	{
	}

	private void Internal_CancelInvoke(string methodName)
	{
	}

	private bool Internal_IsInvoking(string methodName)
	{
		return false;
	}

	// Stops all coroutines named /methodName/ running on this behaviour.
	public void StopCoroutine (string methodName) {

	}

	internal override void Deactivate()
	{
		StopAllCoroutines();
		base.Deactivate();
	}

	//*undocumented*
	internal void StopCoroutineViaEnumerator_Auto (IEnumerator routine)
	{
		Coroutine coroutine = FirstCoroutine;
		while (coroutine != null) {
			Coroutine current = coroutine;
			coroutine = coroutine.listNext;

			if (current.fiber == routine) {
				current.finished = true;
				RemoveCoroutine(current);
				return;
			}
		}
	}

	// Stops all coroutines running on this behaviour.
	void StopAllCoroutines () {
		FirstCoroutine = null;
	}

	private Coroutine FirstCoroutine { get; set;}
	private int CurrentFrame { get; set;}
	private float CurrentTime { get; set;}

	public Coroutine StartCoroutine (IEnumerator routine)
	{
		if (routine == null) {
			return null;
		}
		// create coroutine node and run until we reach first yield
		Coroutine coroutine = new Coroutine (routine);
		AddCoroutine (coroutine);
		return coroutine;
	}

	private void AddCoroutine(Coroutine coroutine)
	{
		if (FirstCoroutine != null) {
			coroutine.listNext = FirstCoroutine;
			FirstCoroutine.listPrevious = coroutine;
		}
		FirstCoroutine = coroutine;
	}

	private void UpdateAllCoroutines (int frame, float time)
	{
		CurrentFrame = frame;
		CurrentTime = time;
		Coroutine coroutine = FirstCoroutine;
		while (coroutine != null) {
			// store listNext before coroutine finishes and is removed from the list
			Coroutine listNext = coroutine.listNext;

			if (coroutine.waitForFrame >= 0 && frame >= coroutine.waitForFrame) {
				coroutine.waitForFrame = -1;
				UpdateCoroutine (coroutine);
			} else if (coroutine.waitForTime > 0.0f && time >= coroutine.waitForTime) {
				coroutine.waitForTime = -1.0f;
				UpdateCoroutine (coroutine);
			} else if (coroutine.waitForCoroutine != null && coroutine.waitForCoroutine.finished) {
				coroutine.waitForCoroutine = null;
				UpdateCoroutine (coroutine);
			} else if (coroutine.waitForFrame == -1 && coroutine.waitForTime == -1.0f && coroutine.waitForCoroutine == null) {
				// initial update
				UpdateCoroutine (coroutine);
			}
			coroutine = listNext;
		}
	}

	private void UpdateCoroutine(Coroutine coroutine) {
		IEnumerator fiber = coroutine.fiber;
		if (coroutine.fiber.MoveNext ()) {
			System.Object yieldCommand = fiber.Current == null ? (System.Object) 1 : fiber.Current;

			if (yieldCommand.GetType ()  == typeof(WaitForSeconds)) {
				WaitForSeconds wait = (WaitForSeconds) yieldCommand;
				coroutine.waitForTime = CurrentTime + wait.m_Seconds;
			} else if (yieldCommand.GetType () == typeof(WaitForEndOfFrame)) {
				WaitForEndOfFrame wait = (WaitForEndOfFrame) yieldCommand;
				coroutine.waitForFrame = CurrentFrame + wait.m_Frames;
			} else if (yieldCommand.GetType () == typeof(Coroutine)) {
				coroutine.waitForCoroutine = (Coroutine) yieldCommand;
			} else if (yieldCommand.GetType () == typeof(WaitForFixedUpdate)) {
				Debug.LogError ("WaitForFixedUpdate not support");
			} else {
				Debug.LogError ("CoroutineScheduler: Unexpected coroutine yield type: " + yieldCommand.GetType ());
			}
		} else {
			// coroutine finished
			coroutine.finished = true;
			RemoveCoroutine (coroutine);
		}
	}

	private void RemoveCoroutine (Coroutine coroutine)
	{
		if (FirstCoroutine == coroutine) {
			// remove first
			FirstCoroutine = coroutine.listNext;
		} else {
			// not head of list
			if (coroutine.listNext != null) {
				// remove between
				coroutine.listPrevious.listNext = coroutine.listNext;
				coroutine.listNext.listPrevious = coroutine.listPrevious;
			} else if (coroutine.listPrevious != null) {
				// and listNext is null
				coroutine.listPrevious.listNext = null;
				// remove last
			}
		}
		coroutine.listPrevious = null;
		coroutine.listNext = null;
	}


	/*
	CUSTOM private void Internal_CancelInvokeAll ()
	{
		CancelInvoke (*self);
	}

	CUSTOM private bool Internal_IsInvokingAll ()
	{
		return IsInvoking (*self);
	}

	// Invokes the method /methodName/ in time seconds.
	CUSTOM void Invoke (string methodName, float time)
	{
		InvokeDelayed (*self, methodName, time, 0.0F);
	}
	
	// Invokes the method /methodName/ in /time/ seconds.
	CUSTOM void InvokeRepeating (string methodName, float time, float repeatRate) 
	{ 
		InvokeDelayed (*self, methodName, time, repeatRate); 
	}


	// Cancels all Invoke calls on this MonoBehaviour.
	CSRAW public void CancelInvoke ()
	{
		Internal_CancelInvokeAll (); 
	} 

	// Cancels all Invoke calls with name /methodName/ on this behaviour.
	CUSTOM void CancelInvoke (string methodName) { CancelInvoke (*self, methodName); }
	

	// Is any invoke on /methodName/ pending?
	CUSTOM bool IsInvoking (string methodName) { return IsInvoking (*self, methodName); }
	
	// Is any invoke pending on this MonoBehaviour?
	CSRAW public bool IsInvoking ()
	{ 
		return Internal_IsInvokingAll (); 
	}
		
	// Starts a coroutine.
	CSRAW public Coroutine StartCoroutine (IEnumerator routine)
	{
		return StartCoroutine_Auto(routine);
	}
	//*undocumented*

	CUSTOM Coroutine StartCoroutine_Auto (IEnumerator routine)
	{
		MonoRaiseIfNull(routine);
		return self->StartCoroutineManaged2(routine);
	}


	// Starts a coroutine named /methodName/.
	CUSTOM Coroutine StartCoroutine (string methodName, object value = null) 
	{
		MonoRaiseIfNull((void*)(!methodName.IsNull()));
		char* cString = ScriptingStringToAllocatedChars (methodName);
		ScriptingObjectPtr coroutine = self->StartCoroutineManaged (cString, value);
		ScriptingStringToAllocatedChars_Free (cString);
		return coroutine;
	}

	// Stops all coroutines named /methodName/ running on this behaviour.
	CUSTOM void StopCoroutine (string methodName) { self->StopCoroutine(methodName.AsUTF8().c_str()); }
	

	// Stops all coroutines running on this behaviour.
	CUSTOM void StopAllCoroutines () { self->StopAllCoroutines(); }
	*/
}

}
