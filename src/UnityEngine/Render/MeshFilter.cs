using System;

namespace UnityEngine
{
	public partial class MeshFilter : Component
	{
		internal MeshFilter ()
		{
		}

		internal MeshImport mesh_raw;
		UnityEngine.Config.FileMap fms;

		// For js side.
		internal string GetMeshName()
		{
			if (fms.guid == "0000000000000000e000000000000000")
			{
				if (fms.fileID == "10210")
				{
					return "__quad__";
				}
				else
				{
					return "__sphere__";
				}
			}
			return mesh_raw.guid;
		}

		internal bool IsPredefinedMesh()
		{
			return fms.guid == "0000000000000000e000000000000000";
		}

		internal bool IsValid()
		{
			return mesh != null || !String.IsNullOrEmpty(fms.guid);
		}

		internal void SetMesh(UnityEngine.Config.FileMap fms)
		{
			this.fms = fms;
			if (IsValid())
				mesh_raw = Resources.GetMeshImport(fms);
		}

		public Mesh mesh;
	}
}

