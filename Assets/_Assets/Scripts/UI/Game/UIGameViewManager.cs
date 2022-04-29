using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameViewManager : MonoBehaviour
{
	[Header("UI Game Options")]
	[SerializeField] private Color winBackground;
	[SerializeField] private Color loseBackground;

	[Header("References")]
	[SerializeField] private GameObject pUiGame;
	[SerializeField] private GameObject pBlockTouchGame;
	[SerializeField] private GameObject pBlockTouchMiniMenu;
	[SerializeField] private GameObject pEndGame;
	[SerializeField] private GameObject scrollView;
	[SerializeField] private GameObject horizontalScrollBar;

	private Vector3 scrollViewUpPosition;
	private Vector3 scrollViewDownPosition;

	private Vector3 scrollBarUpPosition;
	private Vector3 scrollBarDownPosition;

	private Image endGameImage;
	private Color originalColor;
	private Color endGameAlpha0;
	private Color endGameAlpha1;

	private void Awake()
	{
		RectTransform rT = pUiGame.GetComponent<RectTransform>();
		scrollViewUpPosition = new Vector3(rT.localPosition.x, rT.localPosition.y + rT.rect.height, rT.localPosition.z);
		scrollViewDownPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);

		rT = horizontalScrollBar.GetComponent<RectTransform>();
		scrollBarUpPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);
		scrollBarDownPosition = new Vector3(rT.localPosition.x, rT.localPosition.y - rT.rect.height - 10, rT.localPosition.z);

		endGameImage = pEndGame.GetComponent<Image>();
		originalColor = endGameImage.color;
		endGameAlpha0 = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 0);
		endGameAlpha1 = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 1);

		pBlockTouchGame.SetActive(false);
	}

	private void OnEnable()
	{
		Actions.onNewUIState += OnNewUIState;
		Actions.onLvlStart += (lvl) => pBlockTouchMiniMenu.SetActive(true);
		Actions.onLvlEnd += (win) => pBlockTouchMiniMenu.SetActive(false);
	}

	private void OnNewUIState(UIState state)
	{
		if (state == UIState.Levels)
			BlockGameView();
		else
			CleanGameView();
	}

	// Move elements out of the player's sight
	public void CleanGameView(LevelSO lvl = null)
	{
		StopAllCoroutines();
		StartCoroutine(UIHelpers.Instance.MovePanel(scrollView, scrollView.transform.localPosition, scrollViewUpPosition, UIManager.Instance.secondsToMoveLevelPanels, UIManager.Instance.curveToMove));
		StartCoroutine(UIHelpers.Instance.MovePanel(horizontalScrollBar, horizontalScrollBar.transform.localPosition, scrollBarDownPosition, UIManager.Instance.secondsToMoveLevelPanels, UIManager.Instance.curveToMove));
		StartCoroutine(UIHelpers.Instance.ColorChange(endGameImage, endGameImage.color, endGameAlpha0, UIManager.Instance.secondsToChangeAlpha, UIManager.Instance.curveToMove));
		pBlockTouchGame.SetActive(true);
	}

	// Move elements in the player's sight
	public void BlockGameView(bool? win = null)
	{
		Color finalColor;
		if (win == true)
			finalColor = winBackground;
		else if (win == false)
			finalColor = loseBackground;
		else
			finalColor = originalColor;

		StopAllCoroutines();
		StartCoroutine(UIHelpers.Instance.MovePanel(scrollView, scrollView.transform.localPosition, scrollViewDownPosition, UIManager.Instance.secondsToMoveLevelPanels, UIManager.Instance.curveToMove));
		StartCoroutine(UIHelpers.Instance.MovePanel(horizontalScrollBar, horizontalScrollBar.transform.localPosition, scrollBarUpPosition, UIManager.Instance.secondsToMoveLevelPanels, UIManager.Instance.curveToMove));
		StartCoroutine(UIHelpers.Instance.ColorChange(endGameImage, endGameAlpha0, finalColor, UIManager.Instance.secondsToChangeAlpha, UIManager.Instance.curveToMove, () =>
		{
			Actions.onCleanLvl?.Invoke();
			StartCoroutine(UIHelpers.Instance.ColorChange(endGameImage, endGameImage.color, endGameAlpha1, UIManager.Instance.secondsToChangeAlpha * 10, UIManager.Instance.curveToOriginalColor));
		}));
		pBlockTouchGame.SetActive(false);
	}
}