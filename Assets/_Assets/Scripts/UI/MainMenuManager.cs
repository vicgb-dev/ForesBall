using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject mainMenu;

	private Vector3 mainMenuLeftPosition;
	private Vector3 mainMenuOriginPosition;

	private void Start()
	{
		RectTransform rT = mainMenu.GetComponent<RectTransform>();
		mainMenuLeftPosition = new Vector3(rT.localPosition.x - rT.rect.width - 10, rT.localPosition.y, rT.localPosition.z);
		mainMenuOriginPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);
	}

	public void MoveMainMenuLeft()
	{
		StartCoroutine(UIHelpers.Instance.MovePanel(mainMenu, mainMenu.transform.localPosition, mainMenuLeftPosition, UIManager.Instance.secondsToMoveLevelPanels, UIManager.Instance.curveToMove));
	}

	public void MoveMainMenuOrigin()
	{
		StartCoroutine(UIHelpers.Instance.MovePanel(mainMenu, mainMenu.transform.localPosition, mainMenuOriginPosition, UIManager.Instance.secondsToMoveLevelPanels, UIManager.Instance.curveToMove));
	}
}