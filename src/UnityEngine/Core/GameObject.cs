
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngineInternal;

namespace UnityEngine
{

public class GameObject : Object
{
	public T GetComponent<T>()
	{
		return (T)(object)GetComponent(typeof(T));
	}

	public Component GetComponent (string type)
	{
		return GetComponentByName(type);
	}

	public Component GetComponentInChildren (Type type)
	{
		return GetComponentInChildren(type, false);
	}

	internal Component GetComponentInChildren (Type type, bool includeInactive)
	{
		if( activeInHierarchy || includeInactive) {
			Component attachedCom = GetComponent (type);
			if ( attachedCom != null )
				return attachedCom;
		}

		Transform transform = this.transform;
		if ( transform != null )
		{
			foreach (Transform child in transform)
			{
				Component childCom = child.gameObject.GetComponentInChildren (type, includeInactive);
				if (childCom != null)
					return childCom;
			}
		}
		
		return null;
	}

	public T GetComponentInChildren<T>()
	{
		return (T) (object) GetComponentInChildren(typeof(T));
	}

	internal T GetComponentInChildren<T>(bool includeInactive)
	{
		return (T) (object) GetComponentInChildren(typeof(T), includeInactive);
	}

	[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
	public Component GetComponentInParent (Type type)
	{
		if( activeInHierarchy ) {
		
			Component attachedCom = GetComponent (type);
			if ( attachedCom != null )
				return attachedCom;
		}
		
		Transform transform = this.transform.parent;
		if ( transform != null )
		{
			while (transform != null)
			{
				if (transform.gameObject.activeInHierarchy)
				{
				Component parentCom = transform.gameObject.GetComponent(type);
				if (parentCom != null)
					return parentCom;
				}
				transform = transform.parent;
			}
		}
		
		return null;
	}

	public T GetComponentInParent<T>()
	{
		return (T)(object)GetComponentInParent(typeof(T));
	}
	
	public Component[] GetComponents (Type type)
	{
		return (Component[]) GetComponentsInternal (type, false, false, true, false, null);
	}

	public T[] GetComponents<T>() where T : Component
	{
		return (T[]) GetComponentsInternal<T>(true, false, true, false, null);
	}
	
	public void GetComponents (Type type, List<Component> results)
	{
		if (results == null)
			throw new ArgumentException("The results list cannot be null");
		results.Clear();
		results.AddRange (GetComponents(type));
	}

	public void GetComponents<T>(List<T> results) where T : Component
	{
		if (results == null)
			throw new ArgumentException("The results list cannot be null");
		results.Clear();
		results.AddRange (GetComponents<T>());
	}
	
	public Component[] GetComponentsInChildren (Type type, bool includeInactive = false)
	{
		return (Component[])GetComponentsInternal(type, false, true, includeInactive, false, null);
	}
	
	public T[] GetComponentsInChildren<T>(bool includeInactive) where T : Component
	{
		return (T[])GetComponentsInternal<T>(true, true, includeInactive, false, null);
	}
	
	public void GetComponentsInChildren<T>(bool includeInactive, List<T> results) where T : Component
	{
		if (results == null)
			throw new ArgumentException("The results list cannot be null");
		results.Clear();
		results.AddRange (GetComponentsInChildren<T>(includeInactive));
	}
	
	public T[] GetComponentsInChildren<T>() where T : Component
	{
		return GetComponentsInChildren<T>(false);
	}

	public void GetComponentsInChildren<T>(List<T> results) where T : Component
	{
		GetComponentsInChildren<T>(false, results);
	}
			
	public Component[] GetComponentsInParent (Type type, bool includeInactive = false)
	{
		return (Component[])GetComponentsInternal(type, false, true, includeInactive, true, null);
	}
	
	public T[] GetComponentsInParent<T>(bool includeInactive) where T : Component
	{
		return (T[])GetComponentsInternal<T>(true, true, includeInactive, true, null);
	}
	public T[] GetComponentsInParent<T>() where T : Component
	{
		return GetComponentsInParent<T>(false);
	}

	public void SetActive (bool value) {
		SetSelfActive (value);
	}

	public bool activeSelf {
		get {
			return IsSelfActive ();
		}
	}
	
	public bool activeInHierarchy {
		get {
			return IsActive ();
		}
	}

	public bool CompareTag (string tag)
	{
		return this.tag == tag;
	}

	static public GameObject FindWithTag (string tag)
	{
		return FindGameObjectWithTag(tag);
	}

	public void SendMessageUpwards (string methodName, SendMessageOptions options)
	{
		SendMessageUpwards (methodName, null, options);
	}

	public void SendMessage (string methodName, SendMessageOptions options)
	{
		SendMessage (methodName, null, options);
	}
	
	public void BroadcastMessage (string methodName, SendMessageOptions options)
	{
		BroadcastMessage (methodName, null, options);
	}

	public Component AddComponent (Type componentType)
	{
		return Internal_AddComponentWithType(componentType);
	}

	public T AddComponent<T>() where T : Component
	{
		return AddComponent(typeof(T)) as T;
	}
	
	public GameObject (string name)
	{
		Internal_CreateGameObject (this, name);
	}
	
	public GameObject ()
	{
		Internal_CreateGameObject (this, null);
	}

	public GameObject (string name, params Type[] components)
	{
		Internal_CreateGameObject (this, name);
		foreach (Type t in components)
			AddComponent (t);
	}

	Transform _transform;
	public Transform transform
	{
		get {
			if (_transform == null) _transform = GetComponent<Transform>();
			return _transform;
		}
	}

	public Animation animation
	{
		get {
			return GetComponent<Animation>();
		}
	}

	public Collider collider
	{
		get {
			return GetComponent<Collider>();
		}
	}

	public Light light
	{
		get {
			return GetComponent<Light>();
		}
	}

	public Renderer renderer
	{
		get {
			return GetComponent<Renderer>();
		}
	}

	public Camera camera
	{
		get {
			return GetComponent<Camera>();
		}
	}

	public AudioSource audio
	{
		get {
			return GetComponent<AudioSource>();
		}
	}

	public Rigidbody rigidbody
	{
		get {
			return GetComponent<Rigidbody>();
		}
	}

	public int layer;

	// tag <-> to a int.

	string _tag;
	public string tag
	{
		get {
			return _tag;
		}
		set {
			if (TagManager.IsValidTag(value))
			{
				_tag = value;
			}
			else
			{
				Debug.LogError("Invalid tag " +_tag);
			}
		}
	}

	public GameObject gameObject { get { return this; } }

	public static GameObject CreatePrimitive (PrimitiveType type)
	{
		Debug.LogError("GameObject.CreatePrimitive Not Implemented. P3");
		return null;
	}

	internal Component GetComponentByName (string type)
	{
		Debug.LogError("GameObject.GetComponentByName Not Implemented. P2");
		return null;
	}

	internal Component AddComponent (string className)
	{
		return AddComponent(TypeUtil.GetTypeFromString(className));
	}

	//internal bool isStaticBatchable { return self->IsStaticBatchable (); }

	public static GameObject FindGameObjectWithTag (string tag)
	{
		Debug.LogError("GameObject.FindGameObjectWithTag Not Implemented. P2");
		return null;
	}

	public static GameObject[] FindGameObjectsWithTag (string tag)
	{
		Debug.LogError("GameObject.FindGameObjectsWithTag Not Implemented. P2");
		return null;
	}


// impl
	internal List<Component> cs = new List<Component>();

	internal Component GetComponent (Type type)
	{
		foreach (var c in cs)
		{
			if (type.IsAssignableFrom(c.GetType()))
			{
				return c;
			}
		}
		return null;
	}

	internal Component AddComponent (Type componentType, System.Object o)
	{
		return Internal_AddComponentWithType(componentType, o);
	}

	private System.Array GetComponentsInternal<T>(
		bool useSearchTypeAsArrayReturnType, bool recursive,
		bool includeInactive, bool reverse, object resultList) where T : Component
	{
		Component[] ret = GetComponentsInternal(typeof(T), false, recursive,
			includeInactive, reverse, resultList);
		if (useSearchTypeAsArrayReturnType)
		{
			var newret = new T[ret.Length];
			for (int i = 0; i < ret.Length; i++)
			{
				newret[i] = ret[i] as T;
			}
			return newret;
		}
		else
		{
			return ret;
		}
	}

	private Component[] GetComponentsInternal(Type type,
		bool useSearchTypeAsArrayReturnType, bool recursive,
		bool includeInactive, bool reverse, object resultList)
	{
		List<Component> l = new List<Component>();
		GetComponentsInternalImpl(l, type, recursive, includeInactive, reverse);
		/*
		if (useSearchTypeAsArrayReturnType)
		{
			var ret = Array.CreateInstance(type, l.Count);
			for (var i = 0; i < l.Count; i++)
			{
				ret[0] = l[i];
				//ret.SetValue(l[i], i);
			}
			return ret;
		}
		else*/
		{
			return l.ToArray();
		}
	}

	private void GetComponentsInternalImpl(List<Component> ol, Type type, bool recursive,
		bool includeInactive, bool reverse)
	{
		if (!includeInactive && !IsActive()) return;
		foreach (var c in cs)
		{
			if (type.IsAssignableFrom(c.GetType()))
			{
				ol.Add(c);
			}
		}
		if (!recursive) return;
		if (reverse)
		{
			if (transform.parent != null)
				transform.parent.gameObject.GetComponentsInternalImpl(ol, type, recursive, includeInactive, reverse);
		}
		else
		{
			foreach (Transform t in transform)
			{
				t.gameObject.GetComponentsInternalImpl(ol, type, recursive, includeInactive, reverse);
			}
		}
	}	

	internal bool m_IsActive;
	internal void SetSelfActive (bool state)
	{
		if (state)
			Activate();
		else
			Deactivate();
	}

	bool IsSelfActive()
	{
		return m_IsActive;
	}

	internal bool m_IsPrefab;
	internal bool IsPrefabParent() {return m_IsPrefab;}

	internal void ClearPrefabFlag()
	{
		m_IsPrefab = false;
		foreach (Transform t in transform)
		{
			t.gameObject.ClearPrefabFlag();
		}
	}

	public bool IsActive()
	{
		if (m_IsActiveCached != -1)
			return m_IsActiveCached == 1;

		// Calculate active state based on the hierarchy
		m_IsActiveCached = (m_IsActive && !IsPrefabParent()) ? 1 : 0;
		if (transform)
		{
			Transform parent = transform.parent;
			if (parent)
			{
				bool t = (m_IsActiveCached == 1) && parent.gameObject.IsActive ();
				m_IsActiveCached = t ? 1 : 0;
			}
		}

		return m_IsActiveCached == 1;
	}

	internal override void AwakeFromLoad(AwakeFromLoadMode awakeMode)
	{
		base.AwakeFromLoad (awakeMode);
		UpdateActiveGONode ();
	}


	int m_IsActiveCached = -1;
	internal void Deactivate()
	{
		if (!IsActive())
		{
			if (m_IsActive)
			{
				m_IsActive = false;
				SetDirty();
			}
			return;
		}
		SetDirty();
		m_IsActive = false;
		ActivateAwakeRecursively();
	}

	internal void Activate()
	{
		if (IsActive())
			return;

		SetDirty ();

		m_IsActive = true;
		ActivateAwakeRecursively ();
	}

	void ActivateAwakeRecursively ()
	{
		AwakeFromLoadQueue queue = new AwakeFromLoadQueue();
		ActivateAwakeRecursivelyInternal (queue);
		queue.AwakeFromLoad (AwakeFromLoadMode.kActivateAwakeFromLoad);
	}

	internal bool IsActivating()
	{
		return m_IsActivating;
	}
	bool m_IsActivating;
	void ActivateAwakeRecursivelyInternal(AwakeFromLoadQueue queue)
	{
		if (m_IsActivating)
		{
			Debug.LogError("GameObject is already being activated or deactivated.");
			return;
		}
		bool state;
		bool changed;
		m_IsActivating = true;
		if (m_IsActiveCached != -1)
		{
			bool oldState = m_IsActiveCached == 1;
			m_IsActiveCached = -1;
			state = IsActive();
			changed = oldState != state;
		}
		else
		{
			state = IsActive();
			changed = true;
		}

		if (transform != null)
		{
			var count = transform.children.Count;
			for (int i = 0; i < count; i++)
			{
				var t = transform.GetChild(i);
				if (t != null)
				{
					t.gameObject.ActivateAwakeRecursivelyInternal(queue);
				}
			}
		}

		if (changed)
		{
			for (int i=0;i<cs.Count;i++)
			{
				var c = cs[i];
				if (state)
				{
					c.SetGameObjectInternal (this);
					queue.Add(c);
				}
				else
					c.Deactivate ();
			}

			if (state)
				UpdateActiveGONode ();
			else
				GameObjectManager.RemoveObject(this);
		}
		m_IsActivating = false;
	}

	internal void UpdateActiveGONode()
	{
		GameObjectManager.RemoveObject(this);
		if (IsActive())
		{
			GameObjectManager.AddObject(this);
		}
	}

	public void SendMessageUpwards (string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
	{
		SendMessage(methodName, value, options);
		if (transform.parent != null)
			transform.parent.gameObject.SendMessageUpwards(methodName, value, options);
	}

	public void SendMessage (string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
	{
		if (!IsActive())
		{
			return;
		}
		foreach (var c in cs)
		{
			if (c is MonoBehaviour)
			{
				(c as MonoBehaviour).PerformMessage(methodName, value);
			}
		}
	}

	public void BroadcastMessage (string methodName, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
	{
		SendMessage(methodName, parameter, options);
		foreach (Transform t in transform)
		{
			t.gameObject.BroadcastMessage(methodName, parameter, options);
		}
	}

	private Component Internal_AddComponentWithType (Type componentType, System.Object o = null)
	{
		bool isTransform = typeof(Transform).IsAssignableFrom(componentType);
		bool isMonoBehaviour = typeof(MonoBehaviour).IsAssignableFrom(componentType);

		if (!typeof(Component).IsAssignableFrom(componentType))
		{
			return null;
		}
		if (isTransform && transform != null)
		{
			Debug.LogError("Transform can't be add twice.");
			return null;
		}
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		Component co = null;

		// HACK! MonoBehaviour can have default ctor only!
		if (o != null && !isMonoBehaviour)
		{
			co = (Component)Activator.CreateInstance(componentType, flags, null, new object[] {o}, null);
		}
		else
		{
			co = (Component)Activator.CreateInstance(componentType, flags, null, new object[] {}, null);
		}


		if (co == null)
		{
			Debug.LogError("Internal_AddComponentWithType can't create object.");
			return null;
		}
		co.__Reset ();
		AddComponentInternal(co);

		co.AwakeFromLoad(AwakeFromLoadMode.kInstantiateOrCreateFromCodeAwakeFromLoad);
		co.SetDirty ();

		return co;
	}

	private void AddComponentInternal(Component c)
	{
		cs.Add(c);
		//			com->SetHideFlags(GetHideFlags());
		c.SetGameObjectInternal(this);

		if (IsActive ())
			c.AwakeFromLoad (AwakeFromLoadMode.kActivateAwakeFromLoad);
		else
			c.AwakeFromLoad (AwakeFromLoadMode.kDefaultAwakeFromLoad);
		c.SetDirty ();
		SetDirty ();
	}

	internal GameObject(string name, bool is_prefab)
	{
		this.name = name;
		m_IsPrefab = is_prefab;
		AwakeFromLoad(AwakeFromLoadMode.kDefaultAwakeFromLoad);
	}

	private static void Internal_CreateGameObject (GameObject go, string name)
	{
		if (String.IsNullOrEmpty(name))
		{
			go.name = "New Game Object";
		}
		else
		{
			go.name = name;
		}
		go.AwakeFromLoad (AwakeFromLoadMode.kInstantiateOrCreateFromCodeAwakeFromLoad);
		go.Activate ();
		go.AddComponent<Transform> ();
	}

	public static GameObject Find (string name)
	{
		foreach (var go in GameObjectManager.activeObjects)
		{
			if (go.name == name)
			{
				return go;
			}
		};
		return null;
	}

	bool m_IsDestroying;
	internal bool IsDestroying()
	{
		return m_IsDestroying;
	}
	internal void WillDestroyGameObject()
	{
		m_IsDestroying = true;
		foreach (var c in cs)
		{
			c.WillDestroyComponent();
		}
	}

	internal void DoDestroySelf()
	{
		destroyed = true;
		foreach (var c in cs)
		{
			c.destroyed = true;
		}
	}

	internal int GetComponentCount()
	{
		return cs.Count;
	}

	internal void TransformParentHasChanged()
	{
		if (m_IsActiveCached != -1)
			ActivateAwakeRecursively ();
	}

	internal bool m_IsStaticInScene;

	internal string _load_guid;
}

}
