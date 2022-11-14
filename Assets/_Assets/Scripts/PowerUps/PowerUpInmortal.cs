using UnityEngine;

public class PowerUpInmortal : PowerUp
{
	private void Start()
	{
		StartCoroutine(ActivatePowerUpTag(Tag.PowerUpInmortal, secsToActivateCollider));
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

		GameObject.FindGameObjectWithTag(Tag.Player.ToString()).GetComponent<Player>().Inmortal(secsPowerUpEffect, GetComponentInChildren<SpriteRenderer>().color);
	}
}
