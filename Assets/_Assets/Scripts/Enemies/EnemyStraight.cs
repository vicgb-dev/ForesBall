using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraight : MonoBehaviour
{
	[SerializeField] private float initialForce = 100;
	[SerializeField] private float speed = 2f;
	[SerializeField] private float speedIncremental = 0.05f;
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
		rb.AddForce(transform.up * initialForce);
		StartCoroutine(ActivateEnemyTag());
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		speed += speedIncremental;
		direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
		rb.velocity = direction * speed;
	}

	private void OnEnable() => Actions.onCleanLvl += DestroyThis;

	private void OnDisable() => Actions.onCleanLvl -= DestroyThis;

	private void DestroyThis() => Destroy(gameObject);

	private void Update() => lastVelocity = rb.velocity;

	private IEnumerator ActivateEnemyTag()
	{
		yield return new WaitForSecondsRealtime(secsToActivateCollider);
		gameObject.tag = Tag.EnemyStraight.ToString();
	}
}