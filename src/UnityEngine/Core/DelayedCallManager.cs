using UnityEngineInternal;
using System;
using System.Collections.Generic;

namespace UnityEngine
{

	internal class DelayedCallManager
	{
		class Data
		{
			public Action<Object, System.Object> func;
			public Object target;
			public System.Object userdata;
			public float repeatTime;
			public int mode;

			public double runTime;
			//public LinkedListNode<Data> mnode;
		}
		internal const int kRunFixedFrameRate     = 1 << 0;
		internal const int kRunDynamicFrameRate   = 1 << 1;
		internal const int kRunStartupFrame       = 1 << 2;
		internal const int kWaitForNextFrame      = 1 << 3;
		internal const int kAfterLoadingCompleted = 1 << 4;
		internal const int kEndOfFrame            = 1 << 5;
		internal const int kRunOnClearAll         = 1 << 6;     ///< Execute a delayed function when clearing all callbacks
		internal static void CallDelayed(Action<Object, System.Object> func, Object o, float time,
			System.Object userdata, float repeatTime, int mode)
		{
			var d = new Data();
			d.func = func;
			d.target = o;
			d.userdata = userdata;
			d.runTime = time + Time.realtimeSinceStartup;
			d.mode = mode;
			d.repeatTime = repeatTime;
			l.Add(d);
		}

/*
		internal static void Update()
		{
			foreach (var d in l)
			{
				d.mnode = late_l.AddLast (d);
			}
			l.Clear ();
			double curTime = Time.realtimeSinceStartup;
			var t = new LinkedList<Data > ();
			foreach (var d in late_l)
			{
				if (d.runTime >= curTime)
				{
					d.func(d.target, d.userdata);
				}
				if (d.repeatTime > 0)
				{
					d.runTime = curTime + (double)d.repeatTime;
				}
				else
				{
					t.AddLast(d);
				}
			}
			foreach (var d in t)
			{
				d.mnode.List.Remove(d.mnode);
			}
		}
*/

		internal static void Update()
		{
			foreach (var d in l)
			{
				late_l.Add(d);
			}
			l.Clear();
			double curTime = Time.realtimeSinceStartup;
			var t = new List<Data>();
			foreach (var d in late_l)
			{
				if (d.runTime <= curTime)
				{
					d.func(d.target, d.userdata);
					if (d.repeatTime > 0)
					{
						d.runTime = curTime + (double)d.repeatTime;
					}
					else
					{
						t.Add(d);
					}
				}
			}
			foreach (var d in t)
			{
				late_l.Remove(d);
			}
		}
		//static LinkedList<Data> l = new LinkedList<Data>();
		//static LinkedList<Data> late_l = new LinkedList<Data>();
		static List<Data> l = new List<Data>();
		static List<Data> late_l = new List<Data>();
	}

}
