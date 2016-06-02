using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine
{
	public enum SpriteAlignment
	{
		Center = 0,
		TopLeft = 1,
		TopCenter = 2,
		TopRight = 3,
		LeftCenter = 4,
		RightCenter = 5,
		BottomLeft = 6,
		BottomCenter = 7,
		BottomRight = 8,
		Custom = 9,
	}

enum SpritePackingMode
{
	Tight = 0,
	Rectangle
}

enum SpritePackingRotation
{
	None = 0,
	// Reserved
	Any = 15
}

enum SpriteMeshType
{
	FullRect = 0,
	Tight = 1
}

/// Describes one sprite frame.
public class Sprite : Object
{
	public Texture2D texture;
	public Rect textureRect;
	/*
	CUSTOM public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit = 100.0f, uint extrude = 0, SpriteMeshType meshType = SpriteMeshType.Tight, Vector4 border = Vector4.zero)
	{
		if (texture.IsNull())
			return SCRIPTING_NULL;
	
		Sprite* sprite = CreateObjectFromCode<Sprite>();
		sprite->Initialize(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border);
		sprite->AwakeFromLoad(kInstantiateOrCreateFromCodeAwakeFromLoad);
		return Scripting::ScriptingWrapperFor(sprite);
	}

	CUSTOM_PROP Bounds bounds
	{
		return self->GetBounds();
	}

	// Sprite definition rectangle on source texture (in texels).
	CUSTOM_PROP Rect rect
	{
		return self->GetRect();
	}
	
	// The number of pixels in one unit. Note: The C++ side still uses the name pixelsToUnits which is misleading,
	// but has not been changed yet to minimize merge conflicts.
	CUSTOM_PROP float pixelsPerUnit
	{
		return self->GetPixelsToUnits();
	}

	CUSTOM_PROP Texture2D texture
	{
		return Scripting::ScriptingWrapperFor(self->GetRenderDataForPlayMode().texture);
	}

	// Sprite rectangle on texture (in texels).
	CUSTOM_PROP Rect textureRect
	{
		const SpriteRenderData& rd = self->GetRenderDataForPlayMode(); // RenderData must match <texture> accessor's behavior.
		if (rd.settings.packed && rd.settings.packingMode != kSPMRectangle)
			Scripting::RaiseMonoException("Sprite is not rectangle-packed. TextureRect is invalid.");
		return rd.textureRect;
	}

	// Sprite rectangle offset in sprite definition rectangle space (in texels).
	CSRAW public Vector2 textureRectOffset
	{
		get
		{
			Vector2 v;
			Internal_GetTextureRectOffset(this, out v);
			return v;
		}
	}

	CUSTOM_PROP bool packed
	{
		const SpriteRenderData& rd = self->GetRenderData(true); // RenderData must always come from atlasing.
		return rd.settings.packed;
	}

	CUSTOM_PROP SpritePackingMode packingMode
	{
		const SpriteRenderData& rd = self->GetRenderData(true); // RenderData must always come from atlasing.
		if (!rd.settings.packed)
			Scripting::RaiseMonoException("Sprite is not packed.");
		return (SpritePackingMode)rd.settings.packingMode;
	}

	CUSTOM_PROP SpritePackingRotation packingRotation
	{
		const SpriteRenderData& rd = self->GetRenderData(true); // RenderData must always come from atlasing.
		if (!rd.settings.packed)
			Scripting::RaiseMonoException("Sprite is not packed.");
		return (SpritePackingRotation)rd.settings.packingRotation;
	}

	CUSTOM private static void Internal_GetTextureRectOffset(Sprite sprite, out Vector2 output)
	{
		const SpriteRenderData& rd = sprite->GetRenderDataForPlayMode(); // RenderData must match <texture> accessor's behavior.
		if (rd.settings.packed && rd.settings.packingMode != kSPMRectangle)
			Scripting::RaiseMonoException("Sprite is not rectangle-packed. TextureRectOffset is invalid.");
		output->x = rd.textureRectOffset.x;
		output->y = rd.textureRectOffset.y;
	}

	CUSTOM private static void Internal_GetPivot(Sprite sprite, out Vector2 output)
	{
		*output = sprite->GetPivot ();
	}

	// Pivot point in sprite definition rectangle space (in texels).
	CSRAW public Vector2 pivot
	{
		get
		{
			Vector2 v;
			Internal_GetPivot(this, out v);
			return v;
		}
	}

	// Border used for 9-slicing
	CUSTOM_PROP Vector4 border
	{
		return self->GetBorder();
	}

	// Returns a copy of the array containing sprite mesh vertex positions.
	CUSTOM_PROP Vector2[] vertices
	{
		const SpriteRenderData& rd = self->GetRenderData(false); // Always use base data. Packing does not modify geometry.
		const size_t count = rd.vertices.size();
		ScriptingArrayPtr array = CreateScriptingArray<Vector2f>(GetScriptingManager().GetCommonClasses().vector2, count);
		for (int i = 0; i < count; i++)
		{
			Scripting::SetScriptingArrayElement<Vector2f>(array, i, *(Vector2f*)&rd.vertices[i].pos);
		}
		return array;
	}

	// Returns a copy of the array containing sprite mesh triangles.
	CUSTOM_PROP UInt16[] triangles
	{
		const SpriteRenderData& rd = self->GetRenderData(false); // Always use base data. Packing does not modify geometry.
		const size_t count = rd.indices.size();
		ScriptingArrayPtr array = CreateScriptingArray<UInt16>(GetScriptingManager().GetCommonClasses().uInt_16, count);
		for (int i = 0; i < count; i++)
		{
			Scripting::SetScriptingArrayElement<UInt16>(array, i, rd.indices[i]);
		}
		return array;
	}

	// Returns a copy of the array containing sprite mesh UVs.
	CUSTOM_PROP Vector2[] uv
	{
		const SpriteRenderData& rd = self->GetRenderDataForPlayMode(); // RenderData must match <texture> accessor's behavior.
		rd.CalculateUVs();

		const size_t count = rd.vertices.size();
		ScriptingArrayPtr array = CreateScriptingArray<Vector2f>(GetScriptingManager().GetCommonClasses().vector2, count);
		for (int i = 0; i < count; i++)
		{
			Scripting::SetScriptingArrayElement<Vector2f>(array, i, rd.vertices[i].uv);
		}
		return array;
	}

	CUSTOM void OverrideGeometry(Vector2[] vertices, UInt16[] triangles)
	{
		if (self->CanAccessFromScript())
		{
			self->OverrideGeometry(Scripting::GetScriptingArrayStart<Vector2f>(vertices), GetScriptingArraySize(vertices),
			                       Scripting::GetScriptingArrayStart<UInt16>(triangles), GetScriptingArraySize(triangles));
		}
		else
			ErrorStringMsg("Not allowed to override geometry on sprite '%s'", self->GetName());
	}
	*/

}

/// Renders a Sprite.
public class SpriteRenderer : Renderer
{
	public Sprite sprite;
/*
	CSRAW
	public Sprite sprite
	{
		get
		{
			return GetSprite_INTERNAL();
		}
		set
		{
			SetSprite_INTERNAL(value);
		}
	}
	
	CUSTOM private Sprite GetSprite_INTERNAL()
	{
		return Scripting::ScriptingWrapperFor(self->GetSprite());
	}

	CUSTOM private void SetSprite_INTERNAL(Sprite sprite)
	{
		self->SetSprite(sprite);
	}
	
	AUTO_PROP Color color GetColor SetColor
	*/

}

}

