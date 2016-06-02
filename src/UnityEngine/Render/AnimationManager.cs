using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public partial class AnimationManager
	{
		public List<Animation> animations = new List<Animation>();
		internal void Remove(Animation animation)
		{
			animations.Remove(animation);
		}

		internal void Add(Animation animation)
		{
			animations.Add(animation);
		}

		internal void Update()
		{
			foreach (var a in animations)
			{
				a.Update();
			}
		}
	}
}
