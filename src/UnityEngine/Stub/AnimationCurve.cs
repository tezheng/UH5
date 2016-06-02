using System.Collections.Generic;

namespace UnityEngine
{
// A single keyframe that can be injected into an animation curve.
public struct Keyframe
{
	internal float inSlope;
	internal float outSlope;

	// Create a keyframe.
	public Keyframe (float time, float value)
	{
		this.time = time;
		this.value = value;
		inSlope = 0;
		outSlope = 0;
	}
	
	// Create a keyframe.
	public Keyframe (float time, float value, float inTangent, float outTangent)
	{
		this.time = time;
		this.value = value;
		inSlope = inTangent;
		outSlope = outTangent;
	}
	
	// The time of the keyframe.
	public float time;

	// The value of the curve at keyframe.
	public float value;

	// Describes the tangent when approaching this point from the previous point in the curve.
	public float inTangent { get { return inSlope; } set { inSlope = value; }  }

	// Describes the tangent when leaving this point towards the next point in the curve.
	public float outTangent { get { return outSlope; } set { outSlope = value; }  }
}


public class AnimationCurve
{
	public List<Keyframe> m_Curve;
	public int m_PreInfinity;
	public int m_PostInfinity;
	public AnimationCurve (params Keyframe[] keys)
	{
		m_Curve = new List<Keyframe>();
		foreach (var key in keys)
		{
			m_Curve.Add(key);
		}
	}
	public AnimationCurve ()
	{
		m_Curve = new List<Keyframe>();
	}

	public float Evaluate (float curveT)
	{
		if (m_Curve.Count == 0) return 0;
		if(m_Curve.Count == 1)
		{
			return m_Curve[0].value;
		}

		curveT = WrapTime (curveT);

		int lhsIndex, rhsIndex;
		FindIndexForSampling (m_Cache, curveT, out lhsIndex, out rhsIndex);
		Keyframe lhs = m_Curve[lhsIndex];
		Keyframe rhs = m_Curve[rhsIndex];

		float dx = rhs.time - lhs.time;
		float m1;
		float m2;
		float t;
		if (dx != 0.0F)
		{
			t = (curveT - lhs.time) / dx;
			m1 = lhs.outSlope * dx;
			m2 = rhs.inSlope * dx;
		}
		else
		{
			t = 0.0F;
			m1 = 0;
			m2 = 0;
		}

		float output = HermiteInterpolate (t, lhs.value, m1, m2, rhs.value);
		HandleSteppedCurve(lhs, rhs, ref output);
		return output;
		// DebugAssert (IsFinite (output));
	}

	class Cache
	{
		public int index;
		public float time;
		public float timeEnd;
		public float[] coeff = new float[4];

		public Cache () {
			time = System.Single.PositiveInfinity;
			index=0;
			timeEnd = 0.0f;
		}
		void Invalidate () { time = System.Single.PositiveInfinity; index=0; }
	};
	private Cache m_Cache = new Cache();
	private Cache m_ClampCache = new Cache();

	const int SEARCH_AHEAD = 3;
	const int kInternalPingPong = 0, kInternalRepeat = 1, kInternalClamp = 2;
	const float kMaxTan = 5729577.9485111479F;

	private static float PingPong (float t, float begin, float end)
	{
		return PingPong (t - begin, end - begin) + begin;
	}

	private static float PingPong (float t, float length)
	{
		t = Mathf.Repeat (t, length * 2.0F);
		t = length - Mathf.Abs (t - length);
		return t;
	}

	private static float Repeat (float t, float begin, float end)
	{
		return Mathf.Repeat (t - begin, end - begin) + begin;
	}

	private float WrapTime (float curveT)
	{
		float begTime = m_Curve[0].time;
		float endTime = m_Curve[m_Curve.Count - 1].time;

		if (curveT < begTime)
		{
			if (m_PreInfinity == kInternalClamp)
				curveT = begTime;
			else if (m_PreInfinity == kInternalPingPong)
				curveT = PingPong (curveT, begTime, endTime);
			else
				curveT = Repeat (curveT, begTime, endTime);
		}
		else if (curveT > endTime)
		{
			if (m_PostInfinity == kInternalClamp)
				curveT = endTime;
			else if (m_PostInfinity == kInternalPingPong)
				curveT = PingPong (curveT, begTime, endTime);
			else
				curveT = Repeat (curveT, begTime, endTime);
		}
		return curveT;
	}

