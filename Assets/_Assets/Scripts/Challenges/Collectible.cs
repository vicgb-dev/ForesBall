using System;
using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	[SerializeField] private AnimationCurve curve;
	[SerializeField] private float secondsToBounce = 1;
	[SerializeField] private float amplitude = 0.1f;
	[SerializeField] private GameObject prefabDestroyParticles;
	private SpriteRenderer sprite;

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

	private void Disable()
	{
		StopAllCoroutines();
		sprite.color = Color.gray;
		GetComponent<Collider2D>().enabled = false;
	}

	private void DestroyThis()
	{
		Destroy(this.gameObject);
	}

	private void Start()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
		StartCoroutine(FlipSprite());
		StartCoroutine(BounceCollectible());
	}

	private IEnumerator BounceCollectible()
	{
		float time = 0;
		bool to1 = true;
		Vector3 initialPosition = transform.position;
		Vector3 finalPosition = transform.position + new Vector3(0, amplitude, 0);
		while (true)
		{
			if (secondsToBounce != 0)
			{
				if (to1)
				{
					time += Time.unscaledDeltaTime / secondsToBounce;
					if (time > 1) to1 = false;
				}
				else
				{
					time -= Time.unscaledDeltaTime / secondsToBounce;
					if (time < 0) to1 = true;
				}
				transform.position = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time));
			}
			yield return null;
		}
	}

	private IEnumerator FlipSprite()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);
			sprite.flipX = true;
			yield return new WaitForSeconds(0.1f);
			sprite.flipX = false;
		}
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			LvlBuilder.Instance.CollectiblePicked();
			Instantiate(prefabDestroyParticles, this.transform.position, this.transform.rotation);
			Debug.LogWarning("Desafio completado");
			Destroy(this.gameObject);
		}
	}
}
