using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Loadsave
{
	public static void Save(GameState gameState)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/game.save";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, gameState);
		stream.Close();
	}

	public static GameState Load()
	{
		string path = Application.persistentDataPath + "/game.save";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			GameState gameState = formatter.Deserialize(stream) as GameState;
			stream.Close();

			return gameState;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}