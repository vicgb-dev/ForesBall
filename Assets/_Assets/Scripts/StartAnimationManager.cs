using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationManager : MonoBehaviour
{
	[SerializeField] private EnemySO enemyStraightSO;
	[SerializeField] private EnemySO enemyFollowSO;
	[SerializeField] private EnemySO enemyBigSO;
	[SerializeField] private EnemySO enemyRaySO;
	List<GameObject> enemies = new List<GameObject>();
	void Start()
	{
		StartCoroutine(Scheduler());
	}

	private IEnumerator Scheduler()
	{
		WaitForSeconds wait = new WaitForSeconds(0.2f);
		yield return wait;
		InstanceEnemy(enemyStraightSO);
		yield return wait;
		InstanceEnemy(enemyFollowSO);
		yield return wait;
		InstanceEnemy(enemyBigSO);
		yield return wait;
		InstanceEnemy(enemyRaySO);
		yield return wait;
		DestroyEnemy();
		yield return wait;
		DestroyEnemy();
		yield return wait;
		yield return wait;
		yield return wait;
		DestroyEnemy();
		yield return wait;
		DestroyEnemy();
	}

	private void DestroyEnemy()
	{
		if (enemies.Count > 0)
		{
			GameObject enemyGo = enemies[0];
			enemies.RemoveAt(0);
			if (enemyGo.GetComponentInChildren<EnemyBig>() != null)
				enemyGo.GetComponentInChildren<Transform>().localScale = new Vector3(1f, 1f, 1f);

			SoundManager.Instance.PlaySinglePianoTile();
			Destroy(enemyGo);
		}
	}

	private void InstanceEnemy(EnemySO enemySO)
	{
		SoundManager.Instance.PlaySinglePianoTile();
		GameObject startAnimationGo = Instantiate(enemySO.enemyPrefab, transform.position + (new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f))), Quaternion.identity, transform);
		enemySO.SetUpEnemy(startAnimationGo);

		//change tag
		startAnimationGo.tag = "Untagged";

		enemies.Add(startAnimationGo);

		if (startAnimationGo.GetComponentInChildren<EnemyRay>() != null)
			startAnimationGo.GetComponentInChildren<EnemyRay>().speed = 0;
		else
			startAnimationGo.GetComponentInChildren<Enemy>().StopMoving();

		startAnimationGo.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
	}
}
