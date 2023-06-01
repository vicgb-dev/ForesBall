using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{

	[Header("PostProcessing")]
	[Range(0, 0.5f)]
	[SerializeField] private float moveDistorsionX;
	[Range(0, 0.5f)]
	[SerializeField] private float moveDistorsionY;
	[SerializeField] private float audioVolume;

	[Header("Values depending on audio volume")]
	[SerializeField] private float chromaticMin = 0.05f;
	[SerializeField] private float chromaticMax = 0.3f;
	[Space]
	[SerializeField] private float bloomIntensityMin = 2f;
	[SerializeField] private float bloomIntensityMax = 4f;
	[Space]
	[SerializeField] private float bloomScatterMin = 0.13f;
	[SerializeField] private float bloomScatterMax = 0.18f;

	private Volume post;
	private float postWeight;
	private ChromaticAberration chromatic; // default 0.1
	private float chromaticInitial = 0;
	private Bloom bloom; // default 2.5
	private float bloomIntensityInitial = 0;
	private float bloomThresholdInitial = 0;
	private float bloomScatterInitial = 0;
	private LensDistortion lens;

	private AudioSource lvlMusic;

	#region Singleton

	private static PostProcessingManager _instance;
	public static PostProcessingManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<PostProcessingManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<PostProcessingManager>();
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		_instance = this;
		post = GetComponent<Volume>();
		postWeight = PlayerPrefs.GetFloat("postWeight", 1);
		post.weight = postWeight;
		Debug.Log($"cargado{postWeight}");
	}

	#endregion

	private void Start()
	{
		post.profile.TryGet(out lens);
		post.profile.TryGet(out chromatic);
		chromaticInitial = chromatic.intensity.value;
		post.profile.TryGet(out bloom);
		bloomIntensityInitial = bloom.intensity.value;
		bloomThresholdInitial = bloom.threshold.value;
		bloomScatterInitial = bloom.scatter.value;
	}

	private void OnEnable()
	{
		Actions.onLvlMusicChange += SetAudioSource;
		Actions.onCleanLvl += ResetPostprocesses;
	}

	private void OnDisable()
	{
		Actions.onLvlMusicChange -= SetAudioSource;
		Actions.onCleanLvl -= ResetPostprocesses;
	}

	private void Update()
	{
		if (lvlMusic != null)
		{
			audioVolume = CalcularVolumenAudioSource(lvlMusic);
			chromatic.intensity.value = Tools.Remap(audioVolume, 0, 0.2f, chromaticMin, chromaticMax);
			bloom.intensity.value = Tools.Remap(audioVolume, 0, 0.2f, bloomIntensityMin, bloomIntensityMax);
			bloom.threshold.value = Tools.Remap(audioVolume, 0, 0.2f, 1.5f, 1);
			bloom.scatter.value = Tools.Remap(audioVolume, 0, 0.2f, bloomScatterMin, bloomScatterMax);
		}
	}

	private void ResetPostprocesses()
	{
		StartCoroutine(ResetPostprocessesCo());
	}

	private IEnumerator ResetPostprocessesCo()
	{
		Vector2 initialValue = lens.center.value;
		Vector2 finalValue = new Vector2(0.5f, 0.5f);

		float bloomIntensity = bloom.intensity.value;
		float bloomThreshold = bloom.threshold.value;
		float bloomScatter = bloom.scatter.value;

		float finalChromatic = chromatic.intensity.value;

		float seconds = 0.2f;
		float elapsedTime = 0;
		float time = 0;
		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			time += Time.deltaTime / seconds;
			lens.center.value = Vector2.Lerp(initialValue, finalValue, time);

			bloom.intensity.value = Mathf.Lerp(bloomIntensity, bloomIntensityInitial, time);
			bloom.threshold.value = Mathf.Lerp(bloomThreshold, bloomThresholdInitial, time);
			bloom.scatter.value = Mathf.Lerp(bloomScatter, bloomScatterInitial, time);

			chromatic.intensity.value = Mathf.Lerp(finalChromatic, chromaticInitial, time);

			yield return null;
		}
	}

	private void SetAudioSource(AudioSource aS)
	{
		lvlMusic = aS;
	}

	public void ChangePostWeight(float amount)
	{
		amount = Mathf.Clamp(amount, 0, 1);
		post.weight = amount;
		Debug.Log("guardando weight");
		PlayerPrefs.SetFloat("postWeight", amount);
	}

	public float GetPostWeight() => post.weight;

	public void ChangeLensDistorsion(float horizontal, float vertical)
	{
		post.profile.TryGet(out lens);
		lens.center.value = new Vector2(Mathf.Lerp(0.5f - moveDistorsionX, 0.5f + moveDistorsionX, horizontal), Mathf.Lerp(0.5f - moveDistorsionY, 0.5f + moveDistorsionY, vertical));
	}

	public float CalcularVolumenAudioSource(AudioSource aS)
	{
		float clipLoudness = 0;

		int sampleDataLength = 1024;
		float[] clipSampleData = new float[sampleDataLength];

		if (aS != null && (aS.timeSamples + sampleDataLength) < aS.clip.samples)
		{
			aS.clip.GetData(clipSampleData, aS.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
			foreach (var sample in clipSampleData)
			{
				clipLoudness += Mathf.Abs(sample);
			}
			clipLoudness /= sampleDataLength;
		}
		else clipLoudness = 0;

		return clipLoudness;
	}
}
