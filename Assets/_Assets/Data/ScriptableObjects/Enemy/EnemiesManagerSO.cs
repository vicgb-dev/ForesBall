using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/EnemyList")]
public class EnemiesManagerSO : ScriptableObject
{
	[Header("EnemyStraight options")]
	public List<EnemySO> enemies;
}
