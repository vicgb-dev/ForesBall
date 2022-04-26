using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
	public List<AudioClip> songs = new List<AudioClip>();
	public List<AudioClip> endLvlSounds = new List<AudioClip>();

	private LevelSO currentLvl;
	private GameObject lvlMusic;
	private bool winFinished;

	//Definición del patrón Singleton
	#region Singleton

	private static SoundManager _instance;
	public static SoundManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<SoundManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<SoundManager>();
			return _instance;
		}
	}

	private void Awake()
	{
		// Si el singleton aun no ha sido inicializado
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		//DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	public void WinFinished() => winFinished = true;

	public void OnStartLvl(LevelSO lvl)
	{
		currentLvl = lvl;
		CreateAudioChild("LvlMusic", currentLvl.music, currentLvl.musicVolume).Play();
	}

	public void OnEndLvl(bool win)
	{
		if (win) CreateAudioChild("EndLvlSound", currentLvl.winSound, currentLvl.winSoundVolume).Play();
	}

	private IEnumerator VolumeControl(AudioSource aS)
	{
		float finalVolume = aS.volume;
		float time = 0;
		while(!winFinished)
		{
			if(time < 1)
			{
				time += Time.unscaledDeltaTime;
				aS.volume = Mathf.Lerp(0, finalVolume, time);
			}
			yield return null;
		}
		time = 0;
		while(time < 1)
		{
			time += Time.unscaledDeltaTime;
			aS.volume = Mathf.Lerp(finalVolume, 0, time);
			yield return null;
		}
	}

	private AudioSource CreateAudioChild(string name, AudioClip audioClip, float audioVolume, bool selfDestruction = true)
	{
		GameObject lvlAudio = new GameObject(name);
		lvlAudio.transform.SetParent(gameObject.transform);
		AudioSource audioSource = lvlAudio.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.volume = audioVolume;

		if (selfDestruction) StartCoroutine(SelfDestruction(lvlAudio, audioClip.length));
		return audioSource;
	}

	private IEnumerator SelfDestruction(GameObject audioGO, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(audioGO);
	}
}
