using UnityEngine;

public class AccomplishmentsSystem : MonoBehaviour
{
	#region Singleton

	private static AccomplishmentsSystem _instance;
	public static AccomplishmentsSystem Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<AccomplishmentsSystem>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<AccomplishmentsSystem>();
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
	}

	#endregion

	[SerializeField] private Accomplishments accomplishments;

	bool inGame = false;
	float timeAlive;
	float songLength;

	private void Start()
	{
		accomplishments = LoadSaveManager.Instance.LoadAccomplishments();
	}

	private void OnEnable()
	{
		Actions.onLvlStart += onLvlStart;
		Actions.onLvlEnd += onLvlEnd;
	}

	private void onLvlStart(LevelSO lvl)
	{
		timeAlive = 0;
		songLength = lvl.music.length;
		Debug.Log("la cancion dura " + songLength);
		LoadSaveManager.Instance.SaveAccomplishments(accomplishments);
		inGame = true;
	}

	private void onLvlEnd(bool win)
	{
		if (timeAlive < 5)
			AddTimesDeadEarly();

		if (timeAlive > songLength - 5 && !win)
			AddTimesDeadLate();

		LoadSaveManager.Instance.SaveAccomplishments(accomplishments);
		inGame = false;
		LoadChallengesMenu();
	}

	private void Update()
	{
		if (inGame)
		{
			timeAlive += Time.deltaTime;
			AddTimePlayed(Time.deltaTime);
		}
	}

	public void LoadChallengesMenu()
	{
		// construir el scroll con los botones usando las variables de accomplishments
	}

	private void AddTimePlayed(float time) => accomplishments.timePlayed += time;//
	public void AddTimeCloseToEnemyFollow(float time) => accomplishments.timeCloseToEnemyFollow += time;//
	public void AddTimeCloseToEnemyRay(float time) => accomplishments.timeCloseToEnemyRay += time;//

	private void AddTimesDeadEarly() => accomplishments.timesDeadEarly++;//
	private void AddTimesDeadLate() => accomplishments.timesDeadLate++;//

	public void AddLvlCompleted() => accomplishments.timesLvlCompleted++;//
	public void AddHotspot() => accomplishments.timesHotspot++;//
	public void AddCollected() => accomplishments.timesCollected++;//
	public void AddTimesInmortal() => accomplishments.timesInmortal++;//
	public void AddTimesShrink() => accomplishments.timesShrink++;//

	public void LvlReached(int lvlCompleted)//
	{
		if (lvlCompleted > accomplishments.lvlReached)
			accomplishments.lvlReached = lvlCompleted;
	}
}
