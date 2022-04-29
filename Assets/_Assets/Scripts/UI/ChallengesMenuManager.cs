using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengesMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject panelChallenges;

	private void OnEnable()
	{
		Actions.onNewUIState += OnNewUIState;
	}

	private void OnNewUIState(UIState state)
	{
		if (state == UIState.Challenges)
			panelChallenges.SetActive(true);
		else
			panelChallenges.SetActive(false);
	}
}
