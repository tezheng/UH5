using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngineInternal;
using UnityEngine.Config;

namespace UnityEngine
{
	internal class SceneLoader
	{
		List<GameObject> all = new List<GameObject>();
		internal ResourceMapManager rmm;
		internal SceneLoader(ResourceMapManager rmm)
		{
			this.rmm = rmm;
		}

		internal object GetComponentByFileID(string id)
		{
			return fid2Object.ContainsKey(id) ? fid2Object[id] : null;
		}

		Dictionary<string, GameObject> tl = new Dictionary<string, GameObject>();
		Dictionary<Transform, string> transform_helper = new Dictionary<Transform, string>();
		Dictionary<string, Transform> transform_helper2 = new Dictionary<string, Transform>();
		Dictionary<string, object> fid2Object = new Dictionary<string, object>();
		internal void Load(List<System.Object> l, bool prefab)
		{
			bool need_prefab = prefab;
			prefab = true;
			Dictionary<string, object> fid2json = new Dictionary<string, object>();
			foreach (Dictionary<string, System.Object> d in l)
			{
				var type = d["_type"].ToString();
				var fileID = d["fileID"].ToString();
				if (type == "GameObject")
				{
					var o = UnityEngine.Config.GameObject.FromJson(d);
					GameObject go = new GameObject(o.m_Name, prefab);
					go.SetSelfActive(o.m_IsActive == 1);
					go.m_IsStaticInScene = (o.m_StaticEditorFlags == 4294967295);
					go.layer = o.m_Layer;
					go.hideFlags = (HideFlags)o.m_ObjectHideFlags;
					tl[fileID] = go;
					fid2Object[fileID] = go;
				}
				else if (type == "Transform")
				{
					var o = UnityEngine.Config.Transform.FromJson(d);
					var transform = tl[o.m_GameObject.fileID].AddComponent<Transform>();
					transform_helper[transform] = o.m_Father.fileID;
					transform_helper2[o.fileID] = transform;
					if (transform.gameObject.GetComponent<SkinnedMeshRenderer>() != null)
					{
						transform.__Reset();
					}
					else
					{
						transform.localPosition = new Vector3(o.m_LocalPosition.x, o.m_LocalPosition.y, o.m_LocalPosition.z);
						transform.localRotation = new Quaternion(o.m_LocalRotation.x, o.m_LocalRotation.y, o.m_LocalRotation.z, o.m_LocalRotation.w);
						transform.localScale = new Vector3(o.m_LocalScale.x, o.m_LocalScale.y, o.m_LocalScale.z);
					}
					fid2Object[fileID] = transform;
				}
				else if (type == "MonoBehaviour")
				{
					var o = UnityEngine.Config.MonoBehaviour.FromJson(d);
					var filemap = rmm.GetFromInfo(o.m_Script);
					if (filemap == null)
					{
						Debug.LogError("Missing behaviour in map. guid = " + o.guid);
						continue;
					}
					var comp = tl[o.m_GameObject.fileID].AddComponent(TypeUtil.GetTypeFromString(filemap.name), d);
					if (comp == null)
					{
						Debug.LogError("Missing behaviour in type. guid = " + o.guid);
						continue;
					}
					MonoBehaviour mb = comp as MonoBehaviour;
					mb.SetEnabled(o.m_Enabled == 1);
					fid2Object[fileID] = mb;
					fid2json[fileID] = d;
				}
				else if (type == "Camera")
				{
					var o = UnityEngine.Config.Camera.FromJson(d);
					var camera = tl[o.m_GameObject.fileID].AddComponent<Camera>();
					camera.fieldOfView = o.field_of_view;
					camera.nearClipPlane = o.near_clip_plane;
					camera.farClipPlane = o.far_clip_plane;
					camera.orthographicSize = o.orthographic_size;
					camera.orthographic = o.orthographic == 1;
					camera.SetEnabled(o.m_Enabled == 1);
					camera.cullingMask = (int)o.m_CullingMask.m_Bits;
					camera.depth = o.m_Depth;
					camera.clearFlags = (CameraClearFlags)o.m_ClearFlags;
					camera.backgroundColor = new Color(o.m_BackGroundColor.r, o.m_BackGroundColor.g,
						o.m_BackGroundColor.b, o.m_BackGroundColor.a);
					fid2Object[fileID] = camera;
				}
				else if (type == "MeshFilter")
				{
					var o = UnityEngine.Config.MeshFilter.FromJson(d);
					var meshFilter = tl[o.m_GameObject.fileID].AddComponent<MeshFilter>();
					meshFilter.SetMesh(o.m_Mesh);
					fid2Object[fileID] = meshFilter;
				}
				else if (type == "Renderer")
				{
					var o = UnityEngine.Config.Renderer.FromJson(d);
					var renderer = tl[o.m_GameObject.fileID].AddComponent(typeof(MeshRenderer), o);
					fid2Object[fileID] = renderer;
				}
				else if (type == "SkinnedMeshRenderer")
				{
					var o = UnityEngine.Config.SkinnedMeshRenderer.FromJson(d);
					var skinnedMeshRenderer = tl[o.m_GameObject.fileID].AddComponent(typeof(SkinnedMeshRenderer), o);
					fid2Object[fileID] = skinnedMeshRenderer;
				}
				else if (type == "Animation")
				{
					var o = UnityEngine.Config.Animation.FromJson(d);
					var anim = tl[o.m_GameObject.fileID].AddComponent(typeof(Animation), o);
					fid2Object[fileID] = anim;
				}
				else if (type == "Light")
				{
					var o = UnityEngine.Config.Light.FromJson(d);
					var light = tl[o.m_GameObject.fileID].AddComponent(typeof(Light), o);
					fid2Object[fileID] = light;
				}
				else if (type == "BoxCollider")
				{
					var o = UnityEngine.Config.BoxCollider.FromJson(d);
					var boxCollider = tl[o.m_GameObject.fileID].AddComponent(typeof(BoxCollider), o);
					fid2Object[fileID] = boxCollider;
				}
				else if (type == "AudioSource")
				{
					var o = UnityEngine.Config.AudioSource.FromJson(d);
					var boxCollider = tl[o.m_GameObject.fileID].AddComponent(typeof(AudioSource), o);
					fid2Object[fileID] = boxCollider;
				}
			}
			foreach (var kvp in tl)
			{
				var transform = kvp.Value.transform;
				string father = transform_helper[transform];
				if (father == "0")
				{
					// root.
					all.Add(kvp.Value);
				}
				else
				{
					var fatherTr = transform_helper2[father];
					transform.SetParent(fatherTr, false);
				}
			}
			foreach (var item in fid2json)
			{
				MonoBehaviour mb = fid2Object[item.Key] as MonoBehaviour;
				initMonoBehaviour(mb, item.Value);
			}
			foreach (var kvp in tl)
			{
				if (!need_prefab) {
					kvp.Value.ClearPrefabFlag();
				}
				kvp.Value.SetSelfActive(kvp.Value.m_IsActive);
			}
		}

