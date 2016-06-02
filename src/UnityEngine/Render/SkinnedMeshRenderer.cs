using System;

namespace UnityEngine
{
	public class SkinnedMeshRenderer : Renderer
	{
		internal override void UpdateRenderer ()
		{
			UpdateManagerState(ShouldBeInScene());
			DoUpdateSkinnedMeshRenderer(this);
		}

		internal SkinnedMeshRenderer (System.Object o)
			:base(o)
		{
			var r = o as UnityEngine.Config.SkinnedMeshRenderer;
			skinnedMeshRenderer_raw = r;
			SetMesh(r.m_Mesh);
		}

		internal void SetMesh(UnityEngine.Config.FileMap fms)
		{
			this.fms = fms;
			mesh_raw = Resources.GetMeshImport(fms);
		}

		protected override void UpdateManagerState( bool needsUpdate )
		{
			base.UpdateManagerState(needsUpdate);
			if (ShouldBeInScene())
			{
				gameObject.transform.__Reset ();
			}
			DoUpdateSkinnedMeshRenderer(this);
		}

		internal UnityEngine.Config.FileMap fms;
		internal MeshImport mesh_raw;
		internal UnityEngine.Config.SkinnedMeshRenderer skinnedMeshRenderer_raw;

		static extern void DoUpdateSkinnedMeshRenderer(SkinnedMeshRenderer renderer);
	}
}
