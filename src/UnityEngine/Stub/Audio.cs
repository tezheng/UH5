using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngineInternal;

namespace UnityEngine
{



// These are speaker types defined for use with [[AudioSettings.speakerMode]].
enum AudioSpeakerMode
{
	// Channel count is unaffected.
	Raw = 0,
	// Channel count is set to 1. The speakers are monaural.
	Mono = 1,
	// Channel count is set to 2. The speakers are stereo. This is the editor default.
	Stereo = 2,
	// Channel count is set to 4. 4 speaker setup. This includes front left, front right, rear left, rear right.
	Quad = 3,
	// Channel count is set to 5. 5 speaker setup. This includes front left, front right, center, rear left, rear right.
	Surround = 4,
	// Channel count is set to 6. 5.1 speaker setup. This includes front left, front right, center, rear left, rear right and a subwoofer.
	Mode5point1 = 5,
	// Channel count is set to 8. 7.1 speaker setup. This includes front left, front right, center, rear left, rear right, side left, side right and a subwoofer.
	Mode7point1 = 6,
	// Channel count is set to 2. Stereo output, but data is encoded in a way that is picked up by a Prologic/Prologic2 decoder and split into a 5.1 speaker setup.
	Prologic = 7
}


enum AudioDataLoadState
{
	Unloaded = 0,
	Loading = 1,
	Loaded = 2,
	Failed = 3
}

/*

// Controls the global audio settings from script.
CLASS AudioSettings

	// Returns the speaker mode capability of the current audio driver. (RO)
	CUSTOM_PROP static AudioSpeakerMode driverCapabilities
	{
		return GetAudioManager().GetSpeakerModeCaps();
	}

	OBSOLETE error driverCaps is obsolete. Use driverCapabilities instead (UnityUpgradable).
	CUSTOM_PROP static AudioSpeakerMode driverCaps
	{
		return GetAudioManager().GetSpeakerModeCaps();
	}

	// Gets the current speaker mode. Default is 2 channel stereo.
	CUSTOM_PROP static AudioSpeakerMode speakerMode
	{
		return GetAudioManager().GetSpeakerMode();
	}
	{
		ErrorString("Setting the speaker mode from script is no longer supported. The speaker mode has to be set up in the project settings and serves only as a reference with no guarantee that the target actually supports this. The actual audio rate used must always be queried via AudioSettings.GetDSPBufferSize.");
	}

	// Returns the current time of the audio system. This is based on the number of samples the audio system processes and is therefore more exact than the time obtained via the Time.time property.
	// It is constant while Unity is paused.
	THREAD_SAFE CUSTOM_PROP static double dspTime
	{
		return GetAudioManager().GetDSPTime();
	}

	// Get and set the mixer's current output rate.
	CUSTOM_PROP static int outputSampleRate
	{
		int sampleRate;
		GetAudioManager().GetFMODSystem()->getSoftwareFormat(
			&sampleRate,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL
		);
		return sampleRate;
	}
	{
		ErrorString("Setting AudioSettings.outputSampleRate from script is no longer supported. The desired output rate has to be set up in the project settings and serves only as a reference with no guarantee that the target actually supports this. The actual audio rate used must always be queried via AudioSettings.outputSampleRate.");
	}

	// Get or set the mixer's buffer size in samples.
	CUSTOM static void GetDSPBufferSize(out int bufferLength, out int numBuffers)
	{
		CheckFMODError(GetAudioManager().GetFMODSystem()->getDSPBufferSize((unsigned int*)bufferLength, numBuffers));
	}

	OBSOLETE warning Setting the DSP buffer size from script is no longer supported. The desired buffer size has to be set up in the project settings and serves only as a reference with no guarantee that the target actually supports this. The actual audio rate used must always be queried via AudioSettings.GetDSPBufferSize.
	CUSTOM static void SetDSPBufferSize(int bufferLength, int numBuffers)
	{
		ErrorString("Setting the DSP buffer size from script is no longer supported. The desired buffer size has to be set up in the project settings and serves only as a reference with no guarantee that the target actually supports this. The actual audio rate used must always be queried via AudioSettings.GetDSPBufferSize.");
	}

	CONDITIONAL UNITY_EDITOR
	CUSTOM_PROP internal static bool editingInPlaymode 
	{
		return GetAudioManager().IsEditingInPlaymode ();
	}
	{
		GetAudioManager().SetEditingInPlaymode (value);
	}

END

*/

// Not used by new AudioClip, but still reguired for WWW.getAudioClip

// Type of the imported(native) data
enum AudioType
{
	// 3rd party / unknown plugin format.
	UNKNOWN = 0,
	//acc - not supported
	ACC = 1,			 /* [Unity] Not supported/used. But kept here to keep the order of the enum in sync. */
	//aiff
	AIFF = 2,
//  ASF = 3,             /* Microsoft Advanced Systems Format (ie WMA/ASF/WMV). */
//  AT3 = 4,             /* Sony ATRAC 3 format */
//  CDDA = 5,            /* Digital CD audio. */
//  DLS = 6,             /* Sound font / downloadable sound bank. */
//  FLAC = 7,            /* FLAC lossless codec. */
//  FSB = 8,             /* FMOD Sample Bank. */
	//game cube ADPCM
//    GCADPCM = 9,
	//impulse tracker
	IT = 10,
//  MIDI = 11,            /* MIDI. */
	//Protracker / Fasttracker MOD.
	MOD = 12,
	//MP2/MP3 MPEG.
	MPEG = 13,
	//ogg vorbis
	OGGVORBIS = 14,
//  PLAYLIST = 15,        /* Information only from ASX/PLS/M3U/WAX playlists */
//  RAW = 16,             /* Raw PCM data. */
	// ScreamTracker 3.
	S3M = 17,
//  SF2 = 18,             /* Sound font 2 format. */
//  USER = 19,            /* User created sound. */
	//Microsoft WAV.
	WAV = 20,
	// FastTracker 2 XM.
	XM = 21,
	// Xbox360/XboxOne XMA(2)
	XMA = 22,
	VAG = 23,             /* PlayStation 2 / PlayStation Portable adpcm VAG format. */
	//iPhone hardware decoder, supports AAC, ALAC and MP3. Extracodecdata is a pointer to an FMOD_AUDIOQUEUE_EXTRACODECDATA structure.
	AUDIOQUEUE = 24,
//  XWMA = 25,            /* Xbox360 XWMA */
//  BCWAV = 26,           /* 3DS BCWAV container format for DSP ADPCM and PCM */
//  AT9 = 27,             /* NGP ATRAC 9 format */

