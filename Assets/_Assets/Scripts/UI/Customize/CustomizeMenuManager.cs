using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Components;

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
				string accTitle = accomps.Count > 0 ? accomps[0].accomplishmentTitle : "";
				// get string from localization where accomps[0].accomplishmentTitle is the key

				if (accTitle != "")
				{
					LocalizationSettings localizationSettings = LocalizationSettings.Instance;
					StringTable table = localizationSettings.GetStringDatabase().GetTable("Accomplishments");
					string lolizedTitle = table.GetEntry(accTitle).GetLocalizedString();

					LocalizeStringEvent unlockStringEvent = pack.transform.GetChild(0).GetChild(8).GetChild(0).GetComponent<LocalizeStringEvent>();
					unlockStringEvent.StringReference.SetReference("Colors", "completeToUnlock");
					unlockStringEvent.StringReference.Arguments = new object[] { lolizedTitle };
				}
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
		//Debug.Log($"Desbloquando color {idColor}");
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
		{
			return "";
		}
		else
		{

			LocalizationSettings localizationSettings = LocalizationSettings.Instance;
			StringTable table = localizationSettings.GetStringDatabase().GetTable("Colors");
			string lolizedColor = table.GetEntry(colorsList[0].colorName).GetLocalizedString();
			return lolizedColor;
		}
	}
}