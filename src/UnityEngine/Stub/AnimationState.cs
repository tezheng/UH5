
namespace UnityEngine
{
// The AnimationState gives full control over animation blending.
public class AnimationState
{
	public float length;
	public string name = "";
	public float time;
	public float speed;
	public bool enabled;
	public WrapMode wrapMode;
	public float normalizedTime;

	// Enables / disables the animation.
	/*
	AUTO_PROP bool enabled GetEnabled SetEnabled

	// The weight of animation
	AUTO_PROP float weight GetWeight SetWeight

	// Wrapping mode of the animation.
	AUTO_PROP WrapMode wrapMode GetWrapMode SetWrapMode

	// The current time of the animation
	AUTO_PROP float time GetTime SetTime

	// The normalized time of the animation.
	AUTO_PROP float normalizedTime GetNormalizedTime SetNormalizedTime

	// The playback speed of the animation. 1 is normal playback speed.
	AUTO_PROP float speed GetSpeed SetSpeed

	// The normalized playback speed.
	AUTO_PROP float normalizedSpeed GetNormalizedSpeed SetNormalizedSpeed

	// The length of the animation clip in seconds.
	AUTO_PROP float length GetLength

	// The layer of the animation. When calculating the final blend weights, animations in higher layers will get their weights
	AUTO_PROP int layer GetLayer SetLayer

	// The clip that is being played by this animation state.
	AUTO_PTR_PROP AnimationClip clip GetClip

	CONDITIONAL UNITY_WINRT
	CSRAW public void AddMixingTransform (Transform mix, bool recursive = true)
	{
		if (mix == null)
			throw new NullReferenceException();
		AddMixingTransformIntern(mix, recursive);
	}

	CONDITIONAL UNITY_WINRT
	CUSTOM private void AddMixingTransformIntern (Transform mix, bool recursive) { self->AddMixingTransform(*mix, recursive); }

	// Adds a transform which should be animated. This allows you to reduce the number of animations you have to create.
	CONDITIONAL !UNITY_WINRT
	CUSTOM void AddMixingTransform (Transform mix, bool recursive = true) { self->AddMixingTransform(*mix, recursive); }
	
	CONDITIONAL UNITY_WINRT
	CSRAW public void RemoveMixingTransform (Transform mix)
	{
		if (mix == null)
			throw new NullReferenceException();
		RemoveMixingTransformIntern(mix);
	}

	CONDITIONAL UNITY_WINRT
	CUSTOM private void RemoveMixingTransformIntern (Transform mix) { self->RemoveMixingTransform(*mix); }

	// Removes a transform which should be animated.
	CONDITIONAL !UNITY_WINRT
	CUSTOM void RemoveMixingTransform (Transform mix) { self->RemoveMixingTransform(*mix); }

	// The name of the animation
	CUSTOM_PROP string name { return scripting_string_new(self->GetName()); } { self->SetName(value.AsUTF8()); }
	
	// Which blend mode should be used?
	AUTO_PROP AnimationBlendMode blendMode GetBlendMode SetBlendMode
	*/
}

}