// Mark here for impl later

using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
	public class YieldInstruction
	{

	}
	// To be operated by Coroutinue.
	public class WaitForSeconds : YieldInstruction
	{
		internal float m_Seconds;

		public WaitForSeconds (float seconds) { m_Seconds = seconds; }
	}

	// Waits until next fixed frame rate update function. SA: MonoBehaviour::pref::FixedUpdate.
	public class WaitForFixedUpdate : YieldInstruction
	{

	}

	public class WaitForEndOfFrame : YieldInstruction
	{
		internal int m_Frames;

		public WaitForEndOfFrame (int frames) { m_Frames = frames; }

		public WaitForEndOfFrame () { m_Frames = 1; }
	}

	public class Coroutine : YieldInstruction
	{
		public Coroutine listPrevious = null;
		public Coroutine listNext = null;
		public IEnumerator fiber;
		public bool finished = false;
		public int waitForFrame = -1;
		public float waitForTime = -1.0f;
		public Coroutine waitForCoroutine;
		// public IYieldWrapper waitForUnityObject; //lonewolfwilliams

		public Coroutine(IEnumerator _fiber)
		{
			this.fiber = _fiber;
		}
	}

/*
// Suspends the coroutine execution for the given amount of seconds.
CSRAW [StructLayout(LayoutKind.Sequential)]
CLASS WaitForSeconds : YieldInstruction
	//*undocumented*
	CSRAW internal float m_Seconds;

	// Creates a yield instruction to wait for a given number of seconds
	CSRAW public WaitForSeconds (float seconds) { m_Seconds = seconds; }
END

// Waits until next fixed frame rate update function. SA: MonoBehaviour::pref::FixedUpdate.
CLASS WaitForFixedUpdate : YieldInstruction
END

// Waits until the end of the frame after all cameras and GUI is rendered, just before displaying the frame on screen.
CLASS WaitForEndOfFrame : YieldInstruction
END


// MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this class are only used to reference these coroutines and do not hold any exposed properties or functions.
CSRAW [StructLayout (LayoutKind.Sequential)]

CLASS Coroutine : YieldInstruction
	CSRAW internal IntPtr m_Ptr;
	private Coroutine () { }

	THREAD_SAFE
	CUSTOM private void ReleaseCoroutine ()
	{
		Assert (self.GetPtr() != NULL);
		Coroutine::CleanupCoroutineGC (self);
	}

	CSRAW
	~Coroutine ()
	{
		ReleaseCoroutine ();
	}
END
*/

}
