using System.Collections;
using UnityEngine;

public class EnemyRay : Enemy
{
	private Rigidbody2D rb;
	[SerializeField] private float speed;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		StartCoroutine(ActivateEnemyTag(Tag.EnemyRay, secsToActivateCollider));
		StartCoroutine(Move());
	}

	public override void StopMoving()
	{
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		stopped = true;
		StopAllCoroutines();
	}

	private IEnumerator Move()
	{
		float time = 0;
		Vector3 initialScale = transform.localScale;

		while (time <= 5)
		{
			time += Time.deltaTime;
			transform.position += transform.up * speed * Time.deltaTime;
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
