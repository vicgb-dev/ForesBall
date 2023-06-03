using System.Collections;
using UnityEngine;

public class EnemyRay : Enemy
{
	public float speed = 2;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(ActivateEnemyTag(Tag.EnemyRay, secsToActivateCollider));
		StartCoroutine(Move());
	}

	private IEnumerator Move()
	{
		float time = 0;
		Vector3 initialScale = transform.localScale;

		while (time <= 5 && !stopped)
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
