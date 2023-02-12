using System.Collections;
using UnityEngine;

public class EnemyBig : Enemy
{
	[SerializeField] private float frecuency = 1;
	[SerializeField] private float amplitude = 1;

	protected override void Awake()
	{
		base.Awake();
		isAffectedByPowerUpShrink = false;
	}

	private void Start()
	{
		StartCoroutine(ActivateEnemyTag(Tag.EnemyBig, secsToActivateCollider));
		StartCoroutine(ChangeSize());
	}

	private IEnumerator ChangeSize()
	{
		Vector3 initialScale = transform.localScale;
		float time = 0;
		float scale = 0;
		while (scale >= 0)
		{
			time += Time.deltaTime;
			scale = Mathf.Sin(time * frecuency) * amplitude;
			transform.localScale = new Vector3(scale, scale, scale);
			yield return null;
		}

		Disappear();
	}

	private void Disappear()
	{
		GetComponent<Collider2D>().enabled = false;
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}
}