using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField] public Joystick joystick;

	public float secondsToMoveLevelPanels;
	public AnimationCurve curveToMove;
	public float secondsToChangeAlpha;
	public AnimationCurve curveToOriginalColor;
	public int currentPanel;

	private UIGameViewManager uIGameViewManager;
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
		uIGameViewManager = GetComponentInChildren<UIGameViewManager>();
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
		Actions.onLvlEnd += (win) => uIGameViewManager.BlockGameView(win);
		Actions.onLvlStart += (lvl) => uIGameViewManager.CleanGameView(lvl);
	}

	private void Start()
	{
		uIGameViewManager.CleanGameView(null);
	}

	public void MainMenuButton1()
	{
		mainMenuManager.MoveMainMenuLeft();
		// Actions.onNewUIState
	}

	public void MainMenuButton2()
	{
		mainMenuManager.MoveMainMenuLeft();
	}

	public void MainMenuButton3()
	{
		mainMenuManager.MoveMainMenuLeft();
	}

	public void MainMenuButton4()
	{
		mainMenuManager.MoveMainMenuLeft();
		uIGameViewManager.BlockGameView();
		StartCoroutine(joystick.ToButton());
	}
}