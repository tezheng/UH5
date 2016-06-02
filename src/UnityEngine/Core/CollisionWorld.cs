using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public class CollisionWorld
	{
		public List<Collider> colliders = new List<Collider>();
		internal void Remove(Collider collider)
		{
			colliders.Remove(collider);
		}

		internal void Add(Collider collider)
		{
			colliders.Add(collider);
		}

		internal bool Raycast (Ray ray, out RaycastHit hitInfo, float distance, int layerMask)
		{
			RaycastHit t;
			hitInfo = new RaycastHit();
			float v = Mathf.Infinity;
			bool ret = false;
			foreach (var c in colliders)
			{
				if (((1 << c.gameObject.layer) & layerMask) == 0)
					continue;
				var success = c.Raycast (ray, out t, distance);
				if (success && t.distance < v)
				{
					v = t.distance;
					hitInfo = t;
					ret = true;
				}
			}
			return ret;
		}

		internal RaycastHit[] RaycastAll(Ray ray, float distance, int layerMask)
		{
			RaycastHit t;
			List<RaycastHit> ret = new List<RaycastHit>();
			foreach (var c in colliders)
			{
				if (((1 << c.gameObject.layer) & layerMask) == 0)
					continue;
				var success = c.Raycast (ray, out t, distance);
				if (success)
				{
					ret.Add(t);
				}
			}
			return ret.ToArray();
		}
	}

	public class Physics
	{
		public const int IgnoreRaycastLayer = 1 << 2;
		public const int DefaultRaycastLayers = ~IgnoreRaycastLayer;
		static public bool Raycast (Ray ray, out RaycastHit hitInfo, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers)
		{
			return Application.GetCollisionWorld().Raycast(ray, out hitInfo, distance, layerMask);
		}

		static public bool SphereCast (Ray ray, float r, out RaycastHit hitInfo, float distance, int layerMask)
		{
			return Application.GetCollisionWorld().Raycast(ray, out hitInfo, distance, layerMask);
		}

		static public RaycastHit[] RaycastAll (Ray ray, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers)
		{
			return Application.GetCollisionWorld().RaycastAll(ray, distance, layerMask);
			//return RaycastAll (ray.origin, ray.direction, distance, layerMask);
		}
		/*
		CSRAW static public RaycastHit[] RaycastAll (Ray ray, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers)
	{
		return RaycastAll (ray.origin, ray.direction, distance, layerMask);
	}

	// Casts a ray through the scene and returns all hits. Note that order is not guaranteed.
	CUSTOM static RaycastHit[] RaycastAll (Vector3 origin, Vector3 direction, float distance = Mathf.Infinity, int layermask = DefaultRaycastLayers)
	{
		float dirLength = Magnitude (direction);
		if (dirLength > Vector3f::epsilon)
		{
			Vector3f normalizedDirection = direction / dirLength;
			Ray ray (origin, normalizedDirection);

			const RaycastHits& hits = GetPhysicsManager ().RaycastAll (ray, distance, layermask);

			return ConvertNativeRaycastHitsToManaged(hits);
		}
		else
		{
			return CreateEmptyStructArray(GetMonoManager().GetCommonClasses().raycastHit);
		}
	}*/
	}
}
