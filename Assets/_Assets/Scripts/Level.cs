using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-0", menuName = " Scriptable Objects/Level")]
public class Level : ScriptableObject
{
	[Header("Level data")]
	[SerializeField] private int level;
	[SerializeField] private string songName;

	[Header("Enemies")]
	[SerializeField] private List<EnemyStraight> enemiesStraight;
	[SerializeField] private List<EnemyFollow> enemiesFollow;
	[SerializeField] private List<EnemyBig> enemiesBig;

	[Header("PowerUps")]
	[SerializeField] private List<PowerUpWave> powerUpsWave;
	[SerializeField] private List<PowerUpInmortal> powerUpsInmortal;
}
