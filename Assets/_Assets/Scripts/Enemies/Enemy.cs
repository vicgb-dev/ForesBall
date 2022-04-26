using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Enemy : MonoBehaviour
{
	protected bool stopped = false;

	public GameObject deathParticlesPrefab;

	public abstract void StopMoving();

	private void OnEnable() => Actions.onCleanLvl += DestroyThis;

	private void OnDisable() => Actions.onCleanLvl -= DestroyThis;

	private void DestroyThis()
	{
		deathParticlesPrefab = null;
		Destroy(gameObject);
	}
	
	protected IEnumerator ActivateEnemyTag(Tag tag, float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds);
		gameObject.tag = tag.ToString();
	}

	private void OnDestroy()
	{
		if(deathParticlesPrefab != null)
		{
			GameObject particles = Instantiate(deathParticlesPrefab, this.gameObject.transform);
			particles.transform.SetParent(null);
		}
	}
}