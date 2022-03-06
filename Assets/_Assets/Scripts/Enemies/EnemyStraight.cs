using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraight : MonoBehaviour
{
	[SerializeField] private float speed = 100;
	[SerializeField] private float secsToActivateCollider = 1;

	private Vector2 direction;
	private Vector2 lastVelocity;
	private float newAngle;
	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		newAngle = Random.Range(0, 360);
		transform.Rotate(0, 0, newAngle);
	}

	private void Start()
	{
		rb.AddForce(transform.up * speed);
		StartCoroutine(ChangeTag());
	}

	private void OnEnable()
	{
		Actions.onCleanLvl += DestroyThis;
	}

	private void OnDisable()
	{
		Actions.onCleanLvl -= DestroyThis;
	}

	private void DestroyThis()
	{
		Destroy(gameObject);
	}

	private void Update()
	{
		lastVelocity = rb.velocity;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		var speed = lastVelocity.magnitude;
		direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
		rb.velocity = direction * Mathf.Max(speed, 0);
	}

	private IEnumerator ChangeTag()
	{
		yield return new WaitForSeconds(secsToActivateCollider);
		gameObject.tag = "Enemy";
	}
}