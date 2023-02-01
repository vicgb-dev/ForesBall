using UnityEngine;

public class PowerUpShrink : PowerUp
{
	private void Start()
	{
		StartCoroutine(ActivatePowerUpTag(Tag.PowerUpShrink, secsToActivateCollider));
	}

	private void OnEnable()
	{
		Actions.onLvlFinished += Disable;
	}

	private void Disable()
	{
		StopAllCoroutines();
		GetComponentInChildren<SpriteRenderer>().color = Color.gray;
		GetComponent<Collider2D>().enabled = false;
	}

	public override void PlayEffect()
	{
		GetComponent<Collider2D>().enabled = false;
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		Instantiate(deathParticlesPrefab, this.gameObject.transform);
		SoundManager.Instance.PlaySinglePop();
		Vibration.Vibrate(30);
		AccomplishmentsSystem.Instance.AddTimesShrink();

		Actions.powerUpShrink(deathParticlesPrefab, secsPowerUpEffect);
	}
}