	// XBONE TODO: these are supported on xbone in hardware, do we care? XMA and XWMA are above and supported in hardware by xbone
	//PCM = 28,
	//ADPCM = 29,


}

// Imported audio format for [[AudioImporter]].
enum AudioCompressionFormat
{
	PCM = 0,
	Vorbis = 1,
	ADPCM = 2,
	MP3 = 3,
	VAG = 4,
	HEVAG = 5
}

// The way we load audio assets [[AudioImporter]].
enum AudioClipLoadType
{
	DecompressOnLoad = 0,
	CompressedInMemory = 1,
	Streaming = 2
}

public class AudioClip : Object
{
	internal string guid;
/*
	// Check if reading of the audioclip is allowed by crossdomain security and throw if not
	C++RAW
 static void CheckReadAllowedAndThrow(AudioClip *clip)
	{
#if ENABLE_MONO && ENABLE_SECURITY && ENABLE_WWW
		if ( clip&&!clip->GetReadAllowed() )
			Scripting::RaiseSecurityException("No read access to the audioclip data: %s", clip->GetName());
#endif
	}

	// The length of the audio clip in seconds (RO)
	AUTO_PROP float length GetLengthSec

	// The length of the audio clip in samples (RO)
	// Prints how many samples the attached audio source has
	//
	AUTO_PROP int samples GetSampleCount

	// Channels in audio clip (RO)
	AUTO_PROP int channels GetChannelCount

	// Sample frequency (RO)
	AUTO_PROP int frequency GetFrequency

	// Is a streamed audio clip ready to play? (RO)
	OBSOLETE warning Use AudioClip.loadState instead to get more detailed information about the loading process.
	AUTO_PROP bool isReadyToPlay ReadyToPlay

	// AudioClip load type (RO)
	CUSTOM_PROP AudioClipLoadType loadType { return self->GetLoadType(); }

	CUSTOM bool LoadAudioData()
	{
		return self->LoadAudioData();
	}

	CUSTOM bool UnloadAudioData()
	{
		return self->UnloadAudioData();
	}

	CUSTOM_PROP bool preloadAudioData
	{
		return self->GetPreloadAudioData();
	}

	CUSTOM_PROP AudioDataLoadState loadState
	{
		if(self->IsLegacyFormat())
			return self->ReadyToPlay() ? Loaded : Unloaded;

		switch(self->GetLoadState())
		{
			case SoundHandleShared::kUnloaded:    return Unloaded;
			case SoundHandleShared::kLoadingBase:
			case SoundHandleShared::kLoadingSub:  return Loading;
			case SoundHandleShared::kLoaded:      return Loaded;
			case SoundHandleShared::kFailed:      return Failed;
			default: break;
		}
		return Unloaded;
	}

	CUSTOM_PROP bool loadInBackground
	{
		return self->GetLoadInBackground();
	}

	// Fills an array with sample data from the clip. The samples are floats ranging from -1.0f to 1.0f. The sample count is determined by the length of the float array.
	CUSTOM bool GetData(float[] data, int offsetSamples)
	{
		CheckReadAllowedAndThrow(self);
		int numChannels = self->GetChannelCount();
		if(numChannels <= 0)
		{
			ErrorStringObject(Format("AudioClip.GetData failed; AudioClip %s contains no data", self->GetName()), self);
			return false;
		}
		return self->GetData(&Scripting::GetScriptingArrayElement<float>(data, 0), GetScriptingArraySize (data) / numChannels, offsetSamples);
	}

	// Set sample data in a clip. The samples should be floats ranging from 0.0f to 1.0f (exceeding these limits will lead to artifacts and undefined behaviour).
	CUSTOM bool SetData(float[] data, int offsetSamples)
	{
		int numChannels = self->GetChannelCount();
		if(numChannels <= 0)
		{
			ErrorStringObject(Format("AudioClip.GetData failed; AudioClip %s contains no data", self->GetName()), self);
			return false;
		}

		if(offsetSamples < 0 || offsetSamples >= (int)self->GetSampleCount())
		{
			Scripting::RaiseArgumentException("AudioClip.SetData failed; invalid offsetSamples");
			return false;
		}

		int arraySize = GetScriptingArraySize (data);
		if(arraySize == 0)
		{
			Scripting::RaiseArgumentException("AudioClip.SetData failed; invalid data");
			return false;
		}

		return self->SetData(&Scripting::GetScriptingArrayElement<float>(data, 0), arraySize / numChannels, offsetSamples);
	}

	/// *listonly*
	OBSOLETE warning The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.
	CSRAW public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream)
	{
		return Create (name, lengthSamples, channels, frequency, stream);
	}

	OBSOLETE warning The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.
	CSRAW public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, PCMReaderCallback pcmreadercallback)
	{
		return Create (name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
	}

	OBSOLETE warning The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.
	CSRAW public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, PCMReaderCallback pcmreadercallback, PCMSetPositionCallback pcmsetpositioncallback)
	{
		return Create (name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback);
	}

	CSRAW public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
	{
		AudioClip clip = Create (name, lengthSamples, channels, frequency, stream, null, null);
		return clip;
	}

	/// *listonly*
	CSRAW public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, PCMReaderCallback pcmreadercallback)
	{
		AudioClip clip = Create (name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		return clip;
	}

	// Creates a user AudioClip with a name and with the given length in samples, channels and frequency.
	CSRAW public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, PCMReaderCallback pcmreadercallback, PCMSetPositionCallback pcmsetpositioncallback)
	{
		if(name == null) throw new NullReferenceException();
		if (lengthSamples <= 0) throw new ArgumentException ("Length of created clip must be larger than 0");
		if (channels <= 0) throw new ArgumentException ("Number of channels in created clip must be greater than 0");
		if (frequency <= 0) throw new ArgumentException ("Frequency in created clip must be greater than 0");

		AudioClip clip = Construct_Internal();
		if ( pcmreadercallback != null)
			clip.m_PCMReaderCallback += pcmreadercallback;
		if ( pcmsetpositioncallback != null)
			clip.m_PCMSetPositionCallback += pcmsetpositioncallback;

		clip.Init_Internal( name, lengthSamples, channels, frequency, stream );

		return clip;
	}

	/// *listonly*
	CSRAW public delegate void PCMReaderCallback(float[] data);
	CSRAW private event PCMReaderCallback m_PCMReaderCallback = null;
	/// *listonly*
	CSRAW public delegate void PCMSetPositionCallback(int position);
	CSRAW private event PCMSetPositionCallback m_PCMSetPositionCallback = null;

	CSRAW private void InvokePCMReaderCallback_Internal(float[] data)
	{
		if (m_PCMReaderCallback != null)
			m_PCMReaderCallback( data );
	}

	CSRAW private void InvokePCMSetPositionCallback_Internal(int position)
	{
		if (m_PCMSetPositionCallback != null)
			m_PCMSetPositionCallback( position );
	}

	CUSTOM private static AudioClip Construct_Internal()
	{
		AudioClip* clip = NEW_OBJECT(AudioClip);
		return Scripting::ScriptingWrapperFor ( clip );
	}

	CUSTOM private void Init_Internal(string name, int lengthSamples, int channels, int frequency, bool stream)
	{
		self->CreateUserSound( name, lengthSamples, channels, frequency, stream );
	}

*/

}

// Describes when an [[AudioSource]] or [[AudioListener]] is updated.
enum AudioVelocityUpdateMode
{
	// Updates the source or listener in the fixed update loop if it is attached to a [[Rigidbody]], dynamic otherwise.
	Auto = 0,
	// Updates the source or listener in the fixed update loop.
	Fixed = 1,
	// Updates the source or listener in the dynamic update loop.
	Dynamic = 2,
}


// Representation of a listener in 3D space.
public class AudioListener : Behaviour
{
	internal override void AddToManager ()
	{

	}

