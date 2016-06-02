using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{
//	partial class GameObject
//	{
//		internal LinkedListNode<GameObject> node;
//	}
//
	class GameObjectManager
	{
//		internal static LinkedList<GameObject> activeObjects = new LinkedList<GameObject>();

		internal static List<GameObject> activeObjects = new List<GameObject>();
		internal static void AddObject(GameObject go)
		{
			//if (go.node != null) return;
			//go.node = activeObjects.AddLast(go);
			activeObjects.Add(go);
		}

		internal static void RemoveObject(GameObject go)
		{
			//if (go.node != null)
			//	activeObjects.Remove(go.node);
			//go.node = null;
			activeObjects.Remove(go);
		}
	}
}
