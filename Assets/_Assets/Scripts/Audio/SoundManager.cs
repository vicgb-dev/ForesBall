using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioMixerGroup mixerGroup;
	[SerializeField] private AudioClip popSound;
	[Range(0, 1)]
	[SerializeField] private float popVolume;
	[Range(10, 20)]
	[SerializeField] private float secondsMusicPreview;
	[Range(0, 5f)]
	[SerializeField] private float secondsToChangeVolumeMusicPreview;

	private float logVolume;
	private float linearVolume;

	private LevelSO currentLvl;
	private AudioSource lvlMusic;
	private float pitch = 1;
	private bool inLvlsMenu = false;

	private Coroutine musicPreviewCo;
	public List<GameObject> musicPreviewGOs = new List<GameObject>();

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
	}

	#endregion

	private void Start()
	{
		linearVolume = PlayerPrefs.GetFloat("volume", 1);
		SetVolume(linearVolume);
	}

	public float GetLinearVolume() => linearVolume;

	public void SetVolume(float newVolume)
	{
		linearVolume = newVolume;
		logVolume = Mathf.Log10(newVolume) * 20;
		if (logVolume < -20) logVolume = -80;

		mixerGroup.audioMixer.SetFloat("mixerVolume", logVolume);

		PlayerPrefs.SetFloat("volume", linearVolume);
		PlayerPrefs.Save();

	}

	private void OnEnable()
	{
		Actions.onLvlEnd += PlayLose;
		Actions.onLvlStart += StopMusicPreview;
		Actions.onNewUIState += OnNewUIState;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= PlayLose;
		Actions.onLvlStart -= StopMusicPreview;
		Actions.onNewUIState -= OnNewUIState;
	}

	public void OnStartLvl(LevelSO lvl)
	{
		pitch = 1;
		currentLvl = lvl;
		lvlMusic = CreateAudioChild("LvlMusic", currentLvl.music, currentLvl.musicVolume);
		lvlMusic.Play();
	}

	private void OnNewUIState(UIState state)
	{
		if (state == UIState.Levels)
		{
			inLvlsMenu = true;
			PlayMusicPreview(currentLvl);
		}
		else
		{
			StopMusicPreview();
		}
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

		audioSource.outputAudioMixerGroup = mixerGroup;
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

	public void PlaySinglePop(float newPitch = 0)
	{
		AudioSource aS = CreateAudioChild($"PopSound{newPitch.ToString("0.00")}", popSound, popVolume);
		aS.pitch = newPitch == 0 ? pitch : newPitch;
		aS.Play();
	}

	public void PlayPop()
	{
		Vibration.Vibrate(20);
		AudioSource aS = CreateAudioChild($"PopSound{pitch.ToString("0.00")}", popSound, popVolume);
		aS.pitch = pitch;
		pitch += 0.1f;
		aS.Play();
	}

	public void PlayMusicPreview(LevelSO newCurrentLvl)
	{
		currentLvl = newCurrentLvl;

		if (!inLvlsMenu) return;

		StopMusicPreview();
		musicPreviewCo = StartCoroutine(PlayMusicPreviewCo());
	}

	public void PauseLvlMusic()
	{
		if (lvlMusic != null)
			lvlMusic.Pause();
	}

	public void UnPauseLvlMusic()
	{
		if (lvlMusic != null)
			lvlMusic.UnPause();
	}

	private void StopMusicPreview(LevelSO lvl = null)
	{
		if (musicPreviewCo != null)
		{
			StopCoroutine(musicPreviewCo);
			if (musicPreviewGOs.Count > 0)
				StartCoroutine(FadeOut(musicPreviewGOs[musicPreviewGOs.Count - 1]));
		}
	}

	private IEnumerator PlayMusicPreviewCo()
	{
		yield return new WaitForSeconds(0.5f);
		var clip = currentLvl.music;

		GameObject musicPreviewGO = new GameObject(clip.name + "Preview");
		musicPreviewGOs.Add(musicPreviewGO);

		musicPreviewGO.transform.SetParent(gameObject.transform);
		AudioSource audioSource = musicPreviewGO.AddComponent<AudioSource>();
		audioSource.outputAudioMixerGroup = mixerGroup;
		audioSource.clip = clip;
		audioSource.volume = currentLvl.musicVolume;

		audioSource.Play();

		// subir volumen
		float time = 0;
		float elapsedTime = 0;
		while (time <= secondsToChangeVolumeMusicPreview)
		{
			time += Time.deltaTime;
			elapsedTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(0, 1, elapsedTime / secondsToChangeVolumeMusicPreview);
			yield return null;
		}

		yield return new WaitForSeconds(secondsMusicPreview - secondsToChangeVolumeMusicPreview * 2);

		// bajar volumen
		time = 0;
		elapsedTime = 0;
		while (time <= secondsToChangeVolumeMusicPreview)
		{
			time += Time.deltaTime;
			elapsedTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(1, 0, elapsedTime / secondsToChangeVolumeMusicPreview);
			yield return null;
		}

		musicPreviewGOs.Remove(musicPreviewGO);
		Destroy(musicPreviewGO);
	}

	private IEnumerator FadeOut(GameObject go)
	{
		musicPreviewGOs.Remove(go);

		AudioSource aS = go.GetComponent<AudioSource>();
		float time = 0;
		float initialVolume = aS.volume;

		while (time <= 1)
		{
			time += Time.deltaTime;
			aS.volume = Mathf.Lerp(initialVolume, 0, time);
			yield return null;
		}

		Destroy(go);
	}
}