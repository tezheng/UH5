
using System;

namespace Logic
{
	public class Convert
	{
		public static int ToInt32(object val)
		{
			if (val is sbyte
	            || val is byte
	            || val is short
	            || val is ushort
	            || val is int
	            || val is uint
	            || val is long
	            || val is ulong
	            || val is float
	            || val is double
	            || val is decimal)
            return (int) val;
			if (!(val is string))
				throw new System.Exception("Wrong Convert.ToInt32");
			return (int) long.Parse((string)val);
		}

		public static long ToInt64(object val)
		{
			if (val is sbyte
	            || val is byte
	            || val is short
	            || val is ushort
	            || val is int
	            || val is uint
	            || val is long
	            || val is ulong
	            || val is float
	            || val is double
	            || val is decimal)
            return (long) val;
			if (!(val is string))
				throw new System.Exception("Wrong Convert.ToInt32");
			return long.Parse((string)val);
		}

		public static float ToSingle(object val)
		{
			if (val is sbyte
	            || val is byte
	            || val is short
	            || val is ushort
	            || val is int
	            || val is uint
	            || val is long
	            || val is ulong
	            || val is float
	            || val is double
	            || val is decimal)
            return (float) val;
			if (!(val is string))
				throw new System.Exception("Wrong Convert.ToSingle");
			return float.Parse((string)val);
		}

		public static float ToDouble(object val)
		{
			if (val is sbyte
	            || val is byte
	            || val is short
	            || val is ushort
	            || val is int
	            || val is uint
	            || val is long
	            || val is ulong
	            || val is float
	            || val is double
	            || val is decimal)
            return (float) val;
			if (!(val is string))
				throw new System.Exception("Wrong Convert.ToDouble");
			return float.Parse((string)val);
		}

		// public static int ToInt32(string val, int basev)
		// {
		// 	return 0;
		// }

		// public static char ToChar(int a)
		// {
		// 	return 'a';
		// }

		// public static string ToString(int codepoint, int size)
		// {
		// 	return "";
		// }

		// public static string ToBase64String()
		// {
		// 	return "";
		// }

		// public static object FromBase64String(string str)
		// {
		// 	return null;
		// }
	}
}
