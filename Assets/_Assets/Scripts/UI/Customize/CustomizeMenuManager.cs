using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class CustomizeMenuManager : Menu
{
	[SerializeField] private GameObject menuContent;
	[SerializeField] private GameObject colorPackButtonPrefab;
	[SerializeField] private List<ColorsSO> colors;

	Dictionary<int, GameObject> colorButtons = new Dictionary<int, GameObject>();

	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Customize;
		panelDirection = Direction.Right;
		BuildColorsMenu();
	}

	public void BuildColorsMenu()
	{
		int selectedColor = LoadSaveManager.Instance.LoadColorTheme() ?? 0; // si no encuentra color lo inicializa a 0, que es el paquete de color default
		ColorsSO loadedColor = colors.Where(color => color.idColor == selectedColor).ToList().First();
		ColorsManager.Instance.ChangeColors(loadedColor);

		foreach (ColorsSO colorSO in colors)
		{
			// Instancias y populate un panel ColorPackLocked
			GameObject pack = Instantiate(colorPackButtonPrefab, menuContent.transform);

			// Bloqueamos todos los colores que no sean el primero, después los achievements se encargarán de desbloquearlos
			if (colorSO.idColor != 0)
			{
				pack.transform.GetChild(0).GetChild(8).gameObject.SetActive(true);
				pack.GetComponent<Button>().enabled = false;
				pack.GetComponent<ButtonFeedback>().enabled = false;
				List<AccomplishmentSO> accomps = AccomplishmentsSystem.Instance.GetAccomplishmentsList().Where(accomp => accomp.idColorUnlock == colorSO.idColor).ToList();
				string unlockText = accomps.Count > 0 ? accomps[0].accomplishmentTitle : "";

				pack.transform.GetChild(0).GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Complete challenge\n\"{unlockText}\"\nto unlock this color";
			}

			pack.GetComponent<ColorPack>().SetUp(colorSO, colorSO.idColor == selectedColor);
			pack.GetComponent<Button>().onClick.AddListener(() =>
			{
				Actions.colorsChange?.Invoke(colorSO);
				ColorsManager.Instance.ChangeColors(colorSO);
				LoadSaveManager.Instance.SaveColorTheme(colorSO.idColor);
			});

			colorButtons.Add(colorSO.idColor, pack);
		}
	}

	public void UnlockColor(int idColor)
	{
		Debug.Log($"Desbloquando color {idColor}");
		if (colorButtons.ContainsKey(idColor))
		{
			GameObject buttonPack = colorButtons[idColor];
			if (!buttonPack.GetComponent<Button>().enabled)
				NotificationsSystem.Instance.NewNotification($"Color {colors[idColor].colorName} unlocked!");
			buttonPack.transform.GetChild(0).GetChild(8).gameObject.SetActive(false);
			buttonPack.GetComponent<Button>().enabled = true;
			buttonPack.GetComponent<ButtonFeedback>().enabled = true;
			buttonPack.GetComponent<ColorPack>().SetBackgroundButton();
		}
		else
		{
			Debug.LogWarning($"Hay un accomplishments que desbloquea el color {idColor} pero este no existe");
		}
	}

	public String GetColorName(int idColorUnlock)
	{
		var colorsList = colors.Where(color => color.idColor == idColorUnlock).ToList();
		if (colorsList.Count == 0)
			return "";
		else
			return colorsList[0].colorName;
	}
}