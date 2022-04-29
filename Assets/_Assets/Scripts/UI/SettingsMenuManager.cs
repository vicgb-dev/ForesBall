using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject panelSettings;

	private void OnEnable()
	{
		Actions.onNewUIState += OnNewUIState;
	}

	private void OnNewUIState(UIState state)
	{
		if (state == UIState.Settings)
			panelSettings.SetActive(true);
		else
			panelSettings.SetActive(false);
	}
}
