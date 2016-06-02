using System;
using System.Collections.Generic;

namespace UnityEngine
{

// The rendering mode for particle systems (Shuriken).
enum ParticleSystemRenderMode
{
	// Render particles as billboards facing the player. (Default)
	Billboard = 0,
	// Stretch particles in the direction of motion.
	Stretch = 1,
	// Render particles as billboards always facing up along the y-Axis.
	HorizontalBillboard = 2,
	// Render particles as billboards always facing the player, but not pitching along the x-Axis.
	VerticalBillboard = 3,
	
	// Render particles as meshes.
	Mesh = 4
}

// The simulation space for particle systems (Shuriken).
enum ParticleSystemSimulationSpace
{
	// Use local simulation space. (Default)
	Local = 0,
	// Use world simulation space.
	World = 1
}

// Script interface for particle systems (Shuriken).

public class ParticleSystem : Component
{
// Script interface for a Particle
	public struct Particle
	{
		private Vector3 m_Position;
		private Vector3 m_Velocity;
		private Vector3 m_AnimatedVelocity;
		private Vector3 m_AxisOfRotation;
		private float m_Rotation;
		private float m_AngularVelocity;
		private float m_Size;
		private Color32 m_Color;
		private UInt32 m_RandomSeed;
		private float m_Lifetime;
		private float m_StartLifetime;
		private float m_EmitAccumulator0;
		private float m_EmitAccumulator1;
		
		// The position of the particle.
		public Vector3 position { get { return m_Position; } set { m_Position = value; } }
		
		// The velocity of the particle.
		public Vector3 velocity { get { return m_Velocity; } set { m_Velocity = value; } }

		// The lifetime of the particle.
		public float lifetime { get { return m_Lifetime; } set { m_Lifetime = value; } }

		// The starting lifetime of the particle.
		public float startLifetime { get { return m_StartLifetime; } set { m_StartLifetime = value; } }
		
		// The size of the particle.
		public float size { get { return m_Size; } set { m_Size = value; } }
		
		// The rotation axis of the particle.
		public Vector3 axisOfRotation { get { return m_AxisOfRotation; } set { m_AxisOfRotation = value; } }

		// The rotation of the particle.
		public float rotation { get { return m_Rotation * Mathf.Rad2Deg; } set { m_Rotation = value * Mathf.Deg2Rad; } }

		// The angular velocity of the particle.
		public float angularVelocity { get { return m_AngularVelocity * Mathf.Rad2Deg; } set { m_AngularVelocity = value * Mathf.Deg2Rad; } }

		// The color of the particle.
		public Color32 color { get { return m_Color; } set { m_Color = value; } }

		// The random value of the particle.
		//public float randomValue { get { return BitConverter.ToSingle(BitConverter.GetBytes(m_RandomSeed), 0); } set { m_RandomSeed = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0); } }
		
		// The random seed of the particle.
		public UInt32 randomSeed { get { return m_RandomSeed; } set { m_RandomSeed = value; } }
	}