	internal override void RemoveFromManager ()
	{

	}
	/*
	// Controls the game sound volume (0.0 to 1.0)
	CUSTOM_PROP static float volume { return GetAudioManager ().GetVolume (); } { GetAudioManager ().SetVolume (value); }

	// The paused state of the audio. If set to True, the listener will not generate sound.
	CUSTOM_PROP static bool pause { return GetAudioManager ().GetPause (); } { GetAudioManager ().SetPause (value); }

	// This lets you set whether the Audio Listener should be updated in the fixed or dynamic update.

	AUTO_PROP AudioVelocityUpdateMode velocityUpdateMode GetVelocityUpdateMode SetVelocityUpdateMode

	CUSTOM static private void GetOutputDataHelper(float[] samples, int channel)
	{
		FMOD::ChannelGroup* channelGroup;
		FMOD_RESULT result = GetAudioManager().GetFMODSystem()->getMasterChannelGroup(&channelGroup);

		if (result == FMOD_OK && channelGroup)
		{
			int numChannels = 0;
			result = GetAudioManager().GetFMODSystem()->getSoftwareFormat(NULL, NULL, &numChannels, NULL, NULL, NULL);
			if (result == FMOD_OK && 0 <= channel && channel < numChannels)
				channelGroup->getWaveData(Scripting::GetScriptingArrayStart<float>(samples), GetScriptingArraySize(samples), channel);
			else
				Scripting::RaiseArgumentException("AudioListener.GetOutputDataHelper failed; invalid channel");
		}
	}

	CUSTOM static private void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window)
	{
		FMOD::ChannelGroup* channelGroup;
		FMOD_RESULT result = GetAudioManager().GetFMODSystem()->getMasterChannelGroup(&channelGroup);

		if (result == FMOD_OK && channelGroup)
		{
			int numChannels = 0;
			result = GetAudioManager().GetFMODSystem()->getSoftwareFormat(NULL, NULL, &numChannels, NULL, NULL, NULL);
			if (result == FMOD_OK && 0 <= channel && channel < numChannels)
				channelGroup->getSpectrum(Scripting::GetScriptingArrayStart<float>(samples), GetScriptingArraySize(samples), channel, (FMOD_DSP_FFT_WINDOW)window);
			else
				Scripting::RaiseArgumentException("AudioListener.GetSpectrumDataHelper failed; invalid channel");
		}
	}

	// Returns a block of the listener (master)'s output data
	CSRAW
	OBSOLETE warning GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.
	public static float[] GetOutputData(int numSamples, int channel)
	{
		float[] samples = new float[numSamples];
		GetOutputDataHelper(samples, channel);
		return samples;
	}

	// Returns a block of the listener (master)'s output data
	CSRAW static public void GetOutputData(float[] samples, int channel)
	{
		GetOutputDataHelper(samples, channel);
	}

	// Returns a block of the listener (master)'s spectrum data
	CSRAW
	OBSOLETE warning GetSpectrumData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.
	public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
	{
		float[] samples = new float[numSamples];
		GetSpectrumDataHelper(samples, channel, window);
		return samples;
	}

	// Returns a block of the listener (master)'s spectrum data
	CSRAW static public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
	{
		GetSpectrumDataHelper(samples, channel, window);
	}
	*/

}


// Spectrum analysis windowing types
enum FFTWindow
{
	// w[n] = 1.0
	Rectangular = 0,
	// w[n] = TRI(2n/N)
	Triangle = 1,
	// w[n] = 0.54 - (0.46 * COS(n/N) )
	Hamming = 2,
	// w[n] = 0.5 * (1.0 - COS(n/N) )
	Hanning = 3,
	// w[n] = 0.42 - (0.5 * COS(n/N) ) + (0.08 * COS(2.0 * n/N) )
	Blackman = 4,
	// w[n] = 0.35875 - (0.48829 * COS(1.0 * n/N)) + (0.14128 * COS(2.0 * n/N)) - (0.01168 * COS(3.0 * n/N))
	BlackmanHarris = 5
}

// Rolloff modes that a 3D sound can have in an audio source.
public enum AudioRolloffMode
{
	// Use this mode when you want a real-world rolloff.
	Logarithmic = 0,


