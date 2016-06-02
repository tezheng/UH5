using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;

namespace UnityEngine
{
	public class AssemblyDetector
	{

	}

	public class AssemblyHelper
	{

	}

	public class AssemblyRegister
	{
		public static void Register()
		{
			TypeUtil.RegisterAssembly (typeof(AssemblyRegister).Assembly);
		}
	}
}