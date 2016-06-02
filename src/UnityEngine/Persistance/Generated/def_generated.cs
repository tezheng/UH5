// This file is generated, please do not modify it.
using System;
using System.Collections.Generic;




namespace UnityEngine
{
namespace Config
{
	
	public class AABB
	{
		public Vec3 m_Center;
		public Vec3 m_Extent;
		public AABB()
		{
		}

		public void AABB_Set(Vec3 m_Center, Vec3 m_Extent)
		{
			this.m_Center = m_Center;
			this.m_Extent = m_Extent;
		}

		public static AABB FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static AABB FromJson(Dictionary<string, System.Object> json)
		{
			var t = new AABB();
			t.AABB_SetFromJson(json);
			return t;
		}

		public void AABB_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("m_Center")) m_Center = Vec3.FromJson(json["m_Center"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Extent")) m_Extent = Vec3.FromJson(json["m_Extent"] as Dictionary<string, System.Object>);
		}
	}

	
	public class Animation : ComponentBase
	{
		public FileMap m_Animation;
		public FileMap[] m_Animations;
		public int m_WrapMode;
		public int m_PlayAutomatically;
		public int m_AnimatePhysics;
		public int m_CullingType;
		public Animation()
		{
		}

		public void Animation_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, FileMap m_Animation, FileMap[] m_Animations, int m_WrapMode, int m_PlayAutomatically, int m_AnimatePhysics, int m_CullingType)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_Animation = m_Animation;
			this.m_Animations = m_Animations;
			this.m_WrapMode = m_WrapMode;
			this.m_PlayAutomatically = m_PlayAutomatically;
			this.m_AnimatePhysics = m_AnimatePhysics;
			this.m_CullingType = m_CullingType;
		}

