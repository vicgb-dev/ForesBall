using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlBuilder : MonoBehaviour
{
	// Componente que se ocupa de preparar enemigos, powerups y probabilidades

	[Header("Prefabs")]
	public GameObject enemyStraightPrefab;
	public GameObject enemyFollowPrefab;
	public GameObject enemyBigPrefab;

	[Header("Levels")]
	public List<Level> levels;

	private Dictionary<Limits, float> limits;
	private bool spawned = false;
	private Collider2D[] colliders = new Collider2D[0];
	private Level currentLvl;

	//Definición del patrón Singleton
	#region Singleton

	private static LvlBuilder _instance;
	public static LvlBuilder Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<LvlBuilder>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<LvlBuilder>();
			return _instance;
		}
	}

	private void Awake()
	{
		// Si el singleton aun no ha sido inicializado
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion


	private void OnEnable()
	{
		Actions.onLvlEnd += StopSpawns;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= StopSpawns;
	}

	public void StartLevel(int lvlNum)
	{
		currentLvl = levels[lvlNum - 1];
		Actions.onLvlStart?.Invoke(currentLvl);
		Debug.Log("Comienza el lvl " + currentLvl.levelNum);
		limits = GameManager.Instance.limits;

		StopAllCoroutines();
		if (currentLvl.straightCount > 0)
		{
			Debug.Log($"Hay {currentLvl.straightCount} enemigos directos");
			if (currentLvl.straightSpawnTimeStamps.Count > 0)
				StartCoroutine(SpawnEnemy(enemyStraightPrefab, currentLvl.straightCount, currentLvl.straightDelayFirstSpawn, currentLvl.straightSpawnTimeStamps));
			else
				StartCoroutine(SpawnEnemy(enemyStraightPrefab, currentLvl.straightCount, currentLvl.straightDelayFirstSpawn, currentLvl.straightDelayBtwSpawns));
		}

		if (currentLvl.followCount > 0)
		{
			Debug.Log($"Hay {currentLvl.followCount} enemigos seguimiento");
			if (currentLvl.followSpawnTimeStamps.Count > 0)
				StartCoroutine(SpawnEnemy(enemyFollowPrefab, currentLvl.followCount, currentLvl.followDelayFirstSpawn, currentLvl.followSpawnTimeStamps));
			else
				StartCoroutine(SpawnEnemy(enemyFollowPrefab, currentLvl.followCount, currentLvl.followDelayFirstSpawn, currentLvl.followDelayBtwSpawns));
		}

		if (currentLvl.bigCount > 0)
		{
			Debug.Log($"Hay {currentLvl.bigCount} enemigos grandes");
			if (currentLvl.bigSpawnTimeStamps.Count > 0)
				StartCoroutine(SpawnEnemy(enemyBigPrefab, currentLvl.bigCount, currentLvl.bigDelayFirstSpawn, currentLvl.bigSpawnTimeStamps));
			else
				StartCoroutine(SpawnEnemy(enemyBigPrefab, currentLvl.bigCount, currentLvl.bigDelayFirstSpawn, currentLvl.bigDelayBtwSpawns));
		}
	}

	public void EndLevel(bool win)
	{
		Actions.onLvlEnd?.Invoke(win);
		Debug.LogWarning("FIN DEL JUEGO");
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
	}

	private IEnumerator SpawnEnemy(GameObject enemyPrefab, int enemies, float firstSpawn, List<float> timeStamps)
	{
		yield return new WaitForSeconds(firstSpawn);
		SpriteRenderer sRenderer = enemyPrefab.GetComponentInChildren<SpriteRenderer>();
		int counter = 0;
		while (enemies > 0)
		{
			yield return new WaitForSeconds(timeStamps[counter++]);
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
