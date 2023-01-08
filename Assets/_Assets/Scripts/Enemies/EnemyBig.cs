using System.Collections;
using UnityEngine;

public class EnemyBig : Enemy
{
	private Rigidbody2D rb;
	[SerializeField] private float frecuency = 1;
	[SerializeField] private float amplitude = 1;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		isAffectedByPowerUpShrink = false;
	}

	private void Start()
	{
		StartCoroutine(ActivateEnemyTag(Tag.EnemyBig, secsToActivateCollider));
		StartCoroutine(ChangeSize());
	}

	public override void StopMoving()
	{
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		stopped = true;
		StopAllCoroutines();
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