	void FindIndexForSampling (Cache cache, float curveT, out int lhs, out int rhs)
	{
		// Assert (curveT >= GetRange ().first && curveT <= GetRange ().second);
		int actualSize = m_Curve.Count;
		List<Keyframe> frames = m_Curve;

		if (SEARCH_AHEAD > 0) {
			int cacheIndex = cache.index;
			if (cacheIndex != -1) {
				// We can not use the cache time or time end since that is in unwrapped time space!
				float time = m_Curve [cacheIndex].time;

				if (curveT > time) {
					for (int i = 0; i < SEARCH_AHEAD; i++) {
						int index = cacheIndex + i;
						if (index + 1 < actualSize && frames [index + 1].time > curveT) {
							lhs = index;

							rhs = System.Math.Min (lhs + 1, actualSize - 1);
//			Assert (curveT >= frames[lhs].time && curveT <= frames[rhs].time);
//			Assert (!(frames[rhs].time == curveT && frames[lhs].time != curveT));
							return;
						}
					}
				} else {
					for (int i = 0; i < SEARCH_AHEAD; i++) {
						int index = cacheIndex - i;
						if (index >= 0 && curveT >= frames [index].time) {
							lhs = index;
							rhs = System.Math.Min (lhs + 1, actualSize - 1);
//			Assert (curveT >= frames[lhs].time && curveT <= m_Curve[rhs].time);
//			Assert (!(frames[rhs].time == curveT && frames[lhs].time != curveT));
							return;
						}
					}
				}
			}
		}

		// Fall back to using binary search
		// upper bound (first value larger than curveT)
		int __len = actualSize;
		int __half;
		int __middle;
		int __first = 0;
		while (__len > 0)
		{
			__half = __len >> 1;
			__middle = __first + __half;

			if (curveT < frames[__middle].time)
				__len = __half;
			else
			{
				__first = __middle;
				++__first;
				__len = __len - __half - 1;
			}
		}

		// If not within range, we pick the last element twice
		lhs = __first - 1;
		rhs = System.Math.Min(actualSize - 1, __first);

//			Assert (lhs >= 0 && lhs < actualSize);
//			Assert (rhs >= 0 && rhs < actualSize);
//
//			Assert (curveT >= m_Curve[lhs].time && curveT <= m_Curve[rhs].time);
//			Assert (!(frames[rhs].time == curveT && frames[lhs].time != curveT));
	}

	void EvaluateCache (Cache cache, float curveT, ref float output)
	{
		//	DebugAssert (curveT >= cache.time - kCurveTimeEpsilon && curveT <= cache.timeEnd + kCurveTimeEpsilon);
		float t = curveT - cache.time;
		output = (t * (t * (t * cache.coeff[0] + cache.coeff[1]) + cache.coeff[2])) + cache.coeff[3];
		// DebugAssert (IsFinite (output));
	}

	void SetupStepped (float[] coeff, Keyframe lhs, Keyframe rhs)
	{
		// If either of the tangents in the segment are set to stepped, make the constant value equal the value of the left key
		if (lhs.outSlope == System.Single.PositiveInfinity || rhs.inSlope == System.Single.PositiveInfinity)
		{
			coeff[0] = 0.0F;
			coeff[1] = 0.0F;
			coeff[2] = 0.0F;
			coeff[3] = lhs.value;
		}
	}

	void HandleSteppedCurve (Keyframe lhs, Keyframe rhs, ref float value)
	{
		if (lhs.outSlope == System.Single.PositiveInfinity || rhs.inSlope == System.Single.PositiveInfinity)
			value = lhs.value;
	}

	void HandleSteppedTangent (Keyframe lhs, Keyframe rhs, ref float tangent)
	{
		if (lhs.outSlope == System.Single.PositiveInfinity || rhs.inSlope == System.Single.PositiveInfinity)
			tangent = System.Single.PositiveInfinity;
	}

	float HermiteInterpolate (float t, float p0, float m0, float m1, float p1)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float a = 2.0F * t3 - 3.0F * t2 + 1.0F;
		float b = t3 - 2.0F * t2 + t;
		float c = t3 - t2;
		float d = -2.0F * t3 +  3.0F * t2;

		return a * p0 + b * m0 + c * m1 + d * p1;
	}


