using UnityEngine;
using UnityEngine.UI;

public class LeftHanded : MonoBehaviour
{
	[SerializeField] private Image backgroundButton;
	[SerializeField] private Image icon;
	[SerializeField] private RectTransform miniMenuRect;
	[SerializeField] private RectTransform playButtonRect;
	[SerializeField] private Joystick joystick;
	// Referencias del panel donde esta el joysticj y del minimenu y cambiar sus anchoredpositions

	private bool isLeftHanded;

	private void Start()
	{
		isLeftHanded = PlayerPrefs.GetInt("leftHanded", 1) != 1;
		ToggleLeftHanded();
		if (!isLeftHanded)
		{
			Vector2 tempRect = miniMenuRect.position;
			miniMenuRect.position = playButtonRect.position;
			playButtonRect.position = tempRect;
		}
	}

	public void ToggleLeftHanded()
	{
		isLeftHanded = !isLeftHanded;
		if (isLeftHanded)
		{
			icon.transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			icon.transform.localScale = new Vector3(1, 1, 1);
		}
		PlayerPrefs.SetInt("leftHanded", isLeftHanded ? 1 : 0);
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();
		//feedback.SetColorPackSelected(isLeftHanded);

		// cambiar posiciones
		Vector2 tempRect = miniMenuRect.position;
		miniMenuRect.position = playButtonRect.position;
		playButtonRect.position = tempRect;
		joystick.SetUpContainers();

		// if (isLeftHanded)
		// 	backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
	}

	public void UpdateColor()
	{
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();

		// if (isLeftHanded)
		// 	backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
	}
}