	// Use this mode when you want to lower the volume of your sound over the distance
	Linear = 1,


	// Use this when you want to use a custom rolloff.
	Custom = 2
}

// A representation of audio sources in 3D.
public class AudioSource : Behaviour
{
	internal override void AddToManager ()
	{
		if (m_PlayOnAwake) {
			Play();
		}
	}

	internal override void RemoveFromManager ()
	{
		Stop();
	}

	internal AudioSource() {

	}

	internal AudioSource(UnityEngine.Config.AudioSource source) {
		mLoop = source.Loop != 0;
		mVolume = source.m_Volume;
		m_PlayOnAwake = source.m_PlayOnAwake != 0;
		clip = source.m_audioClip != null ? Resources.GetAudioClip(source.m_audioClip) : null;
	}

	private object mAudioObject;
	private extern static void InternalPlay(object audio);
	private extern static void InternalStop(object audio);
	private extern static void InternalSetVolume(object audio, float vol);
	private extern static void InternalSetLoop(object audio, bool loop);
	private extern static bool InternalIsPlaying(object audio);
	private extern static object CreateJSAudioObject(string src);

	public void PlayOneShot (AudioClip clip, float volumeScale = 1.0F)
	{
		object audio = CreateJSAudioObject(clip != null ? clip.guid : null);
		mVolume = volumeScale;
		InternalPlay(audio);
	}
	public float pitch {
		get; set;
	}
	private float mVolume = 1;
	public float volume {
		get {
			return mVolume;
		}
		set {
			mVolume = value;
			InternalSetVolume(mAudioObject, value);
		}
	}
	public bool isPlaying {
		get { return InternalIsPlaying(mAudioObject); }
	}
	public void Stop()
	{
		InternalStop(mAudioObject);
	}
	private AudioClip mClip;
	public AudioClip clip {
		get { return mClip; }
		set {
			mAudioObject = CreateJSAudioObject(value != null ? value.guid : null);
			mClip = value;
			loop = mLoop;
			volume = mVolume;
		}
	}
	public void Play()
	{
		InternalPlay(mAudioObject);
	}
	public AudioRolloffMode rolloffMode;
	public float maxDistance;
	public int priority;
	private bool m_PlayOnAwake;
	private bool mLoop;
	public bool loop {
		get {
			return mLoop;
		}
		set {
			mLoop = value;
			InternalSetLoop(mAudioObject, value);
		}
	}
	/*
	// The volume of the audio source (0.0 to 1.0)
	AUTO_PROP float volume GetVolume SetVolume

	// The pitch of the audio source.
	CUSTOM_PROP float pitch
	{
		return self->GetPitch();
	}
	{
		if(!IsFinite(value))
		{
			WarningStringObject("Attempt to set pitch to infinite value from script ignored!", self);
			return;
		}
		if(IsNAN(value))
		{
			WarningStringObject("Attempt to set pitch to NaN value from script ignored!", self);
			return;
		}
		self->SetPitch(value);
	}

	// Playback position in seconds.
	CONDITIONAL ENABLE_AUDIO
	AUTO_PROP float time GetSecPosition SetSecPosition

	// Playback position in PCM samples.
	THREAD_SAFE AUTO_PROP int timeSamples GetSamplePosition SetSamplePosition

	// The default [[AudioClip]] to play
	AUTO_PTR_PROP AudioClip clip GetAudioClip SetAudioClip

	AUTO_PTR_PROP AudioMixerGroup outputAudioMixerGroup GetOutputAudioMixerGroup SetOutputAudioMixerGroup

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool PlayOnDualShock4(Int32 userId)
	{
#if PS4_PAD_SPEAKER
		if(!self->SetPS4DualShock4OutputDevice(userId))
		{
			WarningStringObject("Attempt to play AudioSample on non-existing user's DualShock4 - ignored!", self);
			return false;
		}
		self->Play();
		return true;
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool PlayOnDualShock4PadIndex(Int32 padIndex)
	{
#if PS4_PAD_SPEAKER
		if(!self->SetPS4DualShock4OutputDevicePadIndex(padIndex))
		{
			WarningStringObject("Attempt to play AudioSample on unassigned DualShock4 pad - ignored!", self);
			return false;
		}
		self->Play();
		return true;
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool SetDualShock4PadSpeakerMixLevel(Int32 userId, Int32 mixLevel)
	{
#if PS4_PAD_SPEAKER
		return self->SetPS4DualShock4PadSpeakerMixLevel(userId, mixLevel);
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool SetDualShock4PadSpeakerMixLevelPadIndex(Int32 padIndex, Int32 mixLevel)
	{
#if PS4_PAD_SPEAKER
		return self->SetPS4DualShock4PadSpeakerMixLevelPadIndex(padIndex, mixLevel);
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool SetDualShock4PadSpeakerMixLevelDefault(Int32 userId)
	{
#if PS4_PAD_SPEAKER
		return self->SetPS4DualShock4PadSpeakerMixLevelDefault(userId);
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool SetDualShock4PadSpeakerMixLevelDefaultPadIndex(Int32 padIndex)
	{
#if PS4_PAD_SPEAKER
		return self->SetPS4DualShock4PadSpeakerMixLevelDefaultPadIndex(padIndex);
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
	CUSTOM bool SetDualShock4PadSpeakerRestrictedAudio(Int32 userId, bool restricted)
	{
#if PS4_PAD_SPEAKER
		return self->SetPS4DualShock4PadSpeakerRestrictedAudio(userId, restricted);
#else
		return false;
#endif
	}

	CONDITIONAL UNITY_PS4_API
			CUSTOM bool SetDualShock4PadSpeakerRestrictedAudioPadIndex(Int32 padIndex, bool restricted)
	{
#if PS4_PAD_SPEAKER
		return self->SetPS4DualShock4PadSpeakerRestrictedAudioPadIndex(padIndex, restricted);
#else
		return false;
#endif
	}

	// Plays the ::ref::clip with a certain delay (the optional delay argument is deprecated since 4.1a3) and the functionality has been replated by PlayDelayed.

	CUSTOM void Play (UInt64 delay=0)
	{
		if (delay > 0)
		{
			WarningStringObject("Delayed playback via the optional argument of Play is deprecated. Use PlayDelayed instead!", self);
		}
		self->Play((double)delay * (const double)(-1.0 / 44100.0));
	}

	// Plays the ::ref::clip with a delay specified in seconds. Users are advised to use this function instead of the old Play(delay) function that took a delay specified in samples relative to a reference rate of 44.1 kHz as an argument.
	CUSTOM void PlayDelayed (float delay)
	{
		self->Play((delay < 0.0f) ? 0.0 : -(double)delay);
	}

	// Schedules the ::ref::clip to play at the specified absolute time. This is the preferred way to stitch AudioClips in music players because it is independent of the frame rate and gives the audio system enough time to prepare the playback of the sound to fetch it from media where the opening and buffering takes a lot of time (streams) without causing sudden performance peaks.
	CUSTOM void PlayScheduled (double time)
	{
		self->Play((time < 0.0) ? 0.0 : time);
	}

	// Changes the time at which a sound that has already been scheduled to play will start. Notice that depending on the timing not all rescheduling requests can be fulfilled.
	CUSTOM void SetScheduledStartTime (double time)
	{
		self->SetScheduledStartTime(time);
	}

	// Changes the time at which a sound that has already been scheduled to play will end. Notice that depending on the timing not all rescheduling requests can be fulfilled.
	CUSTOM void SetScheduledEndTime (double time)
	{
		self->SetScheduledEndTime(time);
	}

	// Stops playing the ::ref::clip.
	CUSTOM void Stop()
	{
		self->Stop(true);
	}

	// Pauses playing the ::ref::clip.
	AUTO void Pause ();

	// Is the ::ref::clip playing right now (RO)?
	AUTO_PROP bool isPlaying IsPlayingScripting

	// Plays an [[AudioClip]], and scales the [[AudioSource]] volume by volumeScale.
	CONDITIONAL ENABLE_AUDIO
	CUSTOM void PlayOneShot (AudioClip clip, float volumeScale = 1.0F)	{ if (clip) self->PlayOneShot (*clip, volumeScale); }

	// Plays the clip at position. Automatically cleans up the audio source after it has finished playing.

	CONDITIONAL ENABLE_AUDIO
	CSRAW public static void PlayClipAtPoint (AudioClip clip, Vector3 position, float volume = 1.0F)
	{
		GameObject go = new GameObject ("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent (typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		source.Play ();
		// Note: timeScale > 1 means that game time is accelerated. However, the sounds play at their normal speed, so we need to postpone the point in time, when the sound is stopped.
		Destroy (go, clip.length * Time.timeScale);
	}

	// Is the audio clip looping?
	AUTO_PROP bool loop GetLoop SetLoop

	// This makes the audio source not take into account the volume of the audio listener.
	CUSTOM_PROP bool ignoreListenerVolume
	{
		return self->GetIgnoreListenerVolume();
	}
	{
		self->SetIgnoreListenerVolume(value);
	}

	// If set to true, the audio source will automatically start playing on awake
	AUTO_PROP bool playOnAwake GetPlayOnAwake SetPlayOnAwake

	// If set to true, the audio source will be playable while the AudioListener is paused
	AUTO_PROP bool ignoreListenerPause GetIgnoreListenerPause SetIgnoreListenerPause

	// Whether the Audio Source should be updated in the fixed or dynamic update.
	AUTO_PROP AudioVelocityUpdateMode velocityUpdateMode GetVelocityUpdateMode SetVelocityUpdateMode

	//Sets how a Mono or 2D sound is panned linearly to the left or right.
	AUTO_PROP float panStereo GetPan SetPan

	//Sets how much a playing sound is treated as a 3D source
	AUTO_PROP float spatialBlend GetPanLevel SetPanLevel

	//Sets how much a playing sound is mixed into the reverb zones
	AUTO_PROP float reverbZoneMix GetReverbZoneMix SetReverbZoneMix

	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	[System.Obsolete ("UnityEditor.panLevel has been deprecated. Use AudioSource.spatialBlend instead (UnityUpgradable).", true)]
	AUTO_PROP float panLevel GetPanLevel SetPanLevel

	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	[System.Obsolete ("UnityEditor.pan has been deprecated. Use AudioSource.panStereo instead (UnityUpgradable).", true)]
	AUTO_PROP float pan GetPan SetPan

	// Bypass effects
	AUTO_PROP bool bypassEffects GetBypassEffects SetBypassEffects

	// Bypass listener effects
	AUTO_PROP bool bypassListenerEffects GetBypassListenerEffects SetBypassListenerEffects
	// Bypass reverb zones
	AUTO_PROP bool bypassReverbZones GetBypassReverbZones SetBypassReverbZones

	// Sets the Doppler scale for this AudioSource
	AUTO_PROP float dopplerLevel GetDopplerLevel SetDopplerLevel

	// Sets the spread angle a 3d stereo or multichannel sound in speaker space.
	AUTO_PROP float spread GetSpread SetSpread

	// Sets the priority of the [[AudioSource]]
	AUTO_PROP int priority GetPriority SetPriority

	// Un- / Mutes the AudioSource. Mute sets the volume=0, Un-Mute restore the original volume.
	AUTO_PROP bool mute GetMute SetMute

	// Within the Min distance the AudioSource will cease to grow louder in volume.
	AUTO_PROP float minDistance GetMinDistance SetMinDistance

	// (Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at.
	AUTO_PROP float maxDistance GetMaxDistance SetMaxDistance

	// Sets/Gets how the AudioSource attenuates over distance
	AUTO_PROP AudioRolloffMode rolloffMode GetRolloffMode SetRolloffMode

	CUSTOM private void GetOutputDataHelper(float[] samples, int channel)
	{
		self->GetOutputData(Scripting::GetScriptingArrayStart<float>(samples), GetScriptingArraySize(samples), channel);
	}

	// Returns a block of the currently playing source's output data
	CSRAW
	OBSOLETE warning GetOutputData return a float[] is deprecated, use GetOutputData passing a pre allocated array instead.
	public float[] GetOutputData(int numSamples, int channel)
	{
		float[] samples = new float[numSamples];
		GetOutputDataHelper(samples, channel);
		return samples;
	}

	// Returns a block of the currently playing source's output data
	CSRAW public void GetOutputData(float[] samples, int channel)
	{
		GetOutputDataHelper(samples, channel);
	}

	CUSTOM private void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window)
	{
		self->GetSpectrumData(Scripting::GetScriptingArrayStart<float>(samples), GetScriptingArraySize(samples), channel, (FMOD_DSP_FFT_WINDOW) window);
	}

	// Returns a block of the currently playing source's spectrum data
	CSRAW
	OBSOLETE warning GetSpectrumData returning a float[] is deprecated, use GetSpectrumData passing a pre allocated array instead.
	public float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
	{
		float[] samples = new float[numSamples];
		GetSpectrumDataHelper(samples, channel, window);
		return samples;
	}

	// Returns a block of the currently playing source's spectrum data
	CSRAW public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
	{
		GetSpectrumDataHelper(samples, channel, window);
	}

	FLUSHCONDITIONS

	OBSOLETE error minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.
	CUSTOM_PROP float minVolume
	{
		ErrorString("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
		return 0.0f;
	}
	{
		ErrorString("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
	}
	OBSOLETE error maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.
	CUSTOM_PROP float maxVolume
	{
		ErrorString("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
		return 0.0f;
	}
	{
		ErrorString("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
	}
	OBSOLETE error rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.
	CUSTOM_PROP float rolloffFactor
	{
		ErrorString("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
		return 0.0f;
	}
	{
		ErrorString("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
	}
	*/
}
// Reverb presets used by the Reverb Zone class and the audio reverb filter
enum AudioReverbPreset
{
	// No reverb preset selected
	Off = 0,