	public float startSize;
	public float startSpeed;
	public void Stop()
	{
		
	}
/*
	// Start delay in seconds.
	SYNC_JOBS AUTO_PROP float startDelay GetStartDelay SetStartDelay

	// Is the particle system playing right now ?
	SYNC_JOBS AUTO_PROP bool isPlaying IsPlaying

	// Is the particle system stopped right now ?
	SYNC_JOBS AUTO_PROP bool isStopped IsStopped

	// Is the particle system paused right now ?
	SYNC_JOBS AUTO_PROP bool isPaused IsPaused

	// Is the particle system looping?
	SYNC_JOBS AUTO_PROP bool loop GetLoop SetLoop

	// If set to true, the particle system will automatically start playing on startup.
	SYNC_JOBS AUTO_PROP bool playOnAwake GetPlayOnAwake SetPlayOnAwake

	// Playback position in seconds.
	SYNC_JOBS AUTO_PROP float time GetSecPosition SetSecPosition

	// The duration of the particle system in seconds (Read Only)
	SYNC_JOBS AUTO_PROP float duration GetLengthInSec

	// The playback speed of the particle system. 1 is normal playback speed.
	SYNC_JOBS AUTO_PROP float playbackSpeed GetPlaybackSpeed SetPlaybackSpeed

	// The current number of particles (Read Only).
	SYNC_JOBS AUTO_PROP int particleCount GetParticleCount

	// When set to false, the particle system will not emit particles
	SYNC_JOBS AUTO_PROP bool enableEmission GetEnableEmission SetEnableEmission

	// The rate of emission
	SYNC_JOBS AUTO_PROP float emissionRate GetEmissionRate SetEmissionRate

	// The initial speed of particles when emitted. When using curves, this values acts as a scale on the curve.
	SYNC_JOBS AUTO_PROP float startSpeed GetStartSpeed SetStartSpeed

	// The initial size of particles when emitted. When using curves, this values acts as a scale on the curve.
	SYNC_JOBS AUTO_PROP float startSize GetStartSize SetStartSize

	// The initial color of particles when emitted.
	SYNC_JOBS AUTO_PROP Color startColor GetStartColor SetStartColor

	// The initial rotation of particles when emitted. When using curves, this values acts as a scale on the curve.
	SYNC_JOBS AUTO_PROP float startRotation GetStartRotation SetStartRotation

	// The total lifetime in seconds that particles will have when emitted. When using curves, this values acts as a scale on the curve. This value is set in the particle when it is create by the particle system.
	SYNC_JOBS AUTO_PROP float startLifetime GetStartLifeTime SetStartLifeTime

	// Scale being applied to the gravity defined by [[Physics.gravity]].
	SYNC_JOBS AUTO_PROP float gravityModifier GetGravityModifier SetGravityModifier

	// Maximum number of particles.
	SYNC_JOBS AUTO_PROP int maxParticles GetMaxNumParticles SetMaxNumParticles

	// Selects the space in which to simulate particles; can be local (default) or world.
	SYNC_JOBS AUTO_PROP ParticleSystemSimulationSpace simulationSpace GetSimulationSpace SetSimulationSpace

	// Random seed used for the particle system emission. If set to 0, it will be assigned a random value on awake.
	SYNC_JOBS AUTO_PROP UInt32 randomSeed GetRandomSeed SetRandomSeed

	// Set the particles of this particle system. /size/ is the number of particles that is set.
	SYNC_JOBS CUSTOM void SetParticles (ParticleSystem.Particle[] particles, int size)
	{
		unsigned int actualSize = GetScriptingArraySize(particles);
		if (size < 0 || actualSize < (unsigned int)size)
			size = actualSize;
	
		self->SetParticlesExternal (Scripting::GetScriptingArrayStart<ParticleSystemParticle>(particles), size);
	}
	
	// Get the particles of this particle system. Returns the number of particles written to the input particle array.
	SYNC_JOBS CUSTOM int GetParticles (ParticleSystem.Particle[] particles)
	{
		int size = std::min<unsigned int>(self->GetParticleCount(), GetScriptingArraySize(particles));
		self->GetParticlesExternal (Scripting::GetScriptingArrayStart<ParticleSystemParticle>(particles), size);
		return size;
	}

	SYNC_JOBS CUSTOM private void Internal_Simulate (float t, bool restart) { self->Simulate (t, restart); }
	SYNC_JOBS CUSTOM private void Internal_Play () { self->Play (); }
	SYNC_JOBS CUSTOM private void Internal_Stop () { self->Stop (); }
	SYNC_JOBS CUSTOM private void Internal_Pause () { self->Pause (); }
	SYNC_JOBS CUSTOM private void Internal_Clear () { self->Clear (); }
	SYNC_JOBS CUSTOM private bool Internal_IsAlive () { return self->IsAlive(); }

	// Fastforwards the particle system by simulating particles over given period of time, then pauses it.
	CSRAW public void Simulate (float t, bool withChildren = true, bool restart = true)
	{
		if (withChildren)
		{
			ParticleSystem[] emitters = GetParticleSystems (this);
			foreach (var emitter in emitters)
				emitter.Internal_Simulate (t, restart);
		}
		else
		{
			Internal_Simulate (t, restart);
		}
	}
	
	// Plays the particle system.
	CSRAW public void Play (bool withChildren = true)
	{
		if (withChildren)
		{
			ParticleSystem[] emitters = GetParticleSystems (this);
			foreach (var emitter in emitters)
				emitter.Internal_Play ();
		}
		else
		{
			Internal_Play ();
		}
	}
	
	// Stops playing the particle system.
	CSRAW public void Stop (bool withChildren = true)
	{
		if (withChildren)
		{
			ParticleSystem[] emitters = GetParticleSystems (this);
			foreach (var emitter in emitters)
				emitter.Internal_Stop ();
		}
		else
		{
			Internal_Stop ();
		}
	}
	
	// Pauses playing the particle system.
	CSRAW public void Pause (bool withChildren = true)
	{
		if (withChildren)
		{
			ParticleSystem[] emitters = GetParticleSystems (this);
			foreach (var emitter in emitters)
				emitter.Internal_Pause ();
		}
		else
		{
			Internal_Pause ();
		}
		
	}

	// Remove all particles in the particle system
	CSRAW public void Clear (bool withChildren = true)
	{
		if (withChildren)
		{
			ParticleSystem[] emitters = GetParticleSystems (this);
			foreach (var emitter in emitters)
				emitter.Internal_Clear ();
		}
		else
		{
			Internal_Clear ();
		}
	}

	// Is the particle system done emitting particles and are all particles dead?
	CSRAW public bool IsAlive (bool withChildren = true)
	{
		if (withChildren)
		{
			ParticleSystem[] emitters = GetParticleSystems (this);
			foreach (var emitter in emitters)
				if(emitter.Internal_IsAlive())
					return true;
			return false;
		}

		return this.Internal_IsAlive ();
	}
	
	// Emit /count/ particles immediately.
	SYNC_JOBS AUTO void Emit (int count);
	
	// Emit a single particle with given parameters.
	CSRAW public void Emit(Vector3 position, Vector3 velocity, float size, float lifetime, Color32 color)
	{
		ParticleSystem.Particle particle = new ParticleSystem.Particle();
		particle.position = position;
		particle.velocity = velocity;
		particle.lifetime = lifetime;
		particle.startLifetime = lifetime;
		particle.size = size;
		particle.rotation = 0.0f;
		particle.angularVelocity = 0.0f;
		particle.color = color;
		particle.randomSeed = 5;
		Internal_Emit(ref particle);
	}

	// Emit a single particle.
	CSRAW public void Emit(ParticleSystem.Particle particle)
	{
		Internal_Emit(ref particle);
	}
	
	SYNC_JOBS CUSTOM private void Internal_Emit (ref ParticleSystem.Particle particle)
	{
		self->EmitParticleExternal (&particle);
	}


	// Returns a list with 'root' and all its direct children.
	CSRAW static internal ParticleSystem[] GetParticleSystems (ParticleSystem root)
	{
		if (!root)
			return null;

		List<ParticleSystem> particleSystems = new List<ParticleSystem>();
		particleSystems.Add(root);
		GetDirectParticleSystemChildrenRecursive(root.transform, particleSystems);
		return particleSystems.ToArray();
	}

	// Adds only active Particle Systems
	CSRAW static private void GetDirectParticleSystemChildrenRecursive(Transform transform, List<ParticleSystem> particleSystems)
	{
		foreach (Transform childTransform in transform)
		{
			ParticleSystem ps = childTransform.gameObject.GetComponent<ParticleSystem>();
			if (ps != null)
			{
				// Note: we do not check for if the gameobject is active (we want inactive particle systems as well due prefabs)
				particleSystems.Add (ps);
				GetDirectParticleSystemChildrenRecursive(childTransform, particleSystems);
			}
		}
	}
	
	CONDITIONAL UNITY_EDITOR
	//*undocumented
	CUSTOM internal void SetupDefaultType (int type)
	{
		ParticleSystemEditor::SetupDefaultParticleSystemType(*self, (ParticleSystemSubType)type);
	}
	*/
}

// Renders particles on to the screen (Shuriken).
public class ParticleSystemRenderer : Renderer
{
	/*
	//  How particles are drawn.
	AUTO_PROP ParticleSystemRenderMode renderMode GetRenderMode SetRenderMode

	// How much are the particles stretched in their direction of motion.
	AUTO_PROP float lengthScale GetLengthScale SetLengthScale

	// How much are the particles strectched depending on "how fast they move"
	AUTO_PROP float velocityScale GetVelocityScale SetVelocityScale

	// How much are the particles strected depending on the [[Camera]]'s speed.
	AUTO_PROP float cameraVelocityScale GetCameraVelocityScale SetCameraVelocityScale

	// Clamp the maximum particle size.
	AUTO_PROP float maxParticleSize GetMaxParticleSize SetMaxParticleSize
	
	// Mesh used as particle instead of billboarded texture
	AUTO_PTR_PROP Mesh mesh GetMesh SetMesh

	CONDITIONAL UNITY_EDITOR
	CUSTOM_PROP internal bool editorEnabled { return self->GetEditorEnabled();} {self->SetEditorEnabled(value);}
	*/
}

// Script interface for a Particle collision event
public struct ParticleCollisionEvent
{
	/*
	private Vector3 m_Intersection;
	private Vector3 m_Normal;
	private Vector3 m_Velocity;
	private int m_ColliderInstanceID;

	CSRAW public Vector3 intersection { get { return m_Intersection; }  }
	CSRAW public Vector3 normal { get { return m_Normal; }  }
	CSRAW public Vector3 velocity { get { return m_Velocity; }  }
	
	CONDITIONAL ENABLE_PHYSICS
	CSRAW public Collider collider { get { return InstanceIDToCollider(m_ColliderInstanceID); } }

	CONDITIONAL ENABLE_PHYSICS
	CUSTOM static private Collider InstanceIDToCollider(int instanceID)
	{
		return instanceID != 0 ? Scripting::ScriptingWrapperFor (PPtr<Collider> (instanceID)) : SCRIPTING_NULL;
	}
	*/
}

internal class ParticleSystemExtensionsImpl
{
	/*
	// Safe array size for the collision event array used with GetCollisionEvents (Read Only).
	CUSTOM internal static int GetSafeCollisionEventSize(ParticleSystem ps)
	{
		return (*ps).GetSafeCollisionEventSize();
	}
	
	// Get the particle collision events recorded for this particle system. Returns the number of particles written to the input collision event array.
	CUSTOM internal static int GetCollisionEvents (ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents)
	{
		return (*ps).GetCollisionEventsExternal (go->GetInstanceID (), Scripting::GetScriptingArrayStart<MonoParticleCollisionEvent>(collisionEvents), GetScriptingArraySize(collisionEvents));
	}
	*/
}

public static class ParticlePhysicsExtensions
{
	/*
	public static int GetSafeCollisionEventSize(this ParticleSystem ps)
	{
		return ParticleSystemExtensionsImpl.GetSafeCollisionEventSize(ps);
	}

	public static int GetCollisionEvents(this ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents)
	{
		return ParticleSystemExtensionsImpl.GetCollisionEvents(ps, go, collisionEvents);
	}
	*/
}

}