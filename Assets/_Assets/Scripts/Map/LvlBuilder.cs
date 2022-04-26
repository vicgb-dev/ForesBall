using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlBuilder : MonoBehaviour
{
	// Componente que se ocupa de preparar enemigos, powerups y probabilidades
	[Header("Options")]
	[SerializeField] private float delayStartTime;

	[Header("Prefabs")]
	public GameObject enemyStraightPrefab;
	public GameObject enemyFollowPrefab;
	public GameObject enemyBigPrefab;

	[Header("Levels")]
	public LevelsManagerSO levelsManagerSO;

	private Dictionary<Limits, float> limits;
	private bool spawned = false;
	private Collider2D[] colliders = new Collider2D[0];
	private LevelSO currentLvl;
	public List<GameObject> enemiesGO;

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
		//DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	public List<LevelSO> GetLevels() => levelsManagerSO.levels;

	private void OnEnable()
	{
		Actions.onLvlStart += StartLevelWithDelay;
		Actions.onLvlEnd += StopSpawns;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= StartLevelWithDelay;
		Actions.onLvlEnd -= StopSpawns;
	}

	public void StartLevelWithDelay(LevelSO lvl)
	{
		currentLvl = lvl;
		Invoke(nameof(StartLevel), delayStartTime);
	}

	public void StartLevel()
	{
		limits = GameManager.Instance.limits;

		SoundManager.Instance.OnStartLvl(currentLvl);

		StopAllCoroutines();

		enemiesGO.Clear();
		StartCoroutine(CountdownToWin(currentLvl.music.length));

		if (currentLvl.straightCount > 0)
		{
			Debug.Log($"Hay {currentLvl.straightCount} enemigos directos");
			if (currentLvl.straightSpawnTimeStamps.Count == currentLvl.straightCount)
				StartCoroutine(SpawnEnemy(enemyStraightPrefab, currentLvl.straightCount, currentLvl.straightDelayFirstSpawn, currentLvl.straightSpawnTimeStamps));
			else
				StartCoroutine(SpawnEnemy(enemyStraightPrefab, currentLvl.straightCount, currentLvl.straightDelayFirstSpawn, currentLvl.straightDelayBtwSpawns));
		}

		if (currentLvl.followCount > 0)
		{
			Debug.Log($"Hay {currentLvl.followCount} enemigos seguimiento");
			if (currentLvl.followSpawnTimeStamps.Count == currentLvl.followCount)
				StartCoroutine(SpawnEnemy(enemyFollowPrefab, currentLvl.followCount, currentLvl.followDelayFirstSpawn, currentLvl.followSpawnTimeStamps));
			else
				StartCoroutine(SpawnEnemy(enemyFollowPrefab, currentLvl.followCount, currentLvl.followDelayFirstSpawn, currentLvl.followDelayBtwSpawns));
		}

		if (currentLvl.bigCount > 0)
		{
			Debug.Log($"Hay {currentLvl.bigCount} enemigos grandes");
			if (currentLvl.bigSpawnTimeStamps.Count == currentLvl.bigCount)
				StartCoroutine(SpawnEnemy(enemyBigPrefab, currentLvl.bigCount, currentLvl.bigDelayFirstSpawn, currentLvl.bigSpawnTimeStamps));
			else
				StartCoroutine(SpawnEnemy(enemyBigPrefab, currentLvl.bigCount, currentLvl.bigDelayFirstSpawn, currentLvl.bigDelayBtwSpawns));
		}
	}

	private IEnumerator CountdownToWin(float timeToWin)
	{
		yield return new WaitForSecondsRealtime(timeToWin);

		SoundManager.Instance.OnEndLvl(true);

		foreach (GameObject enemy in enemiesGO)
		{
			enemy.GetComponent<Enemy>().StopMoving();

			enemy.tag = Tag.Untagged.ToString();
		}
		yield return new WaitForSecondsRealtime(2);

		float timeBetweenDestroy = 1;
		foreach (GameObject enemy in enemiesGO)
		{
			Destroy(enemy);
			timeBetweenDestroy = timeBetweenDestroy - 0.1f > 0.1f ? timeBetweenDestroy - 0.1f : 0.1f;
			yield return new WaitForSecondsRealtime(timeBetweenDestroy);
		}

		yield return new WaitForSecondsRealtime(1);

		SoundManager.Instance.WinFinished();
		Actions.onLvlEnd?.Invoke(true);
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
					GameObject instantiateEnemy = Instantiate(enemyPrefab, newLocation, enemyPrefab.transform.rotation);
					enemiesGO.Add(instantiateEnemy);
					spawned = true;
				}
				else
				{
					//Debug.LogError("Habia algun collider en la zona");
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
			if (counter - 1 < 0)
				yield return new WaitForSeconds(timeStamps[counter]);
			else
				yield return new WaitForSeconds(timeStamps[counter] - timeStamps[counter - 1]);

			counter++;
			while (!spawned)
			{
				Vector3 newLocation = new Vector3(
					Random.Range(limits[Limits.left] + sRenderer.size.x, limits[Limits.right] - sRenderer.size.x),
					Random.Range(limits[Limits.bottom] + sRenderer.size.y, limits[Limits.up] - sRenderer.size.y),
					0);


				if (CanSpawn(newLocation, sRenderer.size.x / 2))
				{
					GameObject instantiateEnemy = Instantiate(enemyPrefab, newLocation, enemyPrefab.transform.rotation);
					enemiesGO.Add(instantiateEnemy);
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
