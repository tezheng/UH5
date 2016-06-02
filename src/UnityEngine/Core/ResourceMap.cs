using System;
using System.Collections.Generic;

using UnityEngine.Config;

namespace UnityEngine
{
	class ResourceMapManager
	{
		List<ResourceMap> all = new List<ResourceMap>();
		Dictionary<string, ResourceMap> guidMap = new Dictionary<string, ResourceMap>();
		Dictionary<string, ResourceMap> pathMap = new Dictionary<string, ResourceMap>();
		Dictionary<string, ResourceMap> resourcesMap = new Dictionary<string, ResourceMap>();
		Dictionary<string, ResourceMap> csMap = new Dictionary<string, ResourceMap>();
		internal ResourceMapManager(List<System.Object> json)
		{
			foreach (var o in json)
			{
				var r = ResourceMap.FromJson(o as Dictionary<string, System.Object>);
				all.Add(r);
				guidMap[r.guid] = r;
				if (r.isr == 1)
				{
					resourcesMap[r.rpath.ToLower()] = r;
				}
				if (r.type == "monobehaviour")
				{
					csMap[r.path.ToLower()] = r;
				}
			}
		}

		string TypeToExt(string type)
		{
			if (type == "scene")
			{
				return ".unity";
			}
			else if (type == "model")
			{
				return ".fbx";
			}
			else if (type == "material")
			{
				return ".mat";
			}
			else if (type == "monobehaviour")
			{
				return ".cs";
			}
			else if (type == "img")
			{
				return ".png";
			}
			else if (type == "prefab")
			{
				return ".prefab";
			}
			else if (type == "txt")
			{
				return ".txt";
			}
			else if (type == "json")
			{
				return ".json";
			}
			return ".unknown";
		}

		internal ResourceMap GetFromPath(string path)
		{
			foreach (var kvp in resourcesMap)
			{
				if (kvp.Value.rpath == path)
				{
					return kvp.Value;
				}
			}
			return null;
		}

		internal ResourceMap GetMonoBehaviourFromName(string name)
		{
			ResourceMap rm;
			csMap.TryGetValue(name.ToLower() + ".cs", out rm);
			return rm;
		}

		internal ResourceMap GetFromNameAndType (string name, string type)
		{
			ResourceMap rm;
			resourcesMap.TryGetValue(name.ToLower() + TypeToExt(type), out rm);
			return rm;
		}

		// Hack. for multi ext type.
		internal ResourceMap GetFromNameAndType2 (string name, string type)
		{
			ResourceMap rm;
			foreach (var kvp in resourcesMap)
			{
				if (kvp.Value.rpath.ToLower().StartsWith(name.ToLower()) && kvp.Value.type == type)
				{
					return kvp.Value;
				}
			}
			return null;
		}

		internal ResourceMap GetSceneFromName(string name)
		{
			foreach (var r in all)
			{
				if (r.type == "scene" && r.name.ToLower() == name.ToLower())
				{
					return r;
				}
			}
			return null;
		}

		internal ResourceMap GetFromInfo(FileMap fm)
		{
			return GetFromGuid(fm.guid);
		}

		internal ResourceMap GetFromGuid(string guid)
		{
			if (String.IsNullOrEmpty(guid))
			{
				return null;
			}
			ResourceMap rm;
			guidMap.TryGetValue(guid, out rm);
			return rm;
		}
	}
}

