
using System;
using System.IO;
using System.Collections.Generic;

namespace UnityEngine
{
	public class TextAsset: Object
	{
		public Byte[] bytes { get; private set; }
		public string text { get; private set; }
//		public string text {
//			get {
//				return System.Text.Encoding.UTF8.GetString(bytes);
//			}
//		}

		public TextAsset(string text)
		{
			this.text = text;
		}
	}
}

