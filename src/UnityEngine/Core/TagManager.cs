using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{
	internal class TagManager
	{
		internal static bool IsValidTag(string tag)
		{
			return tag == "Default" || tag == "UI";
		}
	}
}
