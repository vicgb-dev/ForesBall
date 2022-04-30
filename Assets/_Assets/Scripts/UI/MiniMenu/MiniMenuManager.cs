using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject pBlockTouchMiniMenu;

	private void OnEnable()
	{
		Actions.onLvlStart += (lvl) => pBlockTouchMiniMenu.SetActive(true);
		Actions.onLvlEnd += (win) => pBlockTouchMiniMenu.SetActive(false);
	}
}
