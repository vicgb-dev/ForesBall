using System.Collections.Generic;
using UnityEngine;

public class TestSpawnEnemy : MonoBehaviour
{
	public EnemiesManagerSO enemiesSO;
	public PowerUpsManagerSO powerUpsSO;

	private List<GameObject> powerUpsGO = new List<GameObject>();
	private List<GameObject> enemiesGO = new List<GameObject>();

	void Start()
	{
		// Invoke(nameof(SpawnEnemy), 1);
		// Invoke(nameof(StopEnemy), 3);
		// Invoke(nameof(DestroyEnemy), 4);
	}

	[ContextMenu("SpawnPowerUps")]
	public void SpawnPowerUps()
	{
		for (int i = 0; i < powerUpsSO.powerUps.Count; i++)
		{
			GameObject powerUp = Instantiate(powerUpsSO.powerUps[i].powerUpPrefab);
			powerUpsSO.powerUps[i].SetUpPowerUp(powerUp);
			powerUpsGO.Add(powerUp);
		}
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
