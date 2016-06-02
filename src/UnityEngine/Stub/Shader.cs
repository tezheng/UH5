using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{
internal enum DisableBatchingType
{
	False,
	True,
	WhenLODFading
}

public class Shader : Object
{
	public static Shader Find (string name)
	{
		return null;
	}
	internal void _Init(UnityEngine.Config.FileMap fms)
	{
		this.fms = fms;
	}
	internal UnityEngine.Config.FileMap fms;
	/*
	CUSTOM static Shader Find (string name)
	{
		return Scripting::ScriptingWrapperFor(GetScriptMapper().FindShader(name));
	}
	CUSTOM internal static Shader FindBuiltin (string name)
	{
		return Scripting::ScriptingWrapperFor(GetBuiltinResource<Shader> (name));
	}

	AUTO_PROP bool isSupported IsSupported

	CONDITIONAL UNITY_EDITOR
	CUSTOM_PROP internal string customEditor { return scripting_string_new (self->GetCustomEditorName()); }

	CUSTOM static void EnableKeyword (string keyword)
	{
		ShaderLab::GetDefaultPassContext().keywords.Enable( keywords::Create( keyword.AsUTF8().c_str() ) );
	}
	
	CUSTOM static void DisableKeyword (string keyword)
	{
		ShaderLab::GetDefaultPassContext().keywords.Disable( keywords::Create( keyword.AsUTF8().c_str() ) );
	}

	AUTO_PROP int maximumLOD GetMaximumShaderLOD SetMaximumShaderLOD
	CUSTOM_PROP static int globalMaximumLOD { return Shader::GetGlobalMaximumShaderLOD(); } { Shader::SetGlobalMaximumShaderLOD (value); }

	CUSTOM_PROP int renderQueue { return self->GetShaderLabShader()->GetRenderQueue(); }

	CUSTOM_PROP internal DisableBatchingType disableBatching { return static_cast<int>(self->GetShaderLabShader()->GetDisableBatching()); }

	CSRAW public static void SetGlobalColor (string propertyName, Color color) {
		SetGlobalColor(Shader.PropertyToID(propertyName), color);
	}
	CUSTOM static void SetGlobalColor (int nameID, Color color) {
		ShaderLab::PropertySheet &props = ShaderLab::GetDefaultPassContext().properties;
		ShaderLab::FastPropertyName propName; propName.index = nameID;
		props.SetVector (propName, color.GetVector4f ());
	}
	CSRAW public static void SetGlobalVector (string propertyName, Vector4 vec) {
		SetGlobalColor (propertyName, vec);
	}
	CSRAW public static void SetGlobalVector (int nameID, Vector4 vec) {
		SetGlobalColor (nameID, vec);
	}

	CSRAW public static void SetGlobalFloat (string propertyName, float value) {
		SetGlobalFloat(Shader.PropertyToID(propertyName), value);
	}
	CUSTOM static void SetGlobalFloat (int nameID, float value) {
		ShaderLab::PropertySheet &props = ShaderLab::GetDefaultPassContext().properties;
		ShaderLab::FastPropertyName propName; propName.index = nameID;
		props.SetFloat (propName, value);
	}

	CSRAW public static void SetGlobalInt (string propertyName, int value) { SetGlobalFloat(propertyName, (float)value); }
	CSRAW public static void SetGlobalInt (int nameID, int value) { SetGlobalFloat(nameID, (float)value); }

	CSRAW public static void SetGlobalTexture (string propertyName, Texture tex) {
		SetGlobalTexture (Shader.PropertyToID(propertyName), tex);
	}
	CUSTOM static void SetGlobalTexture (int nameID, Texture tex) {
		ShaderLab::PropertySheet &props = ShaderLab::GetDefaultPassContext().properties;
		ShaderLab::FastPropertyName propName; propName.index = nameID;
		props.SetTexture (propName, tex);
	}

	CSRAW public static void SetGlobalMatrix (string propertyName, Matrix4x4 mat) {
		SetGlobalMatrix (Shader.PropertyToID(propertyName), mat);
	}
	CUSTOM static void SetGlobalMatrix (int nameID, Matrix4x4 mat) {
		ShaderLab::PropertySheet &props = ShaderLab::GetDefaultPassContext().properties;
		ShaderLab::FastPropertyName propName; propName.index = nameID;
		props.SetMatrix (propName, mat);
	}

	OBSOLETE error SetGlobalTexGenMode is not supported anymore. Use programmable shaders to achieve the same effect.
	CUSTOM static void SetGlobalTexGenMode (string propertyName, TexGenMode mode) { }

	OBSOLETE error SetGlobalTextureMatrixName is not supported anymore. Use programmable shaders to achieve the same effect.
	CUSTOM static void SetGlobalTextureMatrixName (string propertyName, string matrixName) { }

	CUSTOM static void SetGlobalBuffer (string propertyName, ComputeBuffer buffer) {
		ShaderLab::PropertySheet &props = ShaderLab::GetDefaultPassContext().properties;
		props.SetComputeBuffer (ScriptingStringToProperty (propertyName), buffer ? buffer->GetBufferHandle() : ComputeBufferID());
	}

	CUSTOM static int PropertyToID (string name) {
		return ScriptingStringToProperty (name).index;
	}

	CUSTOM static void WarmupAllShaders () {
		WarmupAllShaders ();
	}
	*/

}

/*

CLASS ShaderVariantCollection : Object

	STRUCT ShaderVariant

		public ShaderVariant (Shader shader, UnityEngine.Rendering.PassType passType, params string[] keywords)
		{
			this.shader = shader;
			this.passType = passType;
			this.keywords = keywords;
			Internal_CheckVariant (shader, passType, keywords);
		}

		CUSTOM private static void Internal_CheckVariant (Shader shader, UnityEngine.Rendering.PassType passType, string[] keywords)
		{
			// Can only really do this in the editor, where we have shader snippet keyword information
			#if UNITY_EDITOR
			std::string msg = CheckShaderVariant (shader, passType, keywords);
			if (!msg.empty())
			{
				Scripting::RaiseArgumentException(msg.c_str());
			}
			#endif // #if UNITY_EDITOR
		}

		CSRAW
		public Shader shader;
		public UnityEngine.Rendering.PassType passType;
		public string[] keywords;
	END

	CSRAW public ShaderVariantCollection ()
	{
		Internal_Create(this);
	}

	CUSTOM private static void Internal_Create ([Writable]ShaderVariantCollection mono)
	{
		ShaderVariantCollection* r = NEW_OBJECT_MAIN_THREAD (ShaderVariantCollection);
		r->Reset();
		Scripting::ConnectScriptingWrapperToObject (mono.GetScriptingObject(), r);
		r->AwakeFromLoad(kInstantiateOrCreateFromCodeAwakeFromLoad);
	}

	AUTO_PROP int shaderCount GetShaderCount
	AUTO_PROP int variantCount GetVariantCount
	
	// Functions below take ShaderVariant on C# side, and call FooInternal native functions
	// that just take separate arguments. This way no marshalling of ShaderVariant struct is needed
	// to the native side.

	CSRAW public bool Add (ShaderVariant variant)
	{
		return AddInternal (variant.shader, variant.passType, variant.keywords);
	}
	CUSTOM private bool AddInternal (Shader shader, UnityEngine.Rendering.PassType passType, string[] keywords)
	{
		return self->AddVariant (shader, passType, keywords);
	}

	CSRAW public bool Remove (ShaderVariant variant)
	{
		return RemoveInternal (variant.shader, variant.passType, variant.keywords);
	}
	CUSTOM private bool RemoveInternal (Shader shader, UnityEngine.Rendering.PassType passType, string[] keywords)
	{
		return self->RemoveVariant (shader, passType, keywords);
	}

	CSRAW public bool Contains (ShaderVariant variant)
	{
		return ContainsInternal (variant.shader, variant.passType, variant.keywords);
	}
	CUSTOM private bool ContainsInternal (Shader shader, UnityEngine.Rendering.PassType passType, string[] keywords)
	{
		return self->ContainsVariant (shader, passType, keywords);
	}

	CUSTOM void Clear ()
	{
		self->ClearVariants();
	}

	AUTO_PROP bool isWarmedUp IsWarmedUp

	CUSTOM void WarmUp () { self->WarmupShaders(); }

*/
}
