using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
////////////////////////////All hope abandon...///////////////////////////
////////////////////////////Ye who enter here/////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////

public class SoundManager : MonoBehaviour {
	private static SoundManager _instance = null;
	private static float vol = 1f;
	private static float musicVol = 1f;
	private static float soundsVol = 1f;
	private static float UISoundsVol = 1f;

	private static Dictionary<int, Audio> musicAudio;
	private static Dictionary<int, Audio> soundsAudio;
	private static Dictionary<int, Audio> UISoundsAudio;
	private static bool initialized = false;

	private static SoundManager instance{
		get{
			if (_instance == null){
				_instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
				if (_instance == null){
					_instance = (new GameObject("New Sound")).AddComponent<SoundManager>();
				}
			}
			return _instance;
		}
	}
	//This game object
	public static GameObject gameobject { 
		get { 
			return instance.gameObject; 
		} 
	}
	//stops duplicate music
	public static bool ignoreDuplicateMusic { 
		get; 
		set; 
	}
	//dupe sounds
	public static bool ignoreDuplicateSounds { 
		get; 
		set; 
	}
	//dupe UI
	public static bool ignoreDuplicateUISounds { 
		get; 
		set; 
	}
	//volume for entire game
	public static float globalVolume{
		get{
			return vol;
		}
		set{
			vol = value;
		}
	}
	//bgm
	public static float globalMusicVolume{
		get{
			return musicVol;
		}
		set{
			musicVol = value;
		}
	}
	//sfx
	public static float globalSoundsVolume{
		get{
			return soundsVol;
		}
		set{
			soundsVol = value;
		}
	}
	//UI
	public static float globalUISoundsVolume{
		get{
			return UISoundsVol;
		}
		set{
			UISoundsVol = value;
		}
	}

	void Awake(){
		instance.Init();
	}
	void OnLevelWasLoaded(int level)
	{
		List<int> keys;

		// Stop and remove all non-persistent music audio
		keys = new List<int>(musicAudio.Keys);
		foreach (int key in keys)
		{
			Audio audio = musicAudio[key];
			if (!audio.persist && audio.activated)
			{
				Destroy(audio.audioSource);
				musicAudio.Remove(key);
			}
		}

		// Stop and remove all sound fx
		keys = new List<int>(soundsAudio.Keys);
		foreach (int key in keys)
		{
			Audio audio = soundsAudio[key];
			Destroy(audio.audioSource);
			soundsAudio.Remove(key);
		}

		// Stop and remove all UI sound fx
		keys = new List<int>(UISoundsAudio.Keys);
		foreach (int key in keys)
		{
			Audio audio = UISoundsAudio[key];
			Destroy(audio.audioSource);
			UISoundsAudio.Remove(key);
		}
	}
	//Initialize variables
	void Init(){
		if (!initialized)
		{
			musicAudio = new Dictionary<int, Audio>();
			soundsAudio = new Dictionary<int, Audio>();
			UISoundsAudio = new Dictionary<int, Audio>();

			ignoreDuplicateMusic = false;
			ignoreDuplicateSounds = false;
			ignoreDuplicateUISounds = false;

			initialized = true;
			DontDestroyOnLoad(this);
		}
	}

	void Update()
	{
		List<int> keys;

		// Update music
		keys = new List<int>(musicAudio.Keys);
		foreach (int key in keys){
			Audio audio = musicAudio[key];
			audio.Update();

			// If music not playing remove
			if (!audio.playing && !audio.paused){
				Destroy(audio.audioSource);
				//emove(key);
			}
		}

		// Update sound fx
		keys = new List<int>(soundsAudio.Keys);
		foreach (int key in keys){
			Audio audio = soundsAudio[key];
			audio.Update();

			if (!audio.playing && !audio.paused)
			{
				Destroy(audio.audioSource);
				//.Remove(key);
			}
		}
			
		keys = new List<int>(UISoundsAudio.Keys);
		foreach (int key in keys){
			Audio audio = UISoundsAudio[key];
			audio.Update();

			// Remove all UI sound fx clips that are not playing
			if (!audio.playing && !audio.paused){
				Destroy(audio.audioSource);
			}
		}
	}

	/*public static Audio SetGlobalVolume(float vol){
		globalVolume = vol;
	}*/
	//Stop sounds with with music fade
	public static void FadeAll(float fadeOutSeconds){
		StopAllMusic(fadeOutSeconds);
		StopAllSounds();
		StopAllUISounds();
	}
	//stops all sounds
	public static void StopAll(){
		FadeAll(-1f);
	}
	public static void StopMusic(){
		StopAllMusic(-1f);
	}
	public static void StopAllMusic(float fadeOutSeconds){
		List<int> keys = new List<int>(musicAudio.Keys);
		foreach (int key in keys){
			Audio audio = musicAudio[key];
			if (fadeOutSeconds > 0){
				audio.fadeOutSeconds = fadeOutSeconds;
			}
			audio.Stop();
		}
	}
	public static void StopAllSounds(){
		List<int> keys = new List<int>(soundsAudio.Keys);
		foreach (int key in keys){
			Audio audio = soundsAudio[key];
			audio.Stop();
		}
	}
		
