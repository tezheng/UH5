using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{

public enum FontStyle
{
	// No special style is applied.
	Normal = 0,

	// Bold style applied to your texts.
	Bold = 1,

	// Italic style applied to your texts.
	Italic = 2,

	// Bold and Italic styles applied to your texts.
	BoldAndItalic = 3,
}

// How multiline text should be aligned.
enum TextAlignment
{
	// Text lines are aligned on the left side.
	Left = 0,
	// Text lines are centered.
	Center = 1,
	// Text lines are aligned on the right side.
	Right = 2
}


// Where the anchor of the text is placed.
enum TextAnchor
{

	// Text is anchored in upper left corner.
	UpperLeft = 0,

	// Text is anchored in upper side, centered horizontally.
	UpperCenter = 1,

	// Text is anchored in upper right corner.
	UpperRight = 2,

	// Text is anchored in left side, centered vertically.
	MiddleLeft = 3,

	// Text is centered both horizontally and vertically.
	MiddleCenter = 4,

	// Text is anchored in right side, centered vertically.
	MiddleRight = 5,

	// Text is anchored in lower left corner.
	LowerLeft = 6,

	// Text is anchored in lower side, centered horizontally.
	LowerCenter = 7,

	// Text is anchored in lower right corner.
	LowerRight = 8

}

enum HorizontalWrapMode
{

	Wrap = 0,

	Overflow = 1

}

enum VerticalWrapMode
{
	Truncate = 0,

	Overflow = 1

}

/*
// A text string displayed in a GUI.
GUIText : GUIElement

	// The text to display.
	CUSTOM_PROP string text { return scripting_string_new (self->GetText ()); } { self->SetText (value); }

	// The [[Material]] to use for rendering.
	CUSTOM_PROP Material material
	{
		Material* material = self->GetMaterial ();
		if (material == NULL)
			material = GetBuiltinResource<Material> ("Font.mat");

		Material* instantiated = &Material::GetInstantiatedMaterial (material, *self, false);
		if (material != instantiated)
			self->SetMaterial (instantiated);

		return Scripting::ScriptingWrapperFor (instantiated);
	}
	{
		self->SetMaterial (value);
	}

	// Workaround for gcc/msvc where passing small mono structures by value does not work
	CUSTOM private void Internal_GetPixelOffset (out Vector2 output)
	{
		*output = self->GetPixelOffset();
	}
	CUSTOM private void Internal_SetPixelOffset (Vector2 p) { self->SetPixelOffset(p); }

	// The pixel offset of the text.
	CSRAW public Vector2 pixelOffset { get { Vector2 p; Internal_GetPixelOffset(out p); return p; } set { Internal_SetPixelOffset(value); }  }

	// The font used for the text.
	AUTO_PTR_PROP Font font GetFont SetFont

	// The alignment of the text.
	AUTO_PROP TextAlignment alignment GetAlignment SetAlignment

	// The anchor of the text.
	AUTO_PROP TextAnchor anchor GetAnchor SetAnchor

	// The line spacing multiplier.
	AUTO_PROP float lineSpacing GetLineSpacing SetLineSpacing

	// The tab width multiplier.
	AUTO_PROP float tabSize GetTabSize SetTabSize

	// The font size to use (for dynamic fonts)
	AUTO_PROP int fontSize GetFontSize SetFontSize

	// The font style to use (for dynamic fonts)
	AUTO_PROP FontStyle fontStyle GetFontStyle SetFontStyle

	// Enable HTML-style tags for Text Formatting Markup.
	AUTO_PROP bool richText GetRichText SetRichText

	// Color used to render the text
	AUTO_PROP Color color GetColor SetColor
END

// A script interface for the [[wiki:class-TextMesh|text mesh component]].
CLASS TextMesh : Component

	// The text that is displayed.
	CUSTOM_PROP string text { return scripting_string_new (self->GetText ()); } { self->SetText (value); }

	// The [[Font]] used.
	AUTO_PTR_PROP Font font GetFont SetFont


	// The font size to use (for dynamic fonts)
	AUTO_PROP int fontSize GetFontSize SetFontSize

	// The font style to use (for dynamic fonts)
	AUTO_PROP FontStyle fontStyle GetFontStyle SetFontStyle

	// How far should the text be offset from the transform.position.z when drawing
	AUTO_PROP float offsetZ GetOffsetZ SetOffsetZ

	// How lines of text are aligned (Left, Right, Center)
	AUTO_PROP TextAlignment alignment GetAlignment SetAlignment

	// Which point of the text shares the position of the Transform
	AUTO_PROP TextAnchor anchor GetAnchor SetAnchor

	// The size of each character (This scales the whole text)
	AUTO_PROP float characterSize GetCharacterSize SetCharacterSize

	// How much space will be in-between lines of text
	AUTO_PROP float lineSpacing GetLineSpacing SetLineSpacing

	// How much space will be inserted for a tab '\t' character. This is a multiplum of the 'spacebar' character offset
	AUTO_PROP float tabSize GetTabSize SetTabSize

	// Enable HTML-style tags for Text Formatting Markup.
	AUTO_PROP bool richText GetRichText SetRichText

	// Base color in which to render the text
	AUTO_PROP Color color GetColor SetColor
END

*/

// Info how to render a character from the font texture. See /Font.characterInfo/
public struct CharacterInfo
{
	// Unicode value of the character
	public int index;
	public Rect	uv;
	public Rect	vert;
	public float width;
	// The size of the character or 0 if it is the default font size.
	public int size;
	// The style of the character.
	public FontStyle style;
	public bool	flipped;
	// This is needed internally, to translate the vert coordinates to a proper baseline.
	private int ascent;

	public int advance { get { return (int)width; }}
	public int glyphWidth { get { return (int)vert.width; }}
	public int glyphHeight { get { return (int)-vert.height; }}
	public int bearing { get { return (int)vert.x; }}
	public int minY { get { return ascent+(int)(vert.y + vert.height); }}
	public int maxY { get { return ascent+(int)vert.y; }}
	public int minX { get { return (int)vert.x; }}
	public int maxX { get { return (int)(vert.x+vert.width); }}
	internal Vector2 uvBottomLeftUnFlipped { get { return new Vector2(uv.x, uv.y); }}
	internal Vector2 uvBottomRightUnFlipped { get { return new Vector2(uv.x+uv.width, uv.y); }}
	internal Vector2 uvTopRightUnFlipped { get { return new Vector2(uv.x+uv.width, uv.y+uv.height); }}
	internal Vector2 uvTopLeftUnFlipped { get { return new Vector2(uv.x, uv.y+uv.height); }}
	public Vector2 uvBottomLeft { get { return flipped ? uvBottomLeftUnFlipped : uvBottomLeftUnFlipped; }}
	public Vector2 uvBottomRight { get { return flipped ? uvTopLeftUnFlipped : uvBottomRightUnFlipped;  }}
	public Vector2 uvTopRight { get { return flipped ? uvTopRightUnFlipped : uvTopRightUnFlipped;  }}
	public Vector2 uvTopLeft { get { return flipped ? uvBottomRightUnFlipped : uvTopLeftUnFlipped;  }}
}

// Script interface for [[wiki:class-Font|font assets]].
public class Font : Object
{
	public void RequestCharactersInTexture (string characters, int size = 0, FontStyle style = FontStyle.Normal)
	{
		//UTF16String str(characters.AsUTF8().c_str());
		//self->CacheFontForText (str.text, str.length, size, style);
	}

	public bool GetCharacterInfo(char ch, out CharacterInfo info, int size = 0, FontStyle style = FontStyle.Normal)
	{
		info = new CharacterInfo();
		if (false)//self->HasCharacterInTexture (ch, size, style))
		{
			info.index = ch;
			info.size = size;
			info.style = style;
//			self->GetCharacterRenderInfo( ch, size, style, info->vert, info->uv, info->flipped );
//			info->width = self->GetCharacterWidth (ch, size, style);
//			info->ascent = self->GetAscent();
			return true;
		}
		return false;
	}
	public Material material;
	public string[] fontNames;
	public delegate void FontTextureRebuildCallback();
	public FontTextureRebuildCallback textureRebuildCallback;
/*
	CUSTOM static public string[] GetOSInstalledFontNames ()
	{
		TextRenderingPrivate::FontNames names;
		Font::GetOSFontNames(names);
		ScriptingArrayPtr arr = CreateScriptingArray<ScriptingStringPtr>(MONO_COMMON.string, names.size());
		int idx = 0;
		for (TextRenderingPrivate::FontNames::const_iterator i = names.begin(); i != names.end(); ++i)
		{
			Scripting::SetScriptingArrayElement<ScriptingStringPtr>(arr, idx, scripting_string_new(*i));
			idx++;
		}
		return arr;
	}

	CUSTOM private static void Internal_CreateFont ([Writable]Font _font, string name)
	{
		Font* font = NEW_OBJECT (Font);
		SmartResetObject(*font);
		font->SetNameCpp (name);
		Scripting::ConnectScriptingWrapperToObject (_font.GetScriptingObject(), font);
	}

	CUSTOM private static void Internal_CreateDynamicFont ([Writable]Font _font, string[] _names, int size)
	{
		TextRenderingPrivate::FontNames names;
		for (int i = 0; i < GetScriptingArraySize(_names); ++i)
			names.push_back(scripting_cpp_string_for(Scripting::GetScriptingArrayElementNoRef<ScriptingStringPtr>(_names, i)));
		Font* font = Font::CreateDynamicFont (names, size);
		Scripting::ConnectScriptingWrapperToObject (_font.GetScriptingObject(), font);
	}

	CSRAW public static Font CreateDynamicFontFromOSFont (string fontname, int size)
	{
		var font = new Font(new string[] {fontname}, size);
		return font;
	}

	CSRAW public static Font CreateDynamicFontFromOSFont (string[] fontnames, int size)
	{
		var font = new Font(fontnames, size);
		return font;
	}

	// Creates a new font.
	CSRAW public Font () { Internal_CreateFont (this,null); }

	// Creates a new font named /name/.
	CSRAW public Font (string name){ Internal_CreateFont (this,name); }

	CSRAW private Font (string[] names, int size)
	{
		Internal_CreateDynamicFont (this, names, size);
	}

	// The material used for the font display.
	AUTO_PTR_PROP Material material GetMaterial SetMaterial

	// Does this font have a specific character?
	CUSTOM bool HasCharacter (char c) { return self->HasCharacter((int)c);}

	// *undocumented*
	CONDITIONAL ENABLE_MONO || ENABLE_DOTNET || ENABLE_IL2CPP
	CUSTOM_PROP string[] fontNames
	{
		ScriptingArrayPtr arr = CreateScriptingArray<ScriptingStringPtr>(MONO_COMMON.string, self->GetFontNames().size());
		int idx = 0;
		for (UNITY_VECTOR(kMemFont,UnityStr)::const_iterator i = self->GetFontNames().begin(); i != self->GetFontNames().end(); ++i)
		{
			Scripting::SetScriptingArrayElement<ScriptingStringPtr>(arr, idx, scripting_string_new(*i));
			idx++;
		}
		return arr;
	}
	{
		UNITY_VECTOR(kMemFont,UnityStr) names;
		for (int i = 0; i < GetScriptingArraySize(value); ++i)
			names.push_back(scripting_cpp_string_for(Scripting::GetScriptingArrayElementNoRef<ScriptingStringPtr>(value, i)));
		self->SetFontNames(names);
	}

	// Access an array of all characters contained in the font texture.
	CUSTOM_PROP public CharacterInfo[] characterInfo
	{
		const Font::CharacterInfos &infos = self->GetCharacterInfos();
		int size = infos.size();
		int ascent = self->GetAscent();
		ScriptingArrayPtr array = CreateScriptingArray<ScriptingCharacterInfo> (GetScriptingManager().GetCommonClasses().characterInfo, size);
		#if ENABLE_DOTNET
			for (int i=0; i<size; i++)
			{
				ScriptingCharacterInfo temp;
				temp.CopyFrom(infos[i]);
				temp.ascent = ascent;
				ScriptingObjectPtr data;
				MarshallNativeStructIntoManaged(temp, data);
				Scripting::SetScriptingArrayElement(array, i, data);
			}
		#else
			ScriptingCharacterInfo *sci = Scripting::GetScriptingArrayStart<ScriptingCharacterInfo>(array);
			for (int i=0; i<size; i++)
			{
				sci[i].CopyFrom(infos[i]);
				sci[i].ascent = ascent;
			}
		#endif
		return array;
	}
	{
		Font::CharacterInfos infos;
		int size = GetScriptingArraySize(value);
		infos.resize(size);

		#if ENABLE_DOTNET
			for (int i=0; i<size; i++)
			{
				ScriptingCharacterInfo temp;
				MarshallManagedStructIntoNative(Scripting::GetScriptingArrayElementNoRef<ScriptingObjectPtr>(value, i), &temp);
				temp.CopyTo(infos[i]);
			}
		#else
			ScriptingCharacterInfo *sci = Scripting::GetScriptingArrayStart<ScriptingCharacterInfo>(value);
			for (int i=0; i<size; i++)
			{
				sci[i].CopyTo(infos[i]);
			}
		#endif
		self->SetCharacterInfos (infos);
	}

	// Request characters to be added to the font texture (dynamic fonts only).

	CUSTOM void RequestCharactersInTexture (string characters, int size = 0, FontStyle style = FontStyle.Normal)
	{
		UTF16String str(characters.AsUTF8().c_str());
		self->CacheFontForText (str.text, str.length, size, style);
	}

	CSRAW public static event Action<Font> textureRebuilt;

	CSRAW private static void InvokeTextureRebuilt_Internal(Font font)
	{
		// Invoke new (global) callback.
		var callback = textureRebuilt;
		if (callback != null)
			callback(font);

		// Invoke old deprecated (per-instance) callback.  Can be removed
		// once we kill off support for the old one.  These won't work reliably
		// as the native runtime may unload Font objects and then later create
		// a new script wrapper when loading them back in.
		if (font.m_FontTextureRebuildCallback != null)
			font.m_FontTextureRebuildCallback();
	}

	// Old texture rebuild callback replaced for 5.0.
	CSRAW private event FontTextureRebuildCallback m_FontTextureRebuildCallback;
	CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	CSRAW public delegate void FontTextureRebuildCallback(); // Can't yet deprecate as otherwise we can't use it here ourselves without getting warnings.
	CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	OBSOLETE warning Font.textureRebuildCallback has been deprecated. Use Font.textureChanged instead.
	CSRAW public FontTextureRebuildCallback textureRebuildCallback
	{
		get { return m_FontTextureRebuildCallback; }
		set { m_FontTextureRebuildCallback = value; }
	}

	CSRAW public static int GetMaxVertsForString (string str)
	{
		return str.Length * 4 + 4;
	}
	
	// Get rendering info for a specific character
	CUSTOM bool GetCharacterInfo(char ch, out CharacterInfo info, int size = 0, FontStyle style = FontStyle.Normal)
	{
		if (self->HasCharacterInTexture (ch, size, style))
		{
			info->index = ch;
			info->size = size;
			info->style = style;
			self->GetCharacterRenderInfo( ch, size, style, info->vert, info->uv, info->flipped );
			info->width = self->GetCharacterWidth (ch, size, style);
			info->ascent = self->GetAscent();
			return true;
		}
		return false;
	}

	// Is the font dynamic?
	CUSTOM_PROP bool dynamic { return self->GetConvertCase() == Font::kDynamicFont; }

	CUSTOM_PROP int ascent { return self->GetAscent(); }
	CUSTOM_PROP int lineHeight { return self->GetLineSpacing(); }

	AUTO_PROP int fontSize GetFontSize
	*/
}

struct UICharInfo
{
	public Vector2 cursorPos;
	public float charWidth;
}

struct UILineInfo
{
	public int startCharIdx;
	public int height;
}

// static void CleanupNativeScriptingTextGenerator(void* result){ delete ((ScriptingTextGenerator*)result); } ;

/*
CLASS TextGenerator : IDisposable
	CSRAW internal IntPtr m_Ptr;

#if UNITY_EDITOR || UNITY_WP8
	private static readonly List<TextGenerator> s_Instances = new List<TextGenerator>();
#endif

	// Used in other part of partial class to cache
	// the last getnerated data...
	// if we request the same generation we do NOTHING!
	private string m_LastString;
	private TextGenerationSettings m_LastSettings;
	private bool m_HasGenerated;
	private bool m_LastValid;

	// backing store for caching
	private readonly List<UIVertex> m_Verts;
	private readonly List<UICharInfo> m_Characters;
	private readonly List<UILineInfo> m_Lines;

	private bool m_CachedVerts;
	private bool m_CachedCharacters;
	private bool m_CachedLines;
	
	public TextGenerator() : this (50)
	{}

	public TextGenerator(int initialCapacity)
	{
		m_Verts = new List<UIVertex>((initialCapacity + 1) * 4);
		m_Characters = new List<UICharInfo>(initialCapacity + 1);
		m_Lines = new List<UILineInfo>(20);
		Init();
#if UNITY_EDITOR || UNITY_WP8
		lock (s_Instances)
			s_Instances.Add(this);
#endif
	}

	CUSTOM private void Init ()
	{
		self.SetPtr(new ScriptingTextGenerator(), CleanupNativeScriptingTextGenerator);
	}
	
	CSRAW void IDisposable.Dispose ()
	{
#if UNITY_EDITOR || UNITY_WP8
		lock (s_Instances)
			s_Instances.Remove(this);
#endif
		Dispose_cpp();
	}

	THREAD_SAFE
	CUSTOM private void Dispose_cpp()
	{
		delete self.GetPtr();
	}


	CSRAW ~TextGenerator()
	{
		((IDisposable)this).Dispose();
	}

	CSRAW internal bool Populate_Internal (
		string str, Font font, Color color,
		int fontSize, float lineSpacing, FontStyle style, bool richText,
		bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize,
		VerticalWrapMode verticalOverFlow, HorizontalWrapMode horizontalOverflow, bool updateBounds,
		TextAnchor anchor, Vector2 extents, Vector2 pivot, bool generateOutOfBounds)
	{
		return Populate_Internal_cpp (
			str, font, color,
			fontSize, lineSpacing, style, richText,
			resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize,
			(int)verticalOverFlow, (int)horizontalOverflow, updateBounds,
			anchor, extents.x, extents.y, pivot.x, pivot.y, generateOutOfBounds);
	}

	//workaround on XBOX360, we can't pass vector2, so instead we pass each coordinate separatly. Check case 499077 for more details.
	CUSTOM internal bool Populate_Internal_cpp (
		string str, Font font, Color color,
		int fontSize, float lineSpacing, FontStyle style, bool richText,
		bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize,
		int verticalOverFlow, int horizontalOverflow, bool updateBounds,
		TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY,
		bool generateOutOfBounds)
	{
		Vector2f extents(extentsX, extentsY);
		Vector2f pivot(pivotX, pivotY);
		ScriptingTextGenerator::GetStringRenderInfo (*self,
			str, font, ColorRGBA32 (color),
			fontSize, lineSpacing, style, richText,
			resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize,
			(ScriptingTextGenerator::VerticalWrapMode)verticalOverFlow, (ScriptingTextGenerator::HorizontalWrapMode)horizontalOverflow, updateBounds,
			anchor, extents, pivot, false, generateOutOfBounds);
		return self->isValid;
	}

	CUSTOM_PROP Rect rectExtents
	{
		Rectf result;
		result.x = 0.0f;
		result.y = 0.0f;
		result.width = self->textExtents.x;
		result.height = self->textExtents.y;
		return result;
	}
	CUSTOM_PROP int vertexCount { return self->GetVertexCount (); }
	
	CSRAW public void GetVertices (List<UIVertex> vertices)
	{
#if ENABLE_MONO || ENABLE_IL2CPP
		GetVerticesInternal(vertices);
#else
		if (vertices == null)
			throw new ArgumentException("The results list cannot be null");
		vertices.Clear();
		vertices.AddRange (GetVerticesArray());
#endif
	}
	
	CONDITIONAL ENABLE_MONO || ENABLE_IL2CPP
	CUSTOM private void GetVerticesInternal(object vertices)
	{
		if (vertices == SCRIPTING_NULL)
		{
			Scripting::RaiseArgumentException("The results list cannot be null");
			return;
		}
		FillScriptingListFromSimpleObjects<dynamic_array<UI::UIVertex>, UI::UIVertex>(vertices, GetScriptingManager().GetCommonClasses().UIVertex, self->verts);
	}

	CUSTOM public UIVertex[] GetVerticesArray()
	{
		size_t count = self->GetVertexCount();
		ScriptingArrayPtr array = CreateScriptingArray<UI::UIVertex>(GetScriptingManager().GetCommonClasses().UIVertex, count);
		for (int i = 0; i < count; i++)
		{
			Scripting::SetScriptingArrayElement<UI::UIVertex>(array, i, self->verts[i]);
		}
		return array;
	}

	CUSTOM_PROP int characterCount { return self->GetCharacterCount (); }

	// This seems to be the only way to get the number of visible characters out right now.
	// If the whole text string is visible, the vertexCount divided by 4 is one larger than the number of characters in the string for some reason.
	// However, if not the whole string is visible, the vertexCount divided by 4 seems to be accurate.
	// So to get an always accurate result, we take the smaller value of the two.
	// We should find a better way to determine the visible character count.
	CSRAW public int characterCountVisible { get { return Mathf.Min (m_LastString.Length, vertexCount / 4); } }

	CSRAW public void GetCharacters (List<UICharInfo> characters)
	{
#if ENABLE_MONO || ENABLE_IL2CPP
		GetCharactersInternal(characters);
#else
		if (characters == null)
			throw new ArgumentException("The results list cannot be null");
		characters.Clear();
		characters.AddRange (GetCharactersArray());
#endif
	}
	
	CONDITIONAL ENABLE_MONO || ENABLE_IL2CPP
	CUSTOM private void GetCharactersInternal(object characters)
	{
		if (characters == SCRIPTING_NULL)
		{
			Scripting::RaiseArgumentException("The results list cannot be null");
			return;
		}
		FillScriptingListFromSimpleObjects<dynamic_array<CharInfo>, CharInfo>(characters, GetScriptingManager().GetCommonClasses().UICharInfo, self->charInfos);
	}

	CUSTOM public UICharInfo[] GetCharactersArray()
	{
		size_t count = self->GetCharacterCount();
		ScriptingArrayPtr array = CreateScriptingArray<CharInfo>(GetScriptingManager().GetCommonClasses().UICharInfo, count);
		for (int i = 0; i < count; i++)
		{
			Scripting::SetScriptingArrayElement<CharInfo>(array, i, self->charInfos[i]);
		}
		return array;
	}
	
	CUSTOM_PROP int lineCount { return self->GetLineCount (); }
	
	CSRAW public void GetLines (List<UILineInfo> lines)
	{
#if ENABLE_MONO || ENABLE_IL2CPP
		GetLinesInternal(lines);
#else
		if (lines == null)
			throw new ArgumentException("The results list cannot be null");
		lines.Clear();
		lines.AddRange (GetLinesArray());
#endif
	}
	
	CONDITIONAL ENABLE_MONO || ENABLE_IL2CPP
	CUSTOM private void GetLinesInternal(object lines)
	{
		if (lines == SCRIPTING_NULL)
		{
			Scripting::RaiseArgumentException("The results list cannot be null");
			return;
		}
		FillScriptingListFromSimpleObjects<dynamic_array<LineInfo>, LineInfo>(lines, GetScriptingManager().GetCommonClasses().UILineInfo, self->lineInfos);
	}

	CUSTOM public UILineInfo[] GetLinesArray()
	{
		size_t count = self->GetLineCount();
		ScriptingArrayPtr array = CreateScriptingArray<LineInfo>(GetScriptingManager().GetCommonClasses().UILineInfo, count);
		for (int i = 0; i < count; i++)
		{
			Scripting::SetScriptingArrayElement<LineInfo>(array, i, self->lineInfos[i]);
		}
		return array;
	}

	CUSTOM_PROP int fontSizeUsedForBestFit { return self->GetFontSizeFoundForBestFit (); }

	CONDITIONAL UNITY_EDITOR || UNITY_WP8
	CSRAW internal static void InvalidateAll()
	{
		lock (s_Instances)
			foreach (var instance in s_Instances)
				instance.Invalidate();
	}
END
*/


}
