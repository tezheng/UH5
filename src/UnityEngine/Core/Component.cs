using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{
public partial class Component : Object
{
	public Transform transform
	{
		get
		{
			return gameObject.transform;
		}
	}

	public Animation animation
	{
		get {
			return gameObject.animation;//GetComponent<Animation>();
		}
	}

	public Collider collider
	{
		get {
			return gameObject.collider;//GetComponent<Collider>();
		}
	}

	public Light light
	{
		get {
			return gameObject.light;//GetComponent<Collider>();
		}
	}

	public Renderer renderer
	{
		get {
			return gameObject.renderer;
		}
	}

	public Camera camera
	{
		get {
			return gameObject.camera;
		}
	}

	public AudioSource audio
	{
		get {
			return gameObject.audio;
		}
	}

	public Rigidbody rigidbody
	{
		get {
			return gameObject.rigidbody;
		}
	}

	GameObject _gameObject;
	public GameObject gameObject
	{
		get
		{
			return _gameObject;
		}
	}

	[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
	public Component GetComponent (Type type)
	{
		return gameObject.GetComponent(type);
	}

	public T GetComponent<T>()
	{
		return (T)(object)GetComponent(typeof(T));
	}
	
	public Component GetComponent (string type)
	{
		return gameObject.GetComponent(type);
	}

	Component GetComponentInChildren (Type t)
	{
		return gameObject.GetComponentInChildren (t);
	}
	
	public T GetComponentInChildren<T> ()
	{
		return (T) (object) GetComponentInChildren(typeof(T));
	}

	public Component[] GetComponentsInChildren (Type t, bool includeInactive = false)
	{
		return gameObject.GetComponentsInChildren (t, includeInactive);
	}
	
	public T[] GetComponentsInChildren<T> (bool includeInactive) where T : Component
	{
		return gameObject.GetComponentsInChildren<T>(includeInactive);
	}
	
	public void GetComponentsInChildren<T> (bool includeInactive, List<T> result) where T : Component
	{
		gameObject.GetComponentsInChildren<T>(includeInactive, result);
	}

	public T[] GetComponentsInChildren<T> () where T : Component
	{
		return GetComponentsInChildren<T>(false);
	}
	
	public void GetComponentsInChildren<T> (List<T> results) where T : Component
	{
		GetComponentsInChildren<T>(false, results);
	}

	[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
	public Component GetComponentInParent (Type t)
	{
		return gameObject.GetComponentInParent (t);
	}

	public T GetComponentInParent<T> ()
	{
		return (T) (object) GetComponentInParent(typeof(T));
	}

	public Component[] GetComponentsInParent (Type t, bool includeInactive = false)
	{
		return gameObject.GetComponentsInParent (t, includeInactive);
	}
	
	public T[] GetComponentsInParent<T> (bool includeInactive) where T : Component
	{
		return gameObject.GetComponentsInParent<T>(includeInactive);
	}

	public T[] GetComponentsInParent<T> () where T : Component
	{
		return GetComponentsInParent<T>(false);
	}

	public Component[] GetComponents (Type type)
	{
		return gameObject.GetComponents(type);
	}
	
	public string tag
	{
		get { return gameObject.tag; }
		set { gameObject.tag = value; }
	}
	
	public T[] GetComponents<T>() where T : Component
	{
		return gameObject.GetComponents<T>();
	}

	public bool CompareTag (string tag)
	{
		return gameObject.tag == tag;
	}
	
	public void SendMessageUpwards (string methodName, SendMessageOptions options)
	{
		SendMessageUpwards(methodName, null, options);
	}

	public void SendMessage (string methodName, SendMessageOptions options)
	{
		SendMessage (methodName, null, options);
	}
	
	public void BroadcastMessage (string methodName, SendMessageOptions options)
	{
		BroadcastMessage (methodName, null, options);
	}
}

partial class Component
{
/*
	public void GetComponents (Type type, List<Component> results)
	{
		GetComponentsForListInternal(type, results);
	}
	
	public void GetComponents<T>(List<T> results)
	{
		GetComponentsForListInternal(typeof(T), results);
	}

	private void GetComponentsForListInternal(Type searchType, object resultList)
	{
		
	}
*/

	internal virtual void WillDestroyComponent () {}
	internal virtual void Deactivate() {}

	internal void SetGameObjectInternal(GameObject go)
	{
		_gameObject = go;
	}

	public void SendMessageUpwards (string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
	{
		gameObject.SendMessageUpwards(methodName, value, options);
	}

	public void BroadcastMessage (string methodName, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
	{
		gameObject.BroadcastMessage(methodName, parameter, options);
	}

	public void SendMessage (string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
	{
		gameObject.SendMessage(methodName, value, options);
	}

	internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
	{
		base.AwakeFromLoad(awakeMode);
	}

	internal bool IsActive()
	{
		return gameObject != null && gameObject.IsActive();
	}
}

}
