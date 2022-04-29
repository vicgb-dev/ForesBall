using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject panelCustomize;

	private void OnEnable()
	{
		Actions.onNewUIState += OnNewUIState;
	}

	private void OnNewUIState(UIState state)
	{
		if (state == UIState.Customize)
			panelCustomize.SetActive(true);
		else
			panelCustomize.SetActive(false);
	}
}