	// Generic preset.
	Generic = 1,

	// Padded cell preset.
	PaddedCell = 2,

	// Room preset.
	Room = 3,

	// Bathroom preset.
	Bathroom = 4,

	// Livingroom preset
	Livingroom = 5,

	// Stoneroom preset
	Stoneroom = 6,

	// Auditorium preset.
	Auditorium = 7,

	// Concert hall preset.
	Concerthall = 8,

	// Cave preset.
	Cave = 9,

	// Arena preset.
	Arena = 10,

	// Hangar preset.
	Hangar = 11,

	// Carpeted hallway preset.
	CarpetedHallway = 12,

	// Hallway preset.
	Hallway = 13,

	// Stone corridor preset.
	StoneCorridor = 14,

	// Alley preset.
	Alley = 15,

	// Forest preset.
	Forest = 16,

	// City preset.
	City = 17,

	// Mountains preset.
	Mountains = 18,

	// Quarry preset.
	Quarry = 19,

	// Plain preset.
	Plain = 20,

	// Parking Lot preset
	ParkingLot = 21,

	// Sewer pipe preset.
	SewerPipe = 22,

	// Underwater presset
	Underwater = 23,

	// Drugged preset
	Drugged = 24,

	// Dizzy preset.
	Dizzy = 25,

