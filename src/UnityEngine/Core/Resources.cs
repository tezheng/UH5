using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{


public class Resources
{

	internal static T[] ConvertObjects<T>(Object[] rawObjects) where T : Object
	{
		if (rawObjects == null) return null;
		T[] typedObjects = new T[rawObjects.Length];
		for (int i = 0; i < typedObjects.Length; i++)
			typedObjects[i] = (T)rawObjects[i];
		return typedObjects;
	}

	public static T[] FindObjectsOfTypeAll<T> () where T : Object
	{
		return ConvertObjects<T>(FindObjectsOfTypeAll (typeof (T)));
	}

	// Loads an asset stored at /path/ in a Resources folder.

	public static Object Load (string path)
	{
		return Load(path, typeof(Object));
	}

	public static T Load<T> (string path) where T : Object
	{
		return (T) Load (path, typeof (T));
	}

	// Loads all assets in a folder or file at /path/ in a Resources folder.

	public static Object[] LoadAll (string path)
	{
		return LoadAll(path, typeof(Object));
	}

	public static T[] LoadAll<T> (string path) where T : Object
	{
		return ConvertObjects<T>(LoadAll (path, typeof (T)));
	}

	// TODO: to be implemented.
	// Returns a list of all objects of Type /type/.
	[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
	public static Object[] FindObjectsOfTypeAll (Type type) {
		Debug.LogError("Resources.FindObjectsOfTypeAll Not Implemented. P2");
		return null;
		//return Scripting::FindObjectsOfType (type, Scripting::kFindAnything);
	}

	// Loads all assets in a folder or file at /path/ in a Resources folder.
	public static Object[] LoadAll (string path, Type systemTypeInstance)
	{
		Debug.LogError("Resources.LoadAll Not Implemented. P2");
		return null;
	}

	// Unloads /assetToUnload/ from memory.
	public static void UnloadAsset (Object assetToUnload) {
		Debug.LogError("Resources.UnloadAsset Not Implemented. P2");
	}

	// Unloads assets that are not used.
	public static AsyncOperation UnloadUnusedAssets ()
	{
		Debug.LogError("Resources.UnloadUnusedAssets Not Implemented. P2");
		return null;
	}

// internal
	// Loads an asset stored at /path/ in a Resources folder.
	public static Object Load (string path, Type systemTypeInstance)
	{
		var json = rmManager.GetFromNameAndType(path, "json");
		if (json != null) {
			var text = LoadText(json.guid);
			if (text != null)
				return new TextAsset(text);
			else
				return null;
		}

		// Only support dynamic prefab load now.
		if (systemTypeInstance.Name == "TextAsset")
		{
			var m = rmManager.GetFromNameAndType(path, "txt");
			if (m == null) return null;
			var text = LoadText(m.guid);
			if (text != null)
				return new TextAsset(text);
			else
				return null;
		}
		else if (systemTypeInstance.Name == "AudioClip")
		{
			var audio = rmManager.GetFromNameAndType2(path, "audio");
			if (audio != null)
			{
				return GetAudioClip(audio.guid);
			}
			else
				return null;
		}
		else if (systemTypeInstance.Name == "Texture2D" || systemTypeInstance.Name == "Texture")
		{
			var texture = rmManager.GetFromNameAndType2(path, "img");
			if (texture == null)
				return null;

			return GetTexture(texture.guid);
		}
		else
		{
			var prefab = rmManager.GetFromNameAndType (path, "prefab");
			if (prefab == null)
				return null;
			var guid = prefab.guid;
			return LoadPrefab(guid);
		}
	}

	static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	static Dictionary<string, Material> materials = new Dictionary<string, Material>();
	static Dictionary<string, MeshImport> meshImports = new Dictionary<string, MeshImport>();
	static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
	static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();

	internal static Material GetMaterial(UnityEngine.Config.FileMap fms)
	{
		return GetMaterial(fms.guid);
	}

	internal static Shader GetShader(UnityEngine.Config.FileMap fms)
	{
		if (!shaders.ContainsKey(fms.Key()))
		{
			// get a shader from fms.
			var m = rmManager.GetFromInfo(fms);
			if (m == null)	// it is maybe a internal shader, need to manual return here.
			{
				var shader = new Shader();
				shader._Init(fms);
				shader.name = "";
				shaders[fms.Key()] = shader;
			}
			else
			{
				var shader = new Shader();
				shader._Init(fms);
				shader.name = m.p0;
				shaders[fms.Key()] = shader;
			}
		}
		return shaders[fms.Key()];
	}

	internal static Material GetMaterial(string guid)
	{
		if (materials.ContainsKey(guid))
		{
			return materials[guid];
		}
		Material mat = new Material();
		var info = rmManager.GetFromGuid(guid);
		if (info == null)
		{
			mat.guid = guid;
		}
		else
		{
			var jsontext = LoadInternalRaw(info.guid);
			mat.rawJson = jsontext;
			var json = LoadInternalFile(info.guid);
			UnityEngine.Config.Material rawMat = UnityEngine.Config.Material.FromJson(json[0] as Dictionary<string, System.Object>);
			mat.Init(rawMat);
			mat.shaderGuid = rawMat.m_Shader.guid;
		}
		materials[guid] = mat;
		return mat;
	}

	static Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


	internal static AudioClip GetAudioClip(UnityEngine.Config.FileMap fms)
	{
		if (fms.IsEmpty())
			return null;
		return GetAudioClip(fms.guid);
	}

	static AudioClip GetAudioClip(string guid)
	{
		if (!audioClips.ContainsKey(guid))
		{
			AudioClip ac = new AudioClip();
			ac.guid = guid;
			audioClips.Add(guid, ac);
		}
		return audioClips[guid];
	}

	static Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();
	internal static AnimationClip GetAnimationClip(UnityEngine.Config.FileMap fms)
	{
		if (fms.IsEmpty())
		{
			return null;
		}
		MeshImport mi = GetMeshImport(fms);
		if (mi == null || mi.config == null || mi.config.importAnimation == 0)
		{
			return null;
		}
		var key = fms.Key();
		if (animationClips.ContainsKey(key))
		{
			return animationClips[key];
		}
		// find name from fileID
		var name = mi.config.fileIDToRecycleName.GetValueFromKey(fms.fileID);
		if (String.IsNullOrEmpty(name)) return null;
		foreach (var c in mi.config.animations.clipAnimations)
		{
			if (c.name == name)
			{
				var clip = new AnimationClip();
				clip.SetFromRaw(c);
				animationClips[key] = clip;
				return clip;
			}
		}
		// if the name is not found in the file. it is the default.
		var rm = rmManager.GetFromInfo(fms);
		if (rm == null) return null;
		var rawclip = new UnityEngine.Config.ModelImporterAnimationClip();
		rawclip.firstFrame = 0;
		rawclip.lastFrame = 30;
		rawclip.wrapMode = 1;
		var antStarts = rm.name.IndexOf("@");
		if (antStarts != -1)
		{
			rawclip.name = rm.name.Substring(antStarts + 1);
		}
		else
		{
			rawclip.name = name;
		}
		var aclip = new AnimationClip();
		aclip.SetFromRaw(rawclip);
		animationClips[key] = aclip;
		return aclip;
	}

	internal static int GetExecutionOrder(string classname)
	{
		var mmap = rmManager.GetMonoBehaviourFromName(classname);
		if (mmap == null) return 0;
		var guid = mmap.guid;
		var json = LoadInternalFile(guid);
		return Logic.Convert.ToInt32((json[0] as Dictionary<string, System.Object>)["executionOrder"]);
	}

	internal static MeshImport GetMeshImport(UnityEngine.Config.FileMap fms)
	{
		return GetMeshImport(fms.guid);
	}

	internal static MeshImport GetMeshImport(string guid)
	{
		if (meshImports.ContainsKey(guid))
		{
			return meshImports[guid];
		}
		MeshImport mi = new MeshImport();
		var info = rmManager.GetFromGuid(guid);
		if (info == null)
		{
			mi.guid = guid;
		}
		else
		{
			mi.rawJson = LoadInternalRaw(guid);
			mi.guid = info.guid;
			var json = LoadInternalFile(guid);
			mi.config = UnityEngine.Config.ModelImporter.FromJson(json[0] as Dictionary<string, System.Object>);
		}
		meshImports[guid] = mi;
		return mi;
	}


	internal static Texture GetTexture(UnityEngine.Config.FileMap fms)
	{
		return GetTexture(fms.guid);
	}

	internal static Texture GetTexture(string guid)
	{
		if (textures.ContainsKey(guid))
		{
			return textures[guid];
		}
		Texture2D ti;
		var info = rmManager.GetFromGuid(guid);
		if (info == null)
		{
			ti = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
			ti.guid = guid;
		}
		else
		{
			ti = new Texture2D(info.p1, info.p2, TextureFormat.ARGB32, false);
			ti.rawJson = LoadInternalRaw(guid);
			var json = LoadInternalFile(guid);
			ti.guid = info.guid;
		}
		textures[guid] = ti;
		return ti;
	}

	// This must be the first calls.
	internal static void LoadScene(string scene)
	{
		// TODO: the scene file should be registered in the configs.
		DoSetPath();
		LoadMap();
		var json = LoadInternalFile (rmManager.GetSceneFromName (scene).guid);
		LoadSceneSetting(json);
		LoadGameObject(json, false);
	}

	internal static Dictionary<string, System.Object> LoadTagLayerSetting()
	{
		return Read("__tag_layer__.json");
	}

	static void LoadSceneSetting(List<System.Object> l)
	{
		foreach (Dictionary<string, System.Object> d in l)
		{
			var type = d["_type"].ToString();
			//var fileID = d["fileID"].ToString();
			if (type == "RenderSettings")
			{
				var renderSettings = UnityEngine.Config.RenderSettings.FromJson(d);
				DoLoadRenderSetting(renderSettings);
			}
		}
	}

	internal static GameObject LoadPrefab(string guid)
	{
		var json = LoadInternalFile(guid);
		if (!prefabs.ContainsKey(guid))
		{
			var go = LoadGameObject(json, true);
			UnityEngine.Transform.HackRemoveFromRenderTree(go.transform);
			go._load_guid = guid;
			prefabs.Add(guid, go);
		}
		return prefabs[guid];
	}

	internal static GameObject InstantiatePrefab(string guid)
	{
		var json = LoadInternalFile(guid);
		var go = LoadGameObject(json, true);
		go.name = go.name + "(Clone)";
		// hack skinnedmesh/animation here.
		var ani = go.GetComponentInChildren<Animation>(true);
		if (ani != null)
		{
			SkinnedMeshRenderer smr = null;
			var ani_go = ani.gameObject;
			for (int i = 0; i < ani_go.transform.childCount; i++)
			{
				var tr = ani_go.transform.GetChild(i);
				smr = tr.GetComponent<SkinnedMeshRenderer>();
				if (smr != null)
				{
					break;
				}
			}
			if (smr != null)
			{
				for (int i = 0; i < ani_go.transform.childCount; i++)
				{
					var tr = ani_go.transform.GetChild(i);
					var new_smr = tr.GetComponent<SkinnedMeshRenderer>();
					if (new_smr == null || new_smr != smr)
					{
						GameObject.DestroyImmediate(tr.gameObject);
					}
				}
			}
		}
		go.ClearPrefabFlag();
		go.SetSelfActive(go.m_IsActive);
		return go;
	}

	static GameObject LoadGameObject(List<System.Object> l, bool prefab)
	{
		SceneLoader loader = new SceneLoader(rmManager);
		loader.Load(l, prefab);
		return loader.FirstGameObject();
	}

	internal static ResourceMapManager rmManager;

	class FileData
	{
		public string guid;
		public string rawData;
		public List<System.Object> json;
	}
	static Dictionary<string, FileData> internal_files;
	// meta, prefab, scene.
	static List<System.Object> LoadInternalFile(string guid)
	{
		FileData d = internal_files[guid];
		if (d.json == null)
		{
			d.json = CyberyJson.Deserialize(d.rawData) as List<System.Object>;
			d.rawData = null;
		}
		return d.json;
	}

	static string LoadInternalRaw(string guid)
	{
		return internal_files[guid].rawData;
	}

	static void LoadMap()
	{
		rmManager = new ResourceMapManager(Read("__map__.json"));
		internal_files = new Dictionary<string, FileData >();
		var json_one = Read ("__json_one__.json");
		foreach (Dictionary<string, System.Object> o in json_one)
		{
			string guid = (string)o["guid"];
			string json = (string)o["json"];
			FileData d = new FileData();
			d.guid = guid;
			d.rawData = json;
			internal_files.Add(guid, d);
		}
	}

	// Helpers to Load all kinds of objects.
	static string root = "/Resources/";

	static void DoSetPath()
	{
		root = "../../../../Out/Resources/";
	}

	static dynamic Read(string path)
	{
		DoSetPath ();
		string jsontext = LoadText(path);
		return UnityEngine.CyberyJson.Deserialize(jsontext);
	}

	internal static extern string LoadText(string path);

	static Dictionary<string, System.Object> GetElement(List<System.Object> l, string type)
	{
		foreach (Dictionary<string, System.Object> o in l)
		{
			if ((string)(o["_type"]) == type)
			{
				return o;
			}
		}
		return null;
	}

	static extern void DoLoadRenderSetting(UnityEngine.Config.RenderSettings renderSettings);
}


}