using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject pBlockTouchMiniMenu;
	[SerializeField] private GameObject pBlockChallangesMiniMenu;
	[SerializeField] private GameObject pChallangesMiniMenu;

	private Vector3 panelLeftPosition;
	private Vector3 panelRightPosition;
	private Vector3 panelUpPosition;
	private Vector3 panelDownPosition;
	private Vector3 panelCenterPosition;


	#region Singleton

	private static MiniMenuManager _instance;
	public static MiniMenuManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<MiniMenuManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<MiniMenuManager>();
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
	}

	#endregion

	private void OnEnable()
	{
		Actions.onLvlEnd += (win) => pBlockTouchMiniMenu.SetActive(false);

		Actions.onLvlStart += StartLvl;
	}

	private void StartLvl(LevelSO lvl)
	{
		StartCoroutine(StartLvlCo(lvl));
	}

	private IEnumerator StartLvlCo(LevelSO lvl)
	{
		pBlockTouchMiniMenu.SetActive(true);
		pBlockChallangesMiniMenu.SetActive(true);

		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < 1)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime;

			pBlockChallangesMiniMenu.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time));
			yield return null;
		}

		pChallangesMiniMenu.SetActive(true);

		time = 0;
		elapsedTime = 0;
		while (elapsedTime < 1)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime;

			pBlockChallangesMiniMenu.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time));
			yield return null;
		}
	}
}
