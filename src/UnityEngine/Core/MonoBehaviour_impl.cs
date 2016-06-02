using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine
{
	partial class MonoBehaviour
	{
		internal const int C_Awake = 0;
		internal const int C_OnEnable = 1;
		internal const int C_OnDisable = 2;
		internal const int C_Start = 3;
		internal const int C_Update = 4;
		internal const int C_LateUpdate = 5;
		internal const int C_OnDestroy = 6;
		internal const int C_NUM = 7;

		class MethodData
		{
			public int pcount;
			public MethodInfo m;
		}

		MethodData[] md = new MethodData[C_NUM];
		static string[] names = new string[] {"Awake", "OnEnable", "OnDisable", "Start", "Update", "LateUpdate", "OnDestroy"};

		private static MethodInfo GetMethodRecrusive(Type type, string key)
		{
			while (type != null)
			{
				MethodInfo methodInfo = type.GetMethod(key, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (methodInfo != null)
				{
					return methodInfo;
				}
				type = type.BaseType;
			}
			return null;
		}

		internal void PerformMessage(int predefined_m, object value = null)
		{
			if (md[predefined_m] == null)
			{
				Type type = GetType ();
				MethodInfo methodInfo = GetMethodRecrusive(type, names[predefined_m]);
				var t = new MethodData();
				if (methodInfo == null)
				{
					t.m = methodInfo;
					md [predefined_m] = t;
					return;
				}
				var parameters = methodInfo.GetParameters ();
				t.pcount = parameters.Length;
				t.m = methodInfo;
				md[predefined_m] = t;
			}
			if (md[predefined_m].m == null) return;
			if (md[predefined_m].pcount == 1)
			{
				md[predefined_m].m.Invoke (this, new object[] { value });
			}
			else
			{
				md[predefined_m].m.Invoke (this, null);
			}
		}

		internal void PerformMessage(string name, object value = null)
		{
			Type type = GetType ();
			MethodInfo methodInfo = GetMethodRecrusive(type, name);
			if (methodInfo == null) return;
			var parameters = methodInfo.GetParameters ();
			if (parameters.Length == 1) {
				methodInfo.Invoke (this, new object[] { value });
			} else {
				methodInfo.Invoke (this, null);
			}
		}
	}

	class BehaviourList
	{
//		internal static LinkedList<Behaviour> bs = new LinkedList<Behaviour>();
//		static LinkedList<Behaviour> late_bs = new LinkedList<Behaviour>();
		List<Behaviour> bs = new List<Behaviour>();
		List<Behaviour> late_bs = new List<Behaviour>();
		internal int order;
		internal void AddBehaviour(Behaviour b)
		{
			//if (b.mnode != null) return;
			//b.mnode = bs.AddLast(b);
			bs.Add(b);
		}

		internal void RemoveBehaviour(Behaviour b)
		{
			//if (b.mnode != null)
			//	b.mnode.List.Remove (b.mnode);
			//b.mnode = null;
			bs.Remove(b);
			if (in_update)
			{
				pendingDelete.Add(b);
			}
			else
			{
				late_bs.Remove(b);
			}
		}

		List<Behaviour> pendingDelete = new List<Behaviour>();
		bool in_update;

		internal void Run()
		{
			in_update = true;
			foreach (var b in bs)
			{
			//	b.mnode = late_bs.AddLast (b);
				late_bs.Add(b);
			}
			bs.Clear ();
			foreach (var b in late_bs)
			{
				b.__Update();
			}

			foreach (var b in late_bs)
			{
				b.__LateUpdate();
			}
			foreach (var b in pendingDelete)
			{
				late_bs.Remove(b);
			}
			pendingDelete.Clear();
			in_update = false;
		}
	}

	class BehaviourManager
	{
		static List<BehaviourList> l = new List<BehaviourList>();
		internal static BehaviourList GetBehaviourList(int executionOrder)
		{
			foreach (var bl in l)
			{
				if (bl.order == executionOrder)
				{
					return bl;
				}
			}
			return null;
		}
		internal static void AddBehaviour(Behaviour b, int executionOrder)
		{
			var bl = GetBehaviourList(executionOrder);
			if (bl == null)
			{
				bl = new BehaviourList();
				bl.order = executionOrder;
				l.Add(bl);
			}
			bl.AddBehaviour(b);
		}

		internal static void RemoveBehaviour(Behaviour b, int executionOrder)
		{
			var bl = GetBehaviourList(executionOrder);
			if (bl != null)
			{
				bl.RemoveBehaviour(b);
			}
		}

		internal static void Run()
		{
			l.Sort((a, b) => (a.order - b.order < 0 ? -1 : 1));
			foreach (var bl in l)
			{
				bl.Run();
			}
		}
	}
}

