using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LvlChooserManager : MonoBehaviour
{

	[Header("Lvl chooser")]
	[SerializeField] private GameObject lvlPLvlChooser;
	[SerializeField] private GameObject lvlPanelPrefab;

	private void OnEnable()
	{
		// Actions.onLoadLevels += LoadPanelLevels;
	}

	private void OnDisable()
	{
		// Actions.onLoadLevels -= LoadPanelLevels;
	}

	private void LoadPanelLevels(List<Level> levels)
	{
		foreach (Level lvl in levels)
		{
			GameObject lvlPanel = Instantiate(lvlPanelPrefab, lvlPLvlChooser.transform);
			lvlPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lvl.levelNum.ToString();
			lvlPanel.GetComponentInChildren<Button>().onClick.AddListener(() => UIStartLvl(lvl.levelNum));
		}

		lvlPLvlChooser.GetComponent<LvlSwiper>().Populated();
	}

	private void UIStartLvl(int lvl)
	{
		// Activamos el panel que evita que toques otro boton del panel Game
		Debug.Log("El listener funciona");
		// pBlockTouchGame.SetActive(true);
		// FadeChildren(pUIGame, false);
		// StartCoroutine(LvlBuilderStartLvl(lvl));
	}
}
