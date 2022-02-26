using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//TO DO: cambiarlo por una interfaz
	public MapBuilder mapBuilder;
	public GameObject playerPrefab;
	public Joystick joystick;

	private PlayerMove playerMove;
	private List<float> limits;

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

	void Awake()
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

	private void Start()
	{
		playerMove = Instantiate(playerPrefab).GetComponentInChildren<PlayerMove>();
		joystick.Init(playerMove);
	}

	public void SetMapLimits(List<float> limits)
	{
		this.limits = limits;
		mapBuilder.Init(limits);
		playerMove.Init(limits);
	}

}