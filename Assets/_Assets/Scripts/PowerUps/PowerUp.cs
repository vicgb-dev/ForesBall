using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
	protected float secsToActivateCollider = 0.3f;

	public float secsPowerUpEffect = 8f;

	public GameObject deathParticlesPrefab;

	public abstract void PlayEffect();

	private void OnEnable()
	{
		Actions.onCleanLvl += DestroyThis;
		Actions.onLvlFinished += Disable;
	}

	private void OnDisable()
	{
		Actions.onCleanLvl -= DestroyThis;
		Actions.onLvlFinished -= Disable;
	}

	private void DestroyThis()
	{
		deathParticlesPrefab = null;
		Destroy(gameObject);
	}

	private void Disable()
	{
		StopAllCoroutines();
		SpriteRenderer thisRenderer = GetComponentInChildren<SpriteRenderer>();
		if (thisRenderer != null)
			thisRenderer.color = Color.gray;
		GetComponent<Collider2D>().enabled = false;
	}

	protected IEnumerator ActivatePowerUpTag(Tag tag, float seconds)
	{
		yield return new WaitForSeconds(seconds);
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