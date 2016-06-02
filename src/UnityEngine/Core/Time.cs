using System;

namespace UnityEngine
{

// The interface to get time information from Unity.
public class Time
{
	public static TimeManager GetTimeManager() {return TimeManager.Instance;}

	// The time this frame has started (RO). This is the time in seconds since the start of the game.
		public static float time { get {return (float)(GetTimeManager ().GetCurTime ()); } }

	// The time this frame has started (RO). This is the time in seconds since the last level has been loaded.
		public static float timeSinceLevelLoad { get {return (float)(GetTimeManager ().GetTimeSinceLevelLoad ()); } }
	
	// The time in seconds it took to complete the last frame (RO).
	public static float deltaTime { get {return GetTimeManager ().GetDeltaTime (); } }
	
	// The time the latest MonoBehaviour::pref::FixedUpdate has started (RO). This is the time in seconds since the start of the game.
		public static float fixedTime { get {return (float)(GetTimeManager ().GetFixedTime ()); } }

	// The cached real time (realTimeSinceStartup) at the start of this frame
		public static float unscaledTime { get {return (float)(GetTimeManager ().GetRealtimeStartOfFrame ()); } }
	
	// The delta time based upon the realTime
		public static float unscaledDeltaTime { get {return (float)(GetTimeManager ().GetRealtimeDelta ()); } }

	// The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour::pref::FixedUpdate) are performed.
	public static float fixedDeltaTime  { get { return GetTimeManager ().GetFixedDeltaTime (); }  set { GetTimeManager ().SetFixedDeltaTime (value); } }

	// The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour::pref::FixedUpdate)
	public static float maximumDeltaTime  { get { return GetTimeManager ().GetMaximumDeltaTime (); } set { GetTimeManager ().SetMaximumDeltaTime (value); } }
	
	// A smoothed out Time.deltaTime (RO).
	public static float smoothDeltaTime { get {return GetTimeManager ().GetSmoothDeltaTime (); } }

	// The scale at which the time is passing. This can be used for slow motion effects.
	public static float timeScale { get { return GetTimeManager ().GetTimeScale (); }  set { GetTimeManager ().SetTimeScale (value); } }
	
	// The total number of frames that have passed (RO).
	public static int frameCount { get { return GetTimeManager ().GetFrameCount (); } }
	
	//*undocumented*
	public static int renderedFrameCount { get { return GetTimeManager ().GetRenderFrameCount (); } }

	// The real time in seconds since the game started (RO).
		public static float realtimeSinceStartup { get { return (float)(GetTimeManager().GetRealtime ()); } }

	// If /captureFramerate/ is set to a value larger than 0, time will advance in
//	public static int captureFramerate	{ get { return GetTimeManager().GetCaptureFramerate(); } set { GetTimeManager().SetCaptureFramerate (value); } }
}

}