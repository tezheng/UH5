using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{


	public class Gizmos
	{
		static void CheckGizmoDrawing ()
		{

		}


		public static void DrawRay (Ray r)
		{
			Gizmos.DrawLine (r.origin, r.origin + r.direction);
		}
		public static void DrawRay (Vector3 from, Vector3 direction)
		{
			Gizmos.DrawLine (from, from + direction);
		}	

		public static void DrawLine (Vector3 from, Vector3 to)
		{
			CheckGizmoDrawing ();
			#if UNITY_EDITOR
			DrawLine (from, to);
			#endif
		}

		public static void DrawWireSphere (Vector3 center, float radius)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawWireSphere (center, radius);
			#endif
		}

		public static void DrawSphere (Vector3 center, float radius)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawSphere (center, radius);
			#endif
		}

		public static void DrawWireCube (Vector3 center, Vector3 size)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawWireCube (center, size);
			#endif
		}

		public static void DrawCube (Vector3 center, Vector3 size)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawCube (center, size);
			#endif
		}

		public static void DrawMesh (Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale)
	    {
	        DrawMesh (mesh, -1, position, rotation, scale);
	    }
		public static void DrawMesh (Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawGizmoMesh (mesh, position, rotation, scale, submeshIndex, false);
			#endif
		}

	    public static void DrawWireMesh (Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale)
	    {
	        DrawWireMesh (mesh, -1, position, rotation, scale);
	    }
		public static void DrawWireMesh (Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawGizmoMesh (mesh, position, rotation, scale, submeshIndex, true);
			#endif
		}


		public static void DrawIcon (Vector3 center, string name, bool allowScaling = true)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawIcon (center, name, allowScaling);
			#endif
		}


		public static void DrawGUITexture (Rect screenRect, Texture texture, Material mat = null)
		{
			DrawGUITexture (screenRect, texture,0,0,0,0, mat);
		}

		public static void DrawGUITexture (Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat = null)
		{
			#if UNITY_EDITOR
			CheckGizmoDrawing ();
			DrawGUITexture (screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, ColorRGBA32(128,128,128,128), mat);
			#endif
		}

		public static Color color
		{
			get {
				#if UNITY_EDITOR
				return gizmos::g_GizmoColor;
				#endif
				return new Color (1,1,1,1);
			}
			set {
				#if UNITY_EDITOR
				gizmos::g_GizmoColor = value;
				#endif
			}
		}
		
		public static Matrix4x4 matrix
		{
			get {
				#if UNITY_EDITOR
				return GetGizmoMatrix ();
				#endif
				return Matrix4x4.identity;
			}
			set {
				#if UNITY_EDITOR
				SetGizmoMatrix (value);
				#endif
			}
		}

		public static void DrawFrustum (Vector3 center, float fov, float maxRange, float minRange, float aspect)
		{
			#if UNITY_EDITOR
			DrawFrustum (center, fov, maxRange, minRange, aspect);
			#endif
		}
	}
}

