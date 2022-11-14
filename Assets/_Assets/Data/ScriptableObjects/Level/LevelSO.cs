using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-0", menuName = "Levels/New level")]
public class LevelSO : ScriptableObject
{
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

	[Space(10)]
	[Header("Enemies")]
	public List<float> straightSpawnTimeStamps;
	public List<float> followSpawnTimeStamps;
	public List<float> bigSpawnTimeStamps;
	public List<float> raySpawnTimeStamps;

	[Space(10)]
	[Header("Power Ups")]
	public List<float> powerUpsInmortalTimeStamps;
	public List<float> powerUpsShrinkTimeStamps;

	[Space(10)]
	[Header("Challenges")]
	[Range(0f, 1f)]
	public float timeChallenge;
	[Range(0f, 1f)]
	public float hotspot;
	[Range(0f, 1f)]
	public float collectibles;
	public bool unlocked;
}