	// Psychotic preset.
	Psychotic = 26,

	// User defined preset.
	User = 27
}
/*
// Reverb Zones are used when you want to gradually change from a point
CLASS AudioReverbZone : Behaviour
	//  The distance from the centerpoint that the reverb will have full effect at. Default = 10.0.
	AUTO_PROP float minDistance GetMinDistance SetMinDistance

	//  The distance from the centerpoint that the reverb will not have any effect. Default = 15.0.
	AUTO_PROP float maxDistance GetMaxDistance SetMaxDistance

	// Set/Get reverb preset properties
	AUTO_PROP AudioReverbPreset reverbPreset GetReverbPreset SetReverbPreset

	// room effect level (at mid frequencies)
	AUTO_PROP  int room GetRoom SetRoom
	// relative room effect level at high frequencies
	AUTO_PROP  int roomHF GetRoomHF SetRoomHF
	// relative room effect level at low frequencies
	AUTO_PROP  int roomLF GetRoomLF SetRoomLF
	// reverberation decay time at mid frequencies
	AUTO_PROP  float decayTime GetDecayTime SetDecayTime
	//  high-frequency to mid-frequency decay time ratio
	AUTO_PROP  float decayHFRatio GetDecayHFRatio SetDecayHFRatio
	// early reflections level relative to room effect
	AUTO_PROP  int reflections GetReflections SetReflections
	//  initial reflection delay time
	AUTO_PROP  float reflectionsDelay GetReflectionsDelay SetReflectionsDelay
	// late reverberation level relative to room effect
	AUTO_PROP  int reverb GetReverb SetReverb
	//  late reverberation delay time relative to initial reflection
	AUTO_PROP  float reverbDelay GetReverbDelay SetReverbDelay
	//  reference high frequency (hz)
	AUTO_PROP  float HFReference GetHFReference SetHFReference
	// reference low frequency (hz)
	AUTO_PROP  float LFReference GetLFReference SetLFReference
	// like rolloffscale in global settings, but for reverb room size effect
	AUTO_PROP  float roomRolloffFactor GetRoomRolloffFactor SetRoomRolloffFactor
	//  Value that controls the echo density in the late reverberation decay
	AUTO_PROP  float diffusion GetDiffusion SetDiffusion
	// Value that controls the modal density in the late reverberation decay
	AUTO_PROP  float density GetDensity SetDensity
END


// The Audio Low Pass Filter filter passes low frequencies of an
CLASS AudioLowPassFilter : Behaviour
	// Lowpass cutoff frequency in hz. 10.0 to 22000.0. Default = 5000.0.
	AUTO_PROP float cutoffFrequency GetCutoffFrequency SetCutoffFrequency

	// Determines how much the filter's self-resonance is dampened.
	AUTO_PROP float lowpassResonanceQ GetLowpassResonanceQ SetLowpassResonanceQ

	OBSOLETE error lowpassResonaceQ is obsolete. Use lowpassResonanceQ instead (UnityUpgradable).
	AUTO_PROP float lowpassResonaceQ GetLowpassResonanceQ SetLowpassResonanceQ
END
// The Audio High Pass Filter passes high frequencies of an AudioSource and
CLASS AudioHighPassFilter : Behaviour
	// Highpass cutoff frequency in hz. 10.0 to 22000.0. Default = 5000.0.
	AUTO_PROP float cutoffFrequency GetCutoffFrequency SetCutoffFrequency

	// Determines how much the filter's self-resonance isdampened.
	AUTO_PROP float highpassResonanceQ GetHighpassResonanceQ SetHighpassResonanceQ

	OBSOLETE error highpassResonaceQ is obsolete. Use highpassResonanceQ instead (UnityUpgradable).
	AUTO_PROP float highpassResonaceQ GetHighpassResonanceQ SetHighpassResonanceQ
END
// The Audio Distortion Filter distorts the sound from an AudioSource or
CLASS AudioDistortionFilter : Behaviour
	// Distortion value. 0.0 to 1.0. Default = 0.5.
	AUTO_PROP float distortionLevel GetDistortionLevel SetDistortionLevel
END

// The Audio Echo Filter repeats a sound after a given Delay, attenuating
CLASS AudioEchoFilter : Behaviour
	// Echo delay in ms. 10 to 5000. Default = 500.
	AUTO_PROP float delay GetDelay SetDelay


	// Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (i.e. simple 1 line delay). Default = 0.5.
	AUTO_PROP float decayRatio GetDecayRatio SetDecayRatio


	// Volume of original signal to pass to output. 0.0 to 1.0. Default = 1.0.
	AUTO_PROP float dryMix GetDryMix SetDryMix


	// Volume of echo signal to pass to output. 0.0 to 1.0. Default = 1.0.
	AUTO_PROP float wetMix GetWetMix SetWetMix
END

// The Audio Chorus Filter takes an Audio Clip and processes it creating a chorus effect.
CLASS AudioChorusFilter : Behaviour
	// Volume of original signal to pass to output. 0.0 to 1.0. Default = 0.5.
	AUTO_PROP float dryMix GetDryMix SetDryMix


	// Volume of 1st chorus tap. 0.0 to 1.0. Default = 0.5.
	AUTO_PROP float wetMix1 GetWetMix1 SetWetMix1


	// Volume of 2nd chorus tap. This tap is 90 degrees out of phase of the first tap. 0.0 to 1.0. Default = 0.5.
	AUTO_PROP float wetMix2 GetWetMix2 SetWetMix2


	// Volume of 3rd chorus tap. This tap is 90 degrees out of phase of the second tap. 0.0 to 1.0. Default = 0.5.
	AUTO_PROP float wetMix3 GetWetMix3 SetWetMix3


	// Chorus delay in ms. 0.1 to 100.0. Default = 40.0 ms.
	AUTO_PROP float delay GetDelay SetDelay


	// Chorus modulation rate in hz. 0.0 to 20.0. Default = 0.8 hz.
	AUTO_PROP float rate GetRate SetRate


	//  Chorus modulation depth. 0.0 to 1.0. Default = 0.03.
	AUTO_PROP float depth GetDepth SetDepth

	/// Chorus feedback. Controls how much of the wet signal gets fed back into the chorus buffer. 0.0 to 1.0. Default = 0.0.
	OBSOLETE warning feedback is deprecated, this property does nothing.
	CUSTOM_PROP float feedback
	{
		return 0.0f;
	}
	{}

END
// The Audio Reverb Filter takes an Audio Clip and distortionates it in a
CLASS AudioReverbFilter : Behaviour
	// Set/Get reverb preset properties
	AUTO_PROP AudioReverbPreset reverbPreset GetReverbPreset SetReverbPreset
	// Mix level of dry signal in output in mB. Ranges from -10000.0 to 0.0. Default is 0.
	AUTO_PROP float dryLevel GetDryLevel SetDryLevel
	// Room effect level at low frequencies in mB. Ranges from -10000.0 to 0.0. Default is 0.0.
	AUTO_PROP float room GetRoom SetRoom
	// Room effect high-frequency level re. low frequency level in mB. Ranges from -10000.0 to 0.0. Default is 0.0.
	AUTO_PROP float roomHF GetRoomHF SetRoomHF
	// Rolloff factor for room effect. Ranges from 0.0 to 10.0. Default is 10.0
	AUTO_PROP float roomRolloff GetRoomRolloff SetRoomRolloff
	// Reverberation decay time at low-frequencies in seconds. Ranges from 0.1 to 20.0. Default is 1.0.
	AUTO_PROP float decayTime GetDecayTime SetDecayTime
	// Decay HF Ratio : High-frequency to low-frequency decay time ratio. Ranges from 0.1 to 2.0. Default is 0.5.
	AUTO_PROP float decayHFRatio GetDecayHFRatio SetDecayHFRatio
	//  Early reflections level relative to room effect in mB. Ranges from -10000.0 to 1000.0. Default is -10000.0.
	AUTO_PROP float reflectionsLevel GetReflectionsLevel SetReflectionsLevel
	// Late reverberation level relative to room effect in mB. Ranges from -10000.0 to 2000.0. Default is 0.0.
	AUTO_PROP float reflectionsDelay GetReflectionsDelay SetReflectionsDelay
	//  Late reverberation level relative to room effect in mB. Ranges from -10000.0 to 2000.0. Default is 0.0.
	AUTO_PROP float reverbLevel GetReverbLevel SetReverbLevel
	// Late reverberation delay time relative to first reflection in seconds. Ranges from 0.0 to 0.1. Default is 0.04.
	AUTO_PROP float reverbDelay GetReverbDelay SetReverbDelay
	// Reverberation diffusion (echo density) in percent. Ranges from 0.0 to 100.0. Default is 100.0.
	AUTO_PROP float diffusion GetDiffusion SetDiffusion
	// Reverberation density (modal density) in percent. Ranges from 0.0 to 100.0. Default is 100.0.
	AUTO_PROP float density GetDensity SetDensity
	// Reference high frequency in Hz. Ranges from 20.0 to 20000.0. Default is 5000.0.
	AUTO_PROP float hfReference GetHFReference SetHFReference
	// Room effect low-frequency level in mB. Ranges from -10000.0 to 0.0. Default is 0.0.
	AUTO_PROP float roomLF GetRoomLF SetRoomLF
	// Reference low-frequency in Hz. Ranges from 20.0 to 1000.0. Default is 250.0.
	AUTO_PROP float lfReference GetLFReference SetLFReference

	OBSOLETE error lFReference is obsolete. Use lfReference instead (UnityUpgradable).
	AUTO_PROP float lFReference GetLFReference SetLFReference
END
*/



public class Microphone
{
	// Start Recording with device
/*
	CUSTOM static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
	{
		return Scripting::ScriptingWrapperFor( GetAudioManager().StartRecord( GetAudioManager().GetMicrophoneDeviceIDFromName ( deviceName ) , loop, lengthSec, frequency ) );
	}

	// Stops recording
	CUSTOM static void End(string deviceName)
	{
		GetAudioManager().EndRecord( GetAudioManager().GetMicrophoneDeviceIDFromName ( deviceName ) );
	}

	// Gives you a list microphone devices, identified by name.
	CUSTOM_PROP static string[] devices
	{
		std::vector<std::string> names;
		names = GetAudioManager().GetRecordDevices();

		ScriptingArrayPtr array = CreateScriptingArray<ScriptingStringPtr> (MONO_COMMON.string, names.size ());
		for (size_t i=0;i<names.size ();i++)
			Scripting::SetScriptingArrayElement (array, i, scripting_string_new ( (const char*)names[i].c_str() ));

		return array;
	}

	// Query if a device is currently recording.
	CUSTOM static bool IsRecording(string deviceName)
	{
		return GetAudioManager().IsRecording( GetAudioManager().GetMicrophoneDeviceIDFromName ( deviceName ) );
	}

	// Get the position in samples of the recording.
	THREAD_SAFE CUSTOM static int GetPosition(string deviceName)
	{
		return GetAudioManager().GetRecordPosition( GetAudioManager().GetMicrophoneDeviceIDFromName ( deviceName ) );
	}

	// Get the frequency capabilities of a device.
	CUSTOM static void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq)
	{
		GetAudioManager().GetDeviceCaps( GetAudioManager().GetMicrophoneDeviceIDFromName ( deviceName ), minFreq, maxFreq );
	}

*/
}

}