/*
namespace UnityEngine.Sprites
{
CLASS DataUtility

	// UV coordinates for the inner part of a sliced Sprite or the whole Sprite if borders=0
	CUSTOM static public Vector4 GetInnerUV(Sprite sprite)
	{
		//TODO: check Sprite MeshType here?
		return sprite->GetInnerUVs();
	}

	// UV coordinates for the outer part of a sliced Sprite (the whole Sprite)
	CUSTOM static public Vector4 GetOuterUV(Sprite sprite)
	{
		//TODO: check Sprite MeshType here?
		return sprite->GetOuterUVs();
	}

	// Pixel padding from the edges of the sprite to the drawn rectangle (left, bottom, right, top). Valid when the RenderData rect does not match the definition rect (i.e. alpha trimming).
	CUSTOM static public Vector4 GetPadding(Sprite sprite)
	{
		//TODO: check Sprite MeshType here?
		return sprite->GetPadding();
	}

	// Gets the minimum size of a sliced Sprite
	CSRAW static public Vector2 GetMinSize(Sprite sprite)
	{
		Vector2 v;
		Internal_GetMinSize(sprite, out v);
		return v;
	}

	CUSTOM private static void Internal_GetMinSize(Sprite sprite, out Vector2 output)
	{
		const Vector4f& border = sprite->GetBorder();
		output->x = border.x + border.z;
		output->y = border.y + border.w;
	}
	
END
*/

