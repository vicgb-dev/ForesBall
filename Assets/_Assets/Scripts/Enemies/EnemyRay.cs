using System.Collections;
using UnityEngine;

public class EnemyRay : Enemy
{
	[SerializeField] private float speed;

	private void Start()
	{
		StartCoroutine(ActivateEnemyTag(Tag.EnemyRay, secsToActivateCollider));
		StartCoroutine(Move());
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
