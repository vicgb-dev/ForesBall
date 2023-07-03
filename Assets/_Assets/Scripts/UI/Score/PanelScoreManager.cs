using UnityEngine;
using UnityEngine.Localization.Components;

public class PanelScoreManager : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent title;

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
				title.StringReference.SetReference("UI Text", "menuPrincipal");
				break;
			case UIState.Levels:
				title.StringReference.SetReference("UI Text", "niveles");
				break;
			case UIState.Challenges:
				title.StringReference.SetReference("UI Text", "desafios");
				break;
			case UIState.Customize:
				title.StringReference.SetReference("UI Text", "personalizar");
				break;
			case UIState.Settings:
				title.StringReference.SetReference("UI Text", "opciones");
				break;
		}
	}
}
