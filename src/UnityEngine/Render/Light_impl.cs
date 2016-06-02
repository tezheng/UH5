using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{

	partial class Light
	{
		public Light(System.Object o)
		{
			InitWithRaw(o as UnityEngine.Config.Light);
		}

		public Light()
		{

		}

		internal void InitWithRaw(UnityEngine.Config.Light raw)
		{
			SetEnabled (raw.m_Enabled == 1);
			this.raw = raw;
		}

		internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
		{
			base.AwakeFromLoad (awakeMode);
		}

		internal override void AddToManager()
		{
			Application.GetRenderManager().AddLight(this);
		}

		internal override void RemoveFromManager()
		{
			Application.GetRenderManager().RemoveLight(this);
		}

		internal void Update()
		{
			// do nothing now.
		}

		internal UnityEngine.Config.Light raw;
	}
}