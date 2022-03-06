using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-0", menuName = " Scriptable Objects/Level")]
public class Level : ScriptableObject
{
	[Header("Level data")]
	public int level;
	public string songName;

	[Header("Enemies")]
	public int enemiesStraight;
	public int enemiesFollow;
	public int enemiesBig;

	[Header("PowerUps")]
	public int powerUpsWave;
	public int powerUpsInmortal;
}
