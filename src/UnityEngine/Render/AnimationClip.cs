using System;

namespace UnityEngine
{
	public class AnimationClip : Object
	{
		public UnityEngine.Config.ModelImporterAnimationClip raw;
		public void SetFromRaw(UnityEngine.Config.ModelImporterAnimationClip raw)
		{
			this.raw = raw;
			this.name = raw.name;
		}
		public int frameRate {get {return 30;}}
		public float length {get {return Mathf.Abs(raw.firstFrame - raw.lastFrame) / frameRate;}}
		public WrapMode wrapMode {get {return (WrapMode)(raw.wrapMode);}}

		/*
			// Creates a new animation clip
	CSRAW public AnimationClip()
	{
		Internal_CreateAnimationClip(this);
	}

	CUSTOM void SampleAnimation(GameObject go, float time)
	{
		// This method was moved here to prevent GameObject or the Core from depending on Animation.
		// Helps in modularizing managed code.
		SampleAnimation (*go, *self, time, self->GetWrapMode());
	}
	
	CUSTOM private static void Internal_CreateAnimationClip ([Writable]AnimationClip self)
	{
		Object* animClip = NEW_OBJECT(AnimationClip);
		animClip->Reset();
		Scripting::ConnectScriptingWrapperToObject (self.GetScriptingObject(), animClip);
		animClip->AwakeFromLoad(kInstantiateOrCreateFromCodeAwakeFromLoad);
	}
	
	// Animation length in seconds (RO)
	CUSTOM_PROP float length { return self->GetLength(); }

	
	CUSTOM_PROP internal float startTime { return self->GetRange ().first; }
	CUSTOM_PROP internal float stopTime { return self->GetRange ().second; }
	

	// Frame rate at which keyframes are sampled (RO)
	AUTO_PROP float frameRate GetSampleRate SetSampleRate
	
	
	// Assigns the curve to animate a specific property.
	CUSTOM void SetCurve (string relativePath, Type type, string propertyName, AnimationCurve curve)
	{
#if ENABLE_MONO || UNITY_WINRT || ENABLE_IL2CPP
		Scripting::RaiseIfNull(type);
		ScriptingClassPtr klass = scripting_class_from_systemtypeinstance(type);
		MonoScript* script = NULL;
		int classID = Scripting::GetClassIDFromScriptingClass(klass);
		if (classID == ClassID(MonoBehaviour))
		{
			script = GetMonoScriptManager().FindRuntimeScript(klass);
			if (script == NULL)
			{
				ErrorString("The script class couldn't be found");
				return;
			}
		}

		self->SetCurve(relativePath, classID, script, propertyName, curve.GetPtr(), true);
#endif
	}

	//*undocumented*
	AUTO void EnsureQuaternionContinuity();
	
	// Clears all curves from the clip.
	AUTO void ClearCurves();

	// Sets the default wrap mode used in the animation state.
	AUTO_PROP WrapMode wrapMode GetWrapMode SetWrapMode

	// AABB of this Animation Clip in local space of Animation component that it is attached too.
	AUTO_PROP Bounds localBounds GetBounds SetBounds

	CUSTOM_PROP new bool legacy
	{
		return self->IsLegacy();
	}
	{
		self->SetLegacy(value);
	}

	CUSTOM_PROP bool humanMotion
	{
		return self->IsHumanMotion();
	}

	// Adds an animation event to the clip.
	CSRAW public void AddEvent (AnimationEvent evt)
	{
		AddEventInternal(evt);
	}

	CUSTOM internal void AddEventInternal (object evt)
	{
		MonoAnimationEvent monoEvent;
		MarshallManagedStructIntoNative(evt, &monoEvent);

		AnimationEvent cEvent;
		AnimationEventFromMono(monoEvent, cEvent);
		self->AddRuntimeEvent(cEvent);
	}

	// Retrieves all animation events associated with the animation clip
	CSRAW public AnimationEvent[] events
	{
		get
		{
			return (AnimationEvent[])GetEventsInternal();
		}
		set
		{
			SetEventsInternal(value);
		}
	}

	CUSTOM internal void SetEventsInternal(System.Array value)
	{
		AnimationEvents events;
			ScriptingClassArrayToVector<AnimationEvent, MonoAnimationEvent>(value, events, AnimationEventFromMono);
		self->SetRuntimeEvents(events);
	}

	CUSTOM internal System.Array GetEventsInternal()
	{
		return VectorToScriptingStructArray(self->GetEvents(), GetScriptingManager().GetCommonClasses().animationEvent, AnimationEventToMono);
	}
	*/
	}
}
