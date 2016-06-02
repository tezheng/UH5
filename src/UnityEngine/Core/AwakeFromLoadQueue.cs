using System;
using System.Collections.Generic;

namespace UnityEngine
{
	internal class AwakeFromLoadQueue
	{
		List<Object> ol = new List<Object>();
		internal void Add(Object o)
		{
			ol.Add(o);
		}

		internal void AwakeFromLoad (AwakeFromLoadMode mode)
		{
			foreach (var o in ol)
			{
				o.AwakeFromLoad(mode);
			}
		}
	}
}

