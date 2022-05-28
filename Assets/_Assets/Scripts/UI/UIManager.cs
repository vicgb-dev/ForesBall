using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	public float secondsToMovePanels = 1;
	public AnimationCurve curveToMove;
	public float secondsToChangeAlpha = 0.5f;
	public AnimationCurve curveToOriginalColor;
	[HideInInspector] public int currentPanel;

	private LevelsMenuManager levelsMenuManager;
	private MainMenuManager mainMenuManager;

	#region Singleton

	private static UIManager _instance;
	public static UIManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<UIManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<UIManager>();
			return _instance;
		}
	}

	private void Awake()
	{
		levelsMenuManager = GetComponentInChildren<LevelsMenuManager>();
		mainMenuManager = GetComponentInChildren<MainMenuManager>();

		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		_instance = this;
	}

	#endregion

	private void OnEnable()
	{
		Actions.onLvlEnd += (win) => levelsMenuManager.BlockGameView(win);
		Actions.onLvlStart += (lvl) => levelsMenuManager.CleanGameView(lvl);
	}

	private void Start()
	{
		Actions.onNewUIState?.Invoke(UIState.Main);
	}

	public void MainMenuButton1()
	{
		Actions.onNewUIState?.Invoke(UIState.Challenges);
	}

	public void MainMenuButton2()
	{
		Actions.onNewUIState?.Invoke(UIState.Customize);
	}

	public void MainMenuButton3()
	{
		Actions.onNewUIState?.Invoke(UIState.Settings);
	}

	public void MainMenuButton4()
	{
		Actions.onNewUIState?.Invoke(UIState.Levels);
	}

	public void BackButton()
	{
		Actions.onNewUIState?.Invoke(UIState.Main);
	}

	public void PlayLevel()
	{
		Actions.onLvlStart?.Invoke((LvlBuilder.Instance.GetLevels()[currentPanel]));
	}
}