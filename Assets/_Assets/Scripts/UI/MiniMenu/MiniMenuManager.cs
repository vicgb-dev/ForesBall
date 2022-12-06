using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject pBlockTouchMiniMenu;
	[SerializeField] private GameObject pBlockChallangesMiniMenu;
	[SerializeField] private GameObject pChallangesMiniMenu;
	[SerializeField] private float secondsToHideChallenges;

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
		Actions.onLvlEnd += EndLvl;

		Actions.onLvlStart += StartLvl;
	}

	private void StartLvl(LevelSO lvl)
	{
		StopAllCoroutines();
		StartCoroutine(ToggleMiniMenuChallenges(lvl));
	}

	private void EndLvl(bool win)
	{
		StopAllCoroutines();
		StartCoroutine(ToggleMiniMenuChallenges(null));
	}

	private IEnumerator ToggleMiniMenuChallenges(LevelSO lvl)
	{
		pBlockTouchMiniMenu.SetActive(lvl != null);
		pBlockChallangesMiniMenu.SetActive(true);

		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < secondsToHideChallenges)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / secondsToHideChallenges;

			pBlockChallangesMiniMenu.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time));
			yield return null;
		}

		pChallangesMiniMenu.SetActive(lvl != null);

		time = 0;
		elapsedTime = 0;
		while (elapsedTime < secondsToHideChallenges)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / secondsToHideChallenges;

			pBlockChallangesMiniMenu.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time));
			yield return null;
		}

		pBlockChallangesMiniMenu.SetActive(false);
	}
}
