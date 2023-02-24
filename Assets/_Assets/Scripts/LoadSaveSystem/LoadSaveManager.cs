using System.Collections.Generic;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour
{
	private GameState state = null;
	//Definición del patrón Singleton
	#region Singleton

	private static LoadSaveManager _instance;
	public static LoadSaveManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			// Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<LoadSaveManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<LoadSaveManager>();
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

	#region Save

	public void SaveLevel(SavedLevel level)
	{
		GameState currentState = Load();

		if (currentState.savedLevels == null)
			currentState.savedLevels = new List<SavedLevel>();

		// Si encontramos el nivel en el estado lo actualizamos, sino lo añadimos
		int existingIndex = currentState.savedLevels.FindIndex(sl => sl.lvlName == level.lvlName);
		if (existingIndex != -1)
			currentState.savedLevels[existingIndex] = level;
		else
			currentState.savedLevels.Add(level);

		Save(currentState);
	}

	public void SaveAccomplishments(Accomplishments accomplishments)
	{
		GameState currentState = Load();

		currentState.accomplishments = accomplishments;
		Save(currentState);
	}

	public void SaveColorTheme(int idColor)
	{
		GameState currentState = Load();
		currentState.idColor = idColor;
		Save(currentState);
	}

	#endregion

	#region Load

	public List<SavedLevel> LoadLevels() => Load()?.savedLevels;

	public Accomplishments LoadAccomplishments() => Load()?.accomplishments ?? new Accomplishments();

	public int? LoadColorTheme() => Load()?.idColor;

	#endregion

	private void Save(GameState newState)
	{
		state = newState;
		LoadSaveEncrypted.Save(newState);
	}
	private GameState Load() => state ?? LoadSaveEncrypted.Load() ?? new GameState();

	[ContextMenu("BORRAR FICHERO")]
	public void Delete()
	{
		LoadSaveEncrypted.Delete();
	}
}
