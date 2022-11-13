using System.Collections.Generic;
using UnityEngine;

public class TestSpawnEnemy : MonoBehaviour
{
	public EnemiesManagerSO enemiesSO;

	private List<GameObject> enemiesGO = new List<GameObject>();

	void Start()
	{
		// Invoke(nameof(SpawnEnemy), 1);
		// Invoke(nameof(StopEnemy), 3);
		// Invoke(nameof(DestroyEnemy), 4);
	}

	[ContextMenu("SpawnEnemy")]
	public void SpawnEnemy()
	{
		for (int i = 0; i < enemiesSO.enemies.Count; i++)
		{
			GameObject enemy = Instantiate(enemiesSO.enemies[i].enemyPrefab);
			enemiesSO.enemies[i].SetUpEnemy(enemy);
			enemiesGO.Add(enemy);
		}
	}

	[ContextMenu("StopEnemy")]
	public void StopEnemy()
	{
		foreach (var enemyGO in enemiesGO)
		{
			enemyGO.GetComponent<Enemy>().StopMoving();
		}
	}

	[ContextMenu("DestroyEnemy")]
	public void DestroyEnemy()
	{
		foreach (var enemyGO in enemiesGO)
		{
			Destroy(enemyGO);
		}
	}
}
