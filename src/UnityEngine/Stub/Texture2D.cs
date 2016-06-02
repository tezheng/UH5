
namespace UnityEngine
{

	public class Texture : Object
	{
		public int width { get; protected set; }
		public int height { get; protected set; }

		public string guid;
		public string rawJson;
		/*
		CUSTOM_PROP static int masterTextureLimit { return Texture::GetMasterTextureLimit (); } { Texture::SetMasterTextureLimit (value); }

		CUSTOM_PROP static AnisotropicFiltering anisotropicFiltering { return Texture::GetAnisoLimit (); } { Texture::SetAnisoLimit (value); }

		CUSTOM static void SetGlobalAnisotropicFilteringLimits( int forcedMin, int globalMax )
		{
			Texture::SetGlobalAnisoLimits(forcedMin, globalMax);
		}

		CUSTOM private static int Internal_GetWidth (Texture mono) { return mono->GetDataWidth(); }
		CUSTOM private static int Internal_GetHeight (Texture mono) { return mono->GetDataHeight(); }

		
		AUTO_PROP FilterMode filterMode GetFilterMode SetFilterMode
		AUTO_PROP int anisoLevel GetAnisoLevel SetAnisoLevel
		AUTO_PROP TextureWrapMode wrapMode GetWrapMode SetWrapMode
		AUTO_PROP float mipMapBias GetMipMapBias SetMipMapBias

		CUSTOM_PROP public Vector2 texelSize { return Vector2f (self->GetTexelSizeX (), self->GetTexelSizeY ()); }

		CUSTOM IntPtr GetNativeTexturePtr () {
			return self->GetNativeTexturePtr();
		}

		CUSTOM int GetNativeTextureID () {
			return self->GetNativeTextureID();
		}

		*/
	}

public class Texture2D : Texture
{
	public Texture2D (int width, int height, TextureFormat format, bool mipmap)
	{
		this.width = width;
		this.height = height;
	}

	public void SetPixel (int x, int y, Color color)
	{
	}

