using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	protected float secsToActivateCollider = 1;
	protected bool isAffectedByPowerUpShrink = true;

	protected bool stopped = false;

	public GameObject deathParticlesPrefab;

	public abstract void StopMoving();

	private void OnEnable()
	{
		Actions.onCleanLvl += DestroyThis;
		Actions.powerUpShrink += Shrink;
	}

	private void OnDisable()
	{
		Actions.onCleanLvl -= DestroyThis;
		Actions.powerUpShrink -= Shrink;
	}

	private void Shrink(GameObject effectParticles, float secsPowerUpEffect)
	{
		if (!isAffectedByPowerUpShrink) return;
		Instantiate(effectParticles, this.transform);
		transform.localScale *= 0.5f;

		StartCoroutine(BackToNormalSize(secsPowerUpEffect));
	}

	private IEnumerator BackToNormalSize(float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds - 2);

		float time = 0.5f;
		while (transform.localScale.x <= 1)
		{
			time += Time.deltaTime;
			transform.localScale = new Vector3(time, time, time);
			yield return null;
		}
	}

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
		if (deathParticlesPrefab != null)
		{
			GameObject particles = Instantiate(deathParticlesPrefab, this.gameObject.transform);
			particles.transform.SetParent(null);
		}
	}
}