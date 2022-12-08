using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

		foreach (ColorsSO color in colors)
		{
			// Instancias y populate un panel
			GameObject pack = Instantiate(colorPackButton, menuContent.transform);
			pack.GetComponent<ColorPack>().SetUp(color);
			pack.GetComponent<Button>().onClick.AddListener(() => ColorsManager.Instance.ChangeColors(color));
		}
	}

	private void Start()
	{
		button1.GetComponent<Button>().onClick.AddListener(() => ColorsManager.Instance.UpdateGlobalColors());
		button1.GetComponentInChildren<TextMeshProUGUI>().text = "Actualizar colores";
	}
}