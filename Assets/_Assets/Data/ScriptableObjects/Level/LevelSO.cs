using System.Collections;
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
	public List<float> straightSpawnTimeStamps;
	public List<float> followSpawnTimeStamps;
	public List<float> bigSpawnTimeStamps;

	[Space(10)]
	public List<float> powerUpsWaveTimeStamps;
	public List<float> powerUpsInmortalTimeStamps;
}