	public void Apply()
	{
		
	}
	/*
	C++RAW
	#define CHECK_READABLE { if (!self->GetIsReadable()) Scripting::RaiseMonoException("Texture '%s' is not readable, the texture memory can not be accessed from scripts. You can make the texture readable in the Texture Import Settings.", self->GetName()); }

	// Check if reading of texture is allowed by crossdomain security and throw if not
	C++RAW
	static void CheckReadAllowedAndThrow(Texture2D *tex)
	{
#if ENABLE_MONO && ENABLE_SECURITY
		if ( !tex->GetReadAllowed() )
			Scripting::RaiseSecurityException("No read access to the texture data: %s", tex->GetName());
#endif
	}

	C++RAW
	static void CheckScreenReadAllowedAndThrow()
	{
		if ( !Texture2D::GetScreenReadAllowed() )
			Scripting::RaiseSecurityException("Reading from the screen is not allowed when you have used a downloaded texture without proper crossdomain.xml authorization");
	}

	AUTO_PROP int mipmapCount CountDataMipmaps

	CSRAW public Texture2D (int width, int height)
	{
		Internal_Create(this, width, height, TextureFormat.ARGB32, true, false, IntPtr.Zero);
	}

	CSRAW public Texture2D (int width, int height, TextureFormat format, bool mipmap)
	{
		Internal_Create(this, width, height, format, mipmap, false, IntPtr.Zero);
	}

	CSRAW public Texture2D (int width, int height, TextureFormat format, bool mipmap, bool linear)
	{
		Internal_Create(this, width, height, format, mipmap, linear, IntPtr.Zero);
	}

	CUSTOM private static void Internal_Create ([Writable]Texture2D mono, int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
	{
		if(!GetBuildSettings().hasAdvancedVersion && nativeTex != 0)
		{
			Scripting::RaiseMonoException("Creating texture from native texture is PRO only.");
			return;
		}

		Texture2D* texture = NEW_OBJECT_MAIN_THREAD (Texture2D);
		texture->Reset();

		if (texture->InitTexture(width, height, format, mipmap ? Texture2D::kMipmapMask : Texture2D::kNoMipmap, 1, (intptr_t)nativeTex))
		{
			texture->SetStoredColorSpace (linear ? kTexColorSpaceLinear : kTexColorSpaceSRGB);
			Scripting::ConnectScriptingWrapperToObject (mono.GetScriptingObject(), texture);
			texture->AwakeFromLoad(kInstantiateOrCreateFromCodeAwakeFromLoad);
		}
		else
		{
			DestroySingleObject (texture);
			Scripting::RaiseMonoException("Failed to create texture because of invalid parameters.");
		}
	}

	CONDITIONAL ENABLE_TEXTUREID_MAP
	CSRAW internal Texture2D (int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
	{
		Internal_Create(this, width, height, format, mipmap, linear, nativeTex);
	}

	CONDITIONAL ENABLE_TEXTUREID_MAP
	CSRAW static public Texture2D CreateExternalTexture(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
	{
		return new Texture2D(width, height, format, mipmap, linear, nativeTex);
	}

	CONDITIONAL ENABLE_TEXTUREID_MAP
	CUSTOM public void UpdateExternalTexture(IntPtr nativeTex)
	{
		if(!GetBuildSettings().hasAdvancedVersion)
		{
			Scripting::RaiseMonoException("Updating native texture is PRO only.");
			return;
		}
		GetGfxDevice().RegisterNativeTexture(self->GetTextureID(), (intptr_t)nativeTex);
	}

	AUTO_PROP TextureFormat format GetTextureFormat

	CUSTOM_PROP public static Texture2D whiteTexture { return Scripting::ScriptingWrapperFor(builtintex::GetWhiteTexture()); }
	CUSTOM_PROP public static Texture2D blackTexture { return Scripting::ScriptingWrapperFor(builtintex::GetBlackTexture()); }

	CUSTOM void SetPixel (int x, int y, Color color)
	{
		CHECK_READABLE
		self->SetPixel (0, x, y, color);
	}
	CUSTOM Color GetPixel (int x, int y) {
		CHECK_READABLE
		CheckReadAllowedAndThrow(self);
		return self->GetPixel (0, x, y);
	}
	CUSTOM Color GetPixelBilinear (float u, float v) {
		CHECK_READABLE
		CheckReadAllowedAndThrow(self);
		return self->GetPixelBilinear (0, u, v);
	}
	CSRAW public void SetPixels(Color[] colors, int miplevel = 0)
	{
		int w = width >> miplevel; if( w < 1 ) w = 1;
		int h = height >> miplevel; if( h < 1 ) h = 1;
		SetPixels( 0, 0, w, h, colors, miplevel );
	}
	CUSTOM void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, int miplevel = 0)
	{
		CHECK_READABLE
		self->SetPixels( x, y, blockWidth, blockHeight, GetScriptingArraySize(colors), Scripting::GetScriptingArrayStart<ColorRGBAf>(colors), miplevel );
	}
	CUSTOM void SetPixels32(Color32[] colors, int miplevel = 0)
	{
		CHECK_READABLE
		if(miplevel < 0 || miplevel >= self->CountDataMipmaps())
		{
			ErrorString(Format("SetPixels32 failed: invalid miplevel, must be between 0 and %d", self->CountDataMipmaps()));
			return;
		}

		self->SetPixels32( miplevel, Scripting::GetScriptingArrayStart<ColorRGBA32>(colors), GetScriptingArraySize(colors) );
	}

	CUSTOM bool LoadImage (byte[] data)
	{
		return LoadMemoryBufferIntoTexture(*self, Scripting::GetScriptingArrayStart<UInt8>(data), GetScriptingArraySize(data), IsCompressedDXTTextureFormat(self->GetTextureFormat())?kLoadImageDXTCompressDithered:kLoadImageUncompressed);
	}

	CUSTOM void LoadRawTextureData(byte[] data)
	{
		CHECK_READABLE
		if(GetScriptingArraySize(data) < self->GetRawImageDataSize())
		{
			Scripting::RaiseMonoException("LoadRawTextureData: not enough data provided (will result in overread).");
			return;
		}
		::memcpy(self->GetWritableImageData(), Scripting::GetScriptingArrayStart<UInt8>(data), self->GetRawImageDataSize());
	}

	CSRAW public Color[] GetPixels(int miplevel = 0)
	{
		int w = width >> miplevel; if( w < 1 ) w = 1;
		int h = height >> miplevel; if( h < 1 ) h = 1;
		return GetPixels( 0, 0, w, h, miplevel );
	}
	CUSTOM Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, int miplevel = 0)
	{
		CHECK_READABLE
		CheckReadAllowedAndThrow(self);

		int res = blockWidth * blockHeight;
		if (blockWidth != 0 && blockHeight != res / blockWidth) {
			return SCRIPTING_NULL;
		}


		ScriptingArrayPtr colors = CreateScriptingArray<ColorRGBAf>(GetScriptingManager().GetCommonClasses().color, blockWidth * blockHeight);
		ColorRGBAf* firstElement = Scripting::GetScriptingArrayStart<ColorRGBAf>(colors);
		self->GetPixels( x, y, blockWidth, blockHeight, miplevel,  firstElement);
		return colors;
	}

	CUSTOM public Color32[] GetPixels32(int miplevel = 0)
	{
		CHECK_READABLE
		CheckReadAllowedAndThrow(self);

		if(miplevel < 0 || miplevel >= self->CountDataMipmaps())
		{
			ErrorString(Format("GetPixels32 failed: invalid miplevel, must be between 0 and %d", self->CountDataMipmaps()));
			return SCRIPTING_NULL;
		}

		const int minSize = GetMinimumTextureMipSizeForFormat(self->GetTextureFormat());

		int w = self->GetDataWidth() >> miplevel; if( w < minSize ) w = minSize;
		int h = self->GetDataHeight() >> miplevel; if( h < minSize ) h = minSize;

		ScriptingArrayPtr colors = CreateScriptingArray<ColorRGBA32>(GetScriptingManager().GetCommonClasses().color32, w * h);
		ColorRGBA32* firstElement = Scripting::GetScriptingArrayStart<ColorRGBA32>(colors);
		self->GetPixels32( miplevel, firstElement, w*h);
		return colors;
	}

	CUSTOM void Apply (bool updateMipmaps=true, bool makeNoLongerReadable=false)
	{
		CHECK_READABLE
		self->Apply(updateMipmaps, makeNoLongerReadable);
	}

	CUSTOM public bool Resize (int width, int height, TextureFormat format, bool hasMipMap)
	{
		return self->ResizeWithFormat (width, height, format, hasMipMap ? Texture2D::kMipmapMask : Texture2D::kNoMipmap);
	}

	CSRAW public bool Resize (int width, int height) { return Internal_ResizeWH(width, height); }
	CUSTOM private bool Internal_ResizeWH (int width, int height) {
		CHECK_READABLE
		return self->Resize(width, height);
	}

	AUTO void Compress (bool highQuality);

	CUSTOM Rect[] PackTextures( Texture2D[] textures, int padding, int maximumAtlasSize = 2048, bool makeNoLongerReadable = false )
	{
		int textureCount = GetScriptingArraySize(textures);
		Texture2D** texturePtrs = new Texture2D*[textureCount];
		for( int i = 0; i < textureCount; ++i )
		{
			Texture2D* tex = ScriptingObjectToObject<Texture2D>(Scripting::GetScriptingArrayElementNoRef<ScriptingObjectPtr>(textures,i));
			if (tex && !tex->GetIsReadable())
			{
				ErrorString("Texture atlas needs textures to have Readable flag set!");
				tex = NULL;
			}
			texturePtrs[i] = tex;
		}

		ScriptingArrayPtr rects = CreateScriptingArray<Rectf>(GetScriptingManager().GetCommonClasses().rect, textureCount);
		Rectf* firstElement = Scripting::GetScriptingArrayStart<Rectf>(rects);

		if( !PackTextureAtlasSimple( &*self, maximumAtlasSize, textureCount, texturePtrs, firstElement, padding, true, makeNoLongerReadable ) )
		{
			delete[] texturePtrs; // TODO: error
			return SCRIPTING_NULL;
		}

		delete[] texturePtrs;
		return rects;
	}

	CUSTOM void ReadPixels (Rect source, int destX, int destY, bool recalculateMipMaps = true)
	{
		CHECK_READABLE
		CheckScreenReadAllowedAndThrow();
		bool flipVertical = GetGfxDevice().GetInvertProjectionMatrix();
		self->ReadPixels (0, (int)source.x, (int)source.y, (int)source.Width(), (int)source.Height(), destX, destY, flipVertical, recalculateMipMaps);
	}

	CUSTOM byte[] EncodeToPNG ()
	{
	#if ENABLE_PNG_JPG
		CHECK_READABLE
		CheckReadAllowedAndThrow(self);

		Texture2D* tex = self;
		if(!tex)
			return SCRIPTING_NULL;

		dynamic_array<UInt8> buffer;
		if(!tex->EncodeToPNG(buffer))
			return SCRIPTING_NULL;
		return CreateScriptingArray<UInt8>(&buffer[0], buffer.size(), GetScriptingManager().GetCommonClasses().byte);
	#else
		return SCRIPTING_NULL;
	#endif
		}

	CUSTOM byte[] EncodeToJPG(int quality)
	{
	#if ENABLE_PNG_JPG
		CHECK_READABLE
		CheckReadAllowedAndThrow(self);

		Texture2D* tex = self;
		if(!tex)
			return SCRIPTING_NULL;

		quality = quality < 0 ? 0 : quality;
		quality = quality > 100 ? 100 : quality;

		dynamic_array<UInt8> buffer;
		if(!tex->EncodeToJPG(buffer, quality))
			return SCRIPTING_NULL;
		return CreateScriptingArray<UInt8>(&buffer[0], buffer.size(), GetScriptingManager().GetCommonClasses().byte);
	#else
		return SCRIPTING_NULL;
	#endif
	}

	CSRAW public byte[] EncodeToJPG()
	{
		return EncodeToJPG(75);
	}

	CONDITIONAL UNITY_EDITOR
	AUTO_PROP bool alphaIsTransparency GetAlphaIsTransparency SetAlphaIsTransparency

*/
}
}
