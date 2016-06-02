using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{

// Position, rotation and scale of an object.

public class Transform : Component, IEnumerable
{	
	// The position of the transform in world space.
	public Vector3 position
	{
		get {return GetPosition();}
		set {SetPosition(value);}
	}

	// Position of the transform relative to the parent transform.
	public Vector3 localPosition
	{
		get {return GetLocalPosition();}
		set { SetLocalPosition(value); }
	}

	// The rotation as Euler angles in degrees.
	public Vector3 eulerAngles { get { return rotation.eulerAngles; } set { rotation = Quaternion.Euler(value); }  }

	// The rotation as Euler angles in degrees relative to the parent transform's rotation.
	public Vector3 localEulerAngles
	{
		get { return GetLocalEulerAngles (); }
		set { SetLocalEulerAngles (value); }
	}
	
	// The red axis of the transform in world space.
	public Vector3 right  { get { return rotation * Vector3.right; } set { rotation = Quaternion.FromToRotation(Vector3.right, value); } }

	// The green axis of the transform in world space.
	public Vector3 up       { get { return rotation * Vector3.up; }  set { rotation = Quaternion.FromToRotation(Vector3.up, value); } }

	// The blue axis of the transform in world space.
	public Vector3 forward { get { return rotation * Vector3.forward; } set { rotation = Quaternion.LookRotation(value); } }
	
	// The rotation of the transform in world space stored as a [[Quaternion]].
	public Quaternion rotation 
	{
		get {return GetRotation();}
		set {SetRotationSafe(value);}
	}

	// The rotation of the transform relative to the parent transform's rotation.
	public Quaternion localRotation
	{
		get {return GetLocalRotation();}
		set {SetLocalRotationSafe(value);}
	}
	
	// The scale of the transform relative to the parent.
	public Vector3 localScale
	{
		get {return GetLocalScale();}
		set {SetLocalScale(value);}
	}

	// The parent of the transform.
	public Transform parent
	{
		get { return parentInternal; }
		set
		{
			parentInternal = value;
		}
	}
	
	public void SetParent (Transform parent)
	{
		SetParent (parent, true);
	}
	
	// Matrix that transforms a point from world space into local space (RO).
	public Matrix4x4 worldToLocalMatrix
	{
		get {return GetWorldToLocalMatrix();}
	}
	// Matrix that transforms a point from local space into world space (RO).
	public Matrix4x4 localToWorldMatrix
	{
		get {return GetLocalToWorldMatrix();}
	}

	bool _hasChanged = false;

	bool _hackDirty = true;
	bool _needUpdateToRender = true;
	Matrix4x4 m_cachedLTW;	//local to world
	// bool _hackDirtyWTL = true;
	// Matrix4x4 m_cachedWTL;	//world to local

	internal bool hackDirty
	{
		get {
			return _hackDirty;
		}
		set {
			_hackDirty = value;
			_needUpdateToRender = value;
			if (value)
			{
				_hasChanged = true;
				foreach (var c in children)
				{
					c.hackDirty = true;
				}
			}
		}
	}

	// Moves the transform in the direction and distance of /translation/.
	public void Translate (Vector3 translation, Space relativeTo = Space.Self)
	{
		if (relativeTo == Space.World)
			position += translation;
		else
			position += TransformDirection (translation);
		hackDirty = true;
	}
	
	// Moves the transform by /x/ along the x axis, /y/ along the y axis, and /z/ along the z axis.
	public void Translate (float x, float y, float z, Space relativeTo = Space.Self)
	{
		Translate (new Vector3 (x, y, z), relativeTo);
	}

	// Moves the transform in the direction and distance of /translation/.
	public void Translate (Vector3 translation, Transform relativeTo)
	{
		if (relativeTo)
			position += relativeTo.TransformDirection (translation);
		else
			position += translation;
		hackDirty = true;
	}
	
	// Moves the transform by /x/ along the x axis, /y/ along the y axis, and /z/ along the z axis.
	public void Translate (float x, float y, float z, Transform relativeTo)
	{
		Translate (new Vector3 (x, y, z), relativeTo);
	}
	
	// Applies a rotation of /eulerAngles.z/ degrees around the z axis, /eulerAngles.x/ degrees around the x axis, and /eulerAngles.y/ degrees around the y axis (in that order).
	public void Rotate (Vector3 eulerAngles, Space relativeTo = Space.Self)
	{
		Quaternion eulerRot = Quaternion.Euler (eulerAngles.x, eulerAngles.y, eulerAngles.z);
		if (relativeTo == Space.Self)
			localRotation = localRotation * eulerRot;
		else
		{
			rotation = rotation * (Quaternion.Inverse (rotation) * eulerRot * rotation);
		}
		hackDirty = true;
	}

	// Applies a rotation of /zAngle/ degrees around the z axis, /xAngle/ degrees around the x axis, and /yAngle/ degrees around the y axis (in that order).
	public void Rotate (float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self)
	{
		Rotate (new Vector3 (xAngle, yAngle, zAngle), relativeTo);
	}

	// Rotates the transform around /axis/ by /angle/ degrees.
	public void Rotate (Vector3 axis, float angle, Space relativeTo = Space.Self)
	{
		if (relativeTo == Space.Self)
			RotateAroundInternal (transform.TransformDirection (axis), angle * Mathf.Deg2Rad);
		else
			RotateAroundInternal (axis, angle * Mathf.Deg2Rad);
		hackDirty = true;
	}

	// Rotates the transform about /axis/ passing through /point/ in world coordinates by /angle/ degrees.
	public void RotateAround (Vector3 point, Vector3 axis, float angle)
	{
		Vector3 worldPos = position;
		Quaternion q = Quaternion.AngleAxis (angle , axis);
		Vector3 dif = worldPos - point;
		dif = q * dif;
		worldPos = point + dif;
		position = worldPos;
		RotateAroundInternal (axis, angle * Mathf.Deg2Rad);
		hackDirty = true;
	}
	
	// Rotates the transform so the forward vector points at /target/'s current position.
	public void LookAt(Transform target) {LookAt(target, Vector3.up);}
	public void LookAt (Transform target, Vector3 worldUp)
	{
		if (target)	LookAt (target.position, worldUp);
	}

	// Rotates the transform so the forward vector points at /worldPosition/.
	public void LookAt (Vector3 worldPosition) {LookAt(worldPosition, Vector3.up);}

	// Transforms direction /x/, /y/, /z/ from local space to world space.
	public Vector3 TransformDirection (float x, float y, float z) { return TransformDirection (new Vector3 (x, y, z)); }

	// Transforms the direction /x/, /y/, /z/ from world space to local space. The opposite of Transform.TransformDirection.
	public Vector3 InverseTransformDirection (float x, float y, float z) { return InverseTransformDirection (new Vector3 (x, y, z)); }

	// Transforms vector /x/, /y/, /z/ from local space to world space.
	public Vector3 TransformVector (float x, float y, float z) { return TransformVector (new Vector3 (x, y, z)); }

	// Transforms the vector /x/, /y/, /z/ from world space to local space. The opposite of Transform.TransformVector.
	public Vector3 InverseTransformVector (float x, float y, float z) { return InverseTransformVector (new Vector3 (x, y, z)); }

	// Transforms the position /x/, /y/, /z/ from local space to world space.
	public Vector3 TransformPoint (float x, float y, float z) { return TransformPoint (new Vector3 (x, y, z)); }

	// Transforms the position /x/, /y/, /z/ from world space to local space. The opposite of Transform.TransformPoint.
	public Vector3 InverseTransformPoint (float x, float y, float z) { return InverseTransformPoint (new Vector3 (x, y, z)); }

	// The global scale of the object (RO).
	public Vector3 lossyScale
	{
		get {return GetWorldScaleLossy();}
	}

	// Has the transform changed since the last time the flag was set to 'false'?
	public bool hasChanged
	{
		get {return GetChangedFlag();}
		set {SetChangedFlag(value);}
	}

	//*undocumented*
	public Transform FindChild (string name) { return Find(name); }

	//*undocumented* Documented separately
	public IEnumerator GetEnumerator ()
	{
		return new Transform.Enumerator (this);
	}

	private class Enumerator : IEnumerator
	{
		Transform outer;
		int currentIndex = -1;

		internal Enumerator (Transform outer) { this.outer = outer; }
		//*undocumented*
		public object Current
		{
			get { return outer.GetChild (currentIndex); }
		}

		//*undocumented*
		public bool MoveNext ()
		{
			int childCount = outer.childCount;
			return ++currentIndex < childCount;
		}

		//*undocumented*
		public void Reset () { currentIndex = -1; }
	}

	Vector3 m_LocalPosition;
	Vector3 m_LocalScale;
	Quaternion m_LocalRotation;
	Vector3 GetPosition()
	{
		if (parent)
			return parent.localToWorldMatrix.MultiplyPoint(m_LocalPosition);
		else
			return m_LocalPosition;
	}

	void SetPosition(Vector3 position)
	{
		Vector3 newPosition = position;
		Transform p = parent;
		if (p)
		{
			newPosition = p.InverseTransformPoint (newPosition);
		}

		SetLocalPosition (newPosition);
	}

	Vector3 GetLocalPosition()
	{
		return m_LocalPosition;
	}

	void SetLocalPosition(Vector3 localPosition)
	{
		if(m_LocalPosition != localPosition)
		{
			m_LocalPosition = localPosition;
			SetDirty ();
			hackDirty = true;
		}
	}

	Vector3 GetLocalEulerAngles()
	{
		Quaternion rotation = Quaternion.NormalizeSafe (m_LocalRotation);
		return Quaternion.QuaternionToEuler (rotation) * Mathf.Rad2Deg;
	}

	void SetLocalEulerAngles(Vector3 value)
	{
		SetLocalRotationSafe (Quaternion.EulerToQuaternion (value * Mathf.Deg2Rad));
	}

	Quaternion GetRotation()
	{
		// Matrix4x4 mat = new Matrix4x4();
		// Quaternion.QuaternionToMatrix(m_LocalRotation, ref mat);
		// mat = localToWorldMatrix * mat;

		// Quaternion worldRot = new Quaternion();
		// Quaternion.MatrixToQuaternion(mat, ref worldRot);

        Quaternion worldRot = m_LocalRotation;
		Transform p = parent;
		while (p)
		{
			worldRot = p.m_LocalRotation * worldRot;
			p = p.parent;
		}

		return worldRot;
	}

	void SetRotationSafe(Quaternion q)
	{
		Transform p = parent;
		if (p)
			SetLocalRotation (Quaternion.NormalizeSafe (Quaternion.Inverse (p.GetRotation ()) * q));
		else
			SetLocalRotation (Quaternion.NormalizeSafe (q));
	}

	Quaternion GetLocalRotation()
	{
		return m_LocalRotation;
	}

	void SetLocalRotation(Quaternion q)
	{
		if(m_LocalRotation != q)
		{
			m_LocalRotation = q;
			SetDirty ();
			hackDirty = true;
		}
	}

	void SetLocalRotationSafe(Quaternion q)
	{
		SetLocalRotation (Quaternion.NormalizeSafe(q));
	}

	Vector3 GetLocalScale()
	{
		return m_LocalScale;
	}

	void SetLocalScale(Vector3 scale)
	{
		if(m_LocalScale != scale)
		{
			m_LocalScale = scale;
			RecalculateTransformType ();
			SetDirty ();
			hackDirty = true;
		}
	}

	const int kNoScaleTransform = 0;
	const int kUniformScaleTransform = 1 << 0;
	const int kNonUniformScaleTransform = 1 << 1;
	const int kOddNegativeScaleTransform = 1 << 2;

	int m_InternalTransformType;
	void RecalculateTransformType()
	{
		if (Mathf.Approximately (m_LocalScale.x, m_LocalScale.y) && Mathf.Approximately (m_LocalScale.y, m_LocalScale.z))
		{
			if (Mathf.Approximately( m_LocalScale.x, 1.0F))
			{
				m_InternalTransformType = kNoScaleTransform;
			}
			else
			{
				m_InternalTransformType = kUniformScaleTransform;
				if (m_LocalScale.x < 0.0F)
				{
					m_InternalTransformType = kOddNegativeScaleTransform | kNonUniformScaleTransform;
				}
			}
		}
		else
		{
			m_InternalTransformType = kNonUniformScaleTransform;

			int hasOddNegativeScale = m_LocalScale.x * m_LocalScale.y * m_LocalScale.z < 0.0F ? 1 : 0;
			m_InternalTransformType |= hasOddNegativeScale * kOddNegativeScaleTransform;
		}
	}

	Transform _parent;
	internal Transform parentInternal
	{
		get {return _parent;}
		set {SetParent(value, true);}
	}

	internal bool SetParent (Transform NewParent, bool worldPositionStays)
	{
		if (NewParent == parent)
			return true;

		SetDirty();
		SetCacheDirty();
		hackDirty = true;

		var go = gameObject;

		if (go.IsDestroying() || (NewParent && NewParent.gameObject.IsDestroying()))
			return false;

		if ( parent && parent.gameObject.IsActivating() || (NewParent && NewParent.gameObject.IsActivating()))
		{
			Debug.LogError ("Cannot change GameObject hierarchy while activating or deactivating the parent.");
			return false;
		}

		// Make sure that the new father is not a child of this transform.
		if (IsChildOrSameTransform(NewParent, this))
			return false;

		// Save the old position in worldspace
		Vector3 worldPosition = new Vector3();
		Quaternion worldRotation = new Quaternion();
		Matrix4x4 worldScale = new Matrix4x4();

		if (worldPositionStays)
		{
			worldPosition = GetPosition();
			worldRotation = GetRotation();
			worldScale = GetWorldRotationAndScale();
		}

		Transform previousParent = parent;
		if (previousParent)
			previousParent.SetDirty();

		// At this point SetParentInternal MUST return true only if we really changed a parent
		SetParentInternal(NewParent);

		SendParentChanged();

		if (NewParent)
			NewParent.SetDirty();

		if (worldPositionStays)
		{
			SetPositionAndRotationSafeWithoutNotification(worldPosition, worldRotation);
			SetWorldRotationAndScaleWithoutNotification(worldScale);
		}

		return true;
	}

	static bool IsChildOrSameTransform(Transform transform, Transform inParent)
	{
		Transform child = transform;
		while (child)
		{
			if (child == inParent)
				return true;
			child = child.parent;
		}
		return false;
	}

	Matrix4x4 GetWorldRotationAndScale()
	{
		Matrix4x4 ret = new Matrix4x4();
		ret.SetTRS(new Vector3(0, 0, 0), m_LocalRotation, m_LocalScale);
		if (parent)
		{
			Matrix4x4 parentTransform = parent.GetWorldRotationAndScale();
			ret = parentTransform * ret;
		}
		return ret;
	}

	void SetParentInternal(Transform NewParent)
	{
		//Early out if the new parent is already the current parent
		if (NewParent == parent)
			return;

		// If it already has an father, remove this from fathers children
		if (parent)
		{
			parent.__RemoveChild(this);
		}
		if (NewParent)
		{
			NewParent.__AddChild(this);
		}
		_parent = NewParent;
	}

	void SetPositionAndRotationSafeWithoutNotification(Vector3 p, Quaternion q)
	{
		if (parent)
		{
			m_LocalPosition = parent.InverseTransformPoint (p);
			m_LocalRotation = Quaternion.NormalizeSafe (Quaternion.Inverse (parent.GetRotation ()) * q);
			hackDirty = true;
		}
		else
		{
			m_LocalPosition = p;
			m_LocalRotation = Quaternion.NormalizeSafe (q);
			hackDirty = true;
		}
	}

	void SetWorldRotationAndScaleWithoutNotification(Matrix4x4 scale)
	{
		m_LocalScale = Vector3.one;

		Matrix4x4 inverseRS = GetWorldRotationAndScale();
		inverseRS.Invert_Full();

		inverseRS = inverseRS * scale;

		m_LocalScale.x = inverseRS[0, 0];
		m_LocalScale.y = inverseRS[1, 1];
		m_LocalScale.z = inverseRS[2, 2];

		RecalculateTransformType();
		hackDirty = true;
	}

	void SetCacheDirty()
	{
		// do nothing for now.
	}

	Matrix4x4 GetWorldToLocalMatrix()
	{
		Matrix4x4 m = GetLocalToWorldMatrix();
		m.Invert_Full();
		return m;
	}

	Matrix4x4 GetLocalToWorldMatrix()
	{
		if (_hackDirty)
		{
			Matrix4x4 t = new Matrix4x4();
			t.SetTRS(m_LocalPosition, m_LocalRotation, m_LocalScale);
			if (parent != null)
			{
				m_cachedLTW = parent.GetLocalToWorldMatrix() * t;
			}
			else
			{
				m_cachedLTW = t;
			}

			_hackDirty = false;
		}

		return m_cachedLTW;
	}

	void GetPositionAndRotation (out Vector3 pos, out Quaternion q)
	{
		Vector3 worldPos = m_LocalPosition;
		Quaternion worldRot = m_LocalRotation;
		Transform cur = parent;
		while (cur)
		{
			worldPos.Scale (cur.m_LocalScale);
			worldPos = Quaternion.RotateVectorByQuat (cur.m_LocalRotation, worldPos);
			worldPos += cur.m_LocalPosition;

			worldRot = cur.m_LocalRotation * worldRot;

			cur = cur.parent;
		}

		pos = worldPos;
		q = worldRot;
	}

	internal Matrix4x4 GetWorldToLocalMatrixNoScale()
	{
		Vector3 pos;
		Quaternion rot;
		GetPositionAndRotation(out pos, out rot);
		Matrix4x4 m = new Matrix4x4();
		m.SetTRInverse (pos, rot);
		return m;
	}

	internal void RotateAroundInternal (Vector3 worldAxis, float rad)
	{
		Vector3 localAxis = InverseTransformDirection(worldAxis);
		if (localAxis.sqrMagnitude > Vector3.kEpsilon)
		{
			localAxis.Normalize();
			Quaternion q = Quaternion.AxisAngleToQuaternionSafe(localAxis, rad);
			m_LocalRotation = Quaternion.NormalizeSafe(m_LocalRotation * q);
			SetDirty();
		}
	}

	public void LookAt (Vector3 worldPosition, Vector4 worldUp)
	{
		Vector3 forward = worldPosition - GetPosition ();
		Quaternion q = Quaternion.identity;
		if (Quaternion.LookRotationToQuaternion (forward, worldUp, ref q))
			SetRotationSafe (q);
		else
		{
			float mag = forward.magnitude;
			if (mag > Vector3.kEpsilon)
			{
				SetRotationSafe(Quaternion.FromToQuaternionSafe(Vector3.back, forward / mag));
			}
		}
		hackDirty = true;
	}

	// Transforms /direction/ from local space to world space.
	public Vector3 TransformDirection (Vector3 inDirection)
	{
		return Quaternion.RotateVectorByQuat (GetRotation (), inDirection);
	}

	// Transforms a /direction/ from world space to local space. The opposite of Transform.TransformDirection.
	public Vector3 InverseTransformDirection (Vector3 inDirection)
	{
		return Quaternion.RotateVectorByQuat (Quaternion.Inverse(GetRotation()), inDirection);
	}

	// Transforms /vector/ from local space to world space.
	public Vector3 TransformVector (Vector3 inVector)
	{
		Vector3 worldVector = inVector;
	
		Transform cur = this;
		while (cur)
		{
			worldVector.Scale (cur.m_LocalScale);
			worldVector = Quaternion.RotateVectorByQuat (cur.m_LocalRotation, worldVector);
			
			cur = cur.parent;
		}
		return worldVector;
	}

	// Transforms a /vector/ from world space to local space. The opposite of Transform.TransformVector.
	public Vector3 InverseTransformVector (Vector3 inVector)
	{
		Vector3 newVector, localVector;
		Transform father = parent;
		if (father)
			localVector = father.InverseTransformVector (inVector);
		else
			localVector = inVector;
		
		newVector = Quaternion.RotateVectorByQuat (Quaternion.Inverse(m_LocalRotation), localVector);
		if (m_InternalTransformType != kNoScaleTransform)
			newVector.Scale (InverseSafe (m_LocalScale));
		
		return newVector;
	}

	static float InverseSafe (float f)
	{
		if (Mathf.Abs (f) > Vector3.kEpsilon)
			return 1.0F / f;
		else
			return 0.0F;
	}

	static Vector3 InverseSafe (Vector3 v)
	{
		return new Vector3 (InverseSafe (v.x), InverseSafe (v.y), InverseSafe (v.z));
	}

	// Transforms /position/ from local space to world space.
	public Vector3 TransformPoint (Vector3 inPoint)
	{
		Vector3 worldPos = localToWorldMatrix.MultiplyPoint(inPoint);
		return worldPos;
	}

	// Transforms /position/ from world space to local space. The opposite of Transform.TransformPoint.
	public Vector3 InverseTransformPoint (Vector3 inPosition)
	{
		Vector3 newPosition, localPosition;
		if (parent)
			localPosition = parent.InverseTransformPoint (inPosition);
		else
			localPosition = inPosition;

		localPosition -= m_LocalPosition;
		newPosition = Quaternion.RotateVectorByQuat (Quaternion.Inverse(m_LocalRotation), localPosition);
		if (m_InternalTransformType != kNoScaleTransform)
			newPosition.Scale (InverseSafe (m_LocalScale));

		return newPosition;
	}

	// Returns the topmost transform in the hierarchy.
	public Transform root
	{
		get {
			if (parent == null) return this;
			return parent.root;
		}
	}

	// The number of children the Transform has.
	public int childCount
	{
		get {
			return children.Count;
		}
	}

	// Unparents all children.
	public void DetachChildren ()
	{
		List<Transform> tl = new List<Transform>(children);
		foreach (var t in tl)
		{
			t.SetParent(null);
		}
	}

	// Move itself to the end of the parent's array of children
	public void SetAsFirstSibling ()
	{
		SetSiblingIndex(0);
	}

	// Move itself to the beginning of the parent's array of children
	public void SetAsLastSibling ()
	{
		SetSiblingIndex(10000);
	}

	public void SetSiblingIndex(int index)
	{
		if (parent)
		{
			children.Remove (this);
			children.Insert (index, this);
		}
	}

	public int GetSiblingIndex()
	{
		if (parent)
		{
			for (int i = 0; i < children.Count; i++)
			{
				if (children[i] == parent)
				{
					return i;
				}
			}
		}
		return 0;
	}

	// Finds a child by /name/ and returns it.
	public Transform Find (string name)
	{
		Debug.LogError("Transform.Find not implemented. P2");
		return null;
	}

	// Is this transform a child of /parent/?
	public bool IsChildOf (Transform parent)
	{
		return IsChildOrSameTransform(this, parent);
	}
	// Get a transform child by index
	public Transform GetChild (int index)
	{
		if (children.Count > index)
		{
			return children[index];
		}
		else
		{
			return null;
		}
	}

	Matrix4x4 GetWorldScale ()
	{
		Matrix4x4 invRotation = new Matrix4x4();
		Quaternion.QuaternionToMatrix (Quaternion.Inverse (GetRotation ()), ref invRotation);
		Matrix4x4 scaleAndRotation = GetWorldRotationAndScale ();
		return invRotation * scaleAndRotation;
	}

	Vector3 GetWorldScaleLossy()
	{
		Matrix4x4 rot = GetWorldScale ();
		return new Vector3 (rot[0, 0], rot[1, 1], rot[2, 2]);
	}

	bool GetChangedFlag()
	{
		return _hasChanged;
	}

	void SetChangedFlag(bool value)
	{
		_hasChanged = value;
	}

	internal void RemoveFromParent()
	{
		if (!parent) return;
		parent.children.Remove(this);
		parent.SetDirty();
		SetDirty();
		//SendParentChanged();
	}

	void SendParentChanged()
	{
		gameObject.TransformParentHasChanged();
		DoParentChanges(this, parent);
	}

	internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
	{
		base.AwakeFromLoad (awakeMode);
		RecalculateTransformType ();
		DoMewTransform(this);
	}

	// implement details.
	internal List<Transform> children = new List<Transform> ();

	internal Transform()
	{
		DoMewTransform(this);
	}

	internal override void __Reset ()
	{
		m_LocalPosition = Vector3.zero;
		m_LocalScale = Vector3.one;
		m_LocalRotation = Quaternion.identity;
		_hackDirty = true;
	}

	internal override void WillDestroyComponent ()
	{
		base.WillDestroyComponent();
		DoDestroy(this);
	}

	internal static void UpdateAll()
	{
		foreach (var go in GameObjectManager.activeObjects)
		{
			/*
			if (go.transform.parent == null)
			{
				UpdateRecursive(go.transform);
			}*/
			var t = go.transform;
			if (t._needUpdateToRender)
			{
				DoUpdateMatrix(t);
				t._needUpdateToRender = false;
			}
		}
	}
	/*
	static void UpdateRecursive(Transform t)
	{
		if (t.hackDirty)
		{
			t.DoUpdateMatrix();
			t.hackDirty = false;
		}
		foreach (Transform c in t)
		{
			UpdateRecursive(c);
		}
	}*/

	internal void __RemoveChild(Transform child)
	{
		children.Remove(child);
	}

	internal void __AddChild(Transform child)
	{
		children.Add(child);
	}

	static extern void DoMewTransform(Transform self);

	static extern void DoParentChanges(Transform self, Transform parent);

	static extern void DoUpdateMatrix(Transform self);

	static extern void DoDestroy(Transform self);

	public static extern void HackRemoveFromRenderTree(Transform self);
}

}
