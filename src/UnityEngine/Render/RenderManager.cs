using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public class RenderManager
	{
		public List<Camera> cameras = new List<Camera>();
		internal void RemoveCamera(Camera camera)
		{
			cameras.Remove(camera);
		}

		internal void AddCamera(Camera camera)
		{
			cameras.Add(camera);
		}

		List<Renderer> renderers = new List<Renderer>();
		internal void AddRenderer(Renderer renderer)
		{
			renderers.Add(renderer);
		}

		internal void RemoveRenderer(Renderer renderer)
		{
			renderers.Remove(renderer);
		}

		internal List<Light> lights = new List<Light>();
		internal void AddLight(Light light)
		{
			lights.Add(light);
			DoAddLight(this, light);
		}

		internal void RemoveLight(Light light)
		{
			lights.Remove(light);
			DoRemoveLight(this, light);
		}

		internal void Update()
		{
			foreach (var r in renderers)
			{
				r.UpdateRenderer();
			}
			DoBeginRender();
			// cameras sort by depth.
			cameras.Sort((a, b) => (a.depth - b.depth < 0 ? -1 : 1) );
			foreach (var c in cameras)
			{
				UpdateCamera(c);
			}
			DoEndRender();
		}

		void UpdateCamera(Camera camera)
		{
			DoUpdateCamera(this, camera);
		}

		internal Camera GetMainCamera()
		{
			return cameras.Count > 0 ? cameras[0] : null;
		}

		static extern void DoBeginRender();
		static extern void DoUpdateCamera(RenderManager rm, Camera camera);
		static extern void DoEndRender();

		static extern void DoAddLight(RenderManager rm, Light light);

		static extern void DoRemoveLight(RenderManager rm, Light light);
		static extern void DoUpdateRenderVisible(Renderer rm, bool visible);
	}
}
