using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject pBlockTouchMiniMenu;
	[SerializeField] private GameObject pBlockChallangesMiniMenu;
	[SerializeField] private GameObject pChallangesMiniMenu;
	[SerializeField] private float secondsToHideChallenges;

	[Header("Challenges in Lvl")]
	[SerializeField] private Image fillClock;
	[SerializeField] private TextMeshProUGUI clockText;
	[SerializeField] private Image fillHotspot;
	[SerializeField] private TextMeshProUGUI hotspotText;
	[SerializeField] private Image fillCollectibles;
	[SerializeField] private TextMeshProUGUI collectiblesText;

	private Vector3 panelLeftPosition;
	private Vector3 panelRightPosition;
	private Vector3 panelUpPosition;
	private Vector3 panelDownPosition;
	private Vector3 panelCenterPosition;

	private float hotspotScore = 0;
	private int collectiblesScore = 0;
	private int collectiblesAmount;
	private LevelSO currentLvl;

	private Color colorCompleted;

	#region Singleton

	private static MiniMenuManager _instance;
	public static MiniMenuManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<MiniMenuManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<MiniMenuManager>();
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
	}

	#endregion

	private void Start()
	{
		colorCompleted = UIColorsManager.Instance.GetColorsSO().challengesColor;
	}

	private void OnEnable()
	{
		Actions.onLvlEnd += EndLvl;
		Actions.onLvlStart += StartLvl;
		Actions.updateChallenge += UpdateChallenges;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= EndLvl;
		Actions.onLvlStart -= StartLvl;
		Actions.updateChallenge -= UpdateChallenges;
	}

	private void StartLvl(LevelSO lvl)
	{
		StopAllCoroutines();
		DrawChallenges(lvl);
		StartCoroutine(ToggleMiniMenuChallenges(lvl));
	}

	private void DrawChallenges(LevelSO lvl)
	{
		currentLvl = lvl;
		ChangeFillAmount(fillClock, currentLvl.timeChallenge);
		TimeSpan time = TimeSpan.FromSeconds(currentLvl.music.length);
		clockText.text = time.ToString(@"mm\:ss");

		ChangeFillAmount(fillHotspot, currentLvl.hotspot);
		hotspotText.text = "0%";

		collectiblesAmount = currentLvl.collectiblesSpawnTimeStamps.Count;
		ChangeFillAmount(fillCollectibles, currentLvl.collectibles);
		collectiblesText.text = $"0/{collectiblesAmount}";
	}

	private void ChangeFillAmount(Image fill, float value)
	{
		fill.fillAmount = value;
		if (value > 0.99f)
		{
			fill.color = colorCompleted;
			fill.transform.GetChild(0).GetComponent<Image>().color = Color.black;
		}
		else
		{
			fill.color = Color.gray;
			fill.transform.GetChild(0).GetComponent<Image>().color = Color.white;
		}
	}

	private void EndLvl(bool win)
	{
		collectiblesScore = 0;
		StopAllCoroutines();
		StartCoroutine(ToggleMiniMenuChallenges(null));
	}

	private void UpdateChallenges(Actions.ChallengeType challengeType, float score)
	{
		switch (challengeType)
		{
			case Actions.ChallengeType.time:
				if (currentLvl.timeChallenge < score)
					ChangeFillAmount(fillClock, score);

				var seconds = currentLvl.music.length - currentLvl.music.length * score;
				TimeSpan time = TimeSpan.FromSeconds(seconds);
				clockText.text = time.ToString(@"mm\:ss");
				break;

			case Actions.ChallengeType.hotspot:
				if (currentLvl.hotspot < score)
					ChangeFillAmount(fillHotspot, score);

				var roundedScore = Mathf.RoundToInt(score * 100);
				hotspotText.text = $"{roundedScore}%";
				break;

			case Actions.ChallengeType.collectible:
				collectiblesScore++;
				float currentScore = (float)collectiblesScore / (float)currentLvl.collectiblesSpawnTimeStamps.Count;

				if (currentLvl.collectibles < currentScore)
					ChangeFillAmount(fillCollectibles, currentScore);

				collectiblesText.text = $"{collectiblesScore}/{currentLvl.collectiblesSpawnTimeStamps.Count}";
				break;
		}
	}
	// Show/hide challenges of lvl
	private IEnumerator ToggleMiniMenuChallenges(LevelSO lvl)
	{
		pBlockTouchMiniMenu.SetActive(lvl != null);
		pBlockChallangesMiniMenu.SetActive(true);

		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < secondsToHideChallenges)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / secondsToHideChallenges;

			pBlockChallangesMiniMenu.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time));
			yield return null;
		}

		pChallangesMiniMenu.SetActive(lvl != null);

		time = 0;
		elapsedTime = 0;
		while (elapsedTime < secondsToHideChallenges)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / secondsToHideChallenges;

			pBlockChallangesMiniMenu.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time));
			yield return null;
		}

		pBlockChallangesMiniMenu.SetActive(false);
	}
}
