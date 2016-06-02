using System;

namespace UnityEngine
{

public class TimeManager
{
	public static TimeManager Instance;
	class TimeHolder
	{
		public TimeHolder() {}
		public double m_CurFrameTime;
		public double m_LastFrameTime;
		public float m_DeltaTime;
		public float m_SmoothDeltaTime;
		public float m_SmoothingWeight;
		public float m_InvDeltaTime;
	};

	public TimeManager()
	{
		Instance = this;
		m_SetTimeManually = false;
		m_UseFixedTimeStep = false;

		m_FixedTime.m_SmoothDeltaTime = m_FixedTime.m_DeltaTime;

		m_TimeScale = 1.0F;
		m_FixedTime.m_DeltaTime = 0.02F;
		m_MaximumTimestep = kMaximumDeltaTime;
		m_LastSyncEnd = 0;
		ResetTime (true);
	}

	public void Update ()
	{
		m_FrameCount++;
		m_RenderFrameCount++;
		if (m_SetTimeManually)
			return;

		// Capture framerate is always constant
		if (m_CaptureFramerate > 0)
		{
			SetTime (m_DynamicTime.m_CurFrameTime + 1.0F / (float)m_CaptureFramerate * m_TimeScale);
			return;
		}

		// Don't do anything to delta time the first frame!
		if (m_FirstFrameAfterReset)
		{
			m_FirstFrameAfterReset = false;
			return;
		}

		// When coming out of a pause / startup / level load we don't want to have a spike in delta time.
		// So just default to kStartupDeltaTime.
		if (m_FirstFrameAfterPause)
		{
			m_FirstFrameAfterPause = false;
			SetTime (m_DynamicTime.m_CurFrameTime + kStartupDeltaTime * m_TimeScale);
			// This is not a real delta time so don't include in smoothed time
			m_ActiveTime.m_SmoothingWeight = 0.0f;
			m_DynamicTime.m_SmoothingWeight = 0.0f;
			return;
		}

		double timeSinceStartup = GetTimeSinceStartup ();
		double time = timeSinceStartup - m_ZeroTime;
		double zeroTime = timeSinceStartup - m_RealZeroTime;
		m_RealtimeStartOfFrameDelta = zeroTime - m_RealtimeStartOfFrame;
		m_RealtimeStartOfFrame = zeroTime;

		// clamp the delta time in case a frame takes too long.
		if (time - m_DynamicTime.m_CurFrameTime > m_MaximumTimestep)
		{
			SetTime (m_DynamicTime.m_CurFrameTime + m_MaximumTimestep * m_TimeScale);
			return;
		}

		// clamp the delta time in case a frame goes to fast! (prevent delta time being zero)
		if (time - m_DynamicTime.m_CurFrameTime < kMinimumDeltaTime)
		{
			SetTime (m_DynamicTime.m_CurFrameTime + kMinimumDeltaTime * m_TimeScale);
			return;
		}

		// Handle time scale
			if (!Mathf.Approximately (m_TimeScale, 1.0F))
		{
			double deltaTime = time - m_DynamicTime.m_CurFrameTime;
			SetTime (m_DynamicTime.m_CurFrameTime + deltaTime * m_TimeScale);
			return;
		}

		m_DynamicTime.m_LastFrameTime = m_DynamicTime.m_CurFrameTime;
		m_DynamicTime.m_CurFrameTime = time;
		m_DynamicTime.m_DeltaTime = (float)(m_DynamicTime.m_CurFrameTime - m_DynamicTime.m_LastFrameTime);
		m_DynamicTime.m_InvDeltaTime = CalcInvDeltaTime(m_DynamicTime.m_DeltaTime);
		CalcSmoothDeltaTime (m_DynamicTime);

		m_ActiveTime = m_DynamicTime;
	}

	void ResetTime (bool isPlayMode)
	{
		m_DynamicTime.m_CurFrameTime = 0.0F;
		m_DynamicTime.m_LastFrameTime = 0.0F;
		if (isPlayMode)
		{
			m_DynamicTime.m_DeltaTime = 0.02F;
			m_DynamicTime.m_InvDeltaTime = 1.0F / m_DynamicTime.m_DeltaTime;
		}
		else
		{
			m_DynamicTime.m_DeltaTime = 0.0F;
			m_DynamicTime.m_InvDeltaTime = 0.0F;
		}
		m_DynamicTime.m_SmoothDeltaTime = 0.0F;
		m_DynamicTime.m_SmoothingWeight = 0.0F;

		m_FixedTime.m_CurFrameTime = 0.0F;
		m_FixedTime.m_LastFrameTime = 0.0F;
		// Dont erase the fixed delta time
		m_FixedTime.m_InvDeltaTime = 1.0F / m_FixedTime.m_DeltaTime;

		m_ActiveTime = m_DynamicTime;

		m_FirstFrameAfterReset = true;
		m_FirstFrameAfterPause = true;
		m_FirstFixedFrameAfterReset = true;

		m_FrameCount = 0;
		m_RenderFrameCount = 0;
		m_ZeroTime = GetTimeSinceStartup ();
		m_RealZeroTime = m_ZeroTime;
		m_LevelLoadOffset = 0.0F;
		m_CaptureFramerate = 0;
		m_RealtimeStartOfFrame = 0.0;
		m_RealtimeStartOfFrameDelta = 0.0;
	}

	//
	public void SetPause(bool pause)
	{
		m_FirstFrameAfterPause = true;
	}
	
