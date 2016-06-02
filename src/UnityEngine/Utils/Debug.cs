using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics;

namespace UnityEngine
{

public class Debug
{

	// Draws a line from the /point/ start to /end/ with color for a duration of time and with or without depth testing. If duration is 0 then the line is rendered 1 frame.

	public static void DrawLine (Vector3 start, Vector3 end) {DrawLine(start, end, Color.white);}
	public static void DrawLine (Vector3 start, Vector3 end, Color color, float duration = 0.0f, bool depthTest = true) { DebugDrawLine (start, end, color, duration, depthTest); }

	
	// Draws a line from /start/ to /start/ + /dir/ with color for a duration of time and with or without depth testing. If duration is 0 then the line is rendered 1 frame.
	
	public static void DrawRay (Vector3 start, Vector3 dir) {DrawRay(start, dir, Color.white);}
	public static void DrawRay (Vector3 start, Vector3 dir, Color color, float duration = 0.0f, bool depthTest = true) { DrawLine (start, start + dir, color, duration, depthTest); }
	
	static void DebugDrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
	{

	}

	private static void Internal_Log (int level, string msg, Object obj)
	{
		Console.WriteLine(level.ToString() + " : " + msg + " " + ((obj == null) ? "" : obj.ToString()));
	}

	private static void Internal_LogException(Exception exception, Object obj)
	{
		Console.WriteLine("exception:" + exception.ToString ());
	}

	// Logs /message/ to the Unity Console.
	public static void Log (object message) { Internal_Log (0, message != null ? message.ToString () : "Null", null); }

	// Logs /message/ to the Unity Console.
	public static void Log (object message, Object context)
	{
		Internal_Log (0, message != null ? message.ToString () : "Null", context);
	}

	public static void LogFormat (string format, params object[] args)
	{
		Log (string.Format (format, args));
	}

	public static void LogFormat (UnityEngine.Object context, string format, params object[] args)
	{
		Log (string.Format (format, args), context);
	}
	
	// A variant of Debug.Log that logs an error message to the console.
	public static void LogError (object message) { Internal_Log (2, message != null ? message.ToString () : "Null", null); }

	// A variant of Debug.Log that logs an error message to the console.
	public static void LogError (object message, Object context) { Internal_Log (2,message.ToString (), context); }

	public static void LogErrorFormat (string format, params object[] args)
	{
		LogError (string.Format (format, args));
	}

	public static void LogErrorFormat (UnityEngine.Object context, string format, params object[] args)
	{
		LogError (string.Format (format, args), context);
	}

	// A variant of Debug.Log that logs an error message from an exception to the console.
	public static void LogException(Exception exception) { Internal_LogException(exception, null); }

	// A variant of Debug.Log that logs an error message to the console.
	public static void LogException(Exception exception, Object context) { Internal_LogException(exception, context); }
	
	// A variant of Debug.Log that logs a warning message to the console.
	public static void LogWarning (object message) { Internal_Log (1,message.ToString (), null); }

	// A variant of Debug.Log that logs a warning message to the console.
	public static void LogWarning (object message, Object context) { Internal_Log (1,message.ToString (), context); }

	public static void LogWarningFormat (string format, params object[] args)
	{
		LogWarning (string.Format (format, args));
	}

	public static void LogWarningFormat (UnityEngine.Object context, string format, params object[] args)
	{
		LogWarning (string.Format (format, args), context);
	}

	internal static void Assert(bool value)
	{
//		if (value) return;
//		LogError("Assert failed.");
//		StackTrace stackTrace = new StackTrace();           
//		StackFrame[] stackFrames = stackTrace.GetFrames();  
//
//		foreach (StackFrame stackFrame in stackFrames)
//		    Console.WriteLine(stackFrame.GetMethod().Name);
	}
}

}