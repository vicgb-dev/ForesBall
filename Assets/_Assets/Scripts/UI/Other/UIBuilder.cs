using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIBuilder : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject canvas;
	[SerializeField] private GameObject pSafeArea;
	[SerializeField] private GameObject pMinimenu;
	[SerializeField] private GameObject pJoystick;
	[SerializeField] private GameObject pNotification;
	[SerializeField] private GameObject pauseButton;
	[SerializeField] private GameObject pGame;

	[Header("Lvl chooser")]
	[SerializeField] private GameObject lvlPLvlChooser;
	[SerializeField] private GameObject lvlPanelPrefab;

	private LevelSO currentLvl;
	private int currentLvlIndex;
	private bool populated;
	// Fit the size of UI to the safe zone of the screen (responsive)
	private void Awake()
	{
		RectTransform rectTransform = pSafeArea.GetComponent<RectTransform>();
		Rect safeArea = Screen.safeArea;
		Vector2 minAnchor = safeArea.position;
		Vector2 maxAnchor = minAnchor + safeArea.size;

		minAnchor.x /= Screen.width;
		minAnchor.y /= Screen.height;
		maxAnchor.x /= Screen.width;
		maxAnchor.y /= Screen.height;

		rectTransform.anchorMin = minAnchor;
		rectTransform.anchorMax = maxAnchor;
	}

	private void OnEnable()
	{
		Actions.onLvlStart += SaveCurrentLvl;
		Actions.onLvlEnd += ReloadLevelUI;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= SaveCurrentLvl;
		Actions.onLvlEnd -= ReloadLevelUI;
	}

	private void SaveCurrentLvl(LevelSO level)
	{
		currentLvl = level;
		currentLvlIndex = LvlBuilder.Instance.GetLevels().IndexOf(currentLvl);
	}

	private void ReloadLevelUI(bool win)
	{
		StartCoroutine(ReloadLevelUICo(currentLvl));
	}

	private IEnumerator ReloadLevelUICo(LevelSO level)
	{
		// Esperamos un frame para que el LvlBuilder actualice los valores de los desafios del nivel
		yield return null;
		Transform lvlPanel = lvlPLvlChooser.transform.GetChild(currentLvlIndex);

		DrawChallenges(lvlPanel.GetChild(0).GetChild(3).GetChild(0));
	}

	private void DrawChallenges(Transform challenges)
	{
		// Debug.LogWarning($"currentLvl {currentLvl.name}");
		bool timeChallengeCompleted = currentLvl.timeChallenge == 1;
		bool hotspotChallengeCompleted = currentLvl.hotspot == 1;
		bool collectiblesChallengeCompleted = currentLvl.collectibles == 1;
		Color color = ColorsManager.Instance.GetChallengesColor();

		Image timeChallenge = challenges.GetChild(0).GetComponent<Image>();
		timeChallenge.fillAmount = currentLvl.timeChallenge;
		if (timeChallengeCompleted)
		{
			timeChallenge.color = color;
			ColorsManager.Instance.imagesChallenges.Add(timeChallenge);
			timeChallenge.transform.GetChild(0).GetComponent<Image>().color = Color.black;
		}

		Image hotspot = challenges.GetChild(1).GetComponent<Image>();
		hotspot.fillAmount = currentLvl.hotspot;
		if (hotspotChallengeCompleted)
		{
			hotspot.color = color;
			ColorsManager.Instance.imagesChallenges.Add(hotspot);
			hotspot.transform.GetChild(0).GetComponent<Image>().color = Color.black;
		}

		Image collectibles = challenges.GetChild(2).GetComponent<Image>();
		collectibles.fillAmount = currentLvl.collectibles;
		if (collectiblesChallengeCompleted)
		{
			collectibles.color = color;
			ColorsManager.Instance.imagesChallenges.Add(collectibles);
			collectibles.transform.GetChild(0).GetComponent<Image>().color = Color.black;
		}

		UpdateLockedLvl();
	}

	private void UpdateLockedLvl()
	{
		if (!populated) return;

		int totalChallengesComplated = LoadSaveManager.Instance.LoadAccomplishments().totalChallengesCompleted;
		var lvls = LvlBuilder.Instance.GetLevels();
		for (int i = 0; i < lvls.Count; i++)
		{
			lvlPLvlChooser.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"you need\n{lvls[i].objectivesToUnlock - totalChallengesComplated} more objectives\nto unlock";
			if (lvls[i].objectivesToUnlock <= totalChallengesComplated)
			{
				if (lvlPLvlChooser.transform.GetChild(i).GetChild(1).gameObject.activeSelf)
				{
					NotificationsSystem.Instance.NewNotification($"New level unlocked!");
				}
				lvlPLvlChooser.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
			}
		}
	}

	void Start()
	{
		ReorganizeParents();
		LoadPanelLevels(LvlBuilder.Instance.GetLevels());
	}

	// Reorganize parents to use masks
	public void ReorganizeParents()
	{
		pGame.transform.SetParent(canvas.transform);
		pMinimenu.transform.SetParent(canvas.transform);
		pJoystick.transform.SetParent(canvas.transform);
		pNotification.transform.SetParent(canvas.transform);
		pNotification.transform.SetAsLastSibling();
		pSafeArea.transform.SetParent(pGame.transform);
		pSafeArea.transform.SetSiblingIndex(0);

		for (int i = 0; i < pSafeArea.transform.childCount; i++)
			pSafeArea.transform.GetChild(i).SetParent(canvas.transform);

		pauseButton.transform.SetParent(canvas.transform);
		pauseButton.transform.SetAsLastSibling();
		pSafeArea.transform.localScale *= 2;
	}

	// Create panels to choose level
	private void LoadPanelLevels(List<LevelSO> levels)
	{
		int totalChallengesComplated = LoadSaveManager.Instance.LoadAccomplishments().totalChallengesCompleted;
		int cont = 1;
		foreach (LevelSO level in levels)
		{
			currentLvl = level;
			GameObject lvlPanel = Instantiate(lvlPanelPrefab, lvlPLvlChooser.transform);
			lvlPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = cont++ + "";
			lvlPanel.transform.GetChild(0).GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{(level.music.length / 60).ToString("00")}:{(level.music.length % 60).ToString("00")}";
			lvlPanel.transform.GetChild(0).GetChild(1).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = level.musicName.ToLower();
			lvlPanel.transform.GetChild(0).GetChild(1).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"by: {level.musicAuthor.ToLower()}";
			lvlPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"you need\n{level.objectivesToUnlock - totalChallengesComplated} more objectives\nto unlock";
			DrawChallenges(lvlPanel.transform.GetChild(0).GetChild(3).GetChild(0));
			if (level.objectivesToUnlock <= totalChallengesComplated)
				lvlPanel.transform.GetChild(1).gameObject.SetActive(false);
		}
		lvlPLvlChooser.GetComponent<LvlSwiper>().Populate();
		populated = true;
	}
}
