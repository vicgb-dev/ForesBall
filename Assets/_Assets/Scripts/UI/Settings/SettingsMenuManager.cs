using UnityEngine;
using TMPro;

public class SettingsMenuManager : Menu
{
	[SerializeField] private GameObject customizeMenu;
	[SerializeField] private GameObject gameMenu;
	[SerializeField] private TextMeshProUGUI resolutionText;

	int cont = 0;
	Resolution initialResolution;

	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Settings;
		panelDirection = Direction.Right;

		initialResolution = Screen.currentResolution;
		resolutionText.text = $"{initialResolution.width}x{initialResolution.height}";
	}

	public void SwitchResolution()
	{
		switch (cont)
		{
			case 0:
				Screen.SetResolution((int)(initialResolution.width * 0.9f), (int)(initialResolution.height * 0.9f), Screen.fullScreenMode);
				break;
			case 1:
				Screen.SetResolution((int)(initialResolution.width * 0.7f), (int)(initialResolution.height * 0.7f), Screen.fullScreenMode);
				break;
			case 2:
				Screen.SetResolution((int)(initialResolution.width * 0.5f), (int)(initialResolution.height * 0.5f), Screen.fullScreenMode);
				break;
			case 3:
				Screen.SetResolution(initialResolution.width, initialResolution.height, Screen.fullScreenMode);
				break;
		}
		cont++;
		if (cont > 3) cont = 0;


		resolutionText.text = $"{Screen.currentResolution.width}x{Screen.currentResolution.height}";
	}

	public void TestDisbaleCustomAndGameUI()
	{
		Debug.Log("quitando");
		customizeMenu.SetActive(!customizeMenu.activeSelf);
		gameMenu.SetActive(!gameMenu.activeSelf);
	}
}