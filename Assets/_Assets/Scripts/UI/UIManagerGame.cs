using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerGame : MonoBehaviour
{
	public GameObject canvas;
	public GameObject pSafeArea;
	public GameObject pMinimenu;
	public GameObject pJoystick;
	public GameObject pGame;
	public GameObject pEndGame;
	public Color winBackground;
	public Color loseBackground;

	public float alphaSpeed = 0.1f;

	private void Start()
	{
		ReorganizeParents();
		TestButtons();
	}

	private void TestButtons()
	{
		GameObject.Find("TestStartLvl").GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.StartLevel(1));
		GameObject.Find("TestResetScene").GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ResetScene());
	}

	private void OnEnable()
	{
		Actions.onLvlEnd += BlockGameView;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= BlockGameView;
	}

	private void BlockGameView(bool win)
	{
		ChangeColor(win);
		StartCoroutine(Alpha(pEndGame, true));
	}

	private void ChangeColor(bool win)
	{
		if (win) pEndGame.GetComponent<Image>().color = winBackground;
		else pEndGame.GetComponent<Image>().color = loseBackground;
	}

	private IEnumerator Alpha(GameObject panel, bool to1)
	{
		Image background;
		Color color;
		background = panel.GetComponent<Image>();
		color = background.color;
		if (to1)
		{
			color.a = 0;
			while (color.a < 1)
			{
				color.a += alphaSpeed * Time.unscaledDeltaTime;
				background.color = color;
				yield return null;
			}
			Actions.onCleanLvl?.Invoke();
		}
		else
		{
			color.a = 1;
			while (color.a > 0)
			{
				color.a -= alphaSpeed * Time.unscaledDeltaTime;
				background.color = color;
				yield return null;
			}
		}
	}

	//Reorganize parents to use masks
	public void ReorganizeParents()
	{
		pGame.transform.SetParent(canvas.transform);
		pMinimenu.transform.SetParent(canvas.transform);
		pJoystick.transform.SetParent(canvas.transform);
		pSafeArea.transform.SetParent(pGame.transform);

		for (int i = 0; i < pSafeArea.transform.childCount; i++)
			pSafeArea.transform.GetChild(i).SetParent(canvas.transform);

		pSafeArea.transform.localScale *= 2;

	}
}
