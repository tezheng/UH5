using System;

namespace UnityEngine
{

public class Physics2D
{
	public static Collider2D OverlapPoint(Vector3 point, int mask)
	{
		return null;
	}
}

public class Collider2D : Behaviour
{
	internal override void AddToManager()
	{	

	}

	internal override void RemoveFromManager()
	{

	}
/*
	// Gets whether the collider is a trigger or not.
	AUTO_PROP bool isTrigger GetIsTrigger SetIsTrigger

	// Whether the collider is used by an attached effector or not.
	AUTO_PROP bool usedByEffector GetUsedByEffector SetUsedByEffector

	// The local offset of the collider geometry.
	AUTO_PROP Vector2 offset GetOffset SetOffset

	// Gets the attached rigid-body if it exists.
	AUTO_PTR_PROP Rigidbody2D attachedRigidbody GetRigidbody

	// Gets the number of shapes this collider has generated.
	AUTO_PROP int shapeCount GetShapeCount

	// The world space bounding volume of the collider.
	AUTO_PROP Bounds bounds GetBounds

	// Gets the collider error state indicating indicating if anything (and what) went wrong creating collision shape(s).
	CUSTOM_PROP internal ColliderErrorState2D errorState { return self->GetColliderErrorState (); }

	// Checks whether the specified point overlaps the collider or not.
	CUSTOM public bool OverlapPoint (Vector2 point) { return self->OverlapPoint(point); }

	// The shared physics material of this collider.
	CUSTOM_PROP PhysicsMaterial2D sharedMaterial { return Scripting::ScriptingWrapperFor (self->GetMaterial ()); } { self->SetMaterial (value); }

	// Get whether this collider is currently touching a specific collider or not.
	CUSTOM public bool IsTouching (Collider2D collider) { return self->IsTouching (collider); }

	// Get whether the specific collider is touching the specific layer(s).
	CUSTOM public bool IsTouchingLayers (int layerMask = Physics2D.AllLayers) { return self->IsTouchingLayers (layerMask); }

	// OnCollisionEnter2D is called when this [[Collider2D]] has begun touching another [[Collider2D]].
	CSNONE void OnCollisionEnter2D (Collision2D info);

	// OnCollisionStay2D is called once per frame for this [[Collider2D]] if it is continuing to touch another [[Collider2D]].
	CSNONE void OnCollisionStay2D (Collision2D info);

	// OnCollisionExit2D is called when this [[Collider2D]] has stopped touching another [[Collider2D]].
	CSNONE void OnCollisionExit2D (Collision2D info);

	// OnTriggerEnter2D is called when a [[Collider2D]] has begun touching another [[Collider2D]] configured as a trigger.
	CSNONE void OnTriggerEnter2D (Collider2D other);

	// OnTriggerStay2D is called once per frame for a [[Collider2D]] if it is continuing to touch another [[Collider2D]] configured as a trigger.
	CSNONE void OnTriggerStay2D (Collider2D other);

	// OnTriggerExit2D is called when a [[Collider2D]] has stopped touching another [[Collider2D]] configured as a trigger.
	CSNONE void OnTriggerExit2D (Collider2D other);
*/
}

}