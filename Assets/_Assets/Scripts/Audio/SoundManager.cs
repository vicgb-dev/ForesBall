using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioMixerGroup mixerGroup;
	[SerializeField] private AudioClip popSound;
	[SerializeField] private AudioClip peopleTalkingSound;
	[Range(0, 1)]
	[SerializeField] private float popVolume;
	[Range(10, 20)]
	[SerializeField] private float secondsMusicPreview;
	[Range(0, 5f)]
	[SerializeField] private float secondsToChangeVolumeMusicPreview;
	[SerializeField] private List<AudioClip> pianoTiles = new List<AudioClip>();
	[SerializeField] private List<AudioClip> uniquePianoTiles = new List<AudioClip>();
	private List<GameObject> pianoTilesGo = new List<GameObject>();

	private int currentTile = 0;
	private float logVolume;
	private int linearVolume;

	private LevelSO currentLvl;
	private AudioSource lvlMusic;
	private AudioSource peopleAs;
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
		linearVolume = PlayerPrefs.GetInt("mutedVolume", 1);
		SetVolume(linearVolume);

		_instance = this;
	}

	#endregion

	private void Start()
	{
		peopleAs = CreateAudioChild("PeopleTalking", peopleTalkingSound, 1, selfDestruction: false, loop: true);
		StartCoroutine(PlayPeopleTalkingCo());

		foreach (var tile in uniquePianoTiles)
		{
			GameObject go = new GameObject(tile.name);
			go.transform.SetParent(gameObject.transform);
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.outputAudioMixerGroup = mixerGroup;
			audioSource.clip = tile;
			audioSource.volume = popVolume;
			pianoTilesGo.Add(go);
		}
	}

	public int GetLinearVolume() => linearVolume;

	public void SetVolume(int newVolume)
	{
		linearVolume = newVolume;
		logVolume = Mathf.Log10(newVolume) * 20;
		if (logVolume < -20) logVolume = -80;

		mixerGroup.audioMixer.SetFloat("mixerVolume", logVolume);

		PlayerPrefs.SetFloat("mutedVolume", linearVolume);
		PlayerPrefs.Save();
	}

	private void OnEnable()
	{
		Actions.onLvlEnd += PlayLose;
		Actions.onLvlStart += StopMusicPreview;
		Actions.onLvlStart += (lvl) => StartCoroutine(FadePeopleTalking(false));
		Actions.onNewUIState += OnNewUIState;
		Actions.onMute += OnMute;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= PlayLose;
		Actions.onLvlStart -= StopMusicPreview;
		Actions.onNewUIState -= OnNewUIState;
		Actions.onMute -= OnMute;
	}

	private void OnMute(bool mute)
	{
		SoundManager.Instance.SetVolume(mute ? 0 : 1);
	}
	public void OnStartLvl(LevelSO lvl)
	{
		pitch = 1;
		currentLvl = lvl;
		lvlMusic = CreateAudioChild("LvlMusic", currentLvl.music, currentLvl.musicVolume, isLvlMusic: true);
		lvlMusic.Play();
		Actions.onLvlMusicChange?.Invoke(lvlMusic);
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
		if (!win)
		{
			CreateAudioChild("EndLvlSound", currentLvl.loseSound, currentLvl.loseSoundVolume).Play();
		}
		StartCoroutine(FadePeopleTalking(true));
	}

	public void PlayWin()
	{
		Destroy(GameObject.Find("LvlMusic"));
		CreateAudioChild("EndLvlSound", currentLvl.winSound, currentLvl.winSoundVolume).Play();
	}

	private AudioSource CreateAudioChild(string name, AudioClip audioClip, float audioVolume, bool selfDestruction = true, bool isLvlMusic = false, bool loop = false)
	{
		GameObject lvlAudio = new GameObject(name);
		lvlAudio.transform.SetParent(gameObject.transform);
		AudioSource audioSource = lvlAudio.AddComponent<AudioSource>();

		audioSource.loop = loop;
		audioSource.outputAudioMixerGroup = mixerGroup;
		audioSource.clip = audioClip;
		audioSource.volume = audioVolume;

		if (selfDestruction) StartCoroutine(SelfDestruction(lvlAudio, audioClip.length, isLvlMusic));
		return audioSource;
	}

	private IEnumerator SelfDestruction(GameObject audioGO, float delay, bool isLvlMusic = false)
	{
		yield return new WaitForSeconds(delay);
		if (isLvlMusic)
			Actions.onLvlMusicChange?.Invoke(null);
		Destroy(audioGO);
	}

	public void PlaySinglePop(float newPitch = 0)
	{
		AudioSource aS = CreateAudioChild($"PopSound{newPitch.ToString("0.00")}", popSound, popVolume);

		aS.pitch = newPitch == 0 ? pitch : newPitch;
		aS.Play();
	}

	public void PlaySinglePianoTile(float newPitch = 0)
	{
		// AudioSource aS = CreateAudioChild($"PopSound{newPitch.ToString("0.00")}", popSound, popVolume);

		/*
		AudioClip tileSound = pianoTiles[currentTile];
		currentTile = (currentTile + 1) % pianoTiles.Count;

		AudioSource aS = CreateAudioChild($"PopSound{newPitch.ToString("0.00")}", tileSound, popVolume);
		aS.pitch = newPitch == 0 ? pitch : newPitch;
		aS.Play();
		*/
		string tileName = pianoTiles[currentTile].name;
		currentTile = (currentTile + 1) % pianoTiles.Count;

		foreach (var tile in pianoTilesGo)
		{
			if (tile.name == tileName)
			{
				AudioSource aS = tile.GetComponent<AudioSource>();
				aS.Play();
				break;
			}
		}

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
		Debug.Log("StopMusicPreview");
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

	private IEnumerator PlayPeopleTalkingCo()
	{
		yield return new WaitForSeconds(3f);
		peopleAs.Play();
	}

	public IEnumerator FadePeopleTalking(bool to1)
	{
		float time = 0;
		float initialVolume = peopleAs.volume;

		while (time <= 1)
		{
			time += Time.deltaTime;
			peopleAs.volume = Mathf.Lerp(initialVolume, to1 ? 1 : 0, time);
			yield return null;
		}
	}
}