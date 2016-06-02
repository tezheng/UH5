using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{

// Rendering path of a [[Camera]].
enum RenderingPath
{
	// Use Player Settings.
	UsePlayerSettings = -1,

	// Vertex Lit.
	VertexLit = 0,

	// Forward Rendering.
	Forward = 1,

	// Deferred Lighting.
	DeferredLighting = 2,

	// Deferred Shading.
	DeferredShading = 3
}


// Transparent object sorting mode of a [[Camera]].
public enum TransparencySortMode
{
	// Default sorting mode.
	Default = 0,

	// Perspective sorting mode.
	Perspective = 1,

	// Orthographic sorting mode.
	Orthographic = 2
}

// A Camera is a device through which the player views the world.
public class Camera : Behaviour
{
	// The field of view of the camera in degrees.
	public float fieldOfView {
		get {return GetFov();} set {SetFov(value);}
	}

	// The near clipping plane distance.
	public float nearClipPlane {
		get {return GetNear();} set {SetNear(value);}
	}

	// The far clipping plane distance.
	public float farClipPlane {
		get {return GetFar();} set {SetFar(value);}
	}

	// Camera's half-size when in orthographic mode.
	public float orthographicSize {
		get {return GetOrthographicSize();} set {SetOrthographicSize(value);}
	}

	// Is the camera orthographic (''true'') or perspective (''false'')?
	public bool orthographic {
		get {return GetOrthographic();} set {SetOrthographic(value);}
	}

	// Camera's depth in the camera rendering order.
	public float depth {
		get {return GetDepth();} set {SetDepth(value);}
	}

	// The aspect ratio (width divided by height).
	public float aspect {
		get {return GetAspect();} set {SetAspect(value);}
	}

	// The color with which the screen will be cleared.
	public Color backgroundColor {
		get {return GetBackgroundColor();} set {SetBackgroundColor(value);}
	}

	public bool isOrthoGraphic {
		get {return GetOrthographic();} set {SetOrthographic(value);}
	}

	public int cullingMask;
	float m_FieldOfView;
	float m_NearClip;
	float m_FarClip;
	float m_OrthographicSize;
	bool m_Orthographic;
	float m_Depth;
	float m_Aspect;
	Color m_BackGroundColor;

	public CameraClearFlags clearFlags;

	public Matrix4x4 worldToCameraMatrix {
		get {
			return GetWorldToCameraMatrix();
		}
	}

	public void CopyFrom(Camera other)
	{
		cullingMask = other.cullingMask;
		m_FieldOfView = other.m_FieldOfView;
		m_NearClip = other.m_NearClip;
		m_FarClip = other.m_FarClip;
		m_OrthographicSize = other.m_OrthographicSize;
		m_Orthographic = other.m_Orthographic;
		m_Depth = other.m_Depth;
		m_Aspect = other.m_Aspect;
		m_BackGroundColor = other.m_BackGroundColor;
	}

	public Rect pixelRect {get {return GetScreenViewportRect();}}

	public float pixelWidth {
		get {
			return pixelRect.width;
		}
	}

	public float pixelHeight {
		get {
			return pixelRect.height;
		}
	}

	internal Camera()
	{
		m_FieldOfView = 60;
		m_NearClip = 0.3f;
		m_FarClip = 1000;
		m_OrthographicSize = 1;
		m_Orthographic = false;
		m_Depth = 1;
		m_Aspect = 1;
		m_BackGroundColor = new Color(0, 0, 1, 1);
	}

	float GetFov() {return m_FieldOfView;}
	void SetFov(float deg)
	{
		SetDirty();
		m_FieldOfView = deg;
	}
	float GetNear() {return m_NearClip;}
	void SetNear(float n)
	{
		SetDirty();
		m_NearClip = n;
	}
	float GetFar() {return m_FarClip;}
	void SetFar(float f)
	{
		SetDirty();
		m_FarClip = f;
	}
	float GetOrthographicSize() {return m_OrthographicSize;}
	void SetOrthographicSize(float f)
	{
		SetDirty();
		m_OrthographicSize = f;
	}
	bool GetOrthographic() {return m_Orthographic;}
	void SetOrthographic(bool v)
	{
		SetDirty();
		m_Orthographic = v;
	}
	float GetDepth() {return m_Depth;}
	void SetDepth(float depth)
	{
		SetDirty();
		m_Depth = depth;
		if (IsActive () && GetEnabled ()) {
			RemoveFromManager ();
			AddToManager ();
		}
	}
	float GetAspect() {return m_Aspect;}
	void SetAspect(float aspect)
	{
		m_Aspect = aspect;
	}
	Color GetBackgroundColor() {return m_BackGroundColor;}
	void SetBackgroundColor(Color color)
	{
		m_BackGroundColor = color;
		SetDirty();
	}

	Rect GetScreenViewportRect()
	{
		return new Rect(0, 0, Application.width, Application.height);
	}

	public Rect rect = new Rect(0, 0, 1, 1);

	public void ResetProjectionMatrix()
	{
		Rect r = GetScreenViewportRect();
		if (r.height != 0)
			m_Aspect = (r.width / r.height);
		else
			m_Aspect = 1.0f;
	}

	Matrix4x4 GetCameraToWorldMatrix()
	{
		Matrix4x4 m = GetWorldToCameraMatrix();
		m.Invert_Full();
		return m;
	}

	Matrix4x4 GetWorldToCameraMatrix ()
	{
		Matrix4x4 m = Matrix4x4.Scale(new Vector3 (1.0F, 1.0F, -1.0F));
		m = m * transform.GetWorldToLocalMatrixNoScale();
		return m;
	}

	internal override void RemoveFromManager ()
	{
		Application.GetRenderManager().RemoveCamera (this);
	}

	internal override void AddToManager()
	{
		Application.GetRenderManager().AddCamera (this);
	}


	public static Camera main {
		get {
			return Application.GetRenderManager().GetMainCamera();
		}
	}

	// Returns a ray going from camera through a viewport point.
	public Ray ViewportPointToRay (Vector3 position)
	{
		return ScreenPointToRay (new Vector2 (position.x * Application.width, position.y * Application.height));
	}

	// Returns a ray going from camera through a screen point.
	public Ray ScreenPointToRay (Vector3 position)
	{
		return ViewportPointToRay(new Vector3(position.x / Application.width, position.y / Application.height, position.z));
	}

	Matrix4x4 m_ProjectionMatrix = new Matrix4x4();

	Matrix4x4 GetProjectionMatrix ()
	{
		m_Aspect = (float)Application.width / Application.height;
		if (!m_Orthographic)
			m_ProjectionMatrix.SetPerspective( m_FieldOfView, m_Aspect, m_NearClip, m_FarClip );
		else
			m_ProjectionMatrix.SetOrtho( -m_OrthographicSize * m_Aspect, m_OrthographicSize * m_Aspect, -m_OrthographicSize, m_OrthographicSize, m_NearClip, m_FarClip );
		return m_ProjectionMatrix;
	}

	Matrix4x4 GetWorldToClipMatrix()
	{
		Matrix4x4 t = GetProjectionMatrix() * GetWorldToCameraMatrix();
		return t;
	}

	void GetClipToWorldMatrix(out Matrix4x4 outMatrix )
	{
		Matrix4x4.Invert_Full( GetWorldToClipMatrix(), out outMatrix );
	}

	Ray ScreenPointToRay (Vector2 viewPortPos)
	{
		Rect viewport = new Rect(0, 0, Application.width, Application.height);
		Ray ray = new Ray();
		Vector3 o;
		Matrix4x4 clipToWorld;
		GetClipToWorldMatrix( out clipToWorld );

		Matrix4x4 camToWorld = GetCameraToWorldMatrix();
		if( !CameraUnProject( new Vector3(viewPortPos.x, viewPortPos.y, m_NearClip), camToWorld, clipToWorld, viewport, out o) )
		{
			return new Ray (transform.position, new Vector3(0, 0, 1));
		}
		ray.origin = o;

		if (m_Orthographic)
		{
			// In orthographic projection we get better precision by circumventing the whole projection and subtraction.
			ray.direction = Vector3.Normalize (-camToWorld.GetAxisZ());
		}
		else
		{
			// We need to sample a point much further out than the near clip plane to ensure decimals in the ray direction
			// don't get lost when subtracting the ray origin position.
			if( !CameraUnProject( new Vector3(viewPortPos.x, viewPortPos.y, m_NearClip + 1000), camToWorld, clipToWorld, viewport, out o) )
			{
				return new Ray (transform.position, new Vector3(0, 0, 1));
			}
			Vector3 dir = o - ray.origin;
			ray.direction = (Vector3.Normalize (dir));
		}
		return ray;
	}

	bool CameraProject(Vector3 p, Matrix4x4 cameraToWorld, Matrix4x4 worldToClip, Rect viewport, out Vector3 outP )
		{
			Vector3 clipPoint;
			outP = new Vector3 (0, 0, 0);
			if( worldToClip.PerspectiveMultiplyPoint3( p, out clipPoint ) )
			{
				Vector3 cameraPos = cameraToWorld.GetPosition();
				Vector3 dir = p - cameraPos;
				// The camera/projection matrices follow OpenGL convention: positive Z is towards the viewer.
				// So negate it to get into Unity convention.
				Vector3 forward = -cameraToWorld.GetAxisZ();
				float dist = Vector3.Dot( dir, forward );

				outP.x = viewport.x + (1.0f + clipPoint.x) * viewport.width * 0.5f;
				outP.y = viewport.y + (1.0f + clipPoint.y) * viewport.height * 0.5f;
				//outP.z = (1.0f + clipPoint.z) * 0.5f;
				outP.z = dist;

				return true;
			}
			outP.Set( 0.0f, 0.0f, 0.0f );
			return false;
		}

	bool CameraUnProject(Vector3 p, Matrix4x4 cameraToWorld, Matrix4x4 clipToWorld, Rect viewport, out Vector3 outP )
	{
		// pixels to -1..1
		Vector3 in_v;
		in_v.x = (p.x - viewport.x) * 2.0f / viewport.width - 1.0f;
		in_v.y = (p.y - viewport.y) * 2.0f / viewport.height - 1.0f;
		// It does not matter where the point we unproject lies in depth; so we choose 0.95, which
		// is further than near plane and closer than far plane, for precision reasons.
		// In a perspective camera setup (near=0.1, far=1000), a point at 0.95 projected depth is about
		// 5 units from the camera.
		in_v.z = 0.95f;

		Vector3 pointOnPlane;
		if( clipToWorld.PerspectiveMultiplyPoint3( in_v, out pointOnPlane ) )
		{
			// Now we have a point on the plane perpendicular to the viewing direction. We need to return the one that is on the line
			// towards this point, and at p.z distance along camera's viewing axis.
			Vector3 cameraPos = cameraToWorld.GetPosition();
			Vector3 dir = pointOnPlane - cameraPos;

			// The camera/projection matrices follow OpenGL convention: positive Z is towards the viewer.
			// So negate it to get into Unity convention.
			Vector3 forward = -cameraToWorld.GetAxisZ();
			float distToPlane = Vector3.Dot( dir, forward );
			if( Mathf.Abs(distToPlane) >= 1.0e-6f )
			{
				bool isPerspective = clipToWorld.IsPerspective();
				if( isPerspective )
				{
					dir *= p.z / distToPlane;
					outP = cameraPos + dir;
				}
				else
				{
					outP = pointOnPlane - forward * (distToPlane - p.z);
				}
				return true;
			}
		}
		outP = new Vector3( 0.0f, 0.0f, 0.0f );
		return false;
	}

	// Transforms /position/ from screen space into world space.
		public Vector3 ScreenToWorldPoint (Vector3 position)
		{
			Rect viewport = GetScreenViewportRect();

			Vector3 o;
			Matrix4x4 clipToWorld;
			GetClipToWorldMatrix(out clipToWorld );
			if( !CameraUnProject( position, GetCameraToWorldMatrix(), clipToWorld, viewport, out o) )
			{
				//AssertString (Format("Screen position out of view frustum (screen pos %f, %f, %f) (Camera rect %d %d %d %d)", v.x, v.y, v.z, viewport.x, viewport.y, viewport.width, viewport.height));
			}
			return o;
		}

		// Transforms /position/ from world space into screen space.
		public Vector3 WorldToScreenPoint (Vector3 position)
		{
			Rect viewport = GetScreenViewportRect();
			Vector3 o;
			Matrix4x4 clipToWorld;
			GetClipToWorldMatrix(out clipToWorld );
			CameraProject( position, GetCameraToWorldMatrix(), clipToWorld, viewport, out o);
			return o;
		}

		// Transforms /position/ from world space into viewport space.
		public Vector3 WorldToViewportPoint (Vector3 position)
		{
			Vector3 screenPoint = WorldToScreenPoint (position);
			return ScreenToViewportPoint (screenPoint);
		}

		public Vector3 ScreenToViewportPoint (Vector3 position)
		{
			Rect r = GetScreenViewportRect ();
			float nx = (position.x - r.x) / r.width;
			float ny = (position.y - r.y) / r.height;
			return new Vector3 (nx, ny, position.z);
		}

		public Vector3 ViewportToWorldPoint (Vector3 position)
		{
			Vector3 screenPoint = ViewportToScreenPoint (position);
			return ScreenToWorldPoint (screenPoint);
		}

		public Vector3 ViewportToScreenPoint (Vector3 position)
		{
			Rect r = GetScreenViewportRect();
			float nx = position.x * r.width + r.x;
			float ny = position.y * r.height + r.y;
			return new Vector3 (nx, ny, position.z);
		}

		public TransparencySortMode transparencySortMode;
		public int eventMask;
}

}
