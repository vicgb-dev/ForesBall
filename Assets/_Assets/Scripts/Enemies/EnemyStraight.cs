using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraight : Enemy, IEnemy
{
	[SerializeField] private float initialForce = 100;
	[SerializeField] private float speed = 2f;
	[SerializeField] private float speedIncremental = 0.05f;
	[SerializeField] private float secsToActivateCollider = 1;

	private Vector2 direction;
	private Vector2 lastVelocity;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		transform.Rotate(0, 0, Random.Range(0, 360));
	}

	private void Start()
	{
		rb.AddForce(transform.up * initialForce);
		StartCoroutine(ActivateEnemyTag(Tag.EnemyStraight, secsToActivateCollider));
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if(stopped) return;
		speed += speedIncremental;
		direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
		rb.velocity = direction * speed;
	}
	
	private void Update() => lastVelocity = rb.velocity;

	public void StopMoving()
	{
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		stopped = true;
	}

}