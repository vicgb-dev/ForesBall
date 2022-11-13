using UnityEngine;

public class EnemyStraight : Enemy
{
	[SerializeField] private float initialForce = 100;
	[SerializeField] private float speed = 2f;
	[SerializeField] private float speedIncremental = 0.05f;

	private Rigidbody2D rb;
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

	private void Update() => lastVelocity = rb.velocity;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (stopped) return;
		speed += speedIncremental;
		direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
		rb.velocity = direction * speed;
	}

	public override void StopMoving()
	{
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		stopped = true;
	}
}