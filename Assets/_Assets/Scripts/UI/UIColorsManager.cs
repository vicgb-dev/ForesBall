using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIColorsManager : MonoBehaviour
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

	private void Awake()
	{
		currentUIState = UIState.Main;
		ChangeGlobalColors();
	}

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
				ChangeCommonColors(colorsSO.challengesMenuColor);
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

	public void ChangeGlobalColors()
	{
		ChoseColor(currentUIState);

		// Challenges
		ChangeImageColor(imagesChallenges, colorsSO.challengesMenuColor);
		ChangeTextColor(textsChallenges, colorsSO.challengesMenuColor);
		ChangeButtonPressedColor(buttonsChallenges, colorsSO.challengesMenuColor);

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
}