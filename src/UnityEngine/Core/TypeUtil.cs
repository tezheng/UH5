using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;
using System.IO;
using System.Reflection;

namespace UnityEngine
{

	public class TypeUtil
	{
		static List<Assembly> l = new List<Assembly>();
		public static void RegisterAssembly(Assembly a)
		{
			l.Add(a);
		}

		internal static Type GetTypeFromString(string type)
		{
			foreach (var a in l)
			{
				var t = a.GetType(type);
				if (t != null)
				{
					return t;
				}
			}
			Debug.LogError("Type " + type + " is not existed in project.");
			return null;
		}
	}

}
