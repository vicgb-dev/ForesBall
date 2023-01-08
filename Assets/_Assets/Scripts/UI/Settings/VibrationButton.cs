using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{
	[SerializeField] private Image backgroundButton;
	private bool shouldVibrate;
	private Color color;
	private void Start()
	{
		shouldVibrate = PlayerPrefs.GetInt("vibration", 1) != 1;
		ToggleVibration();
	}

	public void ToggleVibration()
	{
		shouldVibrate = !shouldVibrate;
		PlayerPrefs.SetInt("vibration", shouldVibrate ? 1 : 0);
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();
		feedback.SetColorPackSelected(shouldVibrate);
		Vibration.shouldVibrate = shouldVibrate;
		if (shouldVibrate)
		{
			backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
		}
	}

	public void UpdateColor()
	{
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();
		if (shouldVibrate)
		{
			backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
		}
	}
}
