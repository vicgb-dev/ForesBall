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

	[Header("Power Ups")]
	[SerializeField] private PowerUpsManagerSO powerUpsManagerSO;

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

		// Enemies
		StartCoroutine(SpawnEnemy(0, currentLvl.straightSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(1, currentLvl.followSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(2, currentLvl.bigSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(3, currentLvl.raySpawnTimeStamps));

		// PowerUps
		StartCoroutine(SpawnPowerUp(0, currentLvl.powerUpsInmortalTimeStamps));
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
			if (enemy.transform.childCount > 0)
			{
				Destroy(enemy);
				SoundManager.Instance.PlayPop();

				yield return new WaitForSecondsRealtime(1 / y);
				y++;
			}
		}

		yield return new WaitForSecondsRealtime(1);

		Actions.onLvlEnd?.Invoke(true);
	}


	private IEnumerator SpawnPowerUp(int powerUpType, List<float> timeStamps)
	{
		GameObject powerUpPrefab = powerUpsManagerSO.powerUps[powerUpType].powerUpPrefab;
		SpriteRenderer sRenderer = powerUpPrefab.GetComponentInChildren<SpriteRenderer>();
		int powerUps = timeStamps.Count;
		int counter = 0;
		while (powerUps > 0)
		{
			if (counter == 0)
				yield return new WaitForSeconds(timeStamps[counter]);
			else
				yield return new WaitForSeconds(timeStamps[counter] - timeStamps[counter - 1]);

			counter++;

			Vector3 newLocation = new Vector3(
				UnityEngine.Random.Range(limits[Limits.left] + sRenderer.size.x, limits[Limits.right] - sRenderer.size.x),
				UnityEngine.Random.Range(limits[Limits.bottom] + sRenderer.size.y, limits[Limits.up] - sRenderer.size.y),
				0);

			GameObject instantiatedPowerUp = Instantiate(powerUpPrefab, newLocation, powerUpPrefab.transform.rotation);
			powerUps--;
		}
	}

	private IEnumerator SpawnEnemy(int enemyType, List<float> timeStamps)
	{
		GameObject enemyPrefab = enemiesManagerSO.enemies[enemyType].enemyPrefab;
		SpriteRenderer sRenderer = enemyPrefab.GetComponentInChildren<SpriteRenderer>();
		int enemies = timeStamps.Count;
		int counter = 0;
		while (enemies > 0)
		{
			if (counter == 0)
				yield return new WaitForSeconds(timeStamps[counter]);
			else
				yield return new WaitForSeconds(timeStamps[counter] - timeStamps[counter - 1]);

			counter++;

			while (!spawned)
			{
				// Si el enemigo es el tipo rayo
				if (enemyType == 3)
				{
					SpawnRayEnemy(enemyPrefab);
					spawned = true;
				}
				else
				{
					Vector3 newLocation = new Vector3(
						UnityEngine.Random.Range(limits[Limits.left] + sRenderer.size.x, limits[Limits.right] - sRenderer.size.x),
						UnityEngine.Random.Range(limits[Limits.bottom] + sRenderer.size.y, limits[Limits.up] - sRenderer.size.y),
						0);

					if (CanSpawn(newLocation, sRenderer.size.x * 0.75f))
					{
						GameObject instantiatedEnemy = Instantiate(enemyPrefab, newLocation, enemyPrefab.transform.rotation);
						enemiesManagerSO.enemies[enemyType].SetUpEnemy(instantiatedEnemy);
						enemiesGO.Add(instantiatedEnemy);
						spawned = true;
					}
				}
			}
			spawned = false;
			enemies--;
		}
	}

	private void SpawnRayEnemy(GameObject prefab)
	{
		int random = UnityEngine.Random.Range(1, 5);

		// Definimos donde se va a spawnear el rayo
		Vector2 fromPos = Vector2.zero;
		Vector3 toPos = Vector2.zero;

		// Definimos coordenadas de un punto al que va a mirar el rayo
		float thirdWidth = (limits[Limits.right] - limits[Limits.left]) / 3;
		float thirdHeight = (limits[Limits.up] - limits[Limits.bottom]) / 3;
		float randomDirectionX = UnityEngine.Random.Range(limits[Limits.left] + thirdWidth, limits[Limits.right] - thirdWidth);
		float randomDirectionY = UnityEngine.Random.Range(limits[Limits.bottom] + thirdHeight, limits[Limits.up] - thirdHeight);
		toPos = new Vector2(randomDirectionX, randomDirectionY);

		if (random == 1) // Desde arriba
			fromPos = new Vector2(UnityEngine.Random.Range(limits[Limits.left], limits[Limits.right]), limits[Limits.up]);
		else if (random == 2) // Desde la derecha
			fromPos = new Vector2(limits[Limits.right], UnityEngine.Random.Range(limits[Limits.up], limits[Limits.bottom]));
		else if (random == 3) // Desde abajo
			fromPos = new Vector2(UnityEngine.Random.Range(limits[Limits.left], limits[Limits.right]), limits[Limits.bottom]);
		else if (random == 4) // Desde la izquierda
			fromPos = new Vector2(limits[Limits.left], UnityEngine.Random.Range(limits[Limits.up], limits[Limits.bottom]));


		GameObject instantiatedEnemy = Instantiate(prefab, fromPos, Quaternion.Euler(0, 0, 0));
		instantiatedEnemy.transform.up = toPos - instantiatedEnemy.transform.position;
		enemiesManagerSO.enemies[3].SetUpEnemy(instantiatedEnemy);
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