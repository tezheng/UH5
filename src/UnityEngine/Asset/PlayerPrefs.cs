using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
	public sealed class PlayerPrefsException : Exception
	{
		//
		// Constructors
		//
		public PlayerPrefsException (string error) : base (error)
		{
		}
	}

	public class PlayerPrefs
	{

		private static Dictionary<string, object> storage = LoadImpl () as Dictionary<string, object>;

		public static void DeleteAll ()
		{
			storage.Clear ();
		}

		public static void DeleteKey (string key)
		{
			storage.Remove (key);
		}

		public static float GetFloat (string key, float defaultValue)
		{
			if (storage.ContainsKey (key)) {
				object o = storage [key];
				if (o != null) {
					if (o is float) {
						return (float) o;
					}
					float val;
					if (float.TryParse (o.ToString (), out val)) {
						return val;
					}
				}
			}
			return defaultValue;
		}

		public static float GetFloat (string key)
		{
			float defaultValue = 0f;
			return PlayerPrefs.GetFloat (key, defaultValue);
		}

		public static int GetInt (string key)
		{
			int defaultValue = 0;
			return PlayerPrefs.GetInt (key, defaultValue);
		}

		public static int GetInt (string key, int defaultValue)
		{
			if (storage.ContainsKey (key)) {
				object o = storage [key];
				if (o != null) {
					if (o is int) {
						return (int) o;
					}
					int val;
					if (int.TryParse (o.ToString (), out val)) {
						return val;
					}
				}
			}
			return defaultValue;
		}

		public static string GetString (string key)
		{
			string empty = string.Empty;
			return PlayerPrefs.GetString (key, empty);
		}

		public static string GetString (string key, string defaultValue)
		{
			if (storage.ContainsKey (key)) {
				object o = storage [key];
				if (o != null) {
					if (o is string) {
						return o as string;
					}
					return o.ToString();
				}
			}
			return defaultValue;
		}

		public static bool HasKey (string key)
		{
			return storage.ContainsKey(key);
		}

		public static void Save ()
		{
			object obj = null;
			foreach (var item in storage) {
				obj = SaveElement (obj, item.Key, item.Value);
			}
			SaveImpl (obj);
		}

		private static extern object SaveElement (object obj, string key, object val);
		private static extern void SaveImpl (object obj);
		private static extern object LoadImpl ();

		public static void SetFloat (string key, float value)
		{
			if (!PlayerPrefs.TrySetFloat (key, value))
			{
				throw new PlayerPrefsException ("Could not store preference value");
			}
		}

		private static bool TrySetFloat (string key, float value)
		{
			storage [key] = value;
			return true;
		}

		public static void SetInt (string key, int value)
		{
			if (!PlayerPrefs.TrySetInt (key, value))
			{
				throw new PlayerPrefsException ("Could not store preference value");
			}
		}

		private static bool TrySetInt (string key, int value)
		{
			storage [key] = value;
			return true;
		}

		public static void SetString (string key, string value)
		{
			if (!PlayerPrefs.TrySetString (key, value))
			{
				throw new PlayerPrefsException ("Could not store preference value");
			}
		}

		private static bool TrySetString (string key, string value)
		{
			storage [key] = value;
			return true;
		}
	}
}
