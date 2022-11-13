using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioClip popSound;
	[Range(0, 1)]
	[SerializeField] private float popVolume;

	private LevelSO currentLvl;
	private GameObject lvlMusic;
	private float pitch;

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

	private void OnEnable()
	{
		Actions.onLvlEnd += PlayLose;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= PlayLose;
	}

	public void OnStartLvl(LevelSO lvl)
	{
		pitch = 1;
		currentLvl = lvl;
		CreateAudioChild("LvlMusic", currentLvl.music, currentLvl.musicVolume).Play();
	}

	public void PlayLose(bool win)
	{
		Destroy(GameObject.Find("LvlMusic"));
		if (!win) CreateAudioChild("EndLvlSound", currentLvl.loseSound, currentLvl.loseSoundVolume).Play();
	}

	public void PlayWin()
	{
		Destroy(GameObject.Find("LvlMusic"));
		CreateAudioChild("EndLvlSound", currentLvl.winSound, currentLvl.winSoundVolume).Play();
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

	public void PlaySinglePop()
	{
		AudioSource aS = CreateAudioChild($"PopSound{pitch.ToString("0.00")}", popSound, popVolume);
		aS.pitch = pitch;
		aS.Play();
	}

	public void PlayPop()
	{
		AudioSource aS = CreateAudioChild($"PopSound{pitch.ToString("0.00")}", popSound, popVolume);
		aS.pitch = pitch;
		pitch += 0.1f;
		aS.Play();
	}
}