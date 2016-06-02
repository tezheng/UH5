using System;

namespace UnityEngine
{
	public partial class Renderer : Component
	{
		public bool enabled
		{
			get {
				return GetEnabled();
			}
			set {
				SetEnabled(value);
			}
		}

		internal Renderer ()
		{
		}

		internal Renderer (System.Object o)
		{
			var r = o as UnityEngine.Config.Renderer;
			var mat = Resources.GetMaterial(r.m_Materials[0]);
			renderer_raw = r;
			this.material = mat;
			m_Enabled = r.m_Enabled == 1;
		}

		internal UnityEngine.Config.Renderer renderer_raw;
		protected bool inScene;
		protected bool m_Enabled = true;

		bool hackDirty = true;
		MeshFilter meshFilter;

		internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
		{
			base.AwakeFromLoad(awakeMode);
			UpdateManagerState(IsActive());
		}

		public bool ShouldBeInScene()
		{
			return m_Enabled && IsActive();
		}

		internal void SetEnabled(bool v)
		{
			m_Enabled = v;
			bool shouldBeInScene = ShouldBeInScene();
			if (shouldBeInScene == inScene) return;
			UpdateManagerState(shouldBeInScene);
		}

		internal bool GetEnabled()
		{
			return m_Enabled;
		}

		protected virtual void UpdateManagerState( bool needsUpdate )
		{
			if( needsUpdate )
			{
				if (!inScene)
				{
					Application.GetRenderManager().AddRenderer(this);
					inScene = true;
					hackDirty = true;
				}
			}
			else
			{
				if (inScene)
				{
					Application.GetRenderManager().RemoveRenderer(this);
					inScene = false;
					hackDirty = true;
				}
			}
		}

		internal override void Deactivate()
		{
			base.Deactivate();
			UpdateManagerState(false);
			var filter = GetComponent<MeshFilter>();
			DoUpdateRenderer(this, filter);
		}

		internal virtual void UpdateRenderer ()
		{
			if (gameObject.m_IsStaticInScene) {
				return;
			}

			//UpdateManagerState(ShouldBeInScene());
			if (meshFilter == null)
				meshFilter = GetComponent<MeshFilter>();

            if (!hackDirty && (meshFilter.mesh == null || !meshFilter.mesh.dirty)) {
				if (meshFilter.mesh != null && !meshFilter.mesh.dirty)
				{
					// for NGUI render order
					DoUpdateRenderOrder(this, meshFilter);
				}
				return;
			}

			hackDirty = false;
			if (meshFilter.IsValid())
			{
				DoUpdateRenderer(this, meshFilter);
			}
		}

		// sharedMaterial is same with material.
		Material _material;
		public Material material
		{
			get
			{
				return _material;
			}
			set
			{
				//material = value;
			}
		}

		public Material sharedMaterial
		{
			get
			{
				return _material;
			}
			set
			{
				//material = value;
			}
		}

		public int sortingOrder;

		public Material[] sharedMaterials;

