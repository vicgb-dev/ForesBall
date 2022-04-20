using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject canvas;
	[SerializeField] private GameObject pSafeArea;
	[SerializeField] private GameObject pMinimenu;
	[SerializeField] private GameObject pJoystick;
	[SerializeField] private GameObject pGame;
	[SerializeField] private GameObject pUIGame;
	[SerializeField] private GameObject pBlockTouchGame;
	[SerializeField] private GameObject pEndGame;

	[Header("UI Game Options")]
	[SerializeField] private Color winBackground;
	[SerializeField] private Color loseBackground;
	[SerializeField] private float delayStartLvl;

	[Header("Lvl chooser")]
	[SerializeField] private GameObject lvlPLvlChooser;
	[SerializeField] private GameObject lvlPanelPrefab;

	public float alphaSpeed = 0.1f;

	private void Start()
	{
		ReorganizeParents();
		LoadPanelLevels(LvlBuilder.Instance.GetLevels());
		// Desactivamos el panel que evita que toques otro boton del panel Game
		pBlockTouchGame.SetActive(false);
	}

	private void OnEnable()
	{
		Actions.onLvlEnd += BlockGameView;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= BlockGameView;
	}

	// Reorganize parents to use masks
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

	// Crear paneles para elegir nivel
	private void LoadPanelLevels(List<LevelSO> levels)
	{
		Debug.Log(levels.Count);
		int cont = 1;
		foreach (LevelSO level in levels)
		{
			GameObject lvlPanel = Instantiate(lvlPanelPrefab, lvlPLvlChooser.transform);
			lvlPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cont++ + "";
			lvlPanel.GetComponentInChildren<Button>().onClick.AddListener(() => {
				pBlockTouchGame.SetActive(true);
				FadeChildren(pUIGame, false);
				LvlManager.Instance.UIStartedLevel(level);
			});
		}
		lvlPLvlChooser.GetComponent<LvlSwiper>().Populated();
	}

	private void BlockGameView(bool win)
	{
		if (win) pEndGame.GetComponent<Image>().color = winBackground;
		else pEndGame.GetComponent<Image>().color = loseBackground;

		StartCoroutine(Alpha(pEndGame, true));
		FadeChildren(pUIGame, true);
		pBlockTouchGame.SetActive(false);
	}

	private void FadeChildren(GameObject GOFaded, bool to1)
	{
		for (int i = 0; i < GOFaded.transform.childCount; i++)
		{
			if (GOFaded.transform.GetChild(i).transform.childCount > 0)
				FadeChildren(GOFaded.transform.GetChild(i).gameObject, to1);

			StartCoroutine(Alpha(GOFaded.transform.GetChild(i).gameObject, to1));
		}
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
}