using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
	[SerializeField] private Image backgroundButton;
	[SerializeField] private Image muteIcon;
	[SerializeField] private Sprite muteOn;
	[SerializeField] private Sprite muteOff;

	private bool isMuted;

	private void Start()
	{
		isMuted = SoundManager.Instance.GetLinearVolume() == 0;
		UpdateVisualMute();
	}

	private void OnEnable()
	{
		Actions.onMute += OnMute;
	}

	private void OnDisable()
	{
		Actions.onMute -= OnMute;
	}

	private void OnMute(bool mute)
	{
		isMuted = mute;
		UpdateVisualMute();
	}

	private void UpdateVisualMute()
	{

		muteIcon.sprite = isMuted ? muteOn : muteOff;
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();
		feedback.SetColorPackSelected(isMuted);
		if (isMuted)
			backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
	}

	public void ToggleMute()
	{
		isMuted = !isMuted;
		Actions.onMute?.Invoke(isMuted);
	}

	public void UpdateColor()
	{
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();

		if (isMuted)
			backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
	}
}