using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlBuilder : MonoBehaviour
{
	// Componente que se ocupa de preparar enemigos, powerups y probabilidades
	[Header("Options")]
	[SerializeField] private float delayStartTime;

	[Header("Enemies")]
	[SerializeField] private EnemiesManagerSO enemiesManagerSO;

	[Header("Levels")]
	[SerializeField] private LevelsManagerSO levelsManagerSO;

	private Dictionary<Limits, float> limits;
	private bool spawned = false;
	private Collider2D[] colliders = new Collider2D[0];
	private LevelSO currentLvl;
	private List<GameObject> enemiesGO = new List<GameObject>();

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

	private void StopSpawns(bool win) => StopAllCoroutines();

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

		StartCoroutine(SpawnEnemy(0, currentLvl.straightSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(1, currentLvl.followSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(2, currentLvl.bigSpawnTimeStamps));
	}

	private void SetUpEnemy(GameObject enemy)
	{

	}

	private IEnumerator CountdownToWin(float timeToWin)
	{
		yield return new WaitForSecondsRealtime(timeToWin);

		SoundManager.Instance.PlayWin();

		foreach (GameObject enemy in enemiesGO)
		{
			enemy.GetComponent<Enemy>().StopMoving();

			enemy.tag = Tag.Untagged.ToString();
		}
		yield return new WaitForSecondsRealtime(2);

		float y = 1;
		foreach (GameObject enemy in enemiesGO)
		{
			Destroy(enemy);
			SoundManager.Instance.PlayPop();

			yield return new WaitForSecondsRealtime(1 / y);
			y++;
		}

		yield return new WaitForSecondsRealtime(1);

		Actions.onLvlEnd?.Invoke(true);
	}

	private IEnumerator SpawnEnemy(int position, List<float> timeStamps)
	{
		GameObject enemyPrefab = enemiesManagerSO.enemies[position].enemyPrefab;
		SpriteRenderer sRenderer = enemyPrefab.GetComponentInChildren<SpriteRenderer>();
		int enemies = timeStamps.Count;
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
					UnityEngine.Random.Range(limits[Limits.left] + sRenderer.size.x, limits[Limits.right] - sRenderer.size.x),
					UnityEngine.Random.Range(limits[Limits.bottom] + sRenderer.size.y, limits[Limits.up] - sRenderer.size.y),
					0);

				if (CanSpawn(newLocation, sRenderer.size.x * 0.75f))
				{
					GameObject instantiateEnemy = Instantiate(enemyPrefab, newLocation, enemyPrefab.transform.rotation);
					enemiesManagerSO.enemies[position].SetUpEnemy(instantiateEnemy);
					enemiesGO.Add(instantiateEnemy);
					spawned = true;
				}
			}
			spawned = false;
			enemies--;
		}
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
	}
}