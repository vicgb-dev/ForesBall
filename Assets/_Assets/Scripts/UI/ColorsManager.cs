using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorsManager : MonoBehaviour
{
	[SerializeField] private ColorsSO colorsSO;

	[Header("Common UI")]
	[SerializeField] public List<Image> imagesCommon;
	[SerializeField] public List<TextMeshProUGUI> textsCommon;
	[SerializeField] public List<Button> buttonsCommon;

	[Header("Challenges")]
	[SerializeField] public List<Image> imagesChallenges;
	[SerializeField] public List<TextMeshProUGUI> textsChallenges;
	[SerializeField] public List<Button> buttonsChallenges;

	[Header("Customize")]
	[SerializeField] public List<Image> imagesCustomize;
	[SerializeField] public List<TextMeshProUGUI> textsCustomize;
	[SerializeField] public List<Button> buttonsCustomize;

	[Header("Settings")]
	[SerializeField] public List<Image> imagesSettings;
	[SerializeField] public List<TextMeshProUGUI> textsSettings;
	[SerializeField] public List<Button> buttonsSettings;

	[Header("Levels")]
	[SerializeField] public List<Image> imagesLevels;
	[SerializeField] public List<TextMeshProUGUI> textsLevels;
	[SerializeField] public List<Button> buttonsLevels;
	[SerializeField] public List<SpriteRenderer> mapLimits;

	private UIState currentUIState;
	#region Singleton

	private static ColorsManager _instance;
	public static ColorsManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<ColorsManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<ColorsManager>();
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

		currentUIState = UIState.Main;
		UpdateGlobalColors();
		_instance = this;
		//DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	private void OnEnable()
	{
		Actions.onNewUIState += ChoseColor;
	}

	private void ChoseColor(UIState state)
	{
		currentUIState = state;
		switch (currentUIState)
		{
			case UIState.Main:
				ChangeCommonColors(colorsSO.mainMenuColor);
				break;
			case UIState.Challenges:
				ChangeCommonColors(colorsSO.challengesColor);
				break;
			case UIState.Customize:
				ChangeCommonColors(colorsSO.customizeMenuColor);
				break;
			case UIState.Settings:
				ChangeCommonColors(colorsSO.settingsMenuColor);
				break;
			case UIState.Levels:
				ChangeCommonColors(colorsSO.levelsMenuColor);
				break;
		}
	}

	public void UpdateGlobalColors()
	{
		ChoseColor(currentUIState);

		// Challenges
		ChangeImageColor(imagesChallenges, colorsSO.challengesColor);
		ChangeTextColor(textsChallenges, colorsSO.challengesColor);
		ChangeButtonPressedColor(buttonsChallenges, colorsSO.challengesColor);

		// Customize
		ChangeImageColor(imagesCustomize, colorsSO.customizeMenuColor);
		ChangeTextColor(textsCustomize, colorsSO.customizeMenuColor);
		ChangeButtonPressedColor(buttonsCustomize, colorsSO.customizeMenuColor);

		// Settings
		ChangeImageColor(imagesSettings, colorsSO.settingsMenuColor);
		ChangeTextColor(textsSettings, colorsSO.settingsMenuColor);
		ChangeButtonPressedColor(buttonsSettings, colorsSO.settingsMenuColor);

		// Levels
		ChangeImageColor(imagesLevels, colorsSO.levelsMenuColor);
		ChangeTextColor(textsLevels, colorsSO.levelsMenuColor);
		ChangeButtonPressedColor(buttonsLevels, colorsSO.levelsMenuColor);
		ChangeMapBoundsColor(mapLimits, colorsSO.levelsMenuColor);
	}

	private void ChangeMapBoundsColor(List<SpriteRenderer> limits, Color color)
	{
		foreach (SpriteRenderer mapLimit in limits)
		{
			mapLimit.color = color;
		}
	}

	private void ChangeCommonColors(Color color)
	{
		ChangeImageColor(imagesCommon, color);
		ChangeTextColor(textsCommon, color);
		ChangeButtonPressedColor(buttonsCommon, color);
	}

	private void ChangeButtonPressedColor(List<Button> buttons, Color color)
	{
		foreach (Button button in buttons)
		{
			ColorBlock colors = button.colors;
			colors.pressedColor = color;
			button.colors = colors;
		}
	}

	private void ChangeImageColor(List<Image> images, Color color)
	{
		foreach (Image image in images)
			image.color = color;
	}

	private void ChangeTextColor(List<TextMeshProUGUI> texts, Color color)
	{
		foreach (TextMeshProUGUI text in textsCommon)
			text.color = color;
	}

	public ColorsSO GetColorsSO()
	{
		return colorsSO;
	}

	public Color GetEnemyColor(EnemySO.EnemyType enemyType)
	{
		switch (enemyType)
		{
			case EnemySO.EnemyType.straight:
				return colorsSO.straightEnemyColor;
			case EnemySO.EnemyType.follow:
				return colorsSO.followEnemyColor;
			case EnemySO.EnemyType.big:
				return colorsSO.bigEnemyColor;
			case EnemySO.EnemyType.ray:
				return colorsSO.rayEnemyColor;
			default:
				return colorsSO.rayEnemyColor;
		}
	}

	public Color GetPowerUpColor() => colorsSO.powerUpColor;
	public Color GetChallengesColor() => colorsSO.challengesColor;
}