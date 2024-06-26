using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	SpriteRenderer sprite;
	Material material;
	TrailRenderer trail;

	Color inmortalColor;
	Color inmortalColorFaded;

	private bool inmortal = false;
	private void OnEnable()
	{
		material = GetComponentInChildren<SpriteRenderer>().sharedMaterial;
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

		Dictionary<Limits, float> limits = GameManager.Instance.limits;
		Vector3 center = new Vector3(limits[Limits.right] + limits[Limits.left], limits[Limits.up] - (limits[Limits.up] - limits[Limits.bottom]) / 2, 0);
		transform.position = center;
		inmortal = false;
		ResetVisuals();
	}

	private void ResetVisuals()
	{
		// sprite.enabled = true;
		Color playerColor = ColorsManager.Instance.GetColorsSO().playerColor;
		trail.startColor = playerColor;
		trail.endColor = playerColor;
		sprite.color = playerColor;
		material.SetColor("_Color", playerColor * 3);
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
		material.SetColor("_Color", color * 3);

		StartCoroutine(InmortalCoroutine(seconds, deathParticlesPrefab));
	}

	private IEnumerator InmortalCoroutine(float seconds, GameObject deathParticlesPrefab)
	{
		sprite.color = inmortalColorFaded;

		yield return new WaitForSeconds(seconds - 2);

		sprite.color = inmortalColor;
		yield return new WaitForSeconds(0.1f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSeconds(0.1f);


		sprite.color = inmortalColor;
		yield return new WaitForSeconds(0.1f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSeconds(0.1f);


		sprite.color = inmortalColor;
		yield return new WaitForSeconds(0.1f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSeconds(0.2f);


		sprite.color = inmortalColor;
		yield return new WaitForSeconds(0.2f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSeconds(0.2f);


		sprite.color = inmortalColor;
		yield return new WaitForSeconds(0.2f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSeconds(0.2f);


		sprite.color = inmortalColor;
		yield return new WaitForSeconds(0.3f);

		sprite.color = inmortalColorFaded;
		yield return new WaitForSeconds(0.3f);


		sprite.color = inmortalColor;
		Instantiate(deathParticlesPrefab, this.transform);
		yield return new WaitForSeconds(0.3f);
		sprite.color = Color.white;
		inmortal = false;

		ResetVisuals();
	}
}
