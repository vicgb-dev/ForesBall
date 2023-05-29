using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public GameObject deathParticlesPrefab;

	public Rigidbody2D rb;
	public Collider2D coll;

	protected float secsToActivateCollider = 1;
	protected bool isEnemyBig = false;
	protected bool stopped = false;

	protected Coroutine coShrinkEffect;

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collider2D>();
	}

	private void OnEnable()
	{
		Actions.onLvlEnd += StopMoving;
		Actions.onCleanLvl += DestroyThis;
		Actions.powerUpShrink += Shrink;
	}

	private void OnDisable()
	{
		Actions.onLvlEnd -= StopMoving;
		Actions.onCleanLvl -= DestroyThis;
		Actions.powerUpShrink -= Shrink;
	}

	public virtual void StopMoving(bool win = true)
	{
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		coll.enabled = false;
		stopped = true;
		Debug.Log($"$rb.isKinematic = {rb.isKinematic} | rb.velocity = {rb.velocity} | coll.enabled = {coll.enabled} | stopped = {stopped}");

		StopAllCoroutines();
	}

	private void Shrink(GameObject effectParticles, float secsPowerUpEffect)
	{
		if (isEnemyBig) return;
		if (coShrinkEffect != null) StopCoroutine(coShrinkEffect);

		Instantiate(effectParticles, this.transform);
		transform.localScale *= 0.5f;

		coShrinkEffect = StartCoroutine(BackToNormalSize(secsPowerUpEffect));
	}

	private IEnumerator BackToNormalSize(float seconds)
	{
		yield return new WaitForSeconds(seconds - 2);

		float time = 0.5f;
		while (transform.localScale.x <= 1)
		{
			time += Time.deltaTime;
			transform.localScale = new Vector3(time, time, time);
			yield return null;
		}
	}

	public IEnumerator DestroyInSeconds(float seconds)
	{
		if (seconds > 0)
		{
			yield return new WaitForSeconds(seconds);
			Actions.enemyDestroyed?.Invoke(this.gameObject);
			Destroy(gameObject);
		}
	}

	private void DestroyThis()
	{
		deathParticlesPrefab = null;
		Destroy(gameObject);
	}

	protected IEnumerator ActivateEnemyTag(Tag tag, float seconds)
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