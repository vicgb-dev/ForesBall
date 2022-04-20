using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlManager : MonoBehaviour
{
	[SerializeField] private float delayStartLvlTime = 3;

	#region Singleton

	private static LvlManager _instance;
	public static LvlManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<LvlManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<LvlManager>();
			return _instance;
		}
	}

	private void Awake()
	{
		// Si el singleton aun no ha sido inicializado
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		//DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	public void UIStartedLevel(LevelSO lvl)
	{
		StartCoroutine(StartLvlWithDelay(lvl));
	}

	private IEnumerator StartLvlWithDelay(LevelSO lvl)
	{
		yield return new WaitForSecondsRealtime(delayStartLvlTime);
		Actions.onLvlStart?.Invoke(lvl);
	}
}
