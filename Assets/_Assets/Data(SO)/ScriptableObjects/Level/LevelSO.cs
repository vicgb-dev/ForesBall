using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-0", menuName = "Levels/New level")]
public class LevelSO : ScriptableObject
{
	[Header("Sound", order = 0)]
	public string musicName;
	public AudioClip music;
	[Range(0f, 1f)]
	public float musicVolume;
	public AudioClip winSound;
	[Range(0f, 1f)]
	public float winSoundVolume;
	public AudioClip loseSound;
	[Range(0f, 1f)]
	public float loseSoundVolume;

	[Space(15, order = 1)]
	[Header("Enemies", order = 2)]
	public List<float> straightSpawnTimeStamps;
	public List<float> followSpawnTimeStamps;
	public List<float> bigSpawnTimeStamps;
	public List<float> raySpawnTimeStamps;

	[Space(15, order = 3)]
	[Header("Power Ups", order = 4)]
	public List<float> powerUpsInmortalTimeStamps;
	public List<float> powerUpsShrinkTimeStamps;

	[Space(15, order = 5)]
	[Header("Challenges completion", order = 6)]
	[Range(0f, 1f)]
	public float timeChallenge;
	[Range(0f, 1f)]
	public float hotspot;
	[Range(0f, 1f)]
	public float collectibles;

	[Space(5, order = 7)]
	[Header("Challenges options", order = 8)]
	[Range(0, 100)]
	public int percentOfSongToCompleteHotspot = 50;
	public List<float> collectiblesSpawnTimeStamps;
	[Space(10, order = 9)]
	public int objectivesToUnlock;
}
