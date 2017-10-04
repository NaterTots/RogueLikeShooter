using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour , IPersistedController
{
	public AudioSource musicSource;
	public AudioSource effectsSource;

	float effectsVolume = 1.0f;

	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

	void Awake()
	{
		GameController.AddController(this);
	}

	void Start()
	{
		Setting musicVolumeSetting = GameController.GetController<ConfigurationController>().GetSetting(ConfigurationSettings.MusicVolume);
		AudioListener.volume = musicVolumeSetting.GetValueAsFloat();
		musicVolumeSetting.AddChangedEventListener(OnMusicVolumeSettingChanged);

		Setting effectsVolumeSetting = GameController.GetController<ConfigurationController>().GetSetting(ConfigurationSettings.SoundEffectsVolume);
		effectsVolume = effectsVolumeSetting.GetValueAsFloat();
		effectsVolumeSetting.AddChangedEventListener(OnSoundEffectVolumeSettingChanged);
	}

	//Used to play single sound clips.
	public void PlayClip(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		effectsSource.clip = clip;
		effectsSource.volume = effectsVolume;
		//Play the clip.
		effectsSource.Play();
	}

	//Used to play single sound clips with PlayOneShot, which can't be paused
	public void PlayClipOneShot(AudioClip clip)
	{
		//Play the clip.
		effectsSource.volume = effectsVolume;
		effectsSource.PlayOneShot(clip);
	}

	//used to play background music
	//NOTE - calling this method will stop playing the existing music and play the new music instead
	public void PlayMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Stop(); //stop playing the existing clip, and then play the new clip
		musicSource.Play();
	}

	private void OnPause()
	{
		musicSource.Pause(); //explicitly pause the music source since we want the music to pick up where it left off, not just silence it like we do for sound effects
		paused.TransitionTo(0.01f);
	}

	private void OnUnPause()
	{
		musicSource.UnPause();
		unpaused.TransitionTo(0.01f);
	}

	#region Volume Changes Events

	void OnMusicVolumeSettingChanged()
	{
		Setting musicVolumeSetting = GameController.GetController<ConfigurationController>().GetSetting(ConfigurationSettings.MusicVolume);
		AudioListener.volume = musicVolumeSetting.GetValueAsFloat();
	}

	void OnSoundEffectVolumeSettingChanged()
	{
		Setting effectsVolumeSetting = GameController.GetController<ConfigurationController>().GetSetting(ConfigurationSettings.SoundEffectsVolume);
		effectsVolume = effectsVolumeSetting.GetValueAsFloat();
	}

	#endregion

}
