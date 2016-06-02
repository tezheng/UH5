using System;
using System.Collections.Generic;

namespace UnityEngine
{

//(Legacy Particle system)
public struct Particle
{
	private Vector3 m_Position;
	private Vector3 m_Velocity;
	private float   m_Size;
	private float   m_Rotation;
	private float   m_AngularVelocity;
	private float   m_Energy;
	private float   m_StartEnergy;
	private Color   m_Color;

	// The position of the particle.
	public Vector3 position { get { return m_Position; } set { m_Position = value; } }

	// The velocity of the particle.
	public Vector3 velocity { get { return m_Velocity; } set { m_Velocity = value; } }

	// The energy of the particle.
	public float   energy   { get { return m_Energy; }   set { m_Energy = value; } }

	// The starting energy of the particle.
	public float   startEnergy   { get { return m_StartEnergy; }   set { m_StartEnergy = value; } }

	// The size of the particle.
	public float   size     { get { return m_Size; }     set { m_Size = value; } }

	// The rotation of the particle.
	public float   rotation     { get { return m_Rotation; }     set { m_Rotation = value; } }

	// The angular velocity of the particle.
	public float   angularVelocity     { get { return m_AngularVelocity; }     set { m_AngularVelocity = value; } }

	// The color of the particle.
	public Color   color    { get { return m_Color; }    set { m_Color = value; } }
}

// Simple struct that contains all the arguments needed by the internal DrawTexture.
struct InternalEmitParticleArguments
{
	public Vector3 pos;
	public Vector3 velocity;
	public float size;
	public float energy;
	public Color color;
	public float rotation;
	public float angularVelocity;
}

/*
struct MonoInternalEmitParticleArguments {
	Vector3 pos;
	Vector3 velocity;
	float size;
	float energy;
	ColorRGBA color;
	float rotation;
	float angularVelocity;
};
*/

// (Legacy Particles) Script interface for particle emitters.
public class ParticleEmitter : Component
{
/*
	// Should particles be automatically emitted each frame?
	AUTO_PROP bool emit IsEmitting SetEmit

	// The minimum size each particle can be at the time when it is spawned.
	AUTO_PROP float minSize GetMinSize SetMinSize

	// The maximum size each particle can be at the time when it is spawned.
	AUTO_PROP float maxSize GetMaxSize SetMaxSize

	// The minimum lifetime of each particle, measured in seconds.
	AUTO_PROP float minEnergy GetMinEnergy SetMinEnergy

	// The maximum lifetime of each particle, measured in seconds.
	AUTO_PROP float maxEnergy GetMaxEnergy SetMaxEnergy

	// The minimum number of particles that will be spawned every second.
	AUTO_PROP float minEmission GetMinEmission SetMinEmission

	// The maximum number of particles that will be spawned every second.
	AUTO_PROP float maxEmission GetMaxEmission SetMaxEmission

	// he amount of the emitter's speed that the particles inherit.
	AUTO_PROP float emitterVelocityScale GetEmitterVelocityScale SetEmitterVelocityScale

	// The starting speed of particles in world space, along X, Y, and Z.
	AUTO_PROP Vector3 worldVelocity GetWorldVelocity SetWorldVelocity

	// The starting speed of particles along X, Y, and Z, measured in the object's orientation.
	AUTO_PROP Vector3 localVelocity GetLocalVelocity SetLocalVelocity

	// A random speed along X, Y, and Z that is added to the velocity.
	AUTO_PROP Vector3 rndVelocity GetRndVelocity SetRndVelocity

	// If enabled, the particles don't move when the emitter moves. If false, when you move the emitter, the particles follow it around.
	AUTO_PROP bool useWorldSpace GetUseWorldSpace SetUseWorldSpace

	// If enabled, the particles will be spawned with random rotations.
	AUTO_PROP bool rndRotation GetRndRotation SetRndRotation

	// The angular velocity of new particles in degrees per second.
	AUTO_PROP float angularVelocity GetAngularVelocity SetAngularVelocity

	// A random angular velocity modifier for new particles.
	AUTO_PROP float rndAngularVelocity GetRndAngularVelocity SetRndAngularVelocity

	// Returns a copy of all particles and assigns an array of all particles to be the current particles.
	CUSTOM_PROP Particle[] particles
	{
		int size = self->GetParticleCount();
		ScriptingArrayPtr array = CreateScriptingArray<SimpleParticle> (GetScriptingManager().GetCommonClasses().particle, self->GetParticleCount());
		self->ReadParticles(Scripting::GetScriptingArrayStart<SimpleParticle>(array), 0, size);
		return array;
	}
	{
		self->WriteParticles(Scripting::GetScriptingArrayStart<SimpleParticle>(value), GetScriptingArraySize(value));
	}

	// The current number of particles (RO).
	AUTO_PROP int particleCount GetParticleCount

	// Removes all particles from the particle emitter.
	AUTO void ClearParticles();

	// Emit a number of particles.
	public void Emit () { Emit2 ( (int)Random.Range (minEmission, maxEmission) ); }

	// Emit /count/ particles immediately

	public void Emit (int count) { Emit2 (count);}

	// Emit a single particle with given parameters.
	public void Emit (Vector3 pos, Vector3 velocity, float size, float energy, Color color)
	{
		InternalEmitParticleArguments args = new InternalEmitParticleArguments();
		args.pos = pos;
		args.velocity = velocity;
		args.size = size;
		args.energy = energy;
		args.color = color;
		args.rotation = 0;
		args.angularVelocity = 0;
		Emit3 (ref args);
	}
	//
	public void Emit (Vector3 pos, Vector3 velocity, float size, float energy, Color color, float rotation, float angularVelocity)
	{
		InternalEmitParticleArguments args = new InternalEmitParticleArguments();
		args.pos = pos;
		args.velocity = velocity;
		args.size = size;
		args.energy = energy;
		args.color = color;
		args.rotation = rotation;
		args.angularVelocity = angularVelocity;
		Emit3 (ref args);
	}

	CUSTOM private void Emit2 (int count)
	{
		self->EmitResetEmitterPos (count, 0.0F);
	}

	CUSTOM private void Emit3 (ref InternalEmitParticleArguments args)
	{
		self->Emit (args.pos, args.velocity, args.size, args.energy, args.color, args.rotation, args.angularVelocity);
	}

	// Advance particle simulation by given time.
	CUSTOM void Simulate (float deltaTime)
	{
		self->UpdateParticleSystem(deltaTime);
	}

	// Turns the ParticleEmitter on or off.
	AUTO_PROP bool enabled GetEnabled SetEnabled

	CSRAW
	internal ParticleEmitter() {} // only types in UnityEngine are allowed to inherit from ParticleEmitter
	*/
}

// (Legacy Particles)
public class EllipsoidParticleEmitter : ParticleEmitter
{
	internal EllipsoidParticleEmitter() {}
}


// (Legacy Particles)
public class MeshParticleEmitter : ParticleEmitter
{
	internal MeshParticleEmitter() {}
}


// (Legacy Particles) Particle animators move your particles over time, you use them to apply wind, drag & color cycling to your particle emitters.
public class ParticleAnimator : Component
{
	/*
	// Do particles cycle their color over their lifetime?
	AUTO_PROP bool doesAnimateColor GetDoesAnimateColor SetDoesAnimateColor

	// World space axis the particles rotate around.
	AUTO_PROP Vector3 worldRotationAxis GetWorldRotationAxis SetWorldRotationAxis

	// Local space axis the particles rotate around.
	AUTO_PROP Vector3 localRotationAxis GetLocalRotationAxis SetLocalRotationAxis

	// How the particle sizes grow over their lifetime.
	AUTO_PROP float sizeGrow GetSizeGrow SetSizeGrow

	// A random force added to particles every frame.
	AUTO_PROP Vector3 rndForce GetRndForce SetRndForce

	// The force being applied to particles every frame.
	AUTO_PROP Vector3 force GetForce SetForce

	// How much particles are slowed down every frame.
	AUTO_PROP float damping GetDamping SetDamping

	// Does the [[GameObject]] of this particle animator auto destructs?
	AUTO_PROP bool autodestruct GetAutodestruct SetAutodestruct

	// Colors the particles will cycle through over their lifetime.

	CUSTOM_PROP Color[] colorAnimation
	{
		ColorRGBAf col[ParticleAnimator::kColorKeys];
		self->GetColorAnimation(col);
		return CreateScriptingArray(col, ParticleAnimator::kColorKeys, GetScriptingManager().GetCommonClasses().color);
	}
	{
		Scripting::RaiseIfNull(value);
		if(GetScriptingArraySize(value) != ParticleAnimator::kColorKeys)
		{
			Scripting::RaiseMonoException(" Array needs to contain exactly 5 Colors for colorAnimation.");
			return;
		}
		self->SetColorAnimation(Scripting::GetScriptingArrayStart<ColorRGBAf> (value));
	}
	*/
}


// The rendering mode for legacy particles.
enum ParticleRenderMode
{
	// Render the particles as billboards facing the player. (Default)
	Billboard = 0,
	// Stretch particles in the direction of motion.
	Stretch = 3,
	// Sort the particles back-to-front and render as billboards.
	SortedBillboard = 2,
	// Render the particles as billboards always facing up along the y-Axis.
	HorizontalBillboard = 4,
	// Render the particles as billboards always facing the player, but not pitching along the x-Axis.
	VerticalBillboard = 5
}

// (Legacy Particles) Renders particles on to the screen.
public class ParticleRenderer : Renderer
{
	/*
	//  How particles are drawn.
	AUTO_PROP ParticleRenderMode particleRenderMode GetRenderMode SetRenderMode

	// How much are the particles stretched in their direction of motion.
	AUTO_PROP float lengthScale GetLengthScale SetLengthScale

	// How much are the particles strectched depending on "how fast they move"
	AUTO_PROP float velocityScale GetVelocityScale SetVelocityScale

	// How much are the particles strected depending on the [[Camera]]'s speed.
	AUTO_PROP float cameraVelocityScale GetCameraVelocityScale SetCameraVelocityScale

	// Clamp the maximum particle size.
	AUTO_PROP float maxParticleSize GetMaxParticleSize SetMaxParticleSize

	// Set horizontal tiling count.
	AUTO_PROP int uvAnimationXTile GetUVAnimationXTile SetUVAnimationXTile
	// Set vertical tiling count.
	AUTO_PROP int uvAnimationYTile GetUVAnimationYTile SetUVAnimationYTile

	// Set uv animation cycles
	AUTO_PROP float uvAnimationCycles GetUVAnimationCycles SetUVAnimationCycles

	OBSOLETE warning animatedTextureCount has been replaced by uvAnimationXTile and uvAnimationYTile.
	public int animatedTextureCount { get { return uvAnimationXTile; } set { uvAnimationXTile = value; }	}

	//*undocumented fixed typo
	public float maxPartileSize { get { return maxParticleSize; } set { maxParticleSize = value; } }

	//*undocumented UV Rect access
	CUSTOM_PROP Rect[] uvTiles { return CreateScriptingArray(self->GetUVFrames(), self->GetNumUVFrames(), GetScriptingManager().GetCommonClasses().rect); } { Scripting::RaiseIfNull(value); self->SetUVFrames(Scripting::GetScriptingArrayStart<Rectf> (value), GetScriptingArraySize(value)); }

#if ENABLE_MONO
	OBSOLETE error This function has been removed.
	public AnimationCurve widthCurve { get { return null; } set { } }

	OBSOLETE error This function has been removed.
	public AnimationCurve heightCurve { get { return null; } set { } }

	OBSOLETE error This function has been removed.
	public AnimationCurve rotationCurve { get { return null; } set { } }

#endif
	*/
}

}
