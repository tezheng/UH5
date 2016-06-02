using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{

// The animation component is used to play back animations.
public partial class Animation
{
	/*
	// The default animation.
	AUTO_PTR_PROP AnimationClip clip GetClip SetClip
	
	// Should the default animation clip (Animation.clip) automatically start playing on startup.
	AUTO_PROP bool playAutomatically GetPlayAutomatically SetPlayAutomatically

	// How should time beyond the playback range of the clip be treated?
	AUTO_PROP WrapMode wrapMode GetWrapMode SetWrapMode
	
	// Stops all playing animations that were started with this Animation.
	AUTO void Stop ();
	
	// Stops an animation named /name/.
	CSRAW public void Stop (string name) { Internal_StopByName(name); }
	CUSTOM private void Internal_StopByName (string name) { return self->Stop (name); }

	// Rewinds the animation named /name/.
	CSRAW public void Rewind (string name) {  Internal_RewindByName(name); }
	CUSTOM private void Internal_RewindByName (string name) { self->Rewind(name); }

	// Rewinds all animations
	AUTO void Rewind ();

	// Samples animations at the current state.
	AUTO void Sample ();

	// Are we playing any animations?
	AUTO_PROP bool isPlaying IsPlaying

	// Is the animation named /name/ playing?
	CUSTOM bool IsPlaying (string name) { return self->IsPlaying (name); }
	
	
	// Returns the animation state named /name/.
	CSRAW public AnimationState this [string name]
	{
		get { return GetState(name); }
	}
	
	/// *listonly*
	CSRAW public bool Play (PlayMode mode = PlayMode.StopSameLayer) { return PlayDefaultAnimation (mode); }
	
	// Plays animation without any blending.
	CUSTOM bool Play (string animation, PlayMode mode = PlayMode.StopSameLayer) { return self->Play(animation, mode); }

	// Fades the animation with name /animation/ in over a period of /time/ seconds and fades other animations out.
	CUSTOM void CrossFade (string animation, float fadeLength = 0.3F, PlayMode mode = PlayMode.StopSameLayer) { self->CrossFade(animation, fadeLength, mode); }

	// Blends the animation named /animation/ towards /targetWeight/ over the next /time/ seconds.
	CUSTOM void Blend (string animation, float targetWeight = 1.0F, float fadeLength = 0.3F) { self->Blend(animation, targetWeight, fadeLength); }
	

	// Cross fades an animation after previous animations has finished playing.
	CUSTOM AnimationState CrossFadeQueued (string animation, float fadeLength = 0.3F, QueueMode queue = QueueMode.CompleteOthers, PlayMode mode = PlayMode.StopSameLayer)
	{
		AnimationState* as = self->QueueCrossFade(animation, fadeLength, queue, mode);
		return TrackedReferenceBaseToScriptingObject(as,animationState);
	}


	// Plays an animation after previous animations has finished playing.
	CUSTOM AnimationState PlayQueued (string animation, QueueMode queue = QueueMode.CompleteOthers, PlayMode mode = PlayMode.StopSameLayer)
	{
		AnimationState* as = self->QueuePlay(animation, queue, mode);
		return TrackedReferenceBaseToScriptingObject(as, animationState);
	}


	// Adds a /clip/ to the animation with name /newName/.
	CSRAW public void AddClip (AnimationClip clip, string newName) { AddClip (clip, newName, Int32.MinValue, Int32.MaxValue); }

	// Adds /clip/ to the only play between /firstFrame/ and /lastFrame/. The new clip will also be added to the animation with name /newName/.
	CUSTOM void AddClip (AnimationClip clip, string newName, int firstFrame, int lastFrame, bool addLoopFrame = false) { self->AddClip(*clip, newName, firstFrame, lastFrame, addLoopFrame); }

	// Remove clip from the animation list.
	CUSTOM void RemoveClip (AnimationClip clip) { self->RemoveClip (*clip); }

	// Remove clip from the animation list.
	CSRAW public void RemoveClip (string clipName) { RemoveClip2(clipName); }

	// Get the number of clips currently assigned to this animation
	CUSTOM int GetClipCount () { return self->GetClipCount(); }

	CUSTOM private void RemoveClip2 (string clipName) { self->RemoveClip (clipName); }

	CUSTOM private bool PlayDefaultAnimation (PlayMode mode) { return self->Play(mode); }
	
	//*undocumented* deprecated
	OBSOLETE warning use PlayMode instead of AnimationPlayMode.
	CSRAW public bool Play (AnimationPlayMode mode) { return PlayDefaultAnimation((PlayMode)mode); }
	//*undocumented* deprecated
	OBSOLETE warning use PlayMode instead of AnimationPlayMode.
	CSRAW public bool Play (string animation, AnimationPlayMode mode) { return Play(animation, (PlayMode)mode); }
	
	
	// Synchronizes playback speed of all animations in the /layer/.
	AUTO void SyncLayer(int layer);
	
	CUSTOM internal AnimationState GetState(string name)
	{
		AnimationState* state = self->GetState(name);
		return TrackedReferenceBaseToScriptingObject(state, animationState);
	}
	
	CUSTOM internal AnimationState GetStateAtIndex (int index)
	{

		Animation& selfRef = *self;
		if (index >= 0 || index < selfRef.GetAnimationStateCount())
		{
			return TrackedReferenceBaseToScriptingObject(&selfRef.GetAnimationStateAtIndex (index), animationState);
		}
		Scripting::RaiseMonoException("Animation State out of bounds!");
		return SCRIPTING_NULL;
	}
	
	CUSTOM internal int GetStateCount () { return self->GetAnimationStateCount(); }
	
	OBSOLETE planned Returns the animation clip named /name/.
	CSRAW public AnimationClip GetClip (string name) {
		AnimationState state = GetState(name);
		if (state)
			return state.clip;
		else
			return null;
	}


	// When turned on, animations will be executed in the physics loop. This is only useful in conjunction with kinematic rigidbodies.
	AUTO_PROP bool animatePhysics GetAnimatePhysics SetAnimatePhysics

	// When turned on, Unity might stop animating if it thinks that the results of the animation won't be visible to the user.
	OBSOLETE warning Use cullingType instead
	CUSTOM_PROP bool animateOnlyIfVisible
	{
		Animation::CullingType type = self->GetCullingType();
		AssertMsg(type == Animation::kCulling_AlwaysAnimate || type == Animation::kCulling_BasedOnRenderers,
			"Culling type %d cannot be converted to animateOnlyIfVisible. animateOnlyIfVisible is obsolete, please use cullingType instead.", type);
		return type == Animation::kCulling_BasedOnRenderers;
	}
	{
		self->SetCullingType(value ? Animation::kCulling_BasedOnRenderers : Animation::kCulling_AlwaysAnimate);
	}
	
	// Controls culling of this Animation component.
	AUTO_PROP AnimationCullingType cullingType GetCullingType SetCullingType
	
	// AABB of this Animation animation component in local space.
	AUTO_PROP Bounds localBounds GetLocalAABB SetLocalAABB

	*/
}

}
