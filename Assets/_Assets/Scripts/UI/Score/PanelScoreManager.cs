using System.Collections;
using System.Collections.Generic;
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
				title.text = "Main Menu";
				break;
			case UIState.Levels:
				title.text = "Levels";
				break;
			case UIState.Challenges:
				title.text = "Challenges";
				break;
			case UIState.Customize:
				title.text = "Customize";
				break;
			case UIState.Settings:
				title.text = "Settings";
				break;
		}
	}
}