	public double	GetCurTime ()		{ return m_ActiveTime.m_CurFrameTime; }
	public double	GetTimeSinceLevelLoad () 		{ return m_ActiveTime.m_CurFrameTime + m_LevelLoadOffset; }
	public float	GetDeltaTime () 		{ return m_ActiveTime.m_DeltaTime; }
	public float	GetSmoothDeltaTime () 	{ return m_ActiveTime.m_SmoothDeltaTime; }

	public float	GetInvDeltaTime ()	{ return m_ActiveTime.m_InvDeltaTime; }
	public int		GetFrameCount (){ return m_FrameCount; }

	public int		GetRenderFrameCount () { return m_RenderFrameCount; }

	public float	GetFixedDeltaTime () {return m_FixedTime.m_DeltaTime; }
	public void	    SetFixedDeltaTime (float fixedStep)
	{
		fixedStep = Mathf.Clamp(fixedStep, 0.0001F, 10.0F);
		m_FixedTime.m_DeltaTime = fixedStep;
		m_FixedTime.m_InvDeltaTime = 1.0F / m_FixedTime.m_DeltaTime;
		m_FixedTime.m_SmoothDeltaTime = m_FixedTime.m_DeltaTime;

		SetMaximumDeltaTime(m_MaximumTimestep);
	}
	public double	GetFixedTime () {return m_FixedTime.m_CurFrameTime; }

	public double	GetDynamicTime () {return m_DynamicTime.m_CurFrameTime; }

	public float	GetMaximumDeltaTime () {return m_MaximumTimestep; }
	public void			SetMaximumDeltaTime (float maxStep) 
	{
		m_MaximumTimestep = Mathf.Max(maxStep, m_FixedTime.m_DeltaTime);
	}

	public double	GetRealtimeStartOfFrame()  { return m_RealtimeStartOfFrame; }
	public double	GetRealtimeDelta() { return m_RealtimeStartOfFrameDelta; }
	
	public void SetTimeScale (float scale)
	{
		if (scale <= 100.0f && scale >= 0.0f)
		{
			m_TimeScale = scale;
			//SetDirty ();
		}
		else
		{
		}
	}
	public float GetTimeScale () { return m_TimeScale; }

	public double GetRealtime()
	{
		double realtime = GetTimeSinceStartup () - 0;//m_RealZeroTime;
		return realtime;
	}
	
	TimeHolder  m_FixedTime = new TimeHolder();
	TimeHolder  m_DynamicTime = new TimeHolder();
	TimeHolder  m_ActiveTime = new TimeHolder();

	bool        m_FirstFrameAfterReset;
	bool        m_FirstFrameAfterPause;
	bool        m_FirstFixedFrameAfterReset;

	int		    m_FrameCount;
	int		    m_RenderFrameCount;
	int			m_CullFrameCount;
	int			m_CaptureFramerate;
	double      m_ZeroTime;
	double      m_RealZeroTime;
	double      m_LevelLoadOffset;
	double      m_RealtimeStartOfFrame;
	double		m_RealtimeStartOfFrameDelta;
	
	bool        m_SetTimeManually;
	bool        m_UseFixedTimeStep;
	float       m_TimeScale;///< How fast compared to the real time does the game time progress (1.0 is realtime, .5 slow motion) range { -infinity, 100 }
	float		m_MaximumTimestep;

	double      m_LastSyncEnd;

	const float kMinimumDeltaTime = 0.00001F;
	const float kMaximumDeltaTime = 1.0F / 20.0F;
	const float kStartupDeltaTime = 0.02F;
	const float kNewDeltaTimeWeight = 0.2F; // for smoothing

	float CalcInvDeltaTime (float dt)
	{
		if (dt > kMinimumDeltaTime)
			return 1.0F / dt;
		else
			return 1.0F;
	}

	void CalcSmoothDeltaTime (TimeHolder time)
	{
		// If existing weight is zero, don't take existing value into account
		time.m_SmoothingWeight *= (1.0F - kNewDeltaTimeWeight);
		time.m_SmoothingWeight += kNewDeltaTimeWeight;
		// As confidence in smoothed value increases the divisor goes towards 1
		float normalized = kNewDeltaTimeWeight / time.m_SmoothingWeight;
		time.m_SmoothDeltaTime = Mathf.Lerp (time.m_SmoothDeltaTime, time.m_DeltaTime, normalized);
	}
	void SetTime (double time)
	{

		m_DynamicTime.m_LastFrameTime = m_DynamicTime.m_CurFrameTime;
		m_DynamicTime.m_CurFrameTime = time;
			m_DynamicTime.m_DeltaTime = (float)(m_DynamicTime.m_CurFrameTime - m_DynamicTime.m_LastFrameTime);

		m_DynamicTime.m_InvDeltaTime = CalcInvDeltaTime(m_DynamicTime.m_DeltaTime);
		CalcSmoothDeltaTime (m_DynamicTime);

		m_ActiveTime = m_DynamicTime;

		// Sync m_ZeroTime with timemanager time
		m_ZeroTime = GetTimeSinceStartup () - m_DynamicTime.m_CurFrameTime;
		#if DEBUG_TIME_MANAGER
		printf_console( "time: set to %f, sync zero to %f\n", time, m_ZeroTime );
		#endif
	}

		double GetTimeSinceStartup()
		{
			return Application.GetTimeSinceStartup ();
		}
}

}