		void initMonoBehaviour(MonoBehaviour co, object json)
		{
			if (json != null)
			{
				InitObjectFromJson(co, json, co.GetType());
			}
		}

		private static FieldInfo GetFieldRecrusive(Type type, string key)
		{
			while (type != null)
			{
				FieldInfo field = type.GetField(key, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (field != null)
				{
					return field;
				}
				type = type.BaseType;
			}
			return null;
		}

		private void InitObjectFromJson(object co, object json, Type type)
		{
			Dictionary<string, object> values = json as Dictionary<string, object>;
			foreach (var item in values)
			{
				if (item.Value == null) 
				{
					FieldInfo tfield = GetFieldRecrusive(type, item.Key);
					if (tfield != null && tfield.FieldType.Name == "String") {
						tfield.SetValue(co, "");
					}
					continue;
				}
				FieldInfo field = GetFieldRecrusive(type, item.Key);
				if (field == null)
				{
					continue;
				}

				if (field.FieldType.Name == "Boolean") {
					field.SetValue(co, Logic.Convert.ToInt32(item.Value) == 1);
				}
				else if (field.FieldType.Name == "Single") {
					field.SetValue(co, Logic.Convert.ToSingle(item.Value));
				}
				else if (field.FieldType.Name == "Single[]") {
					List<object> valStr = (List<object>) item.Value;
					float[] val = new float [valStr.Count];
					for (int i = val.Length - 1; i >= 0; --i) {
						val[i] = Logic.Convert.ToSingle(valStr[i].ToString());
					}
					field.SetValue(co, val);
				}
				else if (field.FieldType.Name == "Int32") {
					field.SetValue(co, Logic.Convert.ToInt32(item.Value));
				}
				else if (field.FieldType.Name == "Int32[]") {
					string arr = item.Value == null ? null : item.Value.ToString();
					if (arr == null) {
						field.SetValue(co, null);
					}
					else if (!IsDigitOnly(arr) && arr.Length % 8 == 0) {
						int count = arr.Length / 8;
						int[] val = new int [count];
						for (int i = 0, s = 0; i < count; ++i, s += 8) {
							val[i] = ParseHexToInt(arr, s, s + 8);
						}
						field.SetValue(co, val);
					} else {
						Console.WriteLine("Int32[] is broken.");
					}
				}
				else if (field.FieldType.Name == "Byte") {
					field.SetValue(co, (byte) Logic.Convert.ToInt32(item.Value));
				}
				else if (field.FieldType.Name == "Byte[]") {
					string arr = item.Value == null ? null : item.Value.ToString();
					if (arr == null) {
						field.SetValue(co, null);
					}
					else if (!IsDigitOnly(arr) && arr.Length % 2 == 0) {
						int count = arr.Length / 2;
						byte[] val = new byte [count];
						for (int i = 0, s = 0; i < count; ++i, s += 2) {
							val[i] = (byte) ParseHexToInt(arr, s, s + 2);
						}
						field.SetValue(co, val);
					} else {
						Console.WriteLine("Int32[] is broken.");
					}
				}
				else if (field.FieldType.Name == "String") {
					field.SetValue(co, item.Value == null ? "" : item.Value.ToString());
				}
				else if (field.FieldType.Name == "String[]") {
					List<object> valStr = (List<object>) item.Value;
					string[] val = new string [valStr.Count];
					for (int i = val.Length - 1; i >= 0; --i) {
						val[i] = valStr[i].ToString();
					}
					field.SetValue(co, val);
				}
				else if (field.FieldType.IsEnum) {
					if (item.Value != null) {
						var e = EnumUtils.ParseEnum(field.FieldType, item.Value.ToString());
						field.SetValue(co, e);
					}
				}
				else if (field.FieldType.IsArray) {
					List<object> v = item.Value as List<object>;
					for (int i = v.Count - 1; i >= 0; --i) {
						v[i] = CreateObjectByJsonAndType(v[i], field.FieldType.GetElementType());
					}
					field.SetValue(co, v.ToArray());
				}
				else if (field.FieldType.IsGenericType &&
					field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
				{
					IList list = (IList) field.FieldType.GetConstructor(new Type[0]).Invoke();
					List<object> v = item.Value as List<object>;
					for (int i = 0; i < v.Count; ++i) {
						list.Add(CreateObjectByJsonAndType(v[i],
							field.FieldType.GetGenericArguments()[0]));
					}
					field.SetValue(co, list);
				}
				else {
					field.SetValue(co, CreateObjectByJsonAndType(item.Value, field.FieldType));
				}
			}
		}

		private static bool IsDigitOnly(string s) {
			for (int i = 0; i < s.Length; ++i)
			{
				if (!Char.IsDigit(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static extern int ParseHexToInt(string str, int s, int e);

		private object CreateObjectByJsonAndType(object json, Type type)
		{
			// special case for field name not match.
			if (type.Name == "LayerMask")
			{
				var mask = Logic.Convert.ToInt32((json as Dictionary<string, object>)["m_Bits"]);
				var lm = (LayerMask) mask;
				return lm;
			}
			else
			{
				Dictionary<string, object> v = json as Dictionary<string, object>;
				if (v == null) {
					return null;
				}
				if (v.ContainsKey("guid") && v["guid"] != null) {
					string guid = v["guid"].ToString();
					ResourceMap map = rmm.GetFromGuid(guid);
					if (map == null) {
						return null;
					}
					if (map.ext == "prefab") {
						var prefab = Resources.LoadPrefab(guid);
						var c = prefab.GetComponent(type);
						if (c == null) return prefab;
						else return c;
					}
					else if (map.ext == "mat") {
						return Resources.GetMaterial (guid);
					}
					else if (map.ext == "png" || map.ext == "jpg" || map.ext == "jpeg") {
						return Resources.GetTexture (guid);
					}
					else if (map.ext == "shader") {
						return Resources.GetShader (FileMap.FromJson(v));
					}
				}
				else if (v.ContainsKey("fileID") && v["fileID"] != null) {
					return GetComponentByFileID(v["fileID"].ToString());
				}
				else {
					var obj = Activator.CreateInstance (type);
					// Following way fails in Duocode lib.
					// var target = type.Assembly.CreateInstance(type.FullName);
					if (obj != null) {
						InitObjectFromJson(obj, json, type);
					}
					return obj;
				}
			}
			return null;
		}

		internal GameObject FirstGameObject()
		{
			foreach (var go in all)
			{
				if (go.hideFlags == 0)
				{
					return go;
				}
			}
			return null;
		}
	}
}
