using System;

namespace UnityEngine
{
	public class Mesh : Object
	{
		public bool dirty;
		Vector3[] _vertices = new Vector3[] {};
		Vector2[] _uv = new Vector2[] {};
		int[] _triangles = new int[] {};
		Color32[] _color32 = new Color32[] {};
		Vector3[] _normals = new Vector3[] {};
		Vector4[] _tangents = new Vector4[] {};
		public Vector3[] vertices {get { return _vertices; } set { dirty = true; _vertices = value;}}

		public int vertexCount {get {return _vertices.Length;}}

		public Vector2[] uv  {get { return _uv; } set { dirty = true; _uv = value;}}
		public int[] triangles  {get { return _triangles; } set { dirty = true; _triangles = value;}}
		public Color32[] colors32 {get { return _color32; } set { dirty = true; _color32 = value;}}
		public Vector3[] normals {get { return _normals; } set { dirty = true; _normals = value;}}
		public Vector4[] tangents {get { return _tangents; } set { dirty = true; _tangents = value;}}

		public Mesh ()
		{
		}

		public void ClearDitry() {
			dirty = false;
		}

		public void RecalculateNormals()
		{
			// Still not implemented.
		}

		public void Optimize()
		{
			// Still not implemented.
		}

		public void MarkDynamic ()
		{

		}

		public void Clear (bool keepVertexLayout = true)
		{

		}

		public void RecalculateBounds () {}

