using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBuilder : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject canvas;
	[SerializeField] private GameObject pSafeArea;
	[SerializeField] private GameObject pMinimenu;
	[SerializeField] private GameObject pJoystick;
	[SerializeField] private GameObject pGame;

	[Header("Lvl chooser")]
	[SerializeField] private GameObject lvlPLvlChooser;
	[SerializeField] private GameObject lvlPanelPrefab;
	// Fit the size of UI to the safe zone of the screen (responsive)
	private void Awake()
	{
		RectTransform rectTransform = pSafeArea.GetComponent<RectTransform>();
		Rect safeArea = Screen.safeArea;
		Vector2 minAnchor = safeArea.position;
		Vector2 maxAnchor = minAnchor + safeArea.size;

		minAnchor.x /= Screen.width;
		minAnchor.y /= Screen.height;
		maxAnchor.x /= Screen.width;
		maxAnchor.y /= Screen.height;

		rectTransform.anchorMin = minAnchor;
		rectTransform.anchorMax = maxAnchor;
	}

	void Start()
	{
		ReorganizeParents();
		LoadPanelLevels(LvlBuilder.Instance.GetLevels());
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

	// Create panels to choose level
	private void LoadPanelLevels(List<LevelSO> levels)
	{
		int cont = 1;
		foreach (LevelSO level in levels)
		{
			GameObject lvlPanel = Instantiate(lvlPanelPrefab, lvlPLvlChooser.transform);
			lvlPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cont++ + "";
		}
		lvlPLvlChooser.GetComponent<LvlSwiper>().Populated();
	}
}
