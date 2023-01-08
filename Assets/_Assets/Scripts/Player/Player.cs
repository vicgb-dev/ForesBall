using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	SpriteRenderer sprite;
	TrailRenderer trail;

	Color inmortalColor;
	Color inmortalColorFaded;

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
			Vibration.Vibrate(300);
			Debug.LogWarning("FIN DEL JUEGO");
			Actions.onLvlEnd?.Invoke(false);
		}
		else if (other.gameObject.tag.Contains(Tag.PowerUp.ToString()))
		{
			Vibration.Vibrate(30);
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
		// sprite.enabled = true;
		trail.startColor = ColorsManager.Instance.GetColorsSO().playerColor;
		trail.endColor = ColorsManager.Instance.GetColorsSO().playerColor;
		sprite.color = ColorsManager.Instance.GetColorsSO().playerColor;
	}

	// El powerUp Inmortal llama a este metodo
	public void Inmortal(float seconds, Color color, GameObject deathParticlesPrefab)
	{
		StopAllCoroutines();
		inmortal = true;

		inmortalColor = color;
		inmortalColorFaded = new Color(inmortalColor.r, inmortalColor.g, inmortalColor.b, 0.2f);

		trail.startColor = color;
		trail.endColor = color;

		StartCoroutine(InmortalCoroutine(seconds, deathParticlesPrefab));
	}

	private IEnumerator InmortalCoroutine(float seconds, GameObject deathParticlesPrefab)
	{
		sprite.color = inmortalColorFaded;

		yield return new WaitForSecondsRealtime(seconds - 2);

		sprite.color = inmortalColor;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSecondsRealtime(0.1f);


		sprite.color = inmortalColor;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSecondsRealtime(0.1f);


		sprite.color = inmortalColor;
		yield return new WaitForSecondsRealtime(0.1f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSecondsRealtime(0.2f);


		sprite.color = inmortalColor;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSecondsRealtime(0.2f);


		sprite.color = inmortalColor;
		yield return new WaitForSecondsRealtime(0.2f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSecondsRealtime(0.2f);


		sprite.color = inmortalColor;
		yield return new WaitForSecondsRealtime(0.3f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSecondsRealtime(0.3f);


		sprite.color = inmortalColor;
		Instantiate(deathParticlesPrefab, this.transform);
		yield return new WaitForSecondsRealtime(0.3f);
		sprite.color = Color.white;
		inmortal = false;

		ResetVisuals();
	}
}
