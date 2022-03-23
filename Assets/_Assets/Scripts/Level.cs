using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-0", menuName = " Scriptable Objects/Level")]
public class Level : ScriptableObject
{
	[Header("Level data")]
	public int levelNum;

	[Space(10)]
	[Header("Audio")]
	public AudioClip music;
	[Range(0f, 1f)]
	public float musicVolume;
	public AudioClip winSound;
	[Range(0f, 1f)]
	public float winSoundVolume;
	public AudioClip loseSound;
	[Range(0f, 1f)]
	public float loseSoundVolume;



	[Space(10)]
	[Header("EnemyStraight")]
	public int straightCount;
	[Range(0f, 5f)]
	public float straightDelayFirstSpawn;
	[Range(0f, 5f)]
	public float straightDelayBtwSpawns;
	public List<float> straightSpawnTimeStamps;

	[Space(10)]
	[Header("EnemyFollow")]
	public int followCount;
	[Range(0f, 5f)]
	public float followDelayFirstSpawn;
	[Range(0f, 5f)]
	public float followDelayBtwSpawns;
	public List<float> followSpawnTimeStamps;

	[Space(10)]
	[Header("EnemyBig")]
	public int bigCount;
	[Range(0f, 5f)]
	public float bigDelayFirstSpawn;
	[Range(0f, 5f)]
	public float bigDelayBtwSpawns;
	public List<float> bigSpawnTimeStamps;


	[Space(10)]
	[Header("PowerUps")]
	public int powerUpsWave;
	public int powerUpsInmortal;
}