		/*
		// Creates an empty mesh
		CSRAW public Mesh ()
		{
			Internal_Create(this);
		}

		CUSTOM private static void Internal_Create ([Writable]Mesh mono)
		{
			Mesh* mesh = NEW_OBJECT_MAIN_THREAD (Mesh);
			mesh->Reset();
			Scripting::ConnectScriptingWrapperToObject (mono.GetScriptingObject(), mesh);
			mesh->AwakeFromLoad(kInstantiateOrCreateFromCodeAwakeFromLoad);
		}

		// Clears all vertex data and all triangle indices.
		CUSTOM void Clear (bool keepVertexLayout = true)
		{
			self->Clear (keepVertexLayout);
		}

		// Returns state of the Read/Write Enabled checkbox when model was imported.
		AUTO_PROP bool isReadable GetIsReadable

		// Works like isReadable, except it also returns true in editor outside the game loop.
		CUSTOM_PROP internal bool canAccess { return self->CanAccessFromScript(); }

		// Returns a copy of the vertex positions or assigns a new vertex positions array.
		CUSTOM_PROP Vector3[] vertices
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector3;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelVertex))
					return CreateScriptingArrayStride<Vector3f>(self->GetChannelPointer(kShaderChannelVertex), self->GetVertexCount(), klass, self->GetStride(kShaderChannelVertex));
			}
			else
				ErrorStringMsg("Not allowed to access vertices on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetVertices (Scripting::GetScriptingArrayStart<Vector3f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access vertices on mesh '%s'", self->GetName());
		}

		// The normals of the mesh.
		CUSTOM_PROP Vector3[] normals
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector3;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelNormal))
					return CreateScriptingArrayStride<Vector3f>(self->GetChannelPointer(kShaderChannelNormal), self->GetVertexCount(), klass, self->GetStride(kShaderChannelNormal));
			}
			else
				ErrorStringMsg("Not allowed to access normals on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
			self->SetNormals (Scripting::GetScriptingArrayStart<Vector3f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access normals on mesh '%s'", self->GetName());
		}

		// The tangents of the mesh.
		CUSTOM_PROP Vector4[] tangents
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector4;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelTangent))
					return CreateScriptingArrayStride<Vector4f>(self->GetChannelPointer(kShaderChannelTangent), self->GetVertexCount(), klass, self->GetStride(kShaderChannelTangent));
			}
			else
				ErrorStringMsg("Not allowed to access tangents on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
			self->SetTangents (Scripting::GetScriptingArrayStart<Vector4f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access tangents on mesh '%s'", self->GetName());
		}

		// The base texture coordinates of the mesh.
		CUSTOM_PROP Vector2[] uv
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector2;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelTexCoord0))
				{
					ScriptingArrayPtr array = CreateScriptingArray<Vector2f>(klass, self->GetVertexCount());
					self->ExtractUvArray (0, Scripting::GetScriptingArrayStart<Vector2f>(array)->GetPtr(), 2);
					return array;
				}
			}
			else
				ErrorStringMsg("Not allowed to access uv on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetUv (0, Scripting::GetScriptingArrayStart<Vector2f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access uv on mesh '%s'", self->GetName());
		}

		CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		OBSOLETE error Property Mesh.uv1 has been deprecated. Use Mesh.uv2 instead (UnityUpgradable).
		CSRAW public Vector2[] uv1 { get { return null; } set { } }

		// The second texture coordinate set of the mesh, if present.
		CUSTOM_PROP Vector2[] uv2
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector2;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelTexCoord1))
				{
					ScriptingArrayPtr array = CreateScriptingArray<Vector2f>(klass, self->GetVertexCount());
					self->ExtractUvArray (1, Scripting::GetScriptingArrayStart<Vector2f>(array)->GetPtr(), 2);
					return array;
				}
			}
			else
				ErrorStringMsg("Not allowed to access uv2 on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetUv (1, Scripting::GetScriptingArrayStart<Vector2f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access uv2 on mesh '%s'", self->GetName());
		}

		// The third texture coordinate set of the mesh, if present.
		CUSTOM_PROP Vector2[] uv3
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector2;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelTexCoord2))
				{
					ScriptingArrayPtr array = CreateScriptingArray<Vector2f>(klass, self->GetVertexCount());
					self->ExtractUvArray (2, Scripting::GetScriptingArrayStart<Vector2f>(array)->GetPtr(), 2);
					return array;
				}
			}
			else
				ErrorStringMsg("Not allowed to access uv3 on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetUv (2, Scripting::GetScriptingArrayStart<Vector2f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access uv3 on mesh '%s'", self->GetName());
		}

		// The fourth texture coordinate set of the mesh, if present.
		CUSTOM_PROP Vector2[] uv4
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().vector2;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelTexCoord3))
				{
					ScriptingArrayPtr array = CreateScriptingArray<Vector2f>(klass, self->GetVertexCount());
					self->ExtractUvArray (3, Scripting::GetScriptingArrayStart<Vector2f>(array)->GetPtr(), 2);
					return array;
				}
			}
			else
				ErrorStringMsg("Not allowed to access uv4 on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetUv (3, Scripting::GetScriptingArrayStart<Vector2f>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access uv4 on mesh '%s'", self->GetName());
		}

		// The bounding volume of the mesh.
		AUTO_PROP Bounds bounds GetBounds SetBounds


		// Vertex colors of the mesh.
		CUSTOM_PROP Color[] colors
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().color;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelColor))
				{
					ScriptingArrayPtr array = CreateScriptingArray<ColorRGBAf>(klass, self->GetVertexCount());
					self->ExtractColorArray (Scripting::GetScriptingArrayStart<ColorRGBAf>(array));
					return array;
				}
			}
			else
				ErrorStringMsg("Not allowed to access colors on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetColors (Scripting::GetScriptingArrayStart<ColorRGBAf>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access colors on mesh '%s'", self->GetName());
		}

		// Vertex colors of the mesh.
		CUSTOM_PROP Color32[] colors32
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().color32;
			if (self->CanAccessFromScript())
			{
				if (self->HasChannel(kShaderChannelColor))
				{
					ScriptingArrayPtr array = CreateScriptingArray<ColorRGBA32>(klass, self->GetVertexCount());
					self->ExtractColorArray (Scripting::GetScriptingArrayStart<ColorRGBA32>(array));
					return array;
				}
			}
			else
				ErrorStringMsg("Not allowed to access colors on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
				self->SetColors (Scripting::GetScriptingArrayStart<ColorRGBA32>(value), GetScriptingArraySize(value));
			else
				ErrorStringMsg("Not allowed to access colors on mesh '%s'", self->GetName());
		}

		// Recalculate the bounding volume of the mesh from the vertices.
		CUSTOM void RecalculateBounds ()
		{
			if (self->CanAccessFromScript())
				self->RecalculateBounds();
			else
				ErrorStringMsg("Not allowed to call RecalculateBounds() on mesh '%s'", self->GetName());
		}

		// Recalculates the normals of the mesh from the triangles and vertices.
		CUSTOM void RecalculateNormals ()
		{
			if (self->CanAccessFromScript())
				self->RecalculateNormals();
			else
				ErrorStringMsg("Not allowed to call RecalculateNormals() on mesh '%s'", self->GetName());
		}


		// Optimizes the mesh for display.
		CUSTOM void Optimize ()
		{
		}

		// An array containing all triangles in the mesh.
		//
		// If the mesh contains multiple sub meshes (materials) the triangle list will contain all triangles of all submeshes.
		CUSTOM_PROP int[] triangles
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().int_32;
			if (self->CanAccessFromScript())
			{
				Mesh::TemporaryIndexContainer triangles;
				self->GetTriangles(triangles);
				return CreateScriptingArray(&triangles[0], triangles.size(), klass);
			}
			else
				ErrorStringMsg("Not allowed to access triangles on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}
		{
			if (self->CanAccessFromScript())
			{
				self->SetSubMeshCount(1);
				self->SetIndices (Scripting::GetScriptingArrayStart<UInt32>(value), GetScriptingArraySize(value), 0, kPrimitiveTriangles);
			}
			else
				ErrorStringMsg("Not allowed to access triangles on mesh '%s'", self->GetName());
		}


		// Returns the triangle list for the submesh.
		CUSTOM int[] GetTriangles (int submesh)
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().int_32;
			if (self->CanAccessFromScript())
			{
				Mesh::TemporaryIndexContainer triangles;
				self->GetTriangles(triangles, submesh);
				return CreateScriptingArray(&triangles[0], triangles.size(), klass);
			}
			else
				ErrorStringMsg("Not allowed to call GetTriangles() on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}

		// Sets the triangle list for the submesh.
		CUSTOM void SetTriangles(int[] triangles, int submesh)
		{
			if (self->CanAccessFromScript())
			{
				self->SetIndices(Scripting::GetScriptingArrayStart<UInt32>(triangles), GetScriptingArraySize(triangles), submesh, kPrimitiveTriangles);
			}
			else
				ErrorStringMsg("Not allowed to call SetTriangles() on mesh '%s'", self->GetName());
		}


		// Returns the index buffer for the submesh.
		CUSTOM int[] GetIndices (int submesh)
		{
			ScriptingClassPtr klass = GetScriptingManager().GetCommonClasses().int_32;
			if (self->CanAccessFromScript())
			{
				Mesh::TemporaryIndexContainer indices;
				self->GetIndices (indices, submesh);
				return CreateScriptingArray(&indices[0], indices.size(), klass);
			}
			else
				ErrorStringMsg("Not allowed to call GetIndices() on mesh '%s'", self->GetName());

			return CreateEmptyStructArray(klass);
		}

		// Sets the index buffer for the submesh.
		CUSTOM void SetIndices (int[] indices, MeshTopology topology, int submesh)
		{
			if (self->CanAccessFromScript())
			{
				self->SetIndices(Scripting::GetScriptingArrayStart<UInt32>(indices), GetScriptingArraySize(indices), submesh, topology);
			}
			else
				ErrorStringMsg("Not allowed to call SetIndices() on mesh '%s'", self->GetName());
		}

		// Gets the topology of a submesh.
		CUSTOM MeshTopology GetTopology (int submesh)
		{
			if ((unsigned)submesh >= self->GetSubMeshCount())
			{
				ErrorString("Failed getting topology. Submesh index is out of bounds.");
				return kPrimitiveTriangles;
			}
			return self->GetSubMeshFast(submesh).topology;
		}

		// Returns the number of vertices in the mesh (RO).
		AUTO_PROP int vertexCount GetVertexCount

		// The number of submeshes. Every material has a separate triangle list.
		CUSTOM_PROP int subMeshCount
		{
			return self->GetSubMeshCount();
		}
		{
			if(value < 0)
			{
				ErrorString ("subMeshCount can't be set to negative value");
				return;
			}
			self->SetSubMeshCount(value);
		}

		// Combines several meshes into this mesh.
		CUSTOM void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes = true, bool useMatrices = true)
		{
			CombineInstances combineVec;
			combineVec.resize(GetScriptingArraySize(combine));
			MonoCombineInstance* mono = Scripting::GetScriptingArrayStart<MonoCombineInstance>(combine);
			for (size_t i=0;i<combineVec.size();i++)
			{
				combineVec[i].transform = mono[i].transform;
				combineVec[i].subMeshIndex = mono[i].subMeshIndex;
				combineVec[i].mesh = PPtr<Mesh>(mono[i].meshInstanceID);
			}
			CombineMeshes(combineVec, *self, mergeSubMeshes, useMatrices);
		}


		// The bone weights of each vertex
		CUSTOM_PROP BoneWeight[] boneWeights
		{
			int size = self->GetVertexCount();
			BoneInfluence* weights = self->GetBoneWeights();
			return CreateScriptingArray(weights, size, MONO_COMMON.boneWeight);
		}
		{
			self->SetBoneWeights(Scripting::GetScriptingArrayStart<BoneInfluence> (value), GetScriptingArraySize(value));
		}

		// The bind poses. The bind pose at each index refers to the bone with the same index.
		CUSTOM_PROP Matrix4x4[] bindposes
		{
			return CreateScriptingArray(self->GetBindposes(), self->GetBindposeCount(), MONO_COMMON.matrix4x4);
		}
		{
			self->SetBindposes(Scripting::GetScriptingArrayStart<Matrix4x4f> (value), GetScriptingArraySize(value));
		}

		// Optimize mesh for frequent updates.
		CUSTOM void MarkDynamic ()
		{
			if (self->CanAccessFromScript())
				self->MarkDynamic();
		}

		CUSTOM void UploadMeshData(bool markNoLogerReadable)
		{
			if (self->CanAccessFromScript())
				self->UploadMeshData(markNoLogerReadable);
		}

		// Returns BlendShape count on this mesh.
		CUSTOM_PROP int blendShapeCount
		{
			return self->GetBlendShapeChannelCount();
		}

		// Returns name of BlendShape by given index.
		CUSTOM string GetBlendShapeName (int index)
		{
			if(index < 0 || index >= (int)self->GetBlendShapeData().channels.size())
			{
				Scripting::RaiseArgumentException("Blend shape data index out of range.");
				return SCRIPTING_NULL;
			}
		
			return scripting_string_new(GetChannelName (self->GetBlendShapeData(), index));
		}

		// Calculates the index of the blendShape name
		CUSTOM int GetBlendShapeIndex(string blendShapeName)
		{
			return GetChannelIndex (self->GetBlendShapeData(), blendShapeName.AsUTF8().c_str());
		}

		*/
	}
}

