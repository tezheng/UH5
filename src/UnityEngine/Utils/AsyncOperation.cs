using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;

namespace UnityEngine
{


// Asynchronous operation coroutine.
public class AsyncOperation : YieldInstruction
{
	// Has the operation finished? (RO)
	public bool isDone
	{
		get
		{
			return IsDone();
		}
	}
	
	
	// What's the operation's progress (RO)
	public float progress
	{
		get
		{
			return GetProgress();
		}
	}
	
	// Priority lets you tweak in which order async operation calls will be performed.
	public int priority
	{
		get
		{
			return GetPriority();
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			SetPriority(value);
		}
	}

	// Allow scenes to be activated as soon as it is ready.
	public bool allowSceneActivation
	{
		get
		{
			return GetAllowSceneActivation();
		}
		set
		{
			SetAllowSceneActivation(value);
		}
	}

	bool IsDone()
	{
		return false;
	}

	float GetProgress()
	{
		return 0;
	}

	int GetPriority()
	{
		return 0;
	}

	int SetPriority(int p)
	{
		return 0;
	}

	bool GetAllowSceneActivation()
	{
		return true;
	}

	bool SetAllowSceneActivation(bool value)
	{
		return true;
	}
}

}
