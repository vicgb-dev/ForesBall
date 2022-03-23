using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
	public List<AudioClip> songs = new List<AudioClip>();
	public List<AudioClip> endLvlSounds = new List<AudioClip>();

	private Level currentLvl;
	private GameObject lvlMusic;

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
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	private void OnEnable()
	{
		Actions.onLvlStart += PlayAudio;
		Actions.onLvlEnd += StopPlaying;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= PlayAudio;
		Actions.onLvlEnd -= StopPlaying;
	}

	public void PlayAudio(Level lvl)
	{
		currentLvl = lvl;
		CreateAudioChild("LvlMusic", currentLvl.music, currentLvl.musicVolume).Play();
	}

	public void StopPlaying(bool win)
	{
		Destroy(GameObject.Find("LvlMusic"));
		if (win) CreateAudioChild("EndLvlSound", currentLvl.winSound, currentLvl.winSoundVolume).Play();
		else CreateAudioChild("EndLvlSound", currentLvl.loseSound, currentLvl.loseSoundVolume).Play();
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
