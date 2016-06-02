using System;
using System.Collections.Generic;
namespace UnityEngineInternal
{
	// This is a solution to problem where we cannot use:
	// * System.Collections.Stack on WP8 and Metro, because it was stripped from .NET
	// * System.Collections.Generic.Stack cannot use on iOS because it creates a dependency to System.dll thus increasing overall executable size
#if UNITY_WINRT
	public class GenericStack : Stack<Object>
	{

	}
#else
//	public class GenericStack : System.Collections.Stack
//	{
//
//	}
#endif

}