		public static new Animation FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Animation FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Animation();
			t.Animation_SetFromJson(json);
			return t;
		}

		public void Animation_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_Animation")) m_Animation = FileMap.FromJson(json["m_Animation"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Animations"))
			{
				var t = new List<FileMap>();
				foreach (var o in json["m_Animations"] as List<System.Object>)
				{
					t.Add(FileMap.FromJson(o as Dictionary<string, System.Object>));
				}
				m_Animations = t.ToArray();
			}
			if (json.ContainsKey("m_WrapMode")) m_WrapMode = Logic.Convert.ToInt32(json["m_WrapMode"]);
			if (json.ContainsKey("m_PlayAutomatically")) m_PlayAutomatically = Logic.Convert.ToInt32(json["m_PlayAutomatically"]);
			if (json.ContainsKey("m_AnimatePhysics")) m_AnimatePhysics = Logic.Convert.ToInt32(json["m_AnimatePhysics"]);
			if (json.ContainsKey("m_CullingType")) m_CullingType = Logic.Convert.ToInt32(json["m_CullingType"]);
		}
	}

	
	public class AudioImporter : ObjectBase
	{
		public int format;
		public float quality;
		public int stream;
		public int forceToMono;
		public int loopable;
		public AudioImporter()
		{
		}

		public void AudioImporter_Set(string guid, string _type, string fileID, int format, float quality, int stream, int forceToMono, int loopable)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.format = format;
			this.quality = quality;
			this.stream = stream;
			this.forceToMono = forceToMono;
			this.loopable = loopable;
		}

		public static new AudioImporter FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new AudioImporter FromJson(Dictionary<string, System.Object> json)
		{
			var t = new AudioImporter();
			t.AudioImporter_SetFromJson(json);
			return t;
		}

		public void AudioImporter_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("format")) format = Logic.Convert.ToInt32(json["format"]);
			if (json.ContainsKey("quality")) quality = (float)Logic.Convert.ToDouble(json["quality"]);
			if (json.ContainsKey("stream")) stream = Logic.Convert.ToInt32(json["stream"]);
			if (json.ContainsKey("forceToMono")) forceToMono = Logic.Convert.ToInt32(json["forceToMono"]);
			if (json.ContainsKey("loopable")) loopable = Logic.Convert.ToInt32(json["loopable"]);
		}
	}

	
	public class AudioListener : ComponentBase
	{
		public AudioListener()
		{
		}

		public void AudioListener_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
		}

		public static new AudioListener FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new AudioListener FromJson(Dictionary<string, System.Object> json)
		{
			var t = new AudioListener();
			t.AudioListener_SetFromJson(json);
			return t;
		}

		public void AudioListener_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
		}
	}

	
	public class AudioSource : ComponentBase
	{
		public FileMap m_audioClip;
		public int m_PlayOnAwake;
		public int Loop;
		public float m_Volume;
		public AudioSource()
		{
		}

		public void AudioSource_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, FileMap m_audioClip, int m_PlayOnAwake, int Loop, float m_Volume)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_audioClip = m_audioClip;
			this.m_PlayOnAwake = m_PlayOnAwake;
			this.Loop = Loop;
			this.m_Volume = m_Volume;
		}

		public static new AudioSource FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new AudioSource FromJson(Dictionary<string, System.Object> json)
		{
			var t = new AudioSource();
			t.AudioSource_SetFromJson(json);
			return t;
		}

		public void AudioSource_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_audioClip")) m_audioClip = FileMap.FromJson(json["m_audioClip"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_PlayOnAwake")) m_PlayOnAwake = Logic.Convert.ToInt32(json["m_PlayOnAwake"]);
			if (json.ContainsKey("Loop")) Loop = Logic.Convert.ToInt32(json["Loop"]);
			if (json.ContainsKey("m_Volume")) m_Volume = (float)Logic.Convert.ToDouble(json["m_Volume"]);
		}
	}

	
	public class Behaviour : ComponentBase
	{
		public Behaviour()
		{
		}

		public void Behaviour_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
		}

		public static new Behaviour FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Behaviour FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Behaviour();
			t.Behaviour_SetFromJson(json);
			return t;
		}

		public void Behaviour_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
		}
	}

	
	public class BoxCollider : Collider
	{
		public Vec3 m_Size;
		public Vec3 m_Center;
		public BoxCollider()
		{
		}

		public void BoxCollider_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_IsTrigger, Vec3 m_Size, Vec3 m_Center)
		{
			Collider_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled, m_IsTrigger);
			this.m_Size = m_Size;
			this.m_Center = m_Center;
		}

		public static new BoxCollider FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new BoxCollider FromJson(Dictionary<string, System.Object> json)
		{
			var t = new BoxCollider();
			t.BoxCollider_SetFromJson(json);
			return t;
		}

		public void BoxCollider_SetFromJson(Dictionary<string, System.Object> json)
		{
			Collider_SetFromJson(json);
			if (json.ContainsKey("m_Size")) m_Size = Vec3.FromJson(json["m_Size"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Center")) m_Center = Vec3.FromJson(json["m_Center"] as Dictionary<string, System.Object>);
		}
	}

	
	public class Camera : ComponentBase
	{
		public int m_ClearFlags;
		public Color m_BackGroundColor;
		public Rect m_NormalizedViewPortRect;
		public float near_clip_plane;
		public float far_clip_plane;
		public float field_of_view;
		public int orthographic;
		public float orthographic_size;
		public float m_Depth;
		public CullingMask m_CullingMask;
		public int m_RenderingPath;
		public FileMap m_TargetTexture;
		public int m_TargetDisplay;
		public int m_HDR;
		public int m_OcclusionCulling;
		public int m_StereoConvergence;
		public float m_StereoSeparation;
		public Camera()
		{
		}

		public void Camera_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_ClearFlags, Color m_BackGroundColor, Rect m_NormalizedViewPortRect, float near_clip_plane, float far_clip_plane, float field_of_view, int orthographic, float orthographic_size, float m_Depth, CullingMask m_CullingMask, int m_RenderingPath, FileMap m_TargetTexture, int m_TargetDisplay, int m_HDR, int m_OcclusionCulling, int m_StereoConvergence, float m_StereoSeparation)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_ClearFlags = m_ClearFlags;
			this.m_BackGroundColor = m_BackGroundColor;
			this.m_NormalizedViewPortRect = m_NormalizedViewPortRect;
			this.near_clip_plane = near_clip_plane;
			this.far_clip_plane = far_clip_plane;
			this.field_of_view = field_of_view;
			this.orthographic = orthographic;
			this.orthographic_size = orthographic_size;
			this.m_Depth = m_Depth;
			this.m_CullingMask = m_CullingMask;
			this.m_RenderingPath = m_RenderingPath;
			this.m_TargetTexture = m_TargetTexture;
			this.m_TargetDisplay = m_TargetDisplay;
			this.m_HDR = m_HDR;
			this.m_OcclusionCulling = m_OcclusionCulling;
			this.m_StereoConvergence = m_StereoConvergence;
			this.m_StereoSeparation = m_StereoSeparation;
		}

		public static new Camera FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Camera FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Camera();
			t.Camera_SetFromJson(json);
			return t;
		}

		public void Camera_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_ClearFlags")) m_ClearFlags = Logic.Convert.ToInt32(json["m_ClearFlags"]);
			if (json.ContainsKey("m_BackGroundColor")) m_BackGroundColor = Color.FromJson(json["m_BackGroundColor"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_NormalizedViewPortRect")) m_NormalizedViewPortRect = Rect.FromJson(json["m_NormalizedViewPortRect"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("near_clip_plane")) near_clip_plane = (float)Logic.Convert.ToDouble(json["near_clip_plane"]);
			if (json.ContainsKey("far_clip_plane")) far_clip_plane = (float)Logic.Convert.ToDouble(json["far_clip_plane"]);
			if (json.ContainsKey("field_of_view")) field_of_view = (float)Logic.Convert.ToDouble(json["field_of_view"]);
			if (json.ContainsKey("orthographic")) orthographic = Logic.Convert.ToInt32(json["orthographic"]);
			if (json.ContainsKey("orthographic_size")) orthographic_size = (float)Logic.Convert.ToDouble(json["orthographic_size"]);
			if (json.ContainsKey("m_Depth")) m_Depth = (float)Logic.Convert.ToDouble(json["m_Depth"]);
			if (json.ContainsKey("m_CullingMask")) m_CullingMask = CullingMask.FromJson(json["m_CullingMask"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_RenderingPath")) m_RenderingPath = Logic.Convert.ToInt32(json["m_RenderingPath"]);
			if (json.ContainsKey("m_TargetTexture")) m_TargetTexture = FileMap.FromJson(json["m_TargetTexture"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_TargetDisplay")) m_TargetDisplay = Logic.Convert.ToInt32(json["m_TargetDisplay"]);
			if (json.ContainsKey("m_HDR")) m_HDR = Logic.Convert.ToInt32(json["m_HDR"]);
			if (json.ContainsKey("m_OcclusionCulling")) m_OcclusionCulling = Logic.Convert.ToInt32(json["m_OcclusionCulling"]);
			if (json.ContainsKey("m_StereoConvergence")) m_StereoConvergence = Logic.Convert.ToInt32(json["m_StereoConvergence"]);
			if (json.ContainsKey("m_StereoSeparation")) m_StereoSeparation = (float)Logic.Convert.ToDouble(json["m_StereoSeparation"]);
		}
	}

	
	public class Collider : ComponentBase
	{
		public int m_IsTrigger;
		public Collider()
		{
		}

		public void Collider_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_IsTrigger)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_IsTrigger = m_IsTrigger;
		}

		public static new Collider FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Collider FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Collider();
			t.Collider_SetFromJson(json);
			return t;
		}

		public void Collider_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_IsTrigger")) m_IsTrigger = Logic.Convert.ToInt32(json["m_IsTrigger"]);
		}
	}

	
	public class Color
	{
		public float r;
		public float g;
		public float b;
		public float a;
		public Color()
		{
		}

		public void Color_Set(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public static Color FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static Color FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Color();
			t.Color_SetFromJson(json);
			return t;
		}

		public void Color_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("r")) r = (float)Logic.Convert.ToDouble(json["r"]);
			if (json.ContainsKey("g")) g = (float)Logic.Convert.ToDouble(json["g"]);
			if (json.ContainsKey("b")) b = (float)Logic.Convert.ToDouble(json["b"]);
			if (json.ContainsKey("a")) a = (float)Logic.Convert.ToDouble(json["a"]);
		}
	}

	
	public class ComponentBase : ObjectBase
	{
		public int m_ObjectHideFlags;
		public FileMap m_GameObject;
		public int m_Enabled;
		public ComponentBase()
		{
		}

		public void ComponentBase_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_ObjectHideFlags = m_ObjectHideFlags;
			this.m_GameObject = m_GameObject;
			this.m_Enabled = m_Enabled;
		}

		public static new ComponentBase FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new ComponentBase FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ComponentBase();
			t.ComponentBase_SetFromJson(json);
			return t;
		}

		public void ComponentBase_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
			if (json.ContainsKey("m_GameObject")) m_GameObject = FileMap.FromJson(json["m_GameObject"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Enabled")) m_Enabled = Logic.Convert.ToInt32(json["m_Enabled"]);
		}
	}

	
	public class CullingMask
	{
		public int serializedVersion;
		public long m_Bits;
		public CullingMask()
		{
		}

		public void CullingMask_Set(int serializedVersion, long m_Bits)
		{
			this.serializedVersion = serializedVersion;
			this.m_Bits = m_Bits;
		}

		public static CullingMask FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static CullingMask FromJson(Dictionary<string, System.Object> json)
		{
			var t = new CullingMask();
			t.CullingMask_SetFromJson(json);
			return t;
		}

		public void CullingMask_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("serializedVersion")) serializedVersion = Logic.Convert.ToInt32(json["serializedVersion"]);
			if (json.ContainsKey("m_Bits")) m_Bits = Logic.Convert.ToInt64(json["m_Bits"]);
		}
	}

	
	public class GameObject : ObjectBase
	{
		public int m_ObjectHideFlags;
		public FileMap[] m_Component;
		public int m_Layer;
		public string m_Name;
		public string m_TagString;
		public FileMap m_Icon;
		public int m_NavMeshLayer;
		public int m_IsActive;
		public long m_StaticEditorFlags;
		public GameObject()
		{
		}

		public void GameObject_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap[] m_Component, int m_Layer, string m_Name, string m_TagString, FileMap m_Icon, int m_NavMeshLayer, int m_IsActive, long m_StaticEditorFlags)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_ObjectHideFlags = m_ObjectHideFlags;
			this.m_Component = m_Component;
			this.m_Layer = m_Layer;
			this.m_Name = m_Name;
			this.m_TagString = m_TagString;
			this.m_Icon = m_Icon;
			this.m_NavMeshLayer = m_NavMeshLayer;
			this.m_IsActive = m_IsActive;
			this.m_StaticEditorFlags = m_StaticEditorFlags;
		}

		public static new GameObject FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new GameObject FromJson(Dictionary<string, System.Object> json)
		{
			var t = new GameObject();
			t.GameObject_SetFromJson(json);
			return t;
		}

		public void GameObject_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
			if (json.ContainsKey("m_Component"))
			{
				var t = new List<FileMap>();
				foreach (var o in json["m_Component"] as List<System.Object>)
				{
					t.Add(FileMap.FromJson(o as Dictionary<string, System.Object>));
				}
				m_Component = t.ToArray();
			}
			if (json.ContainsKey("m_Layer")) m_Layer = Logic.Convert.ToInt32(json["m_Layer"]);
			if (json.ContainsKey("m_Name")) m_Name = json["m_Name"].ToString();
			if (json.ContainsKey("m_TagString")) m_TagString = json["m_TagString"].ToString();
			if (json.ContainsKey("m_Icon")) m_Icon = FileMap.FromJson(json["m_Icon"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_NavMeshLayer")) m_NavMeshLayer = Logic.Convert.ToInt32(json["m_NavMeshLayer"]);
			if (json.ContainsKey("m_IsActive")) m_IsActive = Logic.Convert.ToInt32(json["m_IsActive"]);
			if (json.ContainsKey("m_StaticEditorFlags")) m_StaticEditorFlags = Logic.Convert.ToInt64(json["m_StaticEditorFlags"]);
		}
	}

	
	public class LevelGameManager : ObjectBase
	{
		public int m_ObjectHideFlags;
		public LevelGameManager()
		{
		}

		public void LevelGameManager_Set(string guid, string _type, string fileID, int m_ObjectHideFlags)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_ObjectHideFlags = m_ObjectHideFlags;
		}

		public static new LevelGameManager FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new LevelGameManager FromJson(Dictionary<string, System.Object> json)
		{
			var t = new LevelGameManager();
			t.LevelGameManager_SetFromJson(json);
			return t;
		}

		public void LevelGameManager_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
		}
	}

	
	public class Light : ComponentBase
	{
		public int m_Type;
		public Color m_Color;
		public float m_Intensity;
		public float m_Range;
		public float m_SpotAngle;
		public int m_RenderMode;
		public CullingMask m_CullingMask;
		public int m_Lightmapping;
		public Light()
		{
		}

		public void Light_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_Type, Color m_Color, float m_Intensity, float m_Range, float m_SpotAngle, int m_RenderMode, CullingMask m_CullingMask, int m_Lightmapping)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_Type = m_Type;
			this.m_Color = m_Color;
			this.m_Intensity = m_Intensity;
			this.m_Range = m_Range;
			this.m_SpotAngle = m_SpotAngle;
			this.m_RenderMode = m_RenderMode;
			this.m_CullingMask = m_CullingMask;
			this.m_Lightmapping = m_Lightmapping;
		}

		public static new Light FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Light FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Light();
			t.Light_SetFromJson(json);
			return t;
		}

		public void Light_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_Type")) m_Type = Logic.Convert.ToInt32(json["m_Type"]);
			if (json.ContainsKey("m_Color")) m_Color = Color.FromJson(json["m_Color"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Intensity")) m_Intensity = (float)Logic.Convert.ToDouble(json["m_Intensity"]);
			if (json.ContainsKey("m_Range")) m_Range = (float)Logic.Convert.ToDouble(json["m_Range"]);
			if (json.ContainsKey("m_SpotAngle")) m_SpotAngle = (float)Logic.Convert.ToDouble(json["m_SpotAngle"]);
			if (json.ContainsKey("m_RenderMode")) m_RenderMode = Logic.Convert.ToInt32(json["m_RenderMode"]);
			if (json.ContainsKey("m_CullingMask")) m_CullingMask = CullingMask.FromJson(json["m_CullingMask"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Lightmapping")) m_Lightmapping = Logic.Convert.ToInt32(json["m_Lightmapping"]);
		}
	}

	
	public class LightmapSettings : ObjectBase
	{
		public int m_ObjectHideFlags;
		public LightmapSettings()
		{
		}

		public void LightmapSettings_Set(string guid, string _type, string fileID, int m_ObjectHideFlags)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_ObjectHideFlags = m_ObjectHideFlags;
		}

		public static new LightmapSettings FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new LightmapSettings FromJson(Dictionary<string, System.Object> json)
		{
			var t = new LightmapSettings();
			t.LightmapSettings_SetFromJson(json);
			return t;
		}

		public void LightmapSettings_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
		}
	}

	
	public class Material : ObjectBase
	{
		public string m_Name;
		public FileMap m_Shader;
		public MaterialProperties m_SavedProperties;
		public Material()
		{
		}

		public void Material_Set(string guid, string _type, string fileID, string m_Name, FileMap m_Shader, MaterialProperties m_SavedProperties)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_Name = m_Name;
			this.m_Shader = m_Shader;
			this.m_SavedProperties = m_SavedProperties;
		}

		public static new Material FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Material FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Material();
			t.Material_SetFromJson(json);
			return t;
		}

		public void Material_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_Name")) m_Name = json["m_Name"].ToString();
			if (json.ContainsKey("m_Shader")) m_Shader = FileMap.FromJson(json["m_Shader"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_SavedProperties")) m_SavedProperties = MaterialProperties.FromJson(json["m_SavedProperties"] as Dictionary<string, System.Object>);
		}
	}

	
	public class MaterialDataName
	{
		public string name;
		public MaterialDataName()
		{
		}

		public void MaterialDataName_Set(string name)
		{
			this.name = name;
		}

		public static MaterialDataName FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static MaterialDataName FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MaterialDataName();
			t.MaterialDataName_SetFromJson(json);
			return t;
		}

		public void MaterialDataName_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("name")) name = json["name"].ToString();
		}
	}

	
	public class MaterialProperties
	{
		public MaterialTexture[] m_TexEnvs;
		public MaterialProperties()
		{
		}

		public void MaterialProperties_Set(MaterialTexture[] m_TexEnvs)
		{
			this.m_TexEnvs = m_TexEnvs;
		}

		public static MaterialProperties FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static MaterialProperties FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MaterialProperties();
			t.MaterialProperties_SetFromJson(json);
			return t;
		}

		public void MaterialProperties_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("m_TexEnvs"))
			{
				var t = new List<MaterialTexture>();
				foreach (var o in json["m_TexEnvs"] as List<System.Object>)
				{
					t.Add(MaterialTexture.FromJson(o as Dictionary<string, System.Object>));
				}
				m_TexEnvs = t.ToArray();
			}
		}
	}

	
	public class MaterialTexture
	{
		public MaterialTextureData data;
		public MaterialTexture()
		{
		}

		public void MaterialTexture_Set(MaterialTextureData data)
		{
			this.data = data;
		}

		public static MaterialTexture FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static MaterialTexture FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MaterialTexture();
			t.MaterialTexture_SetFromJson(json);
			return t;
		}

		public void MaterialTexture_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("data")) data = MaterialTextureData.FromJson(json["data"] as Dictionary<string, System.Object>);
		}
	}

	
	public class MaterialTextureData
	{
		public MaterialTextureDataValue second;
		public MaterialDataName first;
		public MaterialTextureData()
		{
		}

		public void MaterialTextureData_Set(MaterialTextureDataValue second, MaterialDataName first)
		{
			this.second = second;
			this.first = first;
		}

		public static MaterialTextureData FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static MaterialTextureData FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MaterialTextureData();
			t.MaterialTextureData_SetFromJson(json);
			return t;
		}

		public void MaterialTextureData_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("second")) second = MaterialTextureDataValue.FromJson(json["second"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("first")) first = MaterialDataName.FromJson(json["first"] as Dictionary<string, System.Object>);
		}
	}

	
	public class MaterialTextureDataValue
	{
		public FileMap m_Texture;
		public Vec2 m_Scale;
		public Vec2 m_Offset;
		public MaterialTextureDataValue()
		{
		}

		public void MaterialTextureDataValue_Set(FileMap m_Texture, Vec2 m_Scale, Vec2 m_Offset)
		{
			this.m_Texture = m_Texture;
			this.m_Scale = m_Scale;
			this.m_Offset = m_Offset;
		}

		public static MaterialTextureDataValue FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static MaterialTextureDataValue FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MaterialTextureDataValue();
			t.MaterialTextureDataValue_SetFromJson(json);
			return t;
		}

		public void MaterialTextureDataValue_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("m_Texture")) m_Texture = FileMap.FromJson(json["m_Texture"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Scale")) m_Scale = Vec2.FromJson(json["m_Scale"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Offset")) m_Offset = Vec2.FromJson(json["m_Offset"] as Dictionary<string, System.Object>);
		}
	}

	
	public class MeshFilter : ComponentBase
	{
		public FileMap m_Mesh;
		public MeshFilter()
		{
		}

		public void MeshFilter_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, FileMap m_Mesh)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_Mesh = m_Mesh;
		}

		public static new MeshFilter FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new MeshFilter FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MeshFilter();
			t.MeshFilter_SetFromJson(json);
			return t;
		}

		public void MeshFilter_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_Mesh")) m_Mesh = FileMap.FromJson(json["m_Mesh"] as Dictionary<string, System.Object>);
		}
	}

	
	public class ModelImporter
	{
		public int importAnimation;
		public ModelImporterMeshes meshes;
		public int animationType;
		public ModelImporterAnimations animations;
		public ImporterDictionaryStringString fileIDToRecycleName;
		public ModelImporter()
		{
		}

		public void ModelImporter_Set(int importAnimation, ModelImporterMeshes meshes, int animationType, ModelImporterAnimations animations, ImporterDictionaryStringString fileIDToRecycleName)
		{
			this.importAnimation = importAnimation;
			this.meshes = meshes;
			this.animationType = animationType;
			this.animations = animations;
			this.fileIDToRecycleName = fileIDToRecycleName;
		}

		public static ModelImporter FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static ModelImporter FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ModelImporter();
			t.ModelImporter_SetFromJson(json);
			return t;
		}

		public void ModelImporter_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("importAnimation")) importAnimation = Logic.Convert.ToInt32(json["importAnimation"]);
			if (json.ContainsKey("meshes")) meshes = ModelImporterMeshes.FromJson(json["meshes"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("animationType")) animationType = Logic.Convert.ToInt32(json["animationType"]);
			if (json.ContainsKey("animations")) animations = ModelImporterAnimations.FromJson(json["animations"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("fileIDToRecycleName")) fileIDToRecycleName = ImporterDictionaryStringString.FromJson(json["fileIDToRecycleName"] as Dictionary<string, System.Object>);
		}
	}

	
	public class ModelImporterAnimationClip
	{
		public string takeName;
		public int firstFrame;
		public int lastFrame;
		public string name;
		public int wrapMode;
		public ModelImporterAnimationClip()
		{
		}

		public void ModelImporterAnimationClip_Set(string takeName, int firstFrame, int lastFrame, string name, int wrapMode)
		{
			this.takeName = takeName;
			this.firstFrame = firstFrame;
			this.lastFrame = lastFrame;
			this.name = name;
			this.wrapMode = wrapMode;
		}

		public static ModelImporterAnimationClip FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static ModelImporterAnimationClip FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ModelImporterAnimationClip();
			t.ModelImporterAnimationClip_SetFromJson(json);
			return t;
		}

		public void ModelImporterAnimationClip_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("takeName")) takeName = json["takeName"].ToString();
			if (json.ContainsKey("firstFrame")) firstFrame = Logic.Convert.ToInt32(json["firstFrame"]);
			if (json.ContainsKey("lastFrame")) lastFrame = Logic.Convert.ToInt32(json["lastFrame"]);
			if (json.ContainsKey("name")) name = json["name"].ToString();
			if (json.ContainsKey("wrapMode")) wrapMode = Logic.Convert.ToInt32(json["wrapMode"]);
		}
	}

	
	public class ModelImporterAnimations
	{
		public int animationWrapMode;
		public ModelImporterAnimationClip[] clipAnimations;
		public ModelImporterAnimations()
		{
		}

		public void ModelImporterAnimations_Set(int animationWrapMode, ModelImporterAnimationClip[] clipAnimations)
		{
			this.animationWrapMode = animationWrapMode;
			this.clipAnimations = clipAnimations;
		}

		public static ModelImporterAnimations FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static ModelImporterAnimations FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ModelImporterAnimations();
			t.ModelImporterAnimations_SetFromJson(json);
			return t;
		}

		public void ModelImporterAnimations_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("animationWrapMode")) animationWrapMode = Logic.Convert.ToInt32(json["animationWrapMode"]);
			if (json.ContainsKey("clipAnimations"))
			{
				var t = new List<ModelImporterAnimationClip>();
				foreach (var o in json["clipAnimations"] as List<System.Object>)
				{
					t.Add(ModelImporterAnimationClip.FromJson(o as Dictionary<string, System.Object>));
				}
				clipAnimations = t.ToArray();
			}
		}
	}

	
	public class ModelImporterMeshes
	{
		public float globalScale;
		public int swapUVChannels;
		public ModelImporterMeshes()
		{
		}

		public void ModelImporterMeshes_Set(float globalScale, int swapUVChannels)
		{
			this.globalScale = globalScale;
			this.swapUVChannels = swapUVChannels;
		}

		public static ModelImporterMeshes FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static ModelImporterMeshes FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ModelImporterMeshes();
			t.ModelImporterMeshes_SetFromJson(json);
			return t;
		}

		public void ModelImporterMeshes_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("globalScale")) globalScale = (float)Logic.Convert.ToDouble(json["globalScale"]);
			if (json.ContainsKey("swapUVChannels")) swapUVChannels = Logic.Convert.ToInt32(json["swapUVChannels"]);
		}
	}

	
	public class MonoBehaviour : ComponentBase
	{
		public int m_EditorHideFlags;
		public FileMap m_Script;
		public MonoBehaviour()
		{
		}

		public void MonoBehaviour_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_EditorHideFlags, FileMap m_Script)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_EditorHideFlags = m_EditorHideFlags;
			this.m_Script = m_Script;
		}

		public static new MonoBehaviour FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new MonoBehaviour FromJson(Dictionary<string, System.Object> json)
		{
			var t = new MonoBehaviour();
			t.MonoBehaviour_SetFromJson(json);
			return t;
		}

		public void MonoBehaviour_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_EditorHideFlags")) m_EditorHideFlags = Logic.Convert.ToInt32(json["m_EditorHideFlags"]);
			if (json.ContainsKey("m_Script")) m_Script = FileMap.FromJson(json["m_Script"] as Dictionary<string, System.Object>);
		}
	}

	
	public class NavMeshSettings : ObjectBase
	{
		public int m_ObjectHideFlags;
		public NavMeshSettings()
		{
		}

		public void NavMeshSettings_Set(string guid, string _type, string fileID, int m_ObjectHideFlags)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_ObjectHideFlags = m_ObjectHideFlags;
		}

		public static new NavMeshSettings FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new NavMeshSettings FromJson(Dictionary<string, System.Object> json)
		{
			var t = new NavMeshSettings();
			t.NavMeshSettings_SetFromJson(json);
			return t;
		}

		public void NavMeshSettings_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
		}
	}

	
	public class ObjectBase
	{
		public string guid;
		public string _type;
		public string fileID;
		public ObjectBase()
		{
		}

		public void ObjectBase_Set(string guid, string _type, string fileID)
		{
			this.guid = guid;
			this._type = _type;
			this.fileID = fileID;
		}

		public static ObjectBase FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static ObjectBase FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ObjectBase();
			t.ObjectBase_SetFromJson(json);
			return t;
		}

		public void ObjectBase_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("guid")) guid = json["guid"].ToString();
			if (json.ContainsKey("_type")) _type = json["_type"].ToString();
			if (json.ContainsKey("fileID")) fileID = json["fileID"].ToString();
		}
	}

	
	public class OcclusionBakeSettings : ObjectBase
	{
		public float smallestOccluder;
		public float smallestHole;
		public float backfaceThreshold;
		public OcclusionBakeSettings()
		{
		}

		public void OcclusionBakeSettings_Set(string guid, string _type, string fileID, float smallestOccluder, float smallestHole, float backfaceThreshold)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.smallestOccluder = smallestOccluder;
			this.smallestHole = smallestHole;
			this.backfaceThreshold = backfaceThreshold;
		}

		public static new OcclusionBakeSettings FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new OcclusionBakeSettings FromJson(Dictionary<string, System.Object> json)
		{
			var t = new OcclusionBakeSettings();
			t.OcclusionBakeSettings_SetFromJson(json);
			return t;
		}

		public void OcclusionBakeSettings_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("smallestOccluder")) smallestOccluder = (float)Logic.Convert.ToDouble(json["smallestOccluder"]);
			if (json.ContainsKey("smallestHole")) smallestHole = (float)Logic.Convert.ToDouble(json["smallestHole"]);
			if (json.ContainsKey("backfaceThreshold")) backfaceThreshold = (float)Logic.Convert.ToDouble(json["backfaceThreshold"]);
		}
	}

	
	public class PairStringString
	{
		public string first;
		public string second;
		public PairStringString()
		{
		}

		public void PairStringString_Set(string first, string second)
		{
			this.first = first;
			this.second = second;
		}

		public static PairStringString FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static PairStringString FromJson(Dictionary<string, System.Object> json)
		{
			var t = new PairStringString();
			t.PairStringString_SetFromJson(json);
			return t;
		}

		public void PairStringString_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("first")) first = json["first"].ToString();
			if (json.ContainsKey("second")) second = json["second"].ToString();
		}
	}

	
	public class Rect
	{
		public float x;
		public float y;
		public float width;
		public float height;
		public Rect()
		{
		}

		public void Rect_Set(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public static Rect FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static Rect FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Rect();
			t.Rect_SetFromJson(json);
			return t;
		}

		public void Rect_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("x")) x = (float)Logic.Convert.ToDouble(json["x"]);
			if (json.ContainsKey("y")) y = (float)Logic.Convert.ToDouble(json["y"]);
			if (json.ContainsKey("width")) width = (float)Logic.Convert.ToDouble(json["width"]);
			if (json.ContainsKey("height")) height = (float)Logic.Convert.ToDouble(json["height"]);
		}
	}

	
	public class RenderSettings : ObjectBase
	{
		public float m_Fog;
		public Color m_FogColor;
		public int m_FogMode;
		public float m_FogDensity;
		public float m_LinearFogStart;
		public float m_LinearFogEnd;
		public Color m_AmbientLight;
		public FileMap m_SkyboxMaterial;
		public float m_HaloStrength;
		public float m_FlareStrength;
		public float m_FlareFadeSpeed;
		public FileMap m_HaloTexture;
		public FileMap m_SpotCookie;
		public int m_ObjectHideFlags;
		public RenderSettings()
		{
		}

		public void RenderSettings_Set(string guid, string _type, string fileID, float m_Fog, Color m_FogColor, int m_FogMode, float m_FogDensity, float m_LinearFogStart, float m_LinearFogEnd, Color m_AmbientLight, FileMap m_SkyboxMaterial, float m_HaloStrength, float m_FlareStrength, float m_FlareFadeSpeed, FileMap m_HaloTexture, FileMap m_SpotCookie, int m_ObjectHideFlags)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_Fog = m_Fog;
			this.m_FogColor = m_FogColor;
			this.m_FogMode = m_FogMode;
			this.m_FogDensity = m_FogDensity;
			this.m_LinearFogStart = m_LinearFogStart;
			this.m_LinearFogEnd = m_LinearFogEnd;
			this.m_AmbientLight = m_AmbientLight;
			this.m_SkyboxMaterial = m_SkyboxMaterial;
			this.m_HaloStrength = m_HaloStrength;
			this.m_FlareStrength = m_FlareStrength;
			this.m_FlareFadeSpeed = m_FlareFadeSpeed;
			this.m_HaloTexture = m_HaloTexture;
			this.m_SpotCookie = m_SpotCookie;
			this.m_ObjectHideFlags = m_ObjectHideFlags;
		}

		public static new RenderSettings FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new RenderSettings FromJson(Dictionary<string, System.Object> json)
		{
			var t = new RenderSettings();
			t.RenderSettings_SetFromJson(json);
			return t;
		}

		public void RenderSettings_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_Fog")) m_Fog = (float)Logic.Convert.ToDouble(json["m_Fog"]);
			if (json.ContainsKey("m_FogColor")) m_FogColor = Color.FromJson(json["m_FogColor"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_FogMode")) m_FogMode = Logic.Convert.ToInt32(json["m_FogMode"]);
			if (json.ContainsKey("m_FogDensity")) m_FogDensity = (float)Logic.Convert.ToDouble(json["m_FogDensity"]);
			if (json.ContainsKey("m_LinearFogStart")) m_LinearFogStart = (float)Logic.Convert.ToDouble(json["m_LinearFogStart"]);
			if (json.ContainsKey("m_LinearFogEnd")) m_LinearFogEnd = (float)Logic.Convert.ToDouble(json["m_LinearFogEnd"]);
			if (json.ContainsKey("m_AmbientLight")) m_AmbientLight = Color.FromJson(json["m_AmbientLight"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_SkyboxMaterial")) m_SkyboxMaterial = FileMap.FromJson(json["m_SkyboxMaterial"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_HaloStrength")) m_HaloStrength = (float)Logic.Convert.ToDouble(json["m_HaloStrength"]);
			if (json.ContainsKey("m_FlareStrength")) m_FlareStrength = (float)Logic.Convert.ToDouble(json["m_FlareStrength"]);
			if (json.ContainsKey("m_FlareFadeSpeed")) m_FlareFadeSpeed = (float)Logic.Convert.ToDouble(json["m_FlareFadeSpeed"]);
			if (json.ContainsKey("m_HaloTexture")) m_HaloTexture = FileMap.FromJson(json["m_HaloTexture"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_SpotCookie")) m_SpotCookie = FileMap.FromJson(json["m_SpotCookie"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
		}
	}

	
	public class Renderer : ComponentBase
	{
		public int m_CastShadows;
		public int m_ReceiveShadows;
		public int m_LightmapIndex;
		public Vec4 m_LightmapTilingOffset;
		public FileMap[] m_Materials;
		public float m_ScaleInLightmap;
		public int m_SortingLayerID;
		public int m_SortingOrder;
		public Renderer()
		{
		}

		public void Renderer_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_CastShadows, int m_ReceiveShadows, int m_LightmapIndex, Vec4 m_LightmapTilingOffset, FileMap[] m_Materials, float m_ScaleInLightmap, int m_SortingLayerID, int m_SortingOrder)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_CastShadows = m_CastShadows;
			this.m_ReceiveShadows = m_ReceiveShadows;
			this.m_LightmapIndex = m_LightmapIndex;
			this.m_LightmapTilingOffset = m_LightmapTilingOffset;
			this.m_Materials = m_Materials;
			this.m_ScaleInLightmap = m_ScaleInLightmap;
			this.m_SortingLayerID = m_SortingLayerID;
			this.m_SortingOrder = m_SortingOrder;
		}

		public static new Renderer FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Renderer FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Renderer();
			t.Renderer_SetFromJson(json);
			return t;
		}

		public void Renderer_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_CastShadows")) m_CastShadows = Logic.Convert.ToInt32(json["m_CastShadows"]);
			if (json.ContainsKey("m_ReceiveShadows")) m_ReceiveShadows = Logic.Convert.ToInt32(json["m_ReceiveShadows"]);
			if (json.ContainsKey("m_LightmapIndex")) m_LightmapIndex = Logic.Convert.ToInt32(json["m_LightmapIndex"]);
			if (json.ContainsKey("m_LightmapTilingOffset")) m_LightmapTilingOffset = Vec4.FromJson(json["m_LightmapTilingOffset"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Materials"))
			{
				var t = new List<FileMap>();
				foreach (var o in json["m_Materials"] as List<System.Object>)
				{
					t.Add(FileMap.FromJson(o as Dictionary<string, System.Object>));
				}
				m_Materials = t.ToArray();
			}
			if (json.ContainsKey("m_ScaleInLightmap")) m_ScaleInLightmap = (float)Logic.Convert.ToDouble(json["m_ScaleInLightmap"]);
			if (json.ContainsKey("m_SortingLayerID")) m_SortingLayerID = Logic.Convert.ToInt32(json["m_SortingLayerID"]);
			if (json.ContainsKey("m_SortingOrder")) m_SortingOrder = Logic.Convert.ToInt32(json["m_SortingOrder"]);
		}
	}

	
	public class ResourceMap
	{
		public string guid;
		public string path;
		public string type;
		public string name;
		public string ext;
		public int isr;
		public string rpath;
		public string apath;
		public string p0;
		public int p1;
		public int p2;
		public ResourceMap()
		{
		}

		public void ResourceMap_Set(string guid, string path, string type, string name, string ext, int isr, string rpath, string apath, string p0, int p1, int p2)
		{
			this.guid = guid;
			this.path = path;
			this.type = type;
			this.name = name;
			this.ext = ext;
			this.isr = isr;
			this.rpath = rpath;
			this.apath = apath;
			this.p0 = p0;
			this.p1 = p1;
			this.p2 = p2;
		}

		public static ResourceMap FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static ResourceMap FromJson(Dictionary<string, System.Object> json)
		{
			var t = new ResourceMap();
			t.ResourceMap_SetFromJson(json);
			return t;
		}

		public void ResourceMap_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("guid")) guid = json["guid"].ToString();
			if (json.ContainsKey("path")) path = json["path"].ToString();
			if (json.ContainsKey("type")) type = json["type"].ToString();
			if (json.ContainsKey("name")) name = json["name"].ToString();
			if (json.ContainsKey("ext")) ext = json["ext"].ToString();
			if (json.ContainsKey("isr")) isr = Logic.Convert.ToInt32(json["isr"]);
			if (json.ContainsKey("rpath")) rpath = json["rpath"].ToString();
			if (json.ContainsKey("apath")) apath = json["apath"].ToString();
			if (json.ContainsKey("p0")) p0 = json["p0"].ToString();
			if (json.ContainsKey("p1")) p1 = Logic.Convert.ToInt32(json["p1"]);
			if (json.ContainsKey("p2")) p2 = Logic.Convert.ToInt32(json["p2"]);
		}
	}

	
	public class SceneSettings : ObjectBase
	{
		public int m_ObjectHideFlags;
		public OcclusionBakeSettings m_OcclusionBakeSettings;
		public SceneSettings()
		{
		}

		public void SceneSettings_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, OcclusionBakeSettings m_OcclusionBakeSettings)
		{
			ObjectBase_Set(guid, _type, fileID);
			this.m_ObjectHideFlags = m_ObjectHideFlags;
			this.m_OcclusionBakeSettings = m_OcclusionBakeSettings;
		}

		public static new SceneSettings FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new SceneSettings FromJson(Dictionary<string, System.Object> json)
		{
			var t = new SceneSettings();
			t.SceneSettings_SetFromJson(json);
			return t;
		}

		public void SceneSettings_SetFromJson(Dictionary<string, System.Object> json)
		{
			ObjectBase_SetFromJson(json);
			if (json.ContainsKey("m_ObjectHideFlags")) m_ObjectHideFlags = Logic.Convert.ToInt32(json["m_ObjectHideFlags"]);
			if (json.ContainsKey("m_OcclusionBakeSettings")) m_OcclusionBakeSettings = OcclusionBakeSettings.FromJson(json["m_OcclusionBakeSettings"] as Dictionary<string, System.Object>);
		}
	}

	
	public class SkinnedMeshRenderer : Renderer
	{
		public int m_Quality;
		public int m_UpdateWhenOffscreen;
		public FileMap m_Mesh;
		public FileMap[] m_Bones;
		public FileMap m_RootBone;
		public AABB m_AABB;
		public SkinnedMeshRenderer()
		{
		}

		public void SkinnedMeshRenderer_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, int m_CastShadows, int m_ReceiveShadows, int m_LightmapIndex, Vec4 m_LightmapTilingOffset, FileMap[] m_Materials, float m_ScaleInLightmap, int m_SortingLayerID, int m_SortingOrder, int m_Quality, int m_UpdateWhenOffscreen, FileMap m_Mesh, FileMap[] m_Bones, FileMap m_RootBone, AABB m_AABB)
		{
			Renderer_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled, m_CastShadows, m_ReceiveShadows, m_LightmapIndex, m_LightmapTilingOffset, m_Materials, m_ScaleInLightmap, m_SortingLayerID, m_SortingOrder);
			this.m_Quality = m_Quality;
			this.m_UpdateWhenOffscreen = m_UpdateWhenOffscreen;
			this.m_Mesh = m_Mesh;
			this.m_Bones = m_Bones;
			this.m_RootBone = m_RootBone;
			this.m_AABB = m_AABB;
		}

		public static new SkinnedMeshRenderer FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new SkinnedMeshRenderer FromJson(Dictionary<string, System.Object> json)
		{
			var t = new SkinnedMeshRenderer();
			t.SkinnedMeshRenderer_SetFromJson(json);
			return t;
		}

		public void SkinnedMeshRenderer_SetFromJson(Dictionary<string, System.Object> json)
		{
			Renderer_SetFromJson(json);
			if (json.ContainsKey("m_Quality")) m_Quality = Logic.Convert.ToInt32(json["m_Quality"]);
			if (json.ContainsKey("m_UpdateWhenOffscreen")) m_UpdateWhenOffscreen = Logic.Convert.ToInt32(json["m_UpdateWhenOffscreen"]);
			if (json.ContainsKey("m_Mesh")) m_Mesh = FileMap.FromJson(json["m_Mesh"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Bones"))
			{
				var t = new List<FileMap>();
				foreach (var o in json["m_Bones"] as List<System.Object>)
				{
					t.Add(FileMap.FromJson(o as Dictionary<string, System.Object>));
				}
				m_Bones = t.ToArray();
			}
			if (json.ContainsKey("m_RootBone")) m_RootBone = FileMap.FromJson(json["m_RootBone"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_AABB")) m_AABB = AABB.FromJson(json["m_AABB"] as Dictionary<string, System.Object>);
		}
	}

	
	public class Transform : ComponentBase
	{
		public Vec4 m_LocalRotation;
		public Vec3 m_LocalPosition;
		public Vec3 m_LocalScale;
		public FileMap[] m_Children;
		public FileMap m_Father;
		public int m_RootOrder;
		public Transform()
		{
		}

		public void Transform_Set(string guid, string _type, string fileID, int m_ObjectHideFlags, FileMap m_GameObject, int m_Enabled, Vec4 m_LocalRotation, Vec3 m_LocalPosition, Vec3 m_LocalScale, FileMap[] m_Children, FileMap m_Father, int m_RootOrder)
		{
			ComponentBase_Set(guid, _type, fileID, m_ObjectHideFlags, m_GameObject, m_Enabled);
			this.m_LocalRotation = m_LocalRotation;
			this.m_LocalPosition = m_LocalPosition;
			this.m_LocalScale = m_LocalScale;
			this.m_Children = m_Children;
			this.m_Father = m_Father;
			this.m_RootOrder = m_RootOrder;
		}

		public static new Transform FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static new Transform FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Transform();
			t.Transform_SetFromJson(json);
			return t;
		}

		public void Transform_SetFromJson(Dictionary<string, System.Object> json)
		{
			ComponentBase_SetFromJson(json);
			if (json.ContainsKey("m_LocalRotation")) m_LocalRotation = Vec4.FromJson(json["m_LocalRotation"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_LocalPosition")) m_LocalPosition = Vec3.FromJson(json["m_LocalPosition"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_LocalScale")) m_LocalScale = Vec3.FromJson(json["m_LocalScale"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_Children"))
			{
				var t = new List<FileMap>();
				foreach (var o in json["m_Children"] as List<System.Object>)
				{
					t.Add(FileMap.FromJson(o as Dictionary<string, System.Object>));
				}
				m_Children = t.ToArray();
			}
			if (json.ContainsKey("m_Father")) m_Father = FileMap.FromJson(json["m_Father"] as Dictionary<string, System.Object>);
			if (json.ContainsKey("m_RootOrder")) m_RootOrder = Logic.Convert.ToInt32(json["m_RootOrder"]);
		}
	}

	
	public class Vec2
	{
		public float x;
		public float y;
		public Vec2()
		{
		}

		public void Vec2_Set(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public static Vec2 FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static Vec2 FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Vec2();
			t.Vec2_SetFromJson(json);
			return t;
		}

		public void Vec2_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("x")) x = (float)Logic.Convert.ToDouble(json["x"]);
			if (json.ContainsKey("y")) y = (float)Logic.Convert.ToDouble(json["y"]);
		}
	}

	
	public class Vec3
	{
		public float x;
		public float y;
		public float z;
		public Vec3()
		{
		}

		public void Vec3_Set(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vec3 FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static Vec3 FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Vec3();
			t.Vec3_SetFromJson(json);
			return t;
		}

		public void Vec3_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("x")) x = (float)Logic.Convert.ToDouble(json["x"]);
			if (json.ContainsKey("y")) y = (float)Logic.Convert.ToDouble(json["y"]);
			if (json.ContainsKey("z")) z = (float)Logic.Convert.ToDouble(json["z"]);
		}
	}

	
	public class Vec4
	{
		public float x;
		public float y;
		public float z;
		public float w;
		public Vec4()
		{
		}

		public void Vec4_Set(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public static Vec4 FromJson(string json)
		{
			var dict = UnityEngine.CyberyJson.Deserialize(json) as Dictionary<string,System.Object>;
			return FromJson(dict);
		}

		public static Vec4 FromJson(Dictionary<string, System.Object> json)
		{
			var t = new Vec4();
			t.Vec4_SetFromJson(json);
			return t;
		}

		public void Vec4_SetFromJson(Dictionary<string, System.Object> json)
		{
			if (json.ContainsKey("x")) x = (float)Logic.Convert.ToDouble(json["x"]);
			if (json.ContainsKey("y")) y = (float)Logic.Convert.ToDouble(json["y"]);
			if (json.ContainsKey("z")) z = (float)Logic.Convert.ToDouble(json["z"]);
			if (json.ContainsKey("w")) w = (float)Logic.Convert.ToDouble(json["w"]);
		}
	}

}
}
