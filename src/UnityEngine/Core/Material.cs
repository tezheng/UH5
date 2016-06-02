using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
	public class Material : Object
	{
		internal string rawJson;
		internal string shaderGuid;
		internal string guid;

		public Color color;
		public Texture mainTexture;
		public int renderQueue;
		public Shader shader;
		public Vector2 mainTextureOffset = new Vector2(0, 0);
		public Vector2 mainTextureScale = new Vector2(1, 1);


		public Material (string contents) {}

		public Material (Shader shader) {}

		public Material (Material source) : base () {}

		internal Material() {}

		public void Init(UnityEngine.Config.Material mat)
		{
			shader = Resources.GetShader(mat.m_Shader);
			UnityEngine.Config.FileMap fms = mat.m_SavedProperties.m_TexEnvs[0].data.second.m_Texture;
			if (fms.IsEmpty())
			{
				mainTexture = null;
			}
			else
			{
				// Hack, need to read TextureImporter from fms.
				string guid = mat.m_SavedProperties.m_TexEnvs[0].data.second.m_Texture.guid;
				mainTexture = Resources.GetTexture(guid);
				mainTextureOffset.x = mat.m_SavedProperties.m_TexEnvs[0].data.second.m_Offset.x;
				mainTextureOffset.y = mat.m_SavedProperties.m_TexEnvs[0].data.second.m_Offset.y;
				mainTextureScale.x = mat.m_SavedProperties.m_TexEnvs[0].data.second.m_Scale.x;
				mainTextureScale.y = mat.m_SavedProperties.m_TexEnvs[0].data.second.m_Scale.y;
			}
		}

		public void CopyPropertiesFromMaterial (Material mat)
		{
			this.rawJson = mat.rawJson;
			this.shaderGuid = mat.shaderGuid;
			this.guid = mat.guid;
			this.mainTexture = mat.mainTexture;
			this.mainTextureOffset = mat.mainTextureOffset;
			this.mainTextureScale = mat.mainTextureScale;
			this.shader = mat.shader;
			this.renderQueue = mat.renderQueue;
			this.color = mat.color;
		}

		public void SetVector (string propertyName, Vector4 vector)
		{

		}

		public bool HasProperty (string propertyName)
		{
			return false;
		}

		public bool HasProperty (int nameID)
		{
			return false;
		}

		public void SetFloat (string propertyName, float value)
		{

		}
		public void SetFloat (int nameID, float value)
		{
			
		}

		public void SetColor (string propertyName, Color color)
		{

		}
		public void SetColor (int nameID, Color color)
		{
			
		}

		/*
		CSRAW public Material (string contents) { Internal_CreateWithString (this, contents); }

		CSRAW public Material (Shader shader) { Internal_CreateWithShader (this, shader); }

		CSRAW public Material (Material source) : base () { Internal_CreateWithMaterial (this, source); }

		AUTO_PTR_PROP Shader shader GetShader SetShader

		CSRAW public Color color { get { return GetColor ("_Color"); } set { SetColor ("_Color", value); } }
		CSRAW public Texture mainTexture { get { return GetTexture ("_MainTex"); } set { SetTexture ("_MainTex", value); } }
		CSRAW public Vector2 mainTextureOffset { get { return GetTextureOffset("_MainTex"); } set { SetTextureOffset("_MainTex", value); } }
		CSRAW public Vector2 mainTextureScale { get { return GetTextureScale("_MainTex"); } set { SetTextureScale("_MainTex", value); } }

		CSRAW public void SetColor (string propertyName, Color color)
		{
			SetColor (Shader.PropertyToID(propertyName), color);
		}
		CUSTOM void SetColor (int nameID, Color color)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			self->SetColor (propName, color);
		}
		CSRAW public Color GetColor (string propertyName)
		{
			return GetColor (Shader.PropertyToID(propertyName));
		}
		CUSTOM Color GetColor (int nameID)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			return self->GetColor (propName);
		}

		CSRAW public void SetVector (string propertyName, Vector4 vector)
		{
			SetColor (propertyName, new Color (vector.x, vector.y, vector.z, vector.w));
		}
		CSRAW public void SetVector (int nameID, Vector4 vector)
		{
			SetColor (nameID, new Color (vector.x, vector.y, vector.z, vector.w));
		}
		CSRAW public Vector4 GetVector (string propertyName)
		{
			Color temp = GetColor (propertyName);
			return new Vector4 (temp.r, temp.g, temp.b, temp.a);
		}
		CSRAW public Vector4 GetVector (int nameID)
		{
			Color temp = GetColor (nameID);
			return new Vector4 (temp.r, temp.g, temp.b, temp.a);
		}

		CSRAW public void SetTexture (string propertyName, Texture texture)
		{
			SetTexture (Shader.PropertyToID(propertyName), texture);
		}
		CUSTOM void SetTexture (int nameID, Texture texture)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			self->SetTexture (propName, texture);
		}
		CSRAW public Texture GetTexture (string propertyName)
		{
			return GetTexture (Shader.PropertyToID(propertyName));
		}
		CUSTOM Texture GetTexture (int nameID)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			return Scripting::ScriptingWrapperFor (self->GetTexture (propName));
		}


		// Workaround for gcc/msvc where passing small mono structures by value does not work
		CUSTOM private static void Internal_GetTextureOffset (Material mat, string name, out Vector2 output)
		{
			*output = mat->GetTextureOffset( ScriptingStringToProperty(name) );
		}
		CUSTOM private static void Internal_GetTextureScale (Material mat, string name, out Vector2 output)
		{
			*output = mat->GetTextureScale( ScriptingStringToProperty(name) );
		}

		CUSTOM void SetTextureOffset (string propertyName, Vector2 offset)
		{
			self->SetTextureOffset( ScriptingStringToProperty(propertyName), offset );
		}
		CSRAW public Vector2 GetTextureOffset (string propertyName)
		{
			Vector2 r;
			Internal_GetTextureOffset(this, propertyName, out r);
			return r;
		}

		CUSTOM void SetTextureScale (string propertyName, Vector2 scale)
		{
			self->SetTextureScale( ScriptingStringToProperty(propertyName), scale );
		}
		CSRAW public Vector2 GetTextureScale (string propertyName)
		{
			Vector2 r;
			Internal_GetTextureScale(this, propertyName, out r);
			return r;
		}

		CSRAW public void SetMatrix (string propertyName, Matrix4x4 matrix)
		{
			SetMatrix (Shader.PropertyToID(propertyName), matrix);
		}
		CUSTOM void SetMatrix (int nameID, Matrix4x4 matrix)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			self->SetMatrix (propName, matrix);
		}
		CSRAW public Matrix4x4 GetMatrix (string propertyName)
		{
			return GetMatrix (Shader.PropertyToID(propertyName));
		}
		CUSTOM Matrix4x4 GetMatrix (int nameID)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			return self->GetMatrix (propName);
		}

		CSRAW public void SetFloat (string propertyName, float value)
		{
			SetFloat (Shader.PropertyToID(propertyName), value);
		}
		CUSTOM void SetFloat (int nameID, float value)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			self->SetFloat (propName, value);
		}
		CSRAW public float GetFloat (string propertyName)
		{
			return GetFloat (Shader.PropertyToID(propertyName));
		}
		CUSTOM float GetFloat (int nameID)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			return self->GetFloat (propName);
		}

		CSRAW public void SetInt (string propertyName, int value) { SetFloat(propertyName, (float)value); }
		CSRAW public void SetInt (int nameID, int value) { SetFloat(nameID, (float)value); }
		CSRAW public int GetInt (string propertyName) { return (int)GetFloat(propertyName); }
		CSRAW public int GetInt (int nameID) { return (int)GetFloat(nameID); }

		CUSTOM void SetBuffer (string propertyName, ComputeBuffer buffer) {
			FastPropertyName fpName = ScriptingStringToProperty(propertyName);
			self->SetComputeBuffer (fpName, buffer ? buffer->GetBufferHandle() : ComputeBufferID());
		}

		CSRAW public bool HasProperty (string propertyName)
		{
			return HasProperty (Shader.PropertyToID(propertyName));
		}
		CUSTOM bool HasProperty (int nameID)
		{
			ShaderLab::FastPropertyName propName; propName.index = nameID;
			return self->HasProperty (propName);
		}

		CUSTOM string GetTag (string tag, bool searchFallbacks, string defaultValue = "") {
			return scripting_string_new (self->GetTag (tag, !searchFallbacks, defaultValue));
		}

		CUSTOM void Lerp (Material start, Material end, float t)
		{
			const ShaderLab::PropertySheet &s1 = start->GetProperties();
			const ShaderLab::PropertySheet &s2 = end->GetProperties();
			self->GetWritableProperties().LerpProperties( s1, s2, clamp01(t) );
		}

		AUTO_PROP int passCount GetPassCount


		CUSTOM bool SetPass (int pass) {
			Material& mat = *self;
			if (pass < 0 || pass >= mat.GetPassCount())
			{
				ErrorStringMsg("Trying to access pass %d, but material '%s' subshader (0) has only %d valid passes.",
					pass,
					mat.GetName(),
					mat.GetPassCount());
				return false;
			}

			if (!CheckShouldRenderPass (pass, mat))
				return false;
			s_ScriptingCurrentShader = mat.GetShaderPPtr();
			s_ScriptingCurrentChannels = mat.SetPass(pass, ShaderLab::GetDefaultPassContext());
			return s_ScriptingCurrentChannels != NULL;
		}

		AUTO_PROP int renderQueue GetActualRenderQueue SetCustomRenderQueue

		OBSOLETE warning Use the Material constructor instead.
		CSRAW static public Material Create (string scriptContents)
		{
			return new Material (scriptContents);
		}

		CUSTOM private static void Internal_CreateWithString ([Writable]Material mono, string contents)
		{
			Material *mat = Material::CreateMaterial (contents.AsUTF8().c_str(), 0, true);
			Scripting::ConnectScriptingWrapperToObject (mono.GetScriptingObject(), mat);
			mat->ApplyMaterialPropertyDrawers();
		}

		CUSTOM private static void Internal_CreateWithShader ([Writable]Material mono, Shader shader)
		{
			Material *mat = Material::CreateMaterial (*shader, 0, true);
			Scripting::ConnectScriptingWrapperToObject (mono.GetScriptingObject(), mat);
			mat->ApplyMaterialPropertyDrawers();
		}

		CUSTOM private static void Internal_CreateWithMaterial ([Writable]Material mono, Material source)
		{
			Material *mat = Material::CreateMaterial (*source, 0, true);
			Scripting::ConnectScriptingWrapperToObject (mono.GetScriptingObject(), mat);
			mat->ApplyMaterialPropertyDrawers();
		}

		CUSTOM void CopyPropertiesFromMaterial (Material mat)
		{
			if (mat != NULL)
				self->CopyPropertiesFromMaterial(*mat);
			else
				ErrorString ("Trying to copy properties from null material.");
		}

		CUSTOM void EnableKeyword (string keyword)
		{
			self->EnableKeyword (keyword);
		}
		
		CUSTOM void DisableKeyword (string keyword)
		{
			self->DisableKeyword (keyword);
		}

		CUSTOM bool IsKeywordEnabled (string keyword)
		{
			return self->IsKeywordEnabled (keyword);
		}


		CONDITIONAL ENABLE_MONO || UNITY_WINRT || ENABLE_IL2CPP
		CUSTOM_PROP String[] shaderKeywords
		{
			std::vector<std::string> keywords;
			self->GetShaderKeywordNames (keywords);
			return StringVectorToScripting (keywords);
		}
		{
			std::vector<std::string> names;
			StringScriptingToVector (value, names);
			self->SetShaderKeywordNames (names);
		}
	*/
	}
}
