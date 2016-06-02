using System;

namespace UnityEngine
{
	public class MeshImport
	{
		internal string rawJson;
		internal string guid;
		internal UnityEngine.Config.ModelImporter config;

		internal string GetNameFromFileMap(UnityEngine.Config.FileMap fms)
		{
			return config.fileIDToRecycleName.GetValueFromKey(fms.fileID);
		}
	}
}