	public static void StopAllUISounds(){
		List<int> keys = new List<int>(UISoundsAudio.Keys);
		foreach (int key in keys){
			Audio audio = UISoundsAudio[key];
			audio.Stop();
		}
	}

	public static Audio GetAudio(int audioID){
		Audio audio;

		audio = GetMusicAudio(audioID);
		if (audio != null){
			return audio;
		}

		audio = GetSoundAudio(audioID);
		if (audio != null){
			return audio;
		}

		audio = GetUISoundAudio(audioID);
		if (audio != null){
			return audio;
		}

		return null;
	}

	public static Audio GetAudio(AudioClip audioClip){
		Audio audio = GetMusicAudio(audioClip);
		if (audio != null){
			return audio;
		}

		audio = GetSoundAudio(audioClip);
		if (audio != null){
			return audio;
		}

		audio = GetUISoundAudio(audioClip);
		if (audio != null){
			return audio;
		}

		return null;
	}

	public static Audio GetMusicAudio(int audioID){
		List<int> keys = new List<int>(musicAudio.Keys);
		foreach (int key in keys){
			if (audioID == key){
				return musicAudio[key];
			}
		}

		return null;
	}
	public static Audio GetMusicAudio(AudioClip audioClip){
		List<int> keys;
		keys = new List<int>(musicAudio.Keys);
		foreach (int key in keys){
			Audio audio = musicAudio[key];
			if (audio.clip == audioClip){
				return audio;
			}
		}

		return null;
	}

	public static Audio GetSoundAudio(int audioID){
		List<int> keys = new List<int>(soundsAudio.Keys);
		foreach (int key in keys){
			if (audioID == key){
				return soundsAudio[key];
			}
		}

		return null;
	}

	public static Audio GetSoundAudio(AudioClip audioClip){
		List<int> keys;
		keys = new List<int>(soundsAudio.Keys);
		foreach (int key in keys){
			Audio audio = soundsAudio[key];
			if (audio.clip == audioClip){
				return audio;
			}
		}

		return null;
	}
		
	public static Audio GetUISoundAudio(int audioID){
		List<int> keys = new List<int>(UISoundsAudio.Keys);
		foreach (int key in keys){
			if (audioID == key){
				return UISoundsAudio[key];
			}
		}

		return null;
	}

	public static Audio GetUISoundAudio(AudioClip audioClip){
		List<int> keys;
		keys = new List<int>(UISoundsAudio.Keys);
		foreach (int key in keys){
			Audio audio = UISoundsAudio[key];
			if (audio.clip == audioClip){
				return audio;
			}
		}

		return null;
	}
	//Sound clip playing	
	public static int PlaySound(AudioClip clip){
		return PlaySound(clip, 1f, false, null);
	}
	//Overload sound with volume
	public static int PlaySound(AudioClip clip, float volume){
		return PlaySound(clip, volume, false, null);
	}
	//overload sound with loop boolean
	public static int PlaySound(AudioClip clip, bool loop){
		return PlaySound(clip, 1f, loop, null);
	}

	public static int PlaySound(AudioClip clip, float volume, bool loop, Transform sourceTransform){
		if (clip == null){
			Debug.LogError("No Sound", clip);
		}

		if (ignoreDuplicateSounds){
			List<int> keys = new List<int>(soundsAudio.Keys);
			foreach (int key in keys){
				if (soundsAudio[key].audioSource.clip == clip){
					return soundsAudio[key].audioID;
				}
			}
		}

		instance.Init();
		AudioSource audioSource = instance.gameObject.AddComponent<AudioSource>() as AudioSource;
		Audio audio = new Audio(Audio.AudioType.Sound, clip, loop, false, volume, 0f, 0f, sourceTransform);
		soundsAudio.Add(audio.audioID, audio);

		return audio.audioID;
	}

	public static int PlayUISound(AudioClip clip){
		return PlayUISound(clip, 1f);
	}
	public static int PlayUISound(AudioClip clip, float volume){
		if (clip == null){
			Debug.LogError("No UI Sound", clip);
		}

		if (ignoreDuplicateUISounds){
			List<int> keys = new List<int>(UISoundsAudio.Keys);
			foreach (int key in keys)
			{
				if (UISoundsAudio[key].audioSource.clip == clip)
				{
					return UISoundsAudio[key].audioID;
				}
			}
		}

		instance.Init();
		Audio audio = new Audio(Audio.AudioType.UISound, clip, false, false, volume, 0f, 0f, null);
		UISoundsAudio.Add(audio.audioID, audio);

		return audio.audioID;
	}

}
public class Audio
{
	private static int audioCounter = 0;
	private float volume;
	private float targetVolume;
	private float initTargetVolume;
	private float tempFadeSeconds;
	private float fadeInterpolater;
	private float onFadeStartVolume;
	private AudioType audioType;
	private AudioClip initClip;
	private Transform sourceTransform;

