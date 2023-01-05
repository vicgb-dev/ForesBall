using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LvlBuilder : MonoBehaviour
{
	// Componente que se ocupa de preparar enemigos, powerups y probabilidades
	[Header("Options")]
	[SerializeField] private float delayStartTime;

	[Header("Enemies")]
	[SerializeField] private EnemiesManagerSO enemiesManagerSO;

	[Header("Challenges")]
	[SerializeField] private ChallengesManagerSO challengesManagerSO;

	[Header("Power Ups")]
	[SerializeField] private PowerUpsManagerSO powerUpsManagerSO;

	[Header("Levels")]
	[SerializeField] private LevelsManagerSO levelsManagerSO;

	private Dictionary<Limits, float> limits;
	private bool spawned = false;
	private Collider2D[] colliders = new Collider2D[0];
	private LevelSO currentLvl;
	private List<GameObject> enemiesGO = new List<GameObject>();
	private float lvlDuration = 0;
	private float hotspotScore = 0;
	private float collectiblesScore = 0;

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
		List<SavedLevel> savedLevels = LoadSaveManager.Instance.LoadLevels();
		if (savedLevels != null)
			foreach (SavedLevel savedLevel in savedLevels)
				levelsManagerSO.levels.ForEach(lvl =>
				{
					if (lvl.name.Equals(savedLevel.lvlName))
					{
						lvl.timeChallenge = savedLevel.timeChallenge;
						lvl.hotspot = savedLevel.hotspot;
						lvl.collectibles = savedLevel.collectibles;
						lvl.unlocked = savedLevel.unlocked;
					}
				});

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

	private void OnEnable()
	{
		Actions.onLvlStart += StartLevelWithDelay;
		Actions.onLvlEnd += StopSpawns;
		Actions.updateChallenge += UpdateChallenges;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= StartLevelWithDelay;
		Actions.onLvlEnd -= StopSpawns;
		Actions.updateChallenge -= UpdateChallenges;
	}

	public void StartLevelWithDelay(LevelSO lvl)
	{
		currentLvl = lvl;
		Invoke(nameof(StartLevel), delayStartTime);
	}

	// Start Lvl spawner
	public void StartLevel()
	{
		limits = GameManager.Instance.limits;

		SoundManager.Instance.OnStartLvl(currentLvl);

		StopAllCoroutines();

		enemiesGO.Clear();
		StartCoroutine(CountdownToWin(currentLvl.music.length));

		// Enemies
		StartCoroutine(SpawnEnemy(enemiesManagerSO.enemies.Where(enemy => enemy.enemyType == EnemySO.EnemyType.straight).ToList().First(), currentLvl.straightSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(enemiesManagerSO.enemies.Where(enemy => enemy.enemyType == EnemySO.EnemyType.follow).ToList().First(), currentLvl.followSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(enemiesManagerSO.enemies.Where(enemy => enemy.enemyType == EnemySO.EnemyType.big).ToList().First(), currentLvl.bigSpawnTimeStamps));
		StartCoroutine(SpawnEnemy(enemiesManagerSO.enemies.Where(enemy => enemy.enemyType == EnemySO.EnemyType.ray).ToList().First(), currentLvl.raySpawnTimeStamps));

		// PowerUps
		StartCoroutine(SpawnPowerUp(0, currentLvl.powerUpsInmortalTimeStamps));
		StartCoroutine(SpawnPowerUp(1, currentLvl.powerUpsShrinkTimeStamps));

		// Challenges
		SpawnHotspot(challengesManagerSO.challenges.Where(challenge => challenge.challengeType == ChallengeSO.ChallengeType.hotspot).ToList().First());
		StartCoroutine(SpawnCollectibles(challengesManagerSO.challenges.Where(challenge => challenge.challengeType == ChallengeSO.ChallengeType.collectible).ToList().First(), currentLvl.collectiblesSpawnTimeStamps));
	}

	#region spawns

	private void SpawnHotspot(ChallengeSO challenge)
	{
		// Spawn HotSpot
		hotspotScore = 0;
		Vector3 center = new Vector3(limits[Limits.right] + limits[Limits.left], limits[Limits.up] - (limits[Limits.up] - limits[Limits.bottom]) / 2, 0);
		GameObject instantiatedHotspot = Instantiate(challenge.challengePrefab, center, challenge.challengePrefab.transform.rotation);
		challenge.SetUpChallenge(instantiatedHotspot, currentLvl.music.length * currentLvl.percentOfSongToCompleteHotspot / 100);
	}

	private IEnumerator SpawnCollectibles(ChallengeSO challenge, List<float> timeStamps)
	{
		// Spawn collectibles
		collectiblesScore = 0;

		GameObject collectiblePrefab = challenge.challengePrefab;
		SpriteRenderer sRenderer = collectiblePrefab.GetComponentInChildren<SpriteRenderer>();
		int collectibles = timeStamps.Count;
		int counter = 0;
		while (collectibles > 0)
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

			GameObject instantiatedCollectible = Instantiate(collectiblePrefab, newLocation, collectiblePrefab.transform.rotation);
			challenge.SetUpChallenge(instantiatedCollectible);
			collectibles--;
		}

		yield return null;
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
			powerUpsManagerSO.powerUps[powerUpType].SetUpPowerUp(instantiatedPowerUp);
			powerUps--;
		}
	}

	private IEnumerator SpawnEnemy(EnemySO enemy, List<float> timeStamps)
	{
		GameObject enemyPrefab = enemy.enemyPrefab;
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
				if (enemy.enemyType == EnemySO.EnemyType.ray)
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
						enemy.SetUpEnemy(instantiatedEnemy);
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

	#endregion

	// End of game
	private IEnumerator CountdownToWin(float timeToWin)
	{
		lvlDuration = Time.realtimeSinceStartup;

		float time = 0;
		while (time < timeToWin)
		{
			time += Time.unscaledDeltaTime;

			Actions.updateChallenge?.Invoke(Actions.ChallengeType.time, time / currentLvl.music.length > 0.99 ? 1 : time / currentLvl.music.length);
			yield return null;
		}

		SoundManager.Instance.PlayWin();

		Actions.onLvlFinished?.Invoke();

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

	// End lvl win/lose
	private void StopSpawns(bool win)
	{
		StopAllCoroutines();
		CheckCompleteChallenges();
	}

	private void CheckCompleteChallenges()
	{
		// Should show congratulations or not
		int completedChallenges = 0;
		bool showCongratulations = true;
		if (currentLvl.timeChallenge >= 0.99f) completedChallenges++;
		if (currentLvl.hotspot >= 0.999f) completedChallenges++;
		if (currentLvl.collectibles >= 0.99f) completedChallenges++;
		if (completedChallenges >= 2) showCongratulations = false;
		completedChallenges = 0;

		// Update challenges
		float timeCompleted = (Time.realtimeSinceStartup - lvlDuration) / currentLvl.music.length;
		currentLvl.timeChallenge = Mathf.Max(timeCompleted, currentLvl.timeChallenge);
		if (currentLvl.timeChallenge >= 0.99f)
		{
			currentLvl.timeChallenge = 1;
			completedChallenges++;
		}

		currentLvl.hotspot = Mathf.Max(hotspotScore, currentLvl.hotspot);
		if (currentLvl.hotspot >= 0.999f)
		{
			currentLvl.hotspot = 1;
			completedChallenges++;
		}

		currentLvl.collectibles = Mathf.Max(collectiblesScore / currentLvl.collectiblesSpawnTimeStamps.Count, currentLvl.collectibles);
		if (currentLvl.collectibles >= 0.99f)
		{
			currentLvl.collectibles = 1;
			completedChallenges++;
		}

		// Show congratulations and unlock
		if (completedChallenges >= 2)
		{
			var index = levelsManagerSO.levels.IndexOf(currentLvl);
			if (index < levelsManagerSO.levels.Count - 1)
			{
				levelsManagerSO.levels[index + 1].unlocked = true;
				LoadSaveManager.Instance.SaveLevel(new SavedLevel(levelsManagerSO.levels[index + 1].name, levelsManagerSO.levels[index + 1].timeChallenge, levelsManagerSO.levels[index + 1].hotspot, levelsManagerSO.levels[index + 1].collectibles, levelsManagerSO.levels[index + 1].unlocked));
			}
			if (showCongratulations) PlayCongratulations();
		}

		LoadSaveManager.Instance.SaveLevel(new SavedLevel(currentLvl.name, currentLvl.timeChallenge, currentLvl.hotspot, currentLvl.collectibles, currentLvl.unlocked));
	}

	private void PlayCongratulations()
	{
		Debug.Log("FELICIDADES! HAS DESBLOQUEADO EL SIGUIENTE NIVEL");
	}

	#region External Methods

	public List<LevelSO> GetLevels() => levelsManagerSO.levels;

	public float GetMusicLength()
	{
		return currentLvl.music.length;
	}

	private void UpdateChallenges(Actions.ChallengeType challengeType, float score)
	{
		switch (challengeType)
		{
			case Actions.ChallengeType.time:
				break;
			case Actions.ChallengeType.hotspot:
				hotspotScore = score;
				break;
			case Actions.ChallengeType.collectible:
				collectiblesScore++;
				break;
		}
	}

	[ContextMenu("Reset lvls")]
	public void ResetLvls()
	{
		levelsManagerSO.levels.ForEach(lvl =>
		{
			lvl.timeChallenge = 0;
			lvl.collectibles = 0;
			lvl.hotspot = 0;
			lvl.unlocked = false;
		});
		levelsManagerSO.levels[0].unlocked = true;
		LoadSaveManager.Instance.Delete();
	}

	[ContextMenu("Unlock all lvls")]
	public void UnlockAllLvls()
	{
		levelsManagerSO.levels.ForEach(lvl =>
		{
			lvl.timeChallenge = 0;
			lvl.collectibles = 1;
			lvl.hotspot = 1;
			lvl.unlocked = true;
		});
	}

	#endregion
}