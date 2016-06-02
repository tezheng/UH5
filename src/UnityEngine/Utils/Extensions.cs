using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
	public static class EnumUtils
	{
		public static int ParseEnum(Type type, string value )
		{
			dynamic a = Enum.Parse( type, value, true );
			return a.value;
		}
	}

}