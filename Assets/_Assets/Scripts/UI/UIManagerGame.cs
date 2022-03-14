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
	public GameObject pUIGame;
	public GameObject pEndGame;
	public Color winBackground;
	public Color loseBackground;
	public float delayStartLvl;

	public float alphaSpeed = 0.1f;

	private void Start()
	{
		ReorganizeParents();
		TestButtons();
	}

	private void TestButtons()
	{
		// GameObject.Find("TestStartLvl").GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.StartLevel(1));
		// GameObject.Find("TestResetScene").GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ResetScene());
		GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(() => UIStartLvl(1));
	}

	private void OnEnable() => Actions.onLvlEnd += BlockGameView;

	private void OnDisable() => Actions.onLvlEnd -= BlockGameView;

	private void UIStartLvl(int lvl)
	{
		FadeChildren(pUIGame, false);
		StartCoroutine(GameManagerStartLvl());
	}

	private void FadeChildren(GameObject GOFaded, bool to1)
	{
		for (int i = 0; i < GOFaded.transform.childCount; i++)
		{
			if (GOFaded.transform.GetChild(i).transform.childCount > 0)
			{
				FadeChildren(GOFaded.transform.GetChild(i).gameObject, to1);
			}
			StartCoroutine(Alpha(GOFaded.transform.GetChild(i).gameObject, to1));
		}

	}

	private IEnumerator GameManagerStartLvl()
	{
		yield return new WaitForSecondsRealtime(delayStartLvl);
		GameManager.Instance.StartLevel(1);
	}

	private void BlockGameView(bool win)
	{
		ChangeColor(win);
		StartCoroutine(Alpha(pEndGame, true));
		FadeChildren(pUIGame, true);
	}

	private void ChangeColor(bool win)
	{
		if (win) pEndGame.GetComponent<Image>().color = winBackground;
		else pEndGame.GetComponent<Image>().color = loseBackground;
	}

	private IEnumerator Alpha(GameObject UI_GO, bool to1)
	{
		Image background;
		Color color;
		if (UI_GO.GetComponent<Image>() != null)
		{
			background = UI_GO.GetComponent<Image>();
			color = background.color;
			if (to1)
			{
				UI_GO.SetActive(true);
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
				UI_GO.SetActive(true);
				color.a = 1;
				while (color.a > 0)
				{
					color.a -= alphaSpeed * Time.unscaledDeltaTime;
					background.color = color;
					yield return null;
				}
				UI_GO.SetActive(false);
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
		pSafeArea.transform.SetSiblingIndex(0);

		for (int i = 0; i < pSafeArea.transform.childCount; i++)
			pSafeArea.transform.GetChild(i).SetParent(canvas.transform);

		pSafeArea.transform.localScale *= 2;

	}
}
