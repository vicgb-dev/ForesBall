using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerGame : MonoBehaviour
{
	public GameObject canvas;
	public GameObject pSafeArea;
	public GameObject pGame;

	private void Start()
	{
		ReorganizeParents();
	}

	//Reorganize parents to use masks
	public void ReorganizeParents()
	{
		pGame.transform.SetParent(canvas.transform);
		pSafeArea.transform.SetParent(pGame.transform);

		for (int i = 0; i < pSafeArea.transform.childCount; i++)
			pSafeArea.transform.GetChild(i).SetParent(canvas.transform);

	}
}
