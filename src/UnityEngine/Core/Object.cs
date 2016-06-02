using UnityEngineInternal;
using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public enum HideFlags
	{
		// A normal, visible object. This is the default.
		None = 0,

		// The object will not appear in the hierarchy and will not show up in the project view if it is stored in an asset.
		HideInHierarchy = 1,

		// It is not possible to view it in the inspector
		HideInInspector = 2,

		// The object will not be saved to the scene.
		DontSaveInEditor = 4,

		// The object is not be editable in the inspector
		NotEditable = 8,

		// The object will not be unloaded by UnloadUnusedAssets
		DontUnloadUnusedAsset = 16,

		// The object will not be saved when building a player
		DontSaveInBuild = 32,

		DontSave = 4 + 16 + 32,

		// A combination of not shown in the hierarchy and not saved to to scenes.
		HideAndDontSave = 1 + 4 + 8 + 16 + 32
	}

	public class Object
	{
	public HideFlags hideFlags;
	public override bool Equals(object o) { return CompareBaseObjects (this, o as Object); }
	public override int GetHashCode()	{ return GetInstanceID();	}

	// Does the object exist?
	public static implicit operator bool (Object exists)
	{
		return !CompareBaseObjects (exists, null);
	}

	internal bool destroyed;
	static bool IsNativeObjectAlive(UnityEngine.Object o)
	{
		return !o.destroyed;
	}

	static bool CompareBaseObjects (UnityEngine.Object lhs, UnityEngine.Object rhs)
	{
		bool lhsNull = ((object)lhs)==null;
		bool rhsNull = ((object)rhs)==null;
		
		if (rhsNull && lhsNull) return true;

		if (rhsNull) return !IsNativeObjectAlive(lhs);
		if (lhsNull) return !IsNativeObjectAlive(rhs);

		return lhs.m_InstanceID == rhs.m_InstanceID;
	}

	// Returns the instance id of the object.
	public int GetInstanceID ()
	{
		return m_InstanceID;
	}

	// Clones the object /original/ and returns the clone.
	[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
	public static Object Instantiate (Object original, Vector3 position, Quaternion rotation)
	{
		CheckNullArgument(original,"The thing you want to instantiate is null.");
		return Internal_InstantiateSingle (original, position, rotation);
	}
	
	// Clones the object /original/ and returns the clone.
	[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
	public static Object Instantiate (Object original)
	{
		CheckNullArgument(original,"The thing you want to instantiate is null.");
		return Internal_CloneSingle (original);
	}

	public static T Instantiate<T>(T original) where T : UnityEngine.Object
	{
		CheckNullArgument(original,"The thing you want to instantiate is null.");
		return (T)Internal_CloneSingle (original);
	}
	
	static private void CheckNullArgument(object arg, string message)
	{
		if (arg==null)
			throw new System.ArgumentException(message);
	}

	public static T[] FindObjectsOfType<T> () where T: Object
	{
		return Resources.ConvertObjects<T> (FindObjectsOfType (typeof (T)));
	}

	// Returns the first active loaded object of Type /type/.
	[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
	public static Object FindObjectOfType (System.Type type)
	{
		Object[] objects = FindObjectsOfType (type);
		if (objects.Length > 0)
			return objects[0];
		else
			return null;
	}

	public static T FindObjectOfType<T> () where T: Object
	{
		return (T)FindObjectOfType (typeof (T));
	}

	public static bool operator == (Object x, Object y) { return CompareBaseObjects (x, y); }
	
	public static bool operator != (Object x, Object y) { return !CompareBaseObjects (x, y); }

	internal int m_DirtyIndex;
	internal void SetDirty()
	{
		m_DirtyIndex++;
		if (m_DirtyIndex == 0)
			m_DirtyIndex = 1;
		// here call something.
	}

	internal virtual void __Reset ()
	{
	}

	protected bool m_AwakeCalled;
	internal virtual void AwakeFromLoad (AwakeFromLoadMode awakeMode)
	{
		m_AwakeCalled = true;
	}

	public int m_InstanceID;

	private static Object Internal_CloneSingle (Object data)
	{
		// Hack now. 
		GameObject go = data as GameObject;
		if (!go.IsPrefabParent())
		{
			return null;
		}
		return Resources.InstantiatePrefab(go._load_guid);
	}

	private static Object Internal_InstantiateSingle (Object data, Vector3 pos, Quaternion rot)
	{
		GameObject o = Internal_CloneSingle(data) as GameObject;
		if (o != null)
		{
			o.transform.position = pos;
			o.transform.rotation = rot;
		}
		return o;
	}

	// The name of the object.
	public string name;
	
	// Makes the object /target/ not be destroyed automatically when loading a new scene.
	public static void DontDestroyOnLoad (Object target)
	{
		Debug.LogError("Object.DontDestroyOnLoad Not Implemented. P2");
	}

	// Returns the name of the game object.
	public override string ToString() {
		return name;
	}

	// Need to be implemented.
	static int uid = 0;
	internal Object()
	{
		uid++;
		m_InstanceID = uid;
	}

	// Returns a list of all active loaded objects of Type /type/.
	// No assets.
	public static Object[] FindObjectsOfType (Type type)
	{
		List<Object> t = new List<Object>();
		foreach (var go in GameObjectManager.activeObjects)
		{
			foreach (var c in go.cs)
			{
				if (type.IsAssignableFrom(c.GetType()))
				{
					t.Add(c);
				}
			}
		}
		return t.ToArray();
	}

	public static void Destroy(Object obj, float t = 0.0F)
	{
		DestroyObject(obj, t);
	}

	// Removes a gameobject, component or asset.
	public static void DestroyObject(Object obj, float t = 0.0F)
	{
		if (t <= 0.0)
		{
			if (obj is Behaviour)
			{
				(obj as Behaviour).enabled = false;
			}
			if (obj is GameObject)
			{
				GameObject go = obj as GameObject;
				DisableBehaviours(go);
				foreach (Transform tgo in go.transform)
				{
					DisableBehaviours(tgo.gameObject);
				}
			}
		}
		DestroyObjectDelayed(obj, t);
	}

	static void DestroyObjectDelayed(Object obj, float t)
	{
		DelayedCallManager.CallDelayed((o, u) => {DestroyObjectHighLevel(o);}, obj, t,
			null, 0, DelayedCallManager.kRunDynamicFrameRate | DelayedCallManager.kRunFixedFrameRate | DelayedCallManager.kRunOnClearAll);
	}

	static void DisableBehaviours(GameObject go)
	{
		foreach (var c in go.cs)
		{
			if (c is Behaviour)
			{
				(c as Behaviour).enabled = false;
			}
		}
	}

	// Destroys the object /obj/ immediately. It is strongly recommended to use Destroy instead.
	public static void DestroyImmediate (Object obj, bool allowDestroyingAssets = false)
	{
		DestroyObjectHighLevel(obj);
	}

	internal static void DestroyObjectHighLevel (Object o, bool forceDestroy = false)
	{
		if (!o) return;
		{
			if (o is Component)
			{
				if (o is MonoBehaviour && (o as MonoBehaviour).IsDestroying())
				{
					Debug.LogError("Destroying object multiple times. Don't use DestroyImmediate on the same object in OnDisable or OnDestroy.");
					return;
				}
				Component c = o as Component;
				GameObject gameObject = c.gameObject;
				if (gameObject)
				{
					if (gameObject.IsDestroying())
					{
						Debug.LogError("Destroying object multiple times. Don't use DestroyImmediate on the same object in OnDisable or OnDestroy.");
						return;
					}

					if (gameObject.IsActivating())
					{
						Debug.LogError("Cannot destroy Component while GameObject is being activated or deactivated.", gameObject);
						return;
					}
					/*
					string error;
					if (!forceDestroy && !CanRemoveComponent(component, &error))
					{
						ErrorStringObject (error, &component);
						return;
					}
					
					// We need to perform an explicit check here for RectTransform because the above 'CanRemoveComponent' allows this type
					// to be removed because this action under the editor replaces the RectTransform with a standard Transform.
					// Via script, this doesn't happen currently.
					if (component.GetClassID () == ClassID(RectTransform))
					{
						const char* message = "Can't destroy RectTransform component of '%s'. "
							"If you want to destroy the game object, please call 'Destroy' on the game object instead. "
							"Destroying the RectTransform component is not allowed.";

						error = Format (message, gameObject->GetName ());
						ErrorStringObject (error, &component);
						return;
					}
					*/

					if (gameObject.IsActive ())
					{
						c.Deactivate ();
					}

					c.WillDestroyComponent();
					gameObject.cs.Remove(c);
				}
				else
				{
					c.WillDestroyComponent();
				}
				c.destroyed = true;
			}
			else if (o is GameObject)
			{
				GameObject gameObject = o as GameObject;
				if (gameObject.IsDestroying())
				{
					Debug.LogError("Destroying object multiple times. Don't use DestroyImmediate on the same object in OnDisable or OnDestroy.");
					return;
				}

				if (gameObject.IsActivating())
				{
					Debug.LogError("Cannot destroy GameObject while it is being activated or deactivated.");
					return;
				}

				Transform parent = gameObject.transform;
				if (parent)
				{
					parent = parent.parent;
					if (parent && parent.gameObject.IsActivating())
					{
						Debug.LogError("Cannot destroy GameObject while it is being activated or deactivated.");
						return;
					}
				}
				DestroyGameObjectHierarchy(gameObject);
			}
		}
	}

	static void DestroyGameObjectHierarchy (GameObject gameObject)
	{
		// Deactivate and mark is being destroyed recursively
		// Send all necessary callbacks etc.
		gameObject.Deactivate();

		int count = 0;
		PreDestroyRecursive(gameObject, ref count);
		if (gameObject.transform)
		{
			gameObject.transform.RemoveFromParent();
		}

		if(count == 0)
			return;

		// passing objectCount doesn't guarantee that we'll get a buffer big enough
		// to hold that many objects.  There is some buffering going on, and the buffer
		// will be purged when we fill it up during DestroyGameObjectRecursive.
		// For now, all it's doing is limiting the size of the buffer we create incase
		// we have objectCount < buffer size.

		// Destroy the objects (There should be no callbacks happening at this stage anymore)
		DestroyGameObjectRecursive(gameObject);
	}

	static void PreDestroyRecursive(GameObject gameObject, ref int count)
	{
		if (gameObject.IsActivating())
		{
			Debug.LogError("Cannot destroy GameObject while it is being activated or deactivated.");
			return;
		}

		/*
		// the callback is only called if the GameObject is
		// really destroyed (not only removed from memory)
		GameObject::InvokeDestroyedCallback(&gameObject);
		*/

		gameObject.WillDestroyGameObject();
		count += 1 + gameObject.GetComponentCount();

		Transform transform = gameObject.transform;
		if (transform)
		{
			foreach (Transform t in transform)
			{
				PreDestroyRecursive(t.gameObject, ref count);
			}
		}
	}

	// not very needed for current csharp version.
	static void DestroyGameObjectRecursive (GameObject gameObject)
	{
		Debug.Assert(!gameObject.IsActive());
		Debug.Assert(gameObject.IsDestroying());

		var transform = gameObject.transform;
		if (transform)
		{
			foreach (Transform t in transform)
			{
				DestroyGameObjectRecursive(t.gameObject);
			}
		}

		gameObject.DoDestroySelf();

		if (gameObject.IsActivating())
		{
			if (transform)
				transform.RemoveFromParent();
			Debug.LogError("Cannot destroy GameObject while it is being activated or deactivated.");
			return;
		}
	}
}

}
