using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//TO DO: cambiarlo por una interfaz
	public GameObject playerPrefab;

	private Joystick joystick;
	private PlayerMove playerMove;
	public Dictionary<Limits, float> limits = new Dictionary<Limits, float>();

	//Definición del patrón Singleton
	#region Singleton

	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<GameManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<GameManager>();
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
		DontDestroyOnLoad(this.gameObject);


	}
	#endregion

	private void OnEnable()
	{
		Actions.onLvlEnd += TimeStop;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= TimeStop;
	}

	public void SetMapLimits(List<float> limitsFloat)
	{
		limits.Clear();
		limits.Add(Limits.left, limitsFloat[0]);
		limits.Add(Limits.up, limitsFloat[1]);
		limits.Add(Limits.right, limitsFloat[2]);
		limits.Add(Limits.bottom, limitsFloat[3]);
		InitScripts();
	}

	private void InitScripts()
	{
		playerMove = Instantiate(playerPrefab).GetComponentInChildren<PlayerMove>();
		playerMove.Init();

		joystick = GameObject.Find("JDot").GetComponent<Joystick>();
		joystick.Init(playerMove);
	}

	public void StartLevel(int lvlNum)
	{
		Actions.onLvlStart?.Invoke(lvlNum);
	}

	public void EndLevel(bool win)
	{
		Actions.onLvlEnd?.Invoke(win);
	}

	public void TimeStop(bool win)
	{
		Time.timeScale = 0;
	}

	public void ResetScene()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}