		/*
		CUSTOM_PROP internal Transform staticBatchRootTransform { return Scripting::ScriptingWrapperFor (self->GetStaticBatchRoot ()); } { self->SetStaticBatchRoot (value); }

		CUSTOM_PROP internal int staticBatchIndex { return self->GetStaticBatchIndex(); }

		CUSTOM internal void SetSubsetIndex (int index, int subSetIndexForMaterial)
		{
			self->SetMaterialCount (std::max(index+1, self->GetMaterialCount()));
			self->SetSubsetIndex(index, subSetIndexForMaterial);
		}

		// Has this renderer been statically batched with any other renderers?
		CUSTOM_PROP bool isPartOfStaticBatch { return self->GetStaticBatchIndex() != 0; }

		// Matrix that transforms a point from world space into local space (RO).
		AUTO_PROP Matrix4x4 worldToLocalMatrix GetWorldToLocalMatrix
		// Matrix that transforms a point from local space into world space (RO).
		AUTO_PROP Matrix4x4 localToWorldMatrix GetLocalToWorldMatrix


		// Makes the rendered 3D object visible if enabled.
		AUTO_PROP bool enabled GetEnabled SetEnabled


		// Does this object cast shadows?
		AUTO_PROP bool castShadows GetCastShadows SetCastShadows


		// Does this object receive shadows?
		AUTO_PROP bool receiveShadows GetReceiveShadows SetReceiveShadows


		// Returns the first instantiated [[Material]] assigned to the renderer.
		CUSTOM_PROP Material material
		{
			#if UNITY_EDITOR
			if (self->IsPersistent())
			{
				ErrorStringObject("Not allowed to access Renderer.material on prefab object. Use Renderer.sharedMaterial instead", self);
				return NULL;
			}
			#endif

			return Scripting::ScriptingWrapperFor (self->GetAndAssignInstantiatedMaterial (0, false));
		}
		{
			self->SetMaterialCount (std::max(1, self->GetMaterialCount()));
			self->SetMaterial (value, 0);
		}


		// The shared material of this object.
		CUSTOM_PROP Material sharedMaterial
		{
			if (self->GetMaterialCount ())
				return Scripting::ScriptingWrapperFor (self->GetMaterial (0));
			else
				return Scripting::ScriptingObjectNULL (ScriptingClassFor (Material));
		}
		{
			self->SetMaterialCount (std::max(1, self->GetMaterialCount()));
			self->SetMaterial (value, 0);
		}

		// All the materials of this object.
		CUSTOM_PROP Material[] materials
		{
			#if UNITY_EDITOR
			if (self->IsPersistent())
			{
				ErrorStringObject("Not allowed to access Renderer.materials on prefab object. Use Renderer.sharedMaterials instead", self);
				return NULL;
			}
			#endif

			int length = self->GetMaterialCount();
			ScriptingArrayPtr array = CreateScriptingArray<ScriptingObjectPtr> (ScriptingClassFor(Material), length);

			for (int i=0;i<length;i++)
			{
				Material* instantiated = self->GetAndAssignInstantiatedMaterial(i, false);
				Scripting::SetScriptingArrayElement<ScriptingObjectPtr>(array,i,Scripting::ScriptingWrapperFor(instantiated));
			}

			return array;
		}
		{
			if (value == SCRIPTING_NULL)
				Scripting::RaiseNullException("material array is null");

			int size = GetScriptingArraySize(value);
			self->SetMaterialCount (size);
			for (int i=0;i<size;i++)
			{
				ScriptingObjectPtr o = Scripting::GetScriptingArrayElementNoRef<ScriptingObjectPtr>(value,i);
				self->SetMaterial (ScriptingObjectToObject<Material> (o), i);
			}
		}


		// All the shared materials of this object.
		CUSTOM_PROP Material[] sharedMaterials
		{
			return CreateScriptingArrayFromUnityObjects(self->GetMaterialArray(), ClassID(Material));
		}
		{
			if (value == SCRIPTING_NULL)
				Scripting::RaiseNullException("material array is null");

			int size = GetScriptingArraySize(value);
			self->SetMaterialCount (size);
			for (int i=0;i<size;i++)
				self->SetMaterial (ScriptingObjectToObject<Material> (Scripting::GetScriptingArrayElementNoRef<ScriptingObjectPtr>(value, i)), i);
		}


		// The bounding volume of the renderer (RO).
		CUSTOM_PROP Bounds bounds { return CalculateWorldAABB (self->GetGameObject ()); }

		// The index of the lightmap applied to this renderer.
		CUSTOM_PROP int lightmapIndex { return self->GetLightmapIndexInt(); } { return self->SetLightmapIndexInt(value); }

		CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		OBSOLETE error Property lightmapTilingOffset has been deprecated. Use lightmapScaleOffset (UnityUpgradable).
		CSRAW public Vector4 lightmapTilingOffset { get { return Vector4.zero; } set { } }
		
		CUSTOM_PROP Vector4 lightmapScaleOffset
		{
			Vector4f st = self->GetLightmapST();
			return Vector4f(st.x, st.y, st.z, st.w);
		}
		{
			Vector4f st( value.x, value.y, value.z, value.w );
			self->SetLightmapST( st );
		}

		// The scale & offset used for real-time lightmap.
		CUSTOM_PROP Vector4 realtimeLightmapScaleOffset
		{
			Vector4f st = self->GetLightmapST (kDynamicLightmap);
			return Vector4f (st.x, st.y, st.z, st.w);
		}
		{
			Vector4f st (value.x, value.y, value.z, value.w);
			self->SetLightmapST (st, kDynamicLightmap);
		}

		// ''OnBecameVisible'' is called when the object became visible by any camera.
		CSNONE void OnBecameVisible();


		// ''OnBecameInvisible'' is called when the object is no longer visible by any camera.
		CSNONE void OnBecameInvisible();

		// Is this renderer visible in any camera? (RO)
		AUTO_PROP bool isVisible IsVisibleInScene

		// If enabled and baked light probes are present in the scene, an interpolated light probe
		CUSTOM_PROP bool useLightProbes
		{
			return self->GetLightProbeMode() == kLightProbeInterpolated;
		}
		{
			self->SetUseLightProbes(value);
		}

		OBSOLETE error Use probeAnchor instead (UnityUpgradable).
		AUTO_PTR_PROP Transform lightProbeAnchor GetProbeAnchor SetProbeAnchor

		// If set, Renderer will use this Transform's position to find the interpolated light/reflection probe;
		AUTO_PTR_PROP Transform probeAnchor GetProbeAnchor SetProbeAnchor

		CUSTOM_PROP ReflectionProbeUsage reflectionProbeUsage
		{
			return self->GetReflectionProbeUsage();
		}
		{
			self->SetReflectionProbeUsage((ReflectionProbeUsage)value);
		}

		// Lets you add per-renderer material parameters without duplicating a material.
		CUSTOM void SetPropertyBlock (MaterialPropertyBlock properties)
		{
			if (properties.GetPtr())
				self->SetPropertyBlock (*properties);
			else
				self->ClearPropertyBlock ();
		}

		CUSTOM void GetPropertyBlock (MaterialPropertyBlock dest)
		{
			if (!dest.GetPtr())
				Scripting::RaiseNullException("dest property block is null");
			self->GetPropertyBlock (*dest);
		}


		CUSTOM_PROP string sortingLayerName
		{
			DISALLOW_IN_CONSTRUCTOR
			return scripting_string_new(self->GetSortingLayerName());
		}
		{
			DISALLOW_IN_CONSTRUCTOR
			self->SetSortingLayerName(value);
		}
		AUTO_PROP int sortingLayerID GetSortingLayerID SetSortingLayerID
		AUTO_PROP int sortingOrder GetSortingOrder SetSortingOrder


		CONDITIONAL UNITY_EDITOR
		CUSTOM internal void RenderNow (int material)
		{
			Shader* shader = s_ScriptingCurrentShader;
			if (!shader) {
				ErrorString ("Render requires material.SetPass before!");
				return;
			}

			GfxDevice& device = GetGfxDevice();

			// D3D needs begin/end when rendering is called out-of-band
			#if UNITY_EDITOR
			bool outsideOfFrame = !device.IsInsideFrame();
			if( outsideOfFrame )
				device.BeginFrame();
			#endif

			Matrix4x4f matView = device.GetViewMatrix();
			Matrix4x4f matWorld = device.GetWorldMatrix();

			Matrix4x4f transformMatrix;
			TransformType matrixType = self->GetTransform().CalculateTransformMatrix (transformMatrix);
			SetupObjectMatrix (transformMatrix, matrixType);

			self->Render (material, *s_ScriptingCurrentChannels);
			device.SetViewMatrix(matView);
			device.SetWorldMatrix(matWorld);

			#if UNITY_EDITOR
			if( outsideOfFrame )
				device.EndFrame();
			#endif
		}

		CUSTOM private void GetClosestReflectionProbesInternal(object result)
		{
			dynamic_array<ReflectionProbeBlendInfo> outProbes(kMemTempAlloc);
			GetReflectionProbes().GetClosestProbes(CalculateWorldAABB (self->GetGameObject()), self->GetProbeAnchor (), self->GetReflectionProbeUsage(), outProbes);
			
			FillScriptingListFromSimpleObjects<dynamic_array<ReflectionProbeBlendInfo>, ReflectionProbeBlendInfo, ScriptingReflectionProbeBlendInfo>(result,
				GetScriptingManager().GetCommonClasses().reflectionProbeBlendInfo,
				outProbes,
				ReflectionProbeBlendInfoToScripting);
		}

		CSRAW public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			GetClosestReflectionProbesInternal(result);
		}
		*/

	}

	partial class Renderer
	{
		static extern void DoUpdateRenderer(Renderer renderer, MeshFilter filter);

		// for NGUI
		static extern void DoUpdateRenderOrder(Renderer renderer, MeshFilter filter);
	}
}
