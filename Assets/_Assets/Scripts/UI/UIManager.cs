using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField] private UIGameViewManager uIGameViewManager;

	private void OnEnable()
	{
		Actions.onLvlEnd += (win) => uIGameViewManager.BlockGameView(win);
		Actions.onLvlStart += (lvl) => uIGameViewManager.CleanGameView(lvl);
	}

}