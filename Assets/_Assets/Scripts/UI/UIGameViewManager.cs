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
	[SerializeField] private float secondsToChangeAlpha;
	[SerializeField] private float secondsToMoveLevelPanels;
	[SerializeField] private AnimationCurve curveToMove;
	[SerializeField] private AnimationCurve curveToOriginalColor;

	[Header("References")]
	[SerializeField] private GameObject pUiGame;
	[SerializeField] private GameObject pBlockTouchGame;
	[SerializeField] private GameObject pEndGame;
	[SerializeField] private GameObject scrollView;
	[SerializeField] private GameObject horizontalScrollBar;

	private Vector3 scrollViewUpPosition;
	private Vector3 scrollViewDownPosition;

	private Vector3 scrollBarUpPosition;
	private Vector3 scrollBarDownPosition;

	private Image endGameImage;
	private Color endGameAlpha0;
	private Color endGameAlpha1;

	private void Start()
	{
		RectTransform rT = pUiGame.GetComponent<RectTransform>();
		scrollViewUpPosition = new Vector3(rT.localPosition.x, rT.localPosition.y + pUiGame.GetComponent<RectTransform>().rect.height, rT.localPosition.z);
		scrollViewDownPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);

		rT = horizontalScrollBar.GetComponent<RectTransform>();
		scrollBarUpPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);
		scrollBarDownPosition = new Vector3(rT.localPosition.x, rT.localPosition.y - horizontalScrollBar.GetComponent<RectTransform>().rect.height - 10, rT.localPosition.z);

		endGameImage = pEndGame.GetComponent<Image>();
		endGameAlpha0 = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 0);
		endGameAlpha1 = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 1);

		pBlockTouchGame.SetActive(false);
	}

	public void CleanGameView(LevelSO lvl)
	{
		StopAllCoroutines();
		// Move elements out of the player's sight
		StartCoroutine(MovePanel(scrollView, scrollViewDownPosition, scrollViewUpPosition, secondsToMoveLevelPanels, curveToMove));
		StartCoroutine(MovePanel(horizontalScrollBar, scrollBarUpPosition, scrollBarDownPosition, secondsToMoveLevelPanels, curveToMove));
		StartCoroutine(ColorChange(endGameImage, endGameImage.color, endGameAlpha0, secondsToChangeAlpha, curveToMove));
		pBlockTouchGame.SetActive(true);
	}

	public void BlockGameView(bool win)
	{
		// Move elements in the player's sight
		StartCoroutine(MovePanel(scrollView, scrollViewUpPosition, scrollViewDownPosition, secondsToMoveLevelPanels, curveToMove));
		StartCoroutine(MovePanel(horizontalScrollBar, scrollBarDownPosition, scrollBarUpPosition, secondsToMoveLevelPanels, curveToMove));
		StartCoroutine(ColorChange(endGameImage, endGameAlpha0, win ? winBackground : loseBackground, secondsToChangeAlpha, curveToMove, () =>
		{
			Actions.onCleanLvl?.Invoke();
			StartCoroutine(ColorChange(endGameImage, endGameImage.color, endGameAlpha1, secondsToChangeAlpha * 10, curveToOriginalColor));
		}));
		pBlockTouchGame.SetActive(false);
	}

	private IEnumerator MovePanel(GameObject go, Vector3 initialPosition, Vector3 finalPosition, float seconds, AnimationCurve curve)
	{
		float time = 0;
		RectTransform rT = go.GetComponent<RectTransform>();
		while (Vector3.Distance(rT.localPosition, finalPosition) > 0.0001f)
		{
			time += Time.unscaledDeltaTime / seconds;
			rT.localPosition = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time));
			yield return null;
		}
	}

	private IEnumerator ColorChange(Image image, Color initialColor, Color finalColor, float secondsToChangeAlpha, AnimationCurve curve, Action callback = null)
	{
		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < secondsToChangeAlpha)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / secondsToChangeAlpha;
			image.color = Color.Lerp(initialColor, finalColor, curve.Evaluate(time));
			yield return null;
		}
		callback?.Invoke();

	}
}
