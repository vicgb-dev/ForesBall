using System.Collections.Generic;
using System.Linq;
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
			Debug.Log("Buscando singleton en escena");
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
		Debug.LogWarning($"Guardando nivel {level.ToString()}");
		GameState currentState = GetState();

		if (currentState.savedLevels == null)
			currentState.savedLevels = new List<SavedLevel>();

		// Si encontramos el nivel en el estado lo actualizamos, sino lo añadimos
		bool lvlFound = currentState.savedLevels.Where(lvl =>
		{
			if (lvl.lvlName.Equals(level.lvlName))
			{
				lvl = level;
				return true;
			}
			return false;
		}).ToList().Count > 0;

		if (!lvlFound)
			currentState.savedLevels.Add(level);

		Save(currentState);
	}

	public void SaveColorTheme(int idColor)
	{
		Debug.LogWarning($"Guardando colores con id {idColor}");
		GameState currentState = GetState();
		currentState.idColor = idColor;
		Save(currentState);
	}

	#endregion

	#region Load

	public List<SavedLevel> LoadLevels() => Load()?.savedLevels;

	public int? LoadColorTheme() => Load()?.idColor;

	#endregion

	private GameState GetState() => state ?? new GameState();

	private void Save(GameState newState)
	{
		state = newState;
		LoadSaveEncrypted.Save(newState);
	}
	private GameState Load() => state ?? LoadSaveEncrypted.Load();

	[ContextMenu("BORRAR FICHERO")]
	public void Delete()
	{
		LoadSaveEncrypted.Delete();
	}
}
