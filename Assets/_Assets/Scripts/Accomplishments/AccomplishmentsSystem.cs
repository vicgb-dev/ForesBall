using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccomplishmentsSystem : MonoBehaviour
{
	[SerializeField] private Accomplishments accomplishments;
	[SerializeField] private List<AccomplishmentSO> accomplishmentsSO;
	[SerializeField] private ChallengesMenuManager challengesMenu;
	[SerializeField] private CustomizeMenuManager customizeMenu;

	bool inGame = false;
	float timeAlive;
	float songLength;

	public bool losefromPause = false;

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
		accomplishments = LoadSaveManager.Instance.LoadAccomplishments();

		challengesMenu.BuildAccomplishments(accomplishmentsSO);
	}

	#endregion

	private void Start()
	{
		challengesMenu.UpdateAccomplishments(accomplishments);
		CheckCompleteAccomplishments();
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
		LoadSaveManager.Instance.SaveAccomplishments(accomplishments);
		inGame = true;
	}

	private void onLvlEnd(bool win)
	{
		StartCoroutine(onLvlEndCo(win));
	}

	private IEnumerator onLvlEndCo(bool win)
	{
		yield return null;
		if (!losefromPause)
		{
			if (timeAlive < 10)
				AddTimesDeadEarly();

			if (timeAlive > songLength - 5 && !win)
				AddTimesDeadLate();
		}

		LoadSaveManager.Instance.SaveAccomplishments(accomplishments);
		inGame = false;
		challengesMenu.UpdateAccomplishments(accomplishments);
		CheckCompleteAccomplishments();

		losefromPause = false;
	}

	private void Update()
	{
		if (inGame)
		{
			timeAlive += Time.deltaTime;
			AddTimePlayed(Time.deltaTime);
		}
	}

	private void CheckCompleteAccomplishments()
	{
		foreach (AccomplishmentSO accomp in accomplishmentsSO)
		{
			switch (accomp.property)
			{
				case AccompProperty.timePlayed:
					if (accomplishments.timePlayed >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timeCloseToEnemyFollow:
					if (accomplishments.timeCloseToEnemyFollow >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timeCloseToEnemyRay:
					if (accomplishments.timeCloseToEnemyRay >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesDeadEarly:
					if (accomplishments.timesDeadEarly >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesDeadLate:
					if (accomplishments.timesDeadLate >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesLvlCompleted:
					if (accomplishments.timesLvlCompleted >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesHotspot:
					if (accomplishments.timesHotspot >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesCollected:
					if (accomplishments.timesCollected >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesInmortal:
					if (accomplishments.timesInmortal >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.timesShrink:
					if (accomplishments.timesShrink >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
				case AccompProperty.lvlReached:
					if (accomplishments.lvlReached >= accomp.greaterThan)
						customizeMenu.UnlockColor(accomp.idColorUnlock);
					break;
			}
		}
	}

	// TimePlayed
	private void AddTimePlayed(float time) => accomplishments.timePlayed += time;
	public void AddTimeCloseToEnemyFollow(float time) => accomplishments.timeCloseToEnemyFollow += time;
	public void AddTimeCloseToEnemyRay(float time) => accomplishments.timeCloseToEnemyRay += time;

	private void AddTimesDeadEarly() => accomplishments.timesDeadEarly++;
	private void AddTimesDeadLate() => accomplishments.timesDeadLate++;

	public void AddLvlCompleted() => accomplishments.timesLvlCompleted++;
	public void AddHotspot() => accomplishments.timesHotspot++;
	public void AddCollected() => accomplishments.timesCollected++;
	public void AddTimesInmortal() => accomplishments.timesInmortal++;
	public void AddTimesShrink() => accomplishments.timesShrink++;

	public void NewTotalChallengesCompleted(int totalChallenges) => accomplishments.totalChallengesCompleted = totalChallenges;

	public void LvlReached(int lvlCompleted)
	{
		if (lvlCompleted > accomplishments.lvlReached)
			accomplishments.lvlReached = lvlCompleted;
	}

	public List<AccomplishmentSO> GetAccomplishmentsList() => accomplishmentsSO;
}