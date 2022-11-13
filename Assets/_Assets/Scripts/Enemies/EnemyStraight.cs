using UnityEngine;

public class EnemyStraight : Enemy
{
	[SerializeField] private float initialForce = 100;
	[SerializeField] private float speed = 2f;
	[SerializeField] private float speedIncremental = 0.05f;

	private Vector2 direction;
	private Vector2 lastVelocity;

	private void Awake()
	{
		transform.Rotate(0, 0, Random.Range(0, 360));
	}

	private void Start()
	{
		StartCoroutine(ActivateEnemyTag(Tag.EnemyStraight, secsToActivateCollider));
	}

	private void FixedUpdate()
	{
		if (!stopped)
			transform.position += transform.up * speed * Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (stopped) return;
		speed += speedIncremental;
		transform.up = Vector3.Reflect(transform.up, other.contacts[0].normal);
	}

	public override void StopMoving()
	{
		speed = 0;
		stopped = true;
	}
}