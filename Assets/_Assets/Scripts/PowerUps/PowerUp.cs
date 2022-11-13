using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
	protected float secsToActivateCollider = 0.3f;
	[Min(3f)]
	protected float secsPowerUpEffect = 8f;

	public GameObject deathParticlesPrefab;

	public abstract void PlayEffect();

	private void OnEnable() => Actions.onCleanLvl += DestroyThis;

	private void OnDisable() => Actions.onCleanLvl -= DestroyThis;

	private void DestroyThis()
	{
		deathParticlesPrefab = null;
		Destroy(gameObject);
	}

	protected IEnumerator ActivatePowerUpTag(Tag tag, float seconds)
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