	/*
	CSRAW
	internal IntPtr m_Ptr;
	
	THREAD_SAFE
	CUSTOM private void Cleanup () { CleanupAnimationCurve(self.GetPtr()); }

	CSRAW
	~AnimationCurve()
	{
		Cleanup ();
	}
	
	// Evaluate the curve at /time/.
	CUSTOM float Evaluate (float time)
	{
		return self->Evaluate(time);
	}

	//	All keys defined in the animation curve.
	CSRAW public Keyframe[]  keys { get { return GetKeys(); } set { SetKeys(value); } }

	// Add a new key to the curve.
	CUSTOM int AddKey (float time, float value) { return AddKeySmoothTangents(*self, time, value); }

	// Add a new key to the curve.
	CSRAW public int AddKey (Keyframe key) { return AddKey_Internal(key); }

	CUSTOM private int AddKey_Internal (Keyframe key) { return self->AddKey (key); }

	// Removes the keyframe at /index/ and inserts key.
	CUSTOM int MoveKey (int index, Keyframe key)
	{
		if (index >= 0 && index < self->GetKeyCount())
			return MoveCurveKey(*self, index, key);
		else {
			Scripting::RaiseOutOfRangeException("MoveKey");
			return 0;
		}
	}
	
	// Removes a key
	CUSTOM void RemoveKey (int index)
	{
		if (index >= 0 && index < self->GetKeyCount())
			self->RemoveKeys(self->begin() + index, self->begin() + index + 1);
		else
			Scripting::RaiseOutOfRangeException("RemoveKey");
	}

	// Retrieves the key at index (RO)
	CSRAW public Keyframe this [int index]
	{
		get { return GetKey_Internal(index); }
	}

	// The number of keys in the curve (RO)
	CUSTOM_PROP int length { return self->GetKeyCount(); }
	
	// Replace all keyframes with the /keys/ array.
	CUSTOM private void SetKeys (Keyframe[] keys)
	{
		KeyframeTpl<float>* first = Scripting::GetScriptingArrayStart<KeyframeTpl<float> > (keys);
		self->Assign(first, first + GetScriptingArraySize(keys));
		self->Sort();
	}

	CUSTOM private Keyframe GetKey_Internal (int index)
	{
		if (index >= 0 && index < self->GetKeyCount())
		{
			return self->GetKey(index);
		}
		else
		{
			Scripting::RaiseOutOfRangeException("GetKey");
			return KeyframeTpl<float>();
		}
	}
	
	CUSTOM private Keyframe[] GetKeys ()
	{
		if (self->GetKeyCount() <= 0)
			return CreateEmptyStructArray(MONO_COMMON.keyframe);
		return CreateScriptingArray(&self->GetKey(0), self->GetKeyCount(), MONO_COMMON.keyframe);
	}

	// Smooth the in and out tangents of the keyframe at /index/.
	CUSTOM void SmoothTangents (int index, float weight)
	{
		if (index >= 0 && index < self->GetKeyCount())
			RecalculateSplineSlope(*self, index, weight);
		else
			Scripting::RaiseOutOfRangeException("SmoothTangents");
	}
	
	// A straight Line starting at /timeStart/, /valueStart/ and ending at /timeEnd/, /valueEnd/
	CSRAW public static AnimationCurve Linear (float timeStart, float valueStart, float timeEnd, float valueEnd)
	{
		float tangent = (valueEnd - valueStart) / (timeEnd - timeStart);
		Keyframe[] keys = { new Keyframe(timeStart, valueStart, 0.0F, tangent), new Keyframe(timeEnd, valueEnd, tangent, 0.0F) };
		return new AnimationCurve(keys);
	}

	// An ease-in and out curve starting at /timeStart/, /valueStart/ and ending at /timeEnd/, /valueEnd/.
	CSRAW public static AnimationCurve EaseInOut (float timeStart, float valueStart, float timeEnd, float valueEnd)
	{
		Keyframe[] keys = { new Keyframe(timeStart, valueStart, 0.0F, 0.0F), new Keyframe(timeEnd, valueEnd, 0.0F, 0.0F) };
		return new AnimationCurve(keys);
	}

	// The behaviour of the animation before the first keyframe
	CUSTOM_PROP WrapMode preWrapMode { return self->GetPreInfinity(); } { self->SetPreInfinity(value); }
	// The behaviour of the animation after the last keyframe
	CUSTOM_PROP WrapMode postWrapMode { return self->GetPostInfinity();  }  { self->SetPostInfinity(value); }

	// Creates an animation curve from arbitrary number of keyframes.
	CSRAW public AnimationCurve (params Keyframe[] keys) { Init(keys); }
	
	CONDITIONAL UNITY_WINRT
	// *undocumented*
	CSRAW public AnimationCurve(IntPtr nativeptr) { m_Ptr = nativeptr; }
	
	// Creates an empty animation curve
	CSRAW public AnimationCurve ()  { Init(null); }
	
	THREAD_SAFE
	CUSTOM private void Init (Keyframe[] keys)
	{
		self.SetPtr(new AnimationCurve(), CleanupAnimationCurve);
		#if UNITY_WINRT || UNITY_WEBGL
		if (keys != SCRIPTING_NULL) AnimationCurve_CUSTOM_SetKeys(self.object, keys);
		#else
		if (keys != SCRIPTING_NULL) AnimationCurve_CUSTOM_SetKeys(self, keys);
		#endif
	}
	*/
	}

}
