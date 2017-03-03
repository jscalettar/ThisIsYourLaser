using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemVolume : MonoBehaviour {
	public Slider globalVolumeSlider;
	public Slider globalMusicVolumeSlider;
	public Slider globalSoundVolumeSlider;
	public SoundManager[] AudioControls;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void GlobalVolumeChanged()
	{
		SoundManager.globalVolume = globalVolumeSlider.value;
	}

	public void GlobalMusicVolumeChanged()
	{
		SoundManager.globalMusicVolume = globalMusicVolumeSlider.value;
	}

	public void GlobalSoundVolumeChanged()
	{
		SoundManager.globalSoundsVolume = globalSoundVolumeSlider.value;
	}
}