	public int audioID { 
		get; 
		private set; 
	}

	public AudioSource audioSource { 
		get; 
		private set; 
	}


	public AudioClip clip{
		get{
			return audioSource == null ? initClip : audioSource.clip;
		}
	}

	public bool loop { 
		get; 
		set;
	}
		
	public bool persist { 
		get; 
		set; 
	}


	public float fadeInSeconds { 
		get; 
		set; 
	}

	public float fadeOutSeconds { 
		get; 
		set; 
	}
		
	public bool playing { 
		get; 
		set; 
	}
		
	public bool paused { 
		get; 
		private set; 
	}


	public bool stopping { 
		get; 
		private set; 
	}        

	public bool activated {
		get; 
		private set; 
	}

	public enum AudioType{
		Music, Sound, UISound
	}

	public Audio(AudioType audioType, AudioClip clip, bool loop, bool persist, float volume, float fadeInValue, float fadeOutValue, Transform sourceTransform){
		if (sourceTransform == null){
			this.sourceTransform = SoundManager.gameobject.transform;
		}
		else{
			this.sourceTransform = sourceTransform;
		}

		this.audioID = audioCounter;
		audioCounter++;

		this.audioType = audioType;
		this.initClip = clip;
		this.loop = loop;
		this.persist = persist;
		this.targetVolume = volume;
		this.initTargetVolume = volume;
		this.tempFadeSeconds = -1;
		this.volume = 0f;
		this.fadeInSeconds = fadeInValue;
		this.fadeOutSeconds = fadeOutValue;

		this.playing = false;
		this.paused = false;
		this.activated = false;

		CreateAudiosource(clip, loop);
		Play();
	}

	void CreateAudiosource(AudioClip clip, bool loop){
		audioSource = sourceTransform.gameObject.AddComponent<AudioSource>() as AudioSource;

		audioSource.clip = clip;
		audioSource.loop = loop;
		audioSource.volume = 0f;
		if (sourceTransform != SoundManager.gameobject.transform)
		{
			audioSource.spatialBlend = 1;
		}
	}

	public void Play(){
		Play(initTargetVolume);
	}
		
	public void Play(float volume){
		if(audioSource == null)
		{
			CreateAudiosource(initClip, loop);
		}

		audioSource.Play();
		playing = true;

		fadeInterpolater = 0f;
		onFadeStartVolume = this.volume;
		targetVolume = volume;
	}
	public void Stop(){
		fadeInterpolater = 0f;
		onFadeStartVolume = volume;
		targetVolume = 0f;

		stopping = true;
	}
		
	public void Pause(){
		audioSource.Pause();
		paused = true;
	}

	public void Resume(){
		audioSource.UnPause();
		paused = false;
	}
		
	public void SetVolume(float volume){
		if(volume > targetVolume){
			SetVolume(volume, fadeOutSeconds);
		}
		else{
			SetVolume(volume, fadeInSeconds);
		}
	}
	public void SetVolume(float volume, float fadeSeconds){
		SetVolume(volume, fadeSeconds, this.volume);
	}
	public void SetVolume(float volume, float fadeSeconds, float startVolume){
		targetVolume = Mathf.Clamp01(volume);
		fadeInterpolater = 0;
		onFadeStartVolume = startVolume;
		tempFadeSeconds = fadeSeconds;
	}

	public void Update(){
		if(audioSource == null){
			return;
		}

		activated = true;

		if (volume != targetVolume){
			float fadeValue;
			fadeInterpolater += Time.deltaTime;
			if (volume > targetVolume){
				fadeValue = tempFadeSeconds != -1? tempFadeSeconds: fadeOutSeconds;
			}
			else{
				fadeValue = tempFadeSeconds != -1 ? tempFadeSeconds : fadeInSeconds;
			}

			volume = Mathf.Lerp(onFadeStartVolume, targetVolume, fadeInterpolater / fadeValue);
		}
		else if(tempFadeSeconds != -1){
			tempFadeSeconds = -1;
		}

		switch (audioType){
		case AudioType.Music:
			{
				audioSource.volume = volume * SoundManager.globalMusicVolume * SoundManager.globalVolume;
				break;
			}
		case AudioType.Sound:
			{
				audioSource.volume = volume * SoundManager.globalSoundsVolume * SoundManager.globalVolume;
				break;
			}
		case AudioType.UISound:
			{
				audioSource.volume = volume * SoundManager.globalUISoundsVolume * SoundManager.globalVolume;
				break;
			}
		}

		if (volume == 0f && stopping){
			audioSource.Stop();
			stopping = false;
			playing = false;
			paused = false;
		}

		if (audioSource.isPlaying != playing)
		{
			playing = audioSource.isPlaying;
		}
	}
}