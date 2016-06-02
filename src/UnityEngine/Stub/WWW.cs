using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;


public class WWW : IDisposable
{
	public void Dispose ()
	{
	//	DestroyWWW(true);
	}

	public WWW(string url) {

	}

	// Creates a WWW request with the given URL.
	public WWW(string url, WWWForm form ) {

	}


	// We are matching the WWW class here so we can directly access it.

	/*
	#if ENABLE_WWW
	inline WWW* GetWWWChecked (ScriptingObjectWithIntPtrField<WWW>& self)
	{
		WWW* www = self.GetPtr();
		
		if (!www)
			Scripting::RaiseNullException("WWW class has already been disposed.");

		return www;
	}
	#endif

	C++RAW
 #define GET GetWWWChecked (self)

	//*undocumented*
	CSRAW public void Dispose ()
	{
		DestroyWWW(true);
	}

	CSRAW ~WWW() {
		DestroyWWW(false);
	}

	//*undocumented*
	THREAD_SAFE
	CUSTOM private void DestroyWWW(bool cancel) {
		WWW* www = self.GetPtr();

	// no-op if already 0
		self.SetPtr(0);

		if (www)
		{
			if (cancel)
				www->Cancel();
			www->Release();
		}
	}

	//*undocumented*
	CUSTOM void InitWWW( string url , byte[] postData, string[] iHeaders ) {
		string cpp_string = url;
		map<string,string> headers;
		// Copy in headers from the MonoArray if available
		int headersSize = GetScriptingArraySize(iHeaders);
		for(int i=0; i < headersSize-1 ; i += 2) {
			headers[scripting_cpp_string_for(Scripting::GetScriptingArrayElementNoRef<ScriptingStringPtr>(iHeaders,i))] = scripting_cpp_string_for(Scripting::GetScriptingArrayElementNoRef<ScriptingStringPtr>(iHeaders,i+1));
		}
		int rawPostDataLength = -1;
		char* rawPostDataPtr = NULL;
		if(postData != SCRIPTING_NULL) {
			rawPostDataPtr = Scripting::GetScriptingArrayStart<char>(postData); // Will be copied by WWW::Create
			rawPostDataLength = GetScriptingArraySize(postData);
		}
		WWW* www = WWW::Create (cpp_string.c_str(), rawPostDataPtr, rawPostDataLength, headers);
		self.SetPtr(www);
	}

	// Creates a WWW request with the given URL.
	CSRAW public WWW(string url) {
		InitWWW(url, null, null);
	}

	// Creates a WWW request with the given URL.
	CSRAW public WWW(string url, WWWForm form ) {
		string[] flattenedHeaders = FlattenedHeadersFrom(form.headers);

		#if UNITY_WEBPLAYER || UNITY_EDITOR
		if(enforceWebSecurityRestrictions()) {
			CheckSecurityOnHeaders(flattenedHeaders);
		}
		#endif

		InitWWW(url, form.data, flattenedHeaders);
		}

	// Creates a WWW request with the given URL.
	CSRAW public WWW(string url, byte[] postData) {
		InitWWW(url, postData, null);
	}

	CSRAW #if !UNITY_METRO_API && !UNITY_WP8_API || UNITY_BB10_API
	OBSOLETE error This overload is deprecated. Use the one with Dictionary argument.
	CSRAW public WWW(string url, byte[] postData, Hashtable headers ) { Debug.LogError("This overload is deprecated. Use the one with Dictionary argument.");	}
	CSRAW #endif

	// Creates a WWW request with the given URL.
	CSRAW public WWW(string url, byte[] postData, Dictionary<string, string> headers ) {
		string[] flattenedHeaders = FlattenedHeadersFrom(headers);

		#if UNITY_WEBPLAYER || UNITY_EDITOR
		if(enforceWebSecurityRestrictions()) {
			CheckSecurityOnHeaders(flattenedHeaders);
		}
		#endif

		InitWWW(url, postData, flattenedHeaders);
		}
	
	CUSTOM internal bool enforceWebSecurityRestrictions() {
		#if UNITY_WEBPLAYER
		return true;
		#elif UNITY_EDITOR
		BuildTargetPlatform buildTarget = GetEditorUserBuildSettings().GetActiveBuildTarget();
		return  buildTarget == kBuildWebPlayerLZMA || buildTarget == kBuildWebPlayerLZMAStreamed;
		#else
		return false;
		#endif
	}

	// Encodes string into an URL-friendly format.
	
	CSRAW
	public static string EscapeURL(string s, Encoding e=System.Text.Encoding.UTF8)
	{
		if (s == null)
			return null;

		if (s == "")
			return "";

		if (e == null)
			return null;

		return WWWTranscoder.URLEncode(s,e);
	}

	// Decodes string from an URL-friendly format.
	
	CSRAW
	public static string UnEscapeURL(string s, Encoding e=System.Text.Encoding.UTF8)
	{
		if (null == s)
			return null;

		if (s.IndexOf ('%') == -1 && s.IndexOf ('+') == -1)
			return s;

		return WWWTranscoder.URLDecode(s,e);

	}


	// The headers from the HTTP response.
	
	CSRAW
	public System.Collections.Generic.Dictionary<string,string> responseHeaders
	{
		get {
			if (!isDone) throw new UnityException("WWW is not finished downloading yet");
			return ParseHTTPHeaderString(responseHeadersString);
		}
	}

	CUSTOM_PROP private string responseHeadersString
	{
		WWW& www = *GET;

		ScriptingStringPtr res = scripting_string_new(www.GetResponseHeaders().c_str());
		return res;
	}


	// Returns the contents of the fetched web page as a string (RO).
	CSRAW public string text { get {
			if (!isDone) throw new UnityException("WWW is not ready downloading yet");
			var myBytes = bytes;
			return GetTextEncoder().GetString(myBytes, 0, myBytes.Length);
		}
	}
	CSRAW internal static System.Text.Encoding DefaultEncoding
	{
		get
		{
		#if UNITY_WINRT
			return System.Text.Encoding.UTF8;
		#else
			return System.Text.Encoding.ASCII;
		#endif
		}
	}

	CSRAW private Encoding GetTextEncoder()
	{
		// Check for charset type
		string contentType=null;
		if (responseHeaders.TryGetValue("CONTENT-TYPE",out contentType))
		{
			int charsetKeyIndex = contentType.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
			if (charsetKeyIndex > -1)
			{
				int charsetValueIndex = contentType.IndexOf('=', charsetKeyIndex);
				if (charsetValueIndex > -1)
				{
					string encoding = contentType.Substring(charsetValueIndex + 1).Trim().Trim(new []{'\'', '"'}).Trim();
					int semicolonIndex = encoding.IndexOf(';');
					if (semicolonIndex > -1)
						encoding = encoding.Substring(0, semicolonIndex);
					try
					{
						return System.Text.Encoding.GetEncoding(encoding);
					} catch (Exception) {
						Debug.Log("Unsupported encoding: '" + encoding + "'");
					}
				}
			}
		}
		// Use default (utf8)
		return System.Text.Encoding.UTF8;
	}

	FLUSHCONDITIONS

	OBSOLETE warning Please use WWW.text instead
	CSRAW public string data { get { return text; } }

	// Returns the contents of the fetched web page as a byte array (RO).
	CUSTOM_PROP byte[] bytes {
		WWW& www = *GET;
		if (www.GetType() == kWWWTypeCached)
		{
			ErrorString(kWWWCachedAccessError);
			return SCRIPTING_NULL;
		}
		if (www.GetSecurityPolicy() != WWW::kSecurityPolicyAllowAccess)
			Scripting::RaiseSecurityException("No valid crossdomain policy available to allow access");

		if (!www.GetError () && !www.HasDownloadedOrMayBlock ())
			return CreateEmptyStructArray(GetScriptingManager().GetCommonClasses().byte);

		return CreateScriptingArray<UInt8>( www.GetData(), www.GetSize(), GetScriptingManager().GetCommonClasses().byte );
	}

	//*undocumented* OBSOLETE Can do the same with bytes.Length
	CUSTOM_PROP int size {
		WWW& www = *GET;
		if (!www.HasDownloadedOrMayBlock ())
			return 0;
		return www.GetSize();
	}

	//*undocumented*
	CONDITIONAL UNITY_WINRT
	CUSTOM private IntPtr GetError()
	{
		WWW *www = GET;
		if (www)
			return (void*)self->GetError();
		else
			return (void*)kWWWErrCancelled;
	}
	
	// Returns an error message if there was an error during the download (RO).
	CSRAW
	#if UNITY_WINRT
	public string error
	{
		get
		{
			return Marshal.PtrToStringAnsi(GetError());
		}
	}
	#endif

	// Returns an error message if there was an error during the download (RO).
	CONDITIONAL !UNITY_WINRT
	CUSTOM_PROP string error {
		WWW *www = GET;
		if (www)
		{
			const char* e = self->GetError();
			if (e)
				return scripting_string_new(e);
			else
				return SCRIPTING_NULL;
		}
		else
			return scripting_string_new(kWWWErrCancelled);
	}

	//*undocumented*
	CUSTOM private Texture2D GetTexture(bool markNonReadable)
	{
		WWW& www = *GET;
		if (www.GetType() == kWWWTypeCached)
		{
			ErrorString(kWWWCachedAccessError);
			return SCRIPTING_NULL;
		}

		// create the texture
		Texture2D* tex = CreateObjectFromCode<Texture2D>();

		if (www.HasDownloadedOrMayBlock ())
		{
			LoadMemoryBufferIntoTexture( *tex, self->GetData(), self->GetSize(), kLoadImageUncompressed, markNonReadable );
			WWW::SecurityPolicy policy = self->GetSecurityPolicy();
			if (policy != WWW::kSecurityPolicyAllowAccess)
				tex->SetReadAllowed(false);
		}
		else
		{
			LoadMemoryBufferIntoTexture( *tex, NULL, 0, kLoadImageUncompressed, markNonReadable );
		}

		return Scripting::ScriptingWrapperFor(tex);
	}

	// Returns a [[Texture2D]] generated from the downloaded data (RO).
	CSRAW public Texture2D texture { get { return GetTexture(false); } }

	// Returns a non-readable [[Texture2D]] generated from the downloaded data (RO).
	CSRAW public Texture2D textureNonReadable { get { return GetTexture(true); } }

	// Returns a [[AudioClip]] generated from the downloaded data (RO).

	CONDITIONAL ENABLE_AUDIO
	CSRAW public AudioClip audioClip {
		get { return GetAudioClip(true); }
	}

	FLUSHCONDITIONS

	/// *listonly*
	CONDITIONAL ENABLE_AUDIO
	CSRAW public AudioClip GetAudioClip(bool threeD)
	{
		return GetAudioClip(threeD, false);
	}
	
	/// *listonly*
	CONDITIONAL ENABLE_AUDIO
	CSRAW public AudioClip GetAudioClip(bool threeD, bool stream)
	{
		return GetAudioClip(threeD, stream, AudioType.UNKNOWN);
	}

	// Returns an [[AudioClip]] generated from the downloaded data (RO).
	CONDITIONAL ENABLE_AUDIO
	CUSTOM public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
	{
		IAudio *audio = GetIAudio();
		if(audio == NULL)
			return SCRIPTING_NULL;

		WWW& www = *GET;
		if (www.GetType() == kWWWTypeCached)
		{
			ErrorString(kWWWCachedAccessError);
			return SCRIPTING_NULL;
		}

		AudioClip* clip = audio->CreateAudioClipFromWWW(*GET, stream, audioType);
		return Scripting::ScriptingWrapperFor ((Object*)clip);
	}


	// Returns a [[MovieTexture]] generated from the downloaded data (RO).
	CONDITIONAL ENABLE_MOVIES
	CUSTOM_PROP MovieTexture movie
	{

		WWW& www = *GET;
		if (www.GetType() == kWWWTypeCached)
		{
			ErrorString(kWWWCachedAccessError);
			return SCRIPTING_NULL;
		}
		#if UNITY_EDITOR
		if (!LicenseInfo::Flag(lf_pro_version))
		{
			ErrorString("Movie playback is only possible with Unity Pro");
			return SCRIPTING_NULL;
		}
		#endif

		MovieTexture* tex = NULL;
		IAudio *audio = GetIAudio();
		if(audio)
			tex = audio->CreateMovieTextureFromWWW(*GET);
			

		//Not applying any security restrictions here, because currently our API does not allow grabbing samples from the movieclip
		//It's impossible for a hacker to get his hands on the data, or xfer it somewhere. When we start supporting this, we should
		//add a crossdomain securitycheck here.
		return Scripting::ScriptingWrapperFor ((Object*)tex);
	}

	// Replaces the contents of an existing [[Texture2D]] with an image from the downloaded data.
	CUSTOM void LoadImageIntoTexture(Texture2D tex) {
		WWW& www = *GET;
		if (www.GetType() == kWWWTypeCached)
		{
			ErrorString(kWWWCachedAccessError);
			return;
		}

		if (!www.HasDownloadedOrMayBlock ())
			return;

		LoadMemoryBufferIntoTexture( *tex, www.GetData(), www.GetSize(), IsCompressedDXTTextureFormat(tex->GetTextureFormat())?kLoadImageDXTCompressDithered:kLoadImageUncompressed );
		if (www.GetSecurityPolicy() != WWW::kSecurityPolicyAllowAccess) tex->SetReadAllowed(false);
	}

	// Is the download already finished? (RO)
	CUSTOM_PROP bool isDone {
		return (short)GET->IsDone();
	}

	CONDITIONAL !UNITY_WEBGL && !UNITY_WINRT
	OBSOLETE error All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.
	CUSTOM static string GetURL(string url) {
		map<string,string> headers;
		const char* c_string = url.AsUTF8().c_str();
		WWW* fetcher = WWW::Create(c_string, NULL, -1, headers);

		ScriptingStringPtr result;

		if (fetcher->GetSecurityPolicy() != WWW::kSecurityPolicyAllowAccess)
		{
			Scripting::RaiseSecurityException("No valid crossdomain policy available to allow access");
			return scripting_string_new("");
		}

		if (!fetcher->HasDownloadedOrMayBlock ())
			result = scripting_string_new("");
		else
			result = scripting_string_new((char*)fetcher->GetData(), fetcher->GetSize());

		fetcher->Release();
		return result;
	}

	FLUSHCONDITIONS

	CSRAW
#if !UNITY_WEBGL && !UNITY_WINRT && !ENABLE_IL2CPP
	OBSOLETE error All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.
#endif
	public static Texture2D GetTextureFromURL(string url) {
		return new WWW(url).texture;
	}

	// How far has the download progressed (RO).
	CUSTOM_PROP float progress
	{
		return GET->GetProgress();
	}

	// How far has the upload progressed (RO).
	CUSTOM_PROP float uploadProgress
	{
		return GET->GetUploadProgress();
	}
	
	// How many bytes have been downloaded (RO).
	CUSTOM_PROP int bytesDownloaded
	{
		return GET->GetSize();
	}

	CONDITIONAL !UNITY_FLASH && !UNITY_WINRT && ENABLE_AUDIO
	CSRAW [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	OBSOLETE error Property WWW.oggVorbis has been deprecated. Use WWW.audioClip instead (UnityUpgradable).
	CSRAW public AudioClip oggVorbis { get { return default(AudioClip); } }

	// Loads the new web player data file.
	CUSTOM void LoadUnityWeb ()
	{
		WWW& www = *GET;
		if (!www.HasDownloadedOrMayBlock ())
			return;

		if (www.GetSecurityPolicy() != WWW::kSecurityPolicyAllowAccess)
		{
			Scripting::RaiseSecurityException("No valid crossdomain policy available to allow access");
			return; //is this return required?
		}

		#if WEBPLUG
		QueuePlayerLoadWebData (www.GetUnityWebStream());
		www.GetUnityWebStream()->RetainDownload (&www);
		#else
		LogString (Format("Requested loading unity web file %s. This will only be loaded in the web player.", www.GetUrl ()));
		#endif
	}

	// The URL of this WWW request (RO).
	CUSTOM_PROP string url { return scripting_string_new(GET->GetUrl()); }

	// Streams an AssetBundle that can contain any kind of asset from the project folder.
	CUSTOM_PROP AssetBundle assetBundle
	{
		return Scripting::ScriptingWrapperFor (ExtractAssetBundle (*GET));
	}

	// Priority of [[AssetBundle]] decompression thread.
	// SA: [[ThreadPriority]] enum.
	CONDITIONAL !UNITY_WEBGL
	CUSTOM_PROP ThreadPriority threadPriority { return GET->GetThreadPriority(); }  { GET->SetThreadPriority(value); }

	CUSTOM internal WWW (string url, Hash128 hash, uint crc)
	{
		string cpp_string = url;
		#if ENABLE_CACHING
		WWW* www = new WWWCached(cpp_string.c_str(), hash, crc);
		#else
		WWW* www = WWW::Create (cpp_string.c_str(), NULL, 0, WWW::WWWHeaders(), true, false, Hash128(), crc);
		#endif
		self.SetPtr(www);
	}

	// Loads an AssetBundle with the specified version number from the cache.
	// If the AssetBundle is not currently cached, it will automatically be downloaded and stored in the cache for future retrieval from local storage.
	CSRAW public static WWW LoadFromCacheOrDownload( string url, int version, uint crc = 0) {
		Hash128 tempHash = new Hash128(0, 0, 0, (uint)version);
		return LoadFromCacheOrDownload(url, tempHash, crc);
	}

	// Loads an AssetBundle with the specified hash from the cache.
	// If the AssetBundle is not currently cached, it will automatically be downloaded and stored in the cache for future retrieval from local storage.
	CSRAW public static WWW LoadFromCacheOrDownload( string url, Hash128 hash, uint crc = 0) {
		return new WWW(url, hash, crc);
	}
	C++RAW
 #undef GET
 	*/
}
