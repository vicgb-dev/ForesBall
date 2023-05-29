using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class LoadSaveEncrypted
{
	private static readonly byte[] key = Encoding.UTF8.GetBytes("keyParaAES123456");
	private static readonly byte[] iv = Encoding.UTF8.GetBytes("vicvicvictor1234");

	private static readonly string fileName = "/game.save";

	public static void Delete()
	{
		string path = Application.persistentDataPath + fileName;
		if (File.Exists(path))
			File.Delete(path);

		Debug.LogWarning("Fichero de guardado borrado");
	}

	public static void Save(GameState gameState)
	{
		// Convertir el objeto del estado del juego a una matriz de bytes
		BinaryFormatter formatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		formatter.Serialize(memoryStream, gameState);
		byte[] gameStateBytes = memoryStream.ToArray();

		// Cifrar la matriz de bytes utilizando AES
		Aes aes = Aes.Create();
		aes.Key = key;
		aes.IV = iv;
		ICryptoTransform encryptor = aes.CreateEncryptor();
		byte[] encryptedGameStateBytes = encryptor.TransformFinalBlock(gameStateBytes, 0, gameStateBytes.Length);

		// Escribir la matriz de bytes cifrada en el archivo de guardado
		string path = Application.persistentDataPath + fileName;
		FileStream stream = new FileStream(path, FileMode.Create);
		stream.Write(encryptedGameStateBytes, 0, encryptedGameStateBytes.Length);
		stream.Close();
	}

	public static GameState Load()
	{
		// Leer la matriz de bytes cifrada del archivo de guardado
		string path = Application.persistentDataPath + fileName;
		//Debug.Log(path);
		if (File.Exists(path))
		{
			FileStream stream = new FileStream(path, FileMode.Open);
			byte[] encryptedGameStateBytes = new byte[stream.Length];
			stream.Read(encryptedGameStateBytes, 0, encryptedGameStateBytes.Length);
			stream.Close();

			// Descifrar la matriz de bytes utilizando AES
			Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;
			ICryptoTransform decryptor = aes.CreateDecryptor();
			byte[] gameStateBytes = decryptor.TransformFinalBlock(encryptedGameStateBytes, 0, encryptedGameStateBytes.Length);

			// Convertir la matriz de bytes descifrada a un objeto del estado del juego
			MemoryStream memoryStream = new MemoryStream(gameStateBytes);
			BinaryFormatter formatter = new BinaryFormatter();
			GameState gameState = formatter.Deserialize(memoryStream) as GameState;

			return gameState;
		}
		else
		{
			Debug.LogWarning("Fichero no encontrado en: " + path);
			return null;
		}
	}

	public static byte[] GenerateRandomKey()
	{
		using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
		{
			byte[] key = new byte[16];
			rng.GetBytes(key);
			return key;
		}
	}
}