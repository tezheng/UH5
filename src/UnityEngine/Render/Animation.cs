using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
	public enum WrapMode
	{
		Once = 1,
		Loop = 2,
		PingPong = 4,
		Default = 0,
		ClampForever = 8
	}
	public partial class Animation : Behaviour, IEnumerable
	{
		public Animation() {}
		public Animation(System.Object o)
		{
			InitWithRaw(o as UnityEngine.Config.Animation);
		}
		public void Play()
		{
			Play(state, WrapMode.Default);
		}

		public void Play(string a, WrapMode mode = WrapMode.Default)
		{
			Play(this[a], mode);
		}

		/*
		AnimationClip GetClipFromName(string name)
		{
			foreach (var c in clips)
			{
				if (name == c.raw.name)
				{
					return c;
				}
			}
			return null;
		}
		*/

		internal void Play(AnimationState state, WrapMode mode)
		{
			if (state == null)
			{
				return;
			}
			int wrapMode = (int)state.wrapMode;
			if (mode != (int)WrapMode.Default) wrapMode = (int)mode;
			var renderer = firstSkinnedMeshRenderer;
			if (renderer == null)
			{
				cached_state = state;
				cached_mode = wrapMode;
				return;
			}
			DoPlay(this, renderer, state, wrapMode);
			playingAnimation = state.name;
			if (wrapMode == 1)
			{
				animationStopTime = Time.time + state.length + 0.3f;	// 0.3f is a hack.
			}
			else
			{
				animationStopTime = Time.time + 100000;	// means looping.
			}
		}

		public void Stop()
		{
			DoStop(this);
		}

		// Animation playing status. to support IsPlaying.
		float animationStopTime = 0;
		string playingAnimation;

		public bool isPlaying {get {
				return animationStopTime > Time.time;
			}
		}

		// Is the animation named /name/ playing?
		public bool IsPlaying (string name) {
			return playingAnimation == name && isPlaying;
		}

		AnimationState cached_state;
		int cached_mode;

		public AnimationState this [string name]
		{
			get
			{
				foreach (var astate in states)
				{
					if (astate.name == name)
					{
						return astate;
					}
				}
				return null;
			}
		}

		public void CrossFade (string animation, float fadeLength = 0.3F)
		{
			Play(animation);
		}

		internal int GetStateCount () { return states.Count; }
		internal AnimationState GetStateAtIndex (int index)
		{
			return new AnimationState();
		}
		//*undocumented* Documented separately
		public IEnumerator GetEnumerator ()
		{
			return new Animation.Enumerator (this);
		}

		public void Sample()
		{

		}

		//*undocumented*
		class Enumerator : IEnumerator
		{
			private Animation m_Outer;
			private int       m_CurrentIndex = -1;

			internal Enumerator (Animation outer) { m_Outer = outer; }
			//*undocumented*
			public object Current
			{
				get { return m_Outer.GetStateAtIndex (m_CurrentIndex); }
			}

			//*undocumented*
			public bool MoveNext ()
			{
				int childCount = m_Outer.GetStateCount();
				m_CurrentIndex++;
				return m_CurrentIndex < childCount;
			}

			//*undocumented*
			public void Reset () { m_CurrentIndex = -1; }
		}
	}

	partial class Animation
	{
		bool m_PlayAutomatically;
		public AnimationClip clip;
		AnimationState state;
		List<AnimationClip> clips = new List<AnimationClip>();
		List<AnimationState> states = new List<AnimationState>();

		SkinnedMeshRenderer __firstSkinnedMeshRenderer;
		SkinnedMeshRenderer firstSkinnedMeshRenderer {
			get {
				if (__firstSkinnedMeshRenderer == null)
				{
					var l = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
					if (l.Length > 0)
					{
						__firstSkinnedMeshRenderer = l[0];
					}
				}
				return __firstSkinnedMeshRenderer;
			}
		}

		internal void Update()
		{
			if (cached_state != null)
			{
				var renderer = firstSkinnedMeshRenderer;
				if (renderer != null)
				{
					var state = cached_state;
					var wrapMode = cached_mode;
					DoPlay(this, renderer, state, wrapMode);
					cached_state = null;
				}
				return;
			}
			var delta = Time.deltaTime;
			DoUpdate(this, delta);
		}

		internal void InitWithRaw(UnityEngine.Config.Animation raw)
		{
			SetEnabled (raw.m_Enabled == 1);
			m_PlayAutomatically = raw.m_PlayAutomatically == 1;
			clip = Resources.GetAnimationClip(raw.m_Animation);
			foreach (var a in raw.m_Animations)
			{
				var tclip = Resources.GetAnimationClip(a);
				clips.Add(tclip);
				var astate = new AnimationState();
				astate.name = tclip.name;
				astate.time = 0;
				astate.speed = 1;
				astate.enabled = false;
				astate.wrapMode = tclip.wrapMode;
				astate.normalizedTime = 0;
				astate.length = clip.length;
				states.Add(astate);
			}
			state = this[clip.name];
		}

		internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
		{
			base.AwakeFromLoad (awakeMode);
			if (m_PlayAutomatically && IsActive () && GetEnabled())
				Play ();
		}

		internal override void AddToManager()
		{
			Application.GetAnimationManager().Add(this);
		}

		internal override void RemoveFromManager()
		{
			Application.GetAnimationManager().Remove(this);
		}
	}

	partial class Animation
	{
		static extern void DoPlay(Animation self, SkinnedMeshRenderer renderer, AnimationState state, int wrapMode);

		static extern void DoStop(Animation self);

		static extern void DoUpdate(Animation self, float delta);
	}
}

