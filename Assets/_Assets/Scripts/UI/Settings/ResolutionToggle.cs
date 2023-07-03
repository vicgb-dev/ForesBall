using TMPro;
using UnityEngine;

public class ResolutionToggle : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI resolutionText;
	[SerializeField] private SlicedFilledImage slider;

	public Color color = Color.white;
	Resolution initialResolution;
	int resolutionOption = 0;

	private void Start()
	{
		initialResolution = Screen.currentResolution;
		resolutionOption = PlayerPrefs.GetInt("resolutionOption", 3);
		resolutionOption--;
		SwitchResolution();
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));
	}
	public void SetColor(Color newColor)
	{
		color = newColor;
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));
	}

	public void SwitchResolution()
	{
		resolutionOption++;
		if (resolutionOption > 3) resolutionOption = 0;
		int option = resolutionOption;
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();
		feedback.SetColorPackSelected(true);
		slider.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
		switch (option)
		{
			case 0:
				slider.fillAmount = 0.25f;
				Screen.SetResolution((int)(initialResolution.width * 0.5f), (int)(initialResolution.height * 0.5f), Screen.fullScreenMode);
				resolutionText.text = "quality: low";
				break;
			case 1:
				slider.fillAmount = 0.5f;
				Screen.SetResolution((int)(initialResolution.width * 0.7f), (int)(initialResolution.height * 0.7f), Screen.fullScreenMode);
				resolutionText.text = "quality: medium";
				break;
			case 2:
				slider.fillAmount = 0.75f;
				Screen.SetResolution((int)(initialResolution.width * 0.9f), (int)(initialResolution.height * 0.9f), Screen.fullScreenMode);
				resolutionText.text = "quality: high";
				break;
			case 3:
				slider.fillAmount = 1f;
				Screen.SetResolution(initialResolution.width, initialResolution.height, Screen.fullScreenMode);
				resolutionText.text = "quality: ultra";
				break;
		}

		PlayerPrefs.SetInt("resolutionOption", resolutionOption);
	}
}
