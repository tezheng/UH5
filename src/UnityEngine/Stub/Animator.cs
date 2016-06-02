using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{

// Target
enum AvatarTarget
{
	// The root, the position of the game object
	Root = 0,
	// The body, center of mass
	Body = 1,
	// The left foot
	LeftFoot = 2,
	// The right foot
	RightFoot = 3,
	// The left hand
	LeftHand = 4,
	// The right hand
	RightHand = 5,
}

// IK Goal
enum AvatarIKGoal
{
	// The left foot
	LeftFoot = 0,
	// The right foot
	RightFoot = 1,
	// The left hand
	LeftHand = 2,
	// The right hand
	RightHand = 3
}

// IK Hint
enum AvatarIKHint
{
	// The left knee
	LeftKnee = 0,
	// The right knee
	RightKnee = 1,
	// The left elbow
	LeftElbow = 2,
	// The right elbow
	RightElbow = 3
}

// This enum need to match
//	Runtime/mecanim/generic/typetraits.h ValueType enum
//	Editor/Src/Animation/AnimatorControllerParameter.h AnimatorControllerParameterType enum
//*undocumented*
enum AnimatorControllerParameterType
{
	Float = 1,
	Int = 3,
	Bool = 4,
	Trigger = 9,
}

enum TransitionType
{
	Normal = 1 << 0,
	Entry  = 1 << 1,
	Exit   = 1 << 2
}

enum AnimatorRecorderMode
{
	Offline,
	Playback,
	Record
}

/*
// Information about what animation clips is played and its weight
STRUCT AnimatorClipInfo

	// Animation clip that is played
	CSRAW public AnimationClip clip  { get {return m_ClipInstanceID != 0 ? ClipInstanceToScriptingObject(m_ClipInstanceID) : null; } }
	
	// The weight of the animation clip
	CSRAW public float weight        { get { return m_Weight;}}
	
	CUSTOM private static AnimationClip ClipInstanceToScriptingObject(int instanceID)
	{
		return Scripting::ScriptingWrapperFor(PPtr<AnimationClip>(instanceID));
	}

	CSRAW private int m_ClipInstanceID;
	CSRAW private float m_Weight;
END

static int ScriptingStringToCRC32 (ICallString& stringValue);

*/

// Culling mode for the Animator
enum AnimatorCullingMode
{
	// Always animate the entire character. Object is animated even when offscreen.
	AlwaysAnimate = 0,

	// Retarget, IK and write of Transforms are disabled when renderers are not visible.
	CullUpdateTransforms = 1,

	// Animation is completly disabled when renderers are not visible.
	CullCompletely = 2,
}

enum AnimatorUpdateMode
{
	Normal = 0,
	AnimatePhysics = 1,
	UnscaledTime = 2
}


// Information about the current or next state
public struct AnimatorStateInfo
{
	/*
	// Does /name/ match the name of the active state in the statemachine.
	CSRAW public bool IsName (string name)    { int hash = Animator.StringToHash (name); return hash == m_FullPath || hash == m_Name || hash == m_Path; }

	// For backwards compatibility this is actually the path...
	CSRAW public int fullPathHash             { get { return m_FullPath; } }

	OBSOLETE warning Use AnimatorStateInfo.fullPathHash instead.
	CSRAW public int nameHash                 { get { return m_Path; } }

	CSRAW public int shortNameHash            { get { return m_Name; } }
	*/
	// Normalized time of the State
	public float normalizedTime         { get { return m_NormalizedTime; } }
	
	// Current duration of the state
	public float length                 { get { return m_Length; } }
	
	// The Tag of the State
//	CSRAW public int tagHash                  { get { return m_Tag; } }

	// Does /tag/ match the tag of the active state in the statemachine.
//	CSRAW public bool IsTag (string tag)      { return Animator.StringToHash (tag) == m_Tag; }
	
	// Is the state looping
	public bool loop                    { get { return m_Loop != 0;} }
	
	private int    m_Name;
	private int    m_Path;
	private int    m_FullPath;
	private float  m_NormalizedTime;
	private float  m_Length;
	private int    m_Tag;
	private int    m_Loop;
}

/*
// Information about the current transition
STRUCT AnimatorTransitionInfo

	// Does /name/ match the name of the active Transition.
	CSRAW public bool IsName (string name) { return Animator.StringToHash (name) == m_Name  || Animator.StringToHash (name) == m_FullPath ; }

	// Does /userName/ match the name of the active Transition.
	CSRAW public bool IsUserName (string name) { return Animator.StringToHash (name) == m_UserName ; }

	
	CSRAW public int fullPathHash				{ get { return m_FullPath; } }

	// The unique name of the Transition
	CSRAW public int nameHash					{ get { return m_Name; } }

	// The user-specidied name of the Transition
	CSRAW public int userNameHash				{ get { return m_UserName; } }

	// Normalized time of the Transition
	CSRAW public float normalizedTime			{ get { return m_NormalizedTime; } }

	CSRAW public bool anyState					{ get { return m_AnyState; } }

	CSRAW internal bool entry					{ get { return (m_TransitionType & (int)TransitionType.Entry) != 0 ;}}

	CSRAW internal bool exit					{ get { return (m_TransitionType & (int)TransitionType.Exit) != 0;}}
	
	
	CSRAW private int   m_FullPath;
	CSRAW private int   m_UserName;
	CSRAW private int   m_Name;
	CSRAW private float m_NormalizedTime;
	CSRAW private bool	m_AnyState;
	CSRAW private int	m_TransitionType;
	
END



// To specify position and rotation weight mask for Animator::MatchTarget
STRUCT MatchTargetWeightMask

	// MatchTargetWeightMask contructor
	CSRAW public MatchTargetWeightMask(Vector3 positionXYZWeight, float rotationWeight)
	{
		m_PositionXYZWeight = positionXYZWeight;
		m_RotationWeight = rotationWeight;
	}

	// Position XYZ weight
	CSRAW public Vector3 positionXYZWeight
	{
		get { return m_PositionXYZWeight;}
		set { m_PositionXYZWeight = value; }
	}

	// Rotation weight
	CSRAW public float rotationWeight
	{
		get { return m_RotationWeight;}
		set { m_RotationWeight =value;}
	}

	CSRAW private Vector3 m_PositionXYZWeight;
	CSRAW private float m_RotationWeight;
END

*/


enum HumanBodyBones { Hips=0, LeftUpperLeg, RightUpperLeg,	LeftLowerLeg,	RightLowerLeg,	LeftFoot,	RightFoot,	Spine,	Chest,	Neck,	Head,	LeftShoulder,	RightShoulder,	LeftUpperArm,	RightUpperArm,	LeftLowerArm,	RightLowerArm,	LeftHand,	RightHand,	LeftToes,	RightToes,	LeftEye,	RightEye,	Jaw,	LastBone};


// Interface to control the Mecanim animation system
public class Animator : Behaviour
{
	// Gets the current State information on a specified AnimatorController layer
	public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
	{
		/*
		AnimatorStateInfo info;
		self->GetAnimatorStateInfo (layerIndex, Animator::kCurrentState, info);
		return info;
		*/
		return new AnimatorStateInfo();
	}

	public void Play (string stateName, int layer = -1, float normalizedTime = Mathf.NegativeInfinity)
	{
	}

	public void Update(float deltaTime)
	{

	}

	/*
	// Returns true if the current rig is optimizable
	CUSTOM_PROP bool isOptimizable { return self->IsOptimizable(); }

	// Returns true if the current rig is ''humanoid'', false if it is ''generic''
	CUSTOM_PROP bool isHuman { return self->IsHuman(); }

	// Returns true if the current generic rig has a root motion
	CUSTOM_PROP bool hasRootMotion { return self->HasRootMotion(); }

	// Returns true if root translation or rotation is driven by curves
	CUSTOM_PROP internal bool isRootPositionOrRotationControlledByCurves { return self->IsRootTranslationOrRotationControllerByCurves(); }

	// Returns the scale of the current Avatar for a humanoid rig, (1 by default if the rig is generic)
	CUSTOM_PROP float humanScale                         { return self->GetHumanScale(); }
	
	// Gets the value of a float parameter
	CSRAW public float GetFloat(string name)             { return GetFloatString(name); }
	// Gets the value of a float parameter
	CSRAW public float GetFloat(int id)                  { return GetFloatID(id); }
	// Sets the value of a float parameter
	CSRAW public void SetFloat(string name, float value) { SetFloatString(name, value);}
	// Sets the value of a float parameter
	CSRAW public void SetFloat(string name, float value, float dampTime, float deltaTime) { SetFloatStringDamp(name, value, dampTime, deltaTime);}

	// Sets the value of a float parameter
	CSRAW public void SetFloat(int id, float value)       { SetFloatID(id, value);}
	// Sets the value of a float parameter
	CSRAW public void SetFloat(int id, float value, float dampTime, float deltaTime) { SetFloatIDDamp(id, value, dampTime, deltaTime); }
	
	// Gets the value of a bool parameter
	CSRAW public bool GetBool(string name)                { return GetBoolString(name);}
	// Gets the value of a bool parameter
	CSRAW public bool GetBool(int id)                     { return GetBoolID(id);}
	// Sets the value of a bool parameter
	CSRAW public void SetBool(string name, bool value)    { SetBoolString(name, value);}
	// Sets the value of a bool parameter
	CSRAW public void SetBool(int id, bool value)         { SetBoolID(id, value);}
	
	// Gets the value of an integer parameter
	CSRAW public int GetInteger(string name)              { return GetIntegerString(name);}
	// Gets the value of an integer parameter
	CSRAW public int GetInteger(int id)                   { return  GetIntegerID(id);}
	// Sets the value of an integer parameter
	CSRAW public void SetInteger(string name, int value)  { SetIntegerString(name, value);}
	
	// Sets the value of an integer parameter
	CSRAW public void SetInteger(int id, int value)       { SetIntegerID(id, value); }
	
	// Sets the trigger parameter on
	CSRAW public void SetTrigger(string name)       { SetTriggerString(name); }

	// Sets the trigger parameter at on
	CSRAW public void SetTrigger(int id)       { SetTriggerID(id); }

	// Resets the trigger parameter at off
	CSRAW public void ResetTrigger(string name)       { ResetTriggerString(name); }

	// Resets the trigger parameter at off
	CSRAW public void ResetTrigger(int id)       { ResetTriggerID(id); }
	
	// Returns true if a parameter is controlled by an additional curve on an animation
	CSRAW public bool IsParameterControlledByCurve(string name)     { return IsParameterControlledByCurveString(name); }
	// Returns true if a parameter is controlled by an additional curve on an animation
	CSRAW public bool IsParameterControlledByCurve(int id)          { return IsParameterControlledByCurveID(id); }

	// Gets the avatar delta position for the last evaluated frame
	CUSTOM_PROP Vector3	deltaPosition { return self->GetDeltaPosition(); }
	// Gets the avatar delta rotation for the last evaluated frame
	CUSTOM_PROP Quaternion	deltaRotation { return self->GetDeltaRotation(); }

	// Gets the avatar velocity for the last evaluated frame
	CUSTOM_PROP Vector3	velocity { return self->GetVelocity(); }
	// Gets the avatar angular velocity for the last evaluated frame
	CUSTOM_PROP Vector3	angularVelocity { return self->GetAngualrVelocity(); }

	//  The root position, the position of the game object
	CUSTOM_PROP Vector3 rootPosition { return self->GetAvatarPosition();} { self->SetAvatarPosition(value);}
	//  The root rotation, the rotation of the game object
	CUSTOM_PROP Quaternion rootRotation { return self->GetAvatarRotation();} { self->SetAvatarRotation(value);}
	
	// Root is controlled by animations
	CUSTOM_PROP bool applyRootMotion { return self->GetApplyRootMotion(); } { self->SetApplyRootMotion(value); }

	// Linear velocity blending for root motion
	CUSTOM_PROP bool linearVelocityBlending { return self->GetLinearVelocityBlending(); } { self->SetLinearVelocityBlending(value); }

	// When turned on, animations will be executed in the physics loop. This is only useful in conjunction with kinematic rigidbodies.
	OBSOLETE warning Use Animator.updateMode instead
	CSRAW public bool animatePhysics
	{
		get { return updateMode == AnimatorUpdateMode.AnimatePhysics; }
		set { updateMode =  ( value ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal) ; }
	}

	CUSTOM_PROP	AnimatorUpdateMode updateMode { return self->GetUpdateMode() ; } { self->SetUpdateMode(value);}

	// Tell if the corresponding Character has transform hierarchy.
	CUSTOM_PROP bool hasTransformHierarchy { return self->GetHasTransformHierarchy(); }

	CUSTOM_PROP internal bool allowConstantClipSamplingOptimization { return self->GetAllowConstantClipSamplingOptimization(); } { self->SetAllowConstantClipSamplingOptimization(value); }

	// The current gravity weight based on current animations that are played
	CUSTOM_PROP float gravityWeight { return self->GetGravityWeight(); }

	// The position of the body center of mass
	CUSTOM_PROP Vector3 bodyPosition { return self->GetBodyPosition(); } { self->SetBodyPosition(value); }
	// The rotation of the body center of mass
	CUSTOM_PROP Quaternion bodyRotation { return self->GetBodyRotation(); } { self->SetBodyRotation(value); }
	
	// Gets the position of an IK goal
	CUSTOM Vector3 GetIKPosition( AvatarIKGoal goal) {  return self->GetGoalPosition(goal) ;}
	// Sets the position of an IK goal
	CUSTOM void SetIKPosition( AvatarIKGoal goal, Vector3 goalPosition) { self->SetGoalPosition(goal,goalPosition); }
	
	// Gets the rotation of an IK goal
	CUSTOM Quaternion GetIKRotation( AvatarIKGoal goal) { return  self->GetGoalRotation(goal); }
	// Sets the rotation of an IK goal
	CUSTOM void SetIKRotation( AvatarIKGoal goal, Quaternion goalRotation) { self->SetGoalRotation(goal, goalRotation); }
	
	// Gets the translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal)
	CUSTOM float GetIKPositionWeight ( AvatarIKGoal goal) { return self->GetGoalWeightPosition(goal); }
	// Sets the translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal)
	CUSTOM void SetIKPositionWeight ( AvatarIKGoal goal, float value) { self->SetGoalWeightPosition(goal,value); }
	
	// Gets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal)
	CUSTOM float GetIKRotationWeight( AvatarIKGoal goal) { return self->GetGoalWeightRotation(goal); }
	// Sets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal)
	CUSTOM void SetIKRotationWeight( AvatarIKGoal goal, float value) { self->SetGoalWeightRotation(goal,value); }

	// Gets the position of an IK hint
	CUSTOM Vector3 GetIKHintPosition( AvatarIKHint hint) {  return self->GetHintPosition(hint) ;}
	// Sets the position of an IK hint
	CUSTOM void SetIKHintPosition( AvatarIKHint hint, Vector3 hintPosition) { self->SetHintPosition(hint,hintPosition); }

	// Gets the translative weight of an IK hint (0 = at the original animation before IK, 1 = points toward the hint)
	CUSTOM float GetIKHintPositionWeight ( AvatarIKHint hint) { return self->GetHintWeightPosition(hint); }
	// Sets the translative weight of an IK hint (0 = at the original animation before IK, 1 = points toward the hint)
	CUSTOM void SetIKHintPositionWeight ( AvatarIKHint hint, float value) { self->SetHintWeightPosition(hint,value); }

	// Sets the look at position
	CUSTOM void SetLookAtPosition(Vector3 lookAtPosition) { self->SetLookAtPosition(lookAtPosition); }
	
	//Set look at weights
	CUSTOM void SetLookAtWeight(float weight, float bodyWeight = 0.00f, float headWeight = 1.00f, float eyesWeight = 0.00f, float clampWeight = 0.50f)
	{
		self->SetLookAtBodyWeight(weight*bodyWeight);
		self->SetLookAtHeadWeight(weight*headWeight);
		self->SetLookAtEyesWeight(weight*eyesWeight);
		self->SetLookAtClampWeight(clampWeight);
	}

	//CSRAW
	// [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
	CUSTOM internal ScriptableObject GetBehaviour(Type type)
	{
		Scripting::RaiseIfNull(type);
		ScriptingClassPtr klass = scripting_class_from_systemtypeinstance(type);
		return Scripting::ScriptingWrapperFor(self->GetBehaviour(klass));
	}

	CSRAW
	public T GetBehaviour<T>() where T : StateMachineBehaviour { return GetBehaviour(typeof(T)) as T; }

	CUSTOM internal ScriptableObject[] GetBehaviours(Type type)
	{
		Scripting::RaiseIfNull(type);
		ScriptingClassPtr klass = scripting_class_from_systemtypeinstance(type);

		StateMachineBehaviourVector behaviours = self->GetBehaviours(klass);

		ScriptingObjectPtr* scriptingObjects;

		#if ENABLE_DOTNET
			scriptingObjects = new ScriptingObjectPtr[behaviours.size ()];
		#else
			ALLOC_TEMP(scriptingObjects, ScriptingObjectPtr, behaviours.size ());
		#endif
	
		int count = 0;
		for(size_t i = 0;i < behaviours.size (); i++)
		{
			ScriptingObjectPtr mono = Scripting::ScriptingWrapperFor(behaviours[i]);
			Scripting::RaiseIfNull(mono);
			scriptingObjects[count++] = mono;
		}

		ScriptingArrayPtr result = Scripting::CreateScriptingArrayFromScriptingObjects(scriptingObjects, count, klass);

		#if ENABLE_DOTNET
			delete[]scriptingObjects;
		#endif

		return result;
	}

	CSRAW internal static T[] ConvertStateMachineBehaviour<T>(ScriptableObject[] rawObjects) where T : StateMachineBehaviour
	{
		if (rawObjects == null) return null;
		T[] typedObjects = new T[rawObjects.Length];
		for (int i = 0; i < typedObjects.Length; i++)
			typedObjects[i] = (T)rawObjects[i];
		return typedObjects;
	}

	CSRAW
	public T[] GetBehaviours<T>() where T : StateMachineBehaviour
	{
		return ConvertStateMachineBehaviour<T>( GetBehaviours( typeof(T) ));
	}

	// Automatic stabilization of feet during transition and blending
	CUSTOM_PROP bool stabilizeFeet { return self->GetStabilizeFeet(); } { self->SetStabilizeFeet(value);}

	// The AnimatorController layer count
	CUSTOM_PROP int layerCount { return self->GetLayerCount(); }
	// Gets name of the layer
	CUSTOM string GetLayerName( int layerIndex) { return scripting_string_new(self->GetLayerName(layerIndex)) ; }
		CUSTOM int GetLayerIndex( string layerName) { return self->GetLayerIndex(layerName); }
	// Gets the layer's current weight
	CUSTOM float GetLayerWeight( int layerIndex) { return self->GetLayerWeight(layerIndex) ; }
	// Sets the layer's current weight
	CUSTOM void SetLayerWeight( int layerIndex, float weight) { self->SetLayerWeight(layerIndex, weight); }
	
	
	// Gets the current State information on a specified AnimatorController layer
	CUSTOM AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
	{
		AnimatorStateInfo info;
		self->GetAnimatorStateInfo (layerIndex, Animator::kCurrentState, info);
		return info;
	}
	
	// Gets the next State information on a specified AnimatorController layer
	CUSTOM AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex)
	{
		AnimatorStateInfo info;
		self->GetAnimatorStateInfo(layerIndex, Animator::kNextState, info);
		return info;
	}
	
	// Gets the Transition information on a specified AnimatorController layer
	CUSTOM AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex)
	{
		AnimatorTransitionInfo  info;
		self->GetAnimatorTransitionInfo(layerIndex, info);
		return info;
	}
	
	// Gets the list of AnimatorClipInfo currently played by the current state
	CUSTOM AnimatorClipInfo[] GetCurrentAnimationClipState (int layerIndex)
	{
		dynamic_array<AnimatorClipInfo> clips (kMemTempAlloc);
		self->GetAnimationClipState(layerIndex, true, clips);
		return VectorToScriptingStructArray(clips, GetScriptingManager().GetCommonClasses().animatorClipInfo, AnimatorClipInfoToMono);
	}

	// Gets the list of AnimatorClipInfo currently played by the next state
	CUSTOM AnimatorClipInfo[] GetNextAnimationClipState (int layerIndex)
	{
		dynamic_array<AnimatorClipInfo> clips (kMemTempAlloc);
		self->GetAnimationClipState(layerIndex, false, clips);
		return VectorToScriptingStructArray(clips, GetScriptingManager().GetCommonClasses().animatorClipInfo, AnimatorClipInfoToMono);
	}
	
	// Is the specified AnimatorController layer in a transition
	CUSTOM bool IsInTransition(int layerIndex) { return self->IsInTransition(layerIndex); }
	

	CUSTOM_PROP AnimatorControllerParameter[] parameters
	{
		return VectorToScriptingClassArray<AnimatorControllerParameter, MonoAnimatorControllerParameter> (self->GetParameters(), scripting_class_from_fullname(kEngineAssemblyName, kEngineNameSpace, "AnimatorControllerParameter"), AnimatorControllerParameterToMono);
	}
	
	// Blends pivot point between body center of mass and feet pivot. At 0%, the blending point is body center of mass. At 100%, the blending point is feet pivot
	CUSTOM_PROP float feetPivotActive { return self->GetFeetPivotActive(); } { self->SetFeetPivotActive(value);}
	// Gets the pivot weight
	CUSTOM_PROP float pivotWeight { return self->GetPivotWeight(); }
	// Get the current position of the pivot
	CUSTOM_PROP Vector3	pivotPosition { return self->GetPivotPosition(); }
	
	
	// Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress
	CUSTOM void MatchTarget(Vector3 matchPosition,  Quaternion matchRotation, AvatarTarget targetBodyPart,  MatchTargetWeightMask weightMask, float startNormalizedTime, float targetNormalizedTime = 1)
	{
		self->MatchTarget(matchPosition, matchRotation, (int)targetBodyPart, weightMask, startNormalizedTime, targetNormalizedTime);
	}

	// Interrupts the automatic target matching
	CUSTOM void InterruptMatchTarget(bool completeMatch = true) { self->InterruptMatchTarget(completeMatch);}
	// If automatic matching is active
	CUSTOM_PROP bool isMatchingTarget{ return self->IsMatchingTarget();}
	
	
	// The playback speed of the Animator. 1 is normal playback speed
	CUSTOM_PROP float speed { return self->GetSpeed() ; } { self->SetSpeed(value);}

	// Force the normalized time of a state to a user defined value
	OBSOLETE warning ForceStateNormalizedTime is deprecated. Please use Play or CrossFade instead.
	CSRAW public void ForceStateNormalizedTime(float normalizedTime) { Play(0, 0, normalizedTime); }
	

	CSRAW public void CrossFade (string stateName, float transitionDuration, int layer = -1, float normalizedTime = float.NegativeInfinity)
	{
		CrossFade (StringToHash(stateName), transitionDuration, layer, normalizedTime);
	}
	CUSTOM void CrossFade (int stateNameHash, float transitionDuration, int layer = -1, float normalizedTime = float.NegativeInfinity)
	{
		self->GotoState(layer, stateNameHash, normalizedTime, transitionDuration);
	}
	
	CSRAW public void Play (string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity)
	{
		Play (StringToHash(stateName), layer, normalizedTime);
	}
	CUSTOM void Play (int stateNameHash, int layer = -1, float normalizedTime = float.NegativeInfinity)
	{
		self->GotoState(layer, stateNameHash, normalizedTime, 0.0F);
	}



	// Sets an AvatarTarget and a targetNormalizedTime for the current state
	CUSTOM void SetTarget(AvatarTarget targetIndex, float targetNormalizedTime) {self->SetTarget((int)targetIndex, targetNormalizedTime);}
	//  Returns the position of the target specified by SetTarget(AvatarTarget targetIndex, float targetNormalizedTime))
	CUSTOM_PROP Vector3	targetPosition { return self->GetTargetPosition(); }
	//  Returns the rotation of the target specified by SetTarget(AvatarTarget targetIndex, float targetNormalizedTime))
	CUSTOM_PROP Quaternion	targetRotation { return self->GetTargetRotation(); }
	
	
	OBSOLETE error use mask and layers to control subset of transfroms in a skeleton
	CUSTOM bool IsControlled(Transform transform) {return false;}

	// Returns ture if a transform a bone controlled by human
	CUSTOM internal bool IsBoneTransform(Transform transform) {return self->IsBoneTransform(transform);}

	CUSTOM_PROP internal Transform avatarRoot {return Scripting::ScriptingWrapperFor(self->GetAvatarRoot());}

	// Returns transform mapped to this human bone id
	CUSTOM Transform GetBoneTransform(HumanBodyBones humanBoneId) {return Scripting::ScriptingWrapperFor(self->GetBoneTransform((int)humanBoneId));}
	
	// Controls culling of this Animator component.
	AUTO_PROP AnimatorCullingMode cullingMode GetCullingMode SetCullingMode

	// Sets the animator in playback mode
	CUSTOM void StartPlayback() { self->StartPlayback();}

	// Stops animator playback mode
	CUSTOM void StopPlayback() { self->StopPlayback();}

	// Plays recorded data
	CUSTOM_PROP float playbackTime  {return self->GetPlaybackTime(); } { self->SetPlaybackTime(value);}
	
	// Sets the animator in record mode
	CUSTOM void StartRecording(int frameCount) { self->StartRecording(frameCount);}

	// Stops animator record mode
	CUSTOM void StopRecording() { self->StopRecording();}

	// The time at which the recording data starts
	CUSTOM_PROP float recorderStartTime { return self->GetRecorderStartTime();} {}

	// The time at which the recoding data stops
	CUSTOM_PROP float recorderStopTime { return self->GetRecorderStopTime();} {}

	CUSTOM_PROP AnimatorRecorderMode recorderMode{ return self->GetRecorderMode(); }

	// The runtime representation of AnimatorController that controls the Animator
	CUSTOM_PROP RuntimeAnimatorController runtimeAnimatorController {return Scripting::ScriptingWrapperFor(self->GetRuntimeAnimatorController());} { self->SetRuntimeAnimatorController(value);}

	CUSTOM bool HasState(int layerIndex, int stateID) { return self->HasState(layerIndex, stateID) ; }

	C++RAW static int ScriptingStringToCRC32 (ICallString& stringValue)
	{
		if(stringValue.IsNull()) return 0;
		#if ENABLE_MONO
		const gunichar2* chars = mono_string_chars(stringValue.GetScriptingString());
		const size_t length = stringValue.Length();
		
		if (IsUtf16InAsciiRange (chars, length))
		{
			return mecanim::processCRC32UTF16Ascii (chars, length);
		}
		else
		#endif
		{
			return mecanim::processCRC32 (stringValue.AsUTF8().c_str());
		}
	}
	
	// Generates an parameter id from a string
	THREAD_SAFE
	CUSTOM static int StringToHash (string name) { return ScriptingStringToCRC32(name); }

	// Gets/Sets the current Avatar
	CUSTOM_PROP Avatar	avatar {return Scripting::ScriptingWrapperFor(self->GetAvatar());} { self->SetAvatar(value);}

	CUSTOM internal string GetStats()
	{
		return scripting_string_new( self->GetStats() );
	}

	#define GET_NAME_IMPL(Func,x) \
		x value; \
		GetSetValueResult result = self->Func(ScriptingStringToCRC32(name), value); \
		if (result != kGetSetSuccess)  \
			self->ValidateParameterString (result, name); \
		return value;

	#define SET_NAME_IMPL(Func) \
		GetSetValueResult result = self->Func(ScriptingStringToCRC32(name), value); \
		if (result != kGetSetSuccess) \
			self->ValidateParameterString (result, name);


	#define GET_ID_IMPL(Func,x) \
		x value; \
		GetSetValueResult result = self->Func(id, value); \
		if (result != kGetSetSuccess) \
			self->ValidateParameterID (result, id); \
		return value;

	#define SET_ID_IMPL(Func) \
		GetSetValueResult result = self->Func(id, value); \
		if (result != kGetSetSuccess)  \
			self->ValidateParameterID (result, id);


	// Internal
	CUSTOM private void SetFloatString(string name, float value) { SET_NAME_IMPL(SetFloat) }
	CUSTOM private void SetFloatID(int id, float value)          { SET_ID_IMPL  (SetFloat) }

	CUSTOM private float GetFloatString(string name)             { GET_NAME_IMPL(GetFloat, float) }
	CUSTOM private float GetFloatID(int id)                      { GET_ID_IMPL  (GetFloat, float) }

	CUSTOM private void SetBoolString(string name, bool value)   { SET_NAME_IMPL(SetBool)   }
	CUSTOM private void SetBoolID(int id, bool value)            { SET_ID_IMPL  (SetBool)   }

	CUSTOM private bool GetBoolString(string name)               { GET_NAME_IMPL(GetBool, bool)   }
	CUSTOM private bool GetBoolID(int id)                        { GET_ID_IMPL  (GetBool, bool)   }


	CUSTOM private void SetIntegerString(string name, int value) { SET_NAME_IMPL(SetInteger) }
	CUSTOM private void SetIntegerID(int id, int value)          { SET_ID_IMPL  (SetInteger)}

	CUSTOM private int GetIntegerString(string name)             { GET_NAME_IMPL(GetInteger, int) }
	CUSTOM private int GetIntegerID(int id)                      { GET_ID_IMPL  (GetInteger, int) }

	CUSTOM private void SetTriggerString(string name)
	{
		GetSetValueResult result = self->SetTrigger(ScriptingStringToCRC32(name));
		if (result != kGetSetSuccess)
			self->ValidateParameterString (result, name);
	}

	CUSTOM private void SetTriggerID(int id)
	{
		GetSetValueResult result = self->SetTrigger(id);
		if (result != kGetSetSuccess)
			self->ValidateParameterID (result, id);
	}

	CUSTOM private void ResetTriggerString(string name)
	{
		GetSetValueResult result = self->ResetTrigger(ScriptingStringToCRC32(name));
		if (result != kGetSetSuccess)
			self->ValidateParameterString (result, name);
	}

	CUSTOM private void ResetTriggerID(int id)
	{
		GetSetValueResult result = self->ResetTrigger(id);
		if (result != kGetSetSuccess)
			self->ValidateParameterID (result, id);
	}
	
	CUSTOM private bool IsParameterControlledByCurveString(string name)
	{
		GetSetValueResult result = self->ParameterControlledByCurve(ScriptingStringToCRC32(name));
		if (result == kParameterIsControlledByCurve)
			return true;
		else if (result == kGetSetSuccess)
			return false;
		else
		{
			self->ValidateParameterString (result, name);
			return false;
		}
	}
	
	CUSTOM private bool IsParameterControlledByCurveID(int id)
	{
		GetSetValueResult result = self->ParameterControlledByCurve(id);
		if (result == kParameterIsControlledByCurve)
			return true;
		else if (result == kGetSetSuccess)
			return false;
		else
		{
			self->ValidateParameterID (result, id);
			return false;
		}
	}

	CUSTOM private void SetFloatStringDamp(string name, float value, float dampTime, float deltaTime)
	{
		GetSetValueResult result = self->SetFloatDamp(ScriptingStringToCRC32(name), value, dampTime, deltaTime);
		if (result != kGetSetSuccess)
			self->ValidateParameterString (result, name);
	}
	
	CUSTOM private void SetFloatIDDamp(int id, float value, float dampTime, float deltaTime)
	{
		GetSetValueResult result = self->SetFloatDamp(id, value, dampTime, deltaTime);
		if (result != kGetSetSuccess)
			self->ValidateParameterID (result, id);
	}
	
	// True if additional layers affect the center of mass
	CUSTOM_PROP bool layersAffectMassCenter {return self->GetLayersAffectMassCenter();} { self->SetLayersAffectMassCenter(value);}


	// Get left foot bottom height.
	CUSTOM_PROP float leftFeetBottomHeight {return self->GetLeftFeetBottomHeight();}

	// Get right foot bottom height.
	CUSTOM_PROP float rightFeetBottomHeight {return self->GetRightFeetBottomHeight();}

	CONDITIONAL UNITY_EDITOR
	CUSTOM_PROP internal bool supportsOnAnimatorMove { return self->SupportsOnAnimatorMove (); }
	
	CONDITIONAL UNITY_EDITOR
	CUSTOM internal void WriteDefaultPose() { self->WriteDefaultPose(); }
	
	CUSTOM void Update(float deltaTime) { self->Update(deltaTime);}

	CUSTOM void Rebind() { self->Rebind(); }
	
	CONDITIONAL UNITY_EDITOR
	// Evalutes only the StateMachine, does not write into transforms, uses previous deltaTime
	// Mostly used for editor previews ( BlendTrees )
	CUSTOM internal void EvaluateSM()					{self->EvaluateSM();}
	
	CONDITIONAL UNITY_EDITOR
	CUSTOM internal string GetCurrentStateName(int layerIndex) { return scripting_string_new(self->GetAnimatorStateName(layerIndex,true));}

	CONDITIONAL UNITY_EDITOR
	CUSTOM internal string GetNextStateName(int layerIndex) { return scripting_string_new(self->GetAnimatorStateName(layerIndex,false));}

	CUSTOM internal string ResolveHash(int hash) { return scripting_string_new(self->ResolveHash(hash)); }

	CUSTOM_PROP private bool isInManagerList { return self->IsInManagerList();}

	AUTO_PROP bool logWarnings GetLogWarnings SetLogWarnings
	AUTO_PROP bool fireEvents GetFireEvents SetFireEvents

	*/

	internal override void AddToManager ()
	{

	}

	internal override void RemoveFromManager ()
	{

	}
}

}
