using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CustomizeMenuManager : Menu
{
	[SerializeField] private GameObject menuContent;
	[SerializeField] private GameObject colorPackButton;
	[SerializeField] private List<ColorsSO> colors;

	[SerializeField] protected GameObject button1;

	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Customize;
		panelDirection = Direction.Right;

		int? idColor = LoadSaveManager.Instance.LoadColorTheme();
		if (idColor != null)
		{
			ColorsSO loadedColor = colors.Where(color => color.idColor == idColor).ToList().First();
			ColorsManager.Instance.ChangeColors(loadedColor);
		}
		else
		{
			idColor = 0;
		}

		foreach (ColorsSO colorSO in colors)
		{
			// Instancias y populate un panel
			GameObject pack = Instantiate(colorPackButton, menuContent.transform);
			pack.GetComponent<ColorPack>().SetUp(colorSO, colorSO.idColor == idColor);
			pack.GetComponent<Button>().onClick.AddListener(() =>
			{
				Actions.colorsChange?.Invoke(colorSO);
				ColorsManager.Instance.ChangeColors(colorSO);
				LoadSaveManager.Instance.SaveColorTheme(colorSO.idColor);
			});
		}
	}

	private void Start()
	{
		button1.GetComponent<Button>().onClick.AddListener(() => ColorsManager.Instance.UpdateGlobalColors());
		button1.GetComponentInChildren<TextMeshProUGUI>().text = "Actualizar colores";
	}
}