using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

	[SerializeField] private GameObject endPowerUpParticlesPrefab;

	SpriteRenderer sprite;
	TrailRenderer trail;

	private bool inmortal = false;
	private void OnEnable()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
		trail = GetComponentInChildren<TrailRenderer>();
		Actions.onCleanLvl += ResetPlayer;
		Actions.onLvlStart += ResetPlayerStart;
	}

	private void OnDisable()
	{
		Actions.onCleanLvl -= ResetPlayer;
		Actions.onLvlStart -= ResetPlayerStart;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Trigger(other);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		Trigger(other);
	}

	private void Trigger(Collider2D other)
	{
		if (other.gameObject.tag.Contains(Tag.Enemy.ToString()) && !inmortal)
		{
			Debug.LogWarning("FIN DEL JUEGO");
			Actions.onLvlEnd?.Invoke(false);
		}
		else if (other.gameObject.tag.Contains(Tag.PowerUp.ToString()))
		{
			other.GetComponent<PowerUp>().PlayEffect();
		}
	}

	private void ResetPlayerStart(LevelSO obj)
	{
		ResetPlayer();
	}
	private void ResetPlayer()
	{
		StopAllCoroutines();
		transform.position = Vector3.zero;
		inmortal = false;
		ResetVisuals();
	}

	private void ResetVisuals()
	{
		sprite.enabled = true;
		trail.startColor = Color.white;
		trail.endColor = Color.white;
		sprite.color = Color.white;
	}

	// El powerUp Inmortal llama a este metodo
	public void Inmortal(float seconds, Color color)
	{
		StopAllCoroutines();
		inmortal = true;
		sprite.color = color;
		trail.startColor = color;
		trail.endColor = color;

		StartCoroutine(InmortalCoroutine(seconds));
	}

	private IEnumerator InmortalCoroutine(float seconds)
	{
		sprite.enabled = false;

		yield return new WaitForSecondsRealtime(seconds - 2);

		sprite.enabled = true;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.enabled = false;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.enabled = true;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.enabled = false;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.enabled = true;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.enabled = false;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.enabled = true;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.enabled = false;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.enabled = true;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.enabled = false;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.enabled = true;
		yield return new WaitForSecondsRealtime(0.3f);

		sprite.enabled = false;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.enabled = true;
		sprite.color = Color.white;
		Instantiate(endPowerUpParticlesPrefab, this.transform);
		yield return new WaitForSecondsRealtime(0.2f);
		inmortal = false;

		ResetVisuals();
	}
}
