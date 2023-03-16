using UnityEngine;
using TMPro;

public class PanelScoreManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI title;

	private void OnEnable()
	{
		Actions.onNewUIState += OnNewUIState;
	}

	private void OnDisable()
	{
		Actions.onNewUIState -= OnNewUIState;
	}

	private void OnNewUIState(UIState state)
	{
		switch (state)
		{
			case UIState.Main:
				title.text = "main menu";
				break;
			case UIState.Levels:
				title.text = "levels";
				break;
			case UIState.Challenges:
				title.text = "challenges";
				break;
			case UIState.Customize:
				title.text = "customize";
				break;
			case UIState.Settings:
				title.text = "settings";
				break;
		}
	}
}
