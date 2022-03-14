using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlBuilder : MonoBehaviour
{
	// Componente que se ocupa de preparar enemigos, powerups y probabilidades

	[Header("Configuration")]
	public float delayFirstSpawn;
	public float timeBtwSpawns;

	[Header("Prefabs")]
	public GameObject enemyStraightPrefab;
	public GameObject enemyFollowPrefab;
	public GameObject enemyBigPrefab;

	[Header("Levels")]
	public List<Level> levels;

	private Dictionary<Limits, float> limits;
	private bool spawned = false;
	private Collider2D[] colliders = new Collider2D[0];

	private void OnEnable()
	{
		Actions.onLvlStart += StartLevel;
		Actions.onLvlEnd += StopSpawns;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= StartLevel;
		Actions.onLvlEnd -= StopSpawns;
	}

	private void StartLevel(int lvlNum)
	{
		Debug.Log("Comienza el lvl " + lvlNum);
		limits = GameManager.Instance.limits;

		SoundManager.Instance.PlayAudio(levels[lvlNum - 1].songName);

		StopAllCoroutines();
		StartCoroutine(SpawnEnemy(enemyStraightPrefab, levels[lvlNum - 1].enemiesStraight, delayFirstSpawn, timeBtwSpawns));
		// StartCoroutine(SpawnEnemy(enemyFollowPrefab, levels[lvlNum - 1].enemiesFollow));
		// StartCoroutine(SpawnEnemy(enemyBigPrefab, levels[lvlNum - 1].enemiesBig));
	}

	private IEnumerator SpawnEnemy(GameObject enemyPrefab, int enemies, float firstSpawn, float btwSpawns)
	{
		yield return new WaitForSeconds(firstSpawn);
		SpriteRenderer sRenderer = enemyPrefab.GetComponentInChildren<SpriteRenderer>();
		while (enemies > 0)
		{
			yield return new WaitForSeconds(btwSpawns);
			while (!spawned)
			{
				Vector3 newLocation = new Vector3(
					Random.Range(limits[Limits.left] + sRenderer.size.x, limits[Limits.right] - sRenderer.size.x),
					Random.Range(limits[Limits.bottom] + sRenderer.size.y, limits[Limits.up] - sRenderer.size.y),
					0);


				if (CanSpawn(newLocation, sRenderer.size.x / 2))
				{
					Instantiate(enemyPrefab, newLocation, enemyPrefab.transform.rotation);
					spawned = true;
				}
				else
				{
					Debug.LogError("Habia algun collider en la zona");
				}

				yield return null;
			}
			spawned = false;
			enemies--;
		}
		Debug.Log("Fin de la rutina");

	}

	private void StopSpawns(bool win)
	{
		StopAllCoroutines();
	}

	private bool CanSpawn(Vector3 center, float radius)
	{
		colliders = Physics2D.OverlapCircleAll(center, radius);

		for (int i = 0; i < colliders.Length; i++)
		{
			Vector3 centerPoint = colliders[i].bounds.center;
			float width = colliders[i].bounds.extents.x;
			float heigth = colliders[i].bounds.extents.y;

			float leftExtent = centerPoint.x - width * 2;
			float rightExtent = centerPoint.x + width * 2;
			float lowerExtent = centerPoint.y - heigth * 2;
			float upperExtent = centerPoint.y + heigth * 2;

			if (center.x >= leftExtent && center.x <= rightExtent && center.y >= lowerExtent && center.y <= upperExtent)
				return false;

		}
		return true;
		//return Physics2D.OverlapCircleNonAlloc(center, radius, colliders) == 0 ? true : false);
	}
}
