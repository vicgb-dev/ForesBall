using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : Enemy, IEnemy
{
	[SerializeField] private float initialForce = 0.1f;
	[SerializeField] private float acceleration = 0.1f;
	[SerializeField] private float speedIncremental = 0.01f;
	[SerializeField] private float secsToActivateCollider = 1;

	private Rigidbody2D rb;
	private Vector2 direction;
	private Vector2 lastVelocity;
	private GameObject targetPlayer;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		transform.Rotate(0, 0, Random.Range(0, 360));
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
	}

	private void Start()
	{
		rb.AddForce(transform.up * initialForce);
		StartCoroutine(ActivateEnemyTag(Tag.EnemyFollow, secsToActivateCollider));
		StartCoroutine(MoreSpeed());
	}

	private void Update()
	{
		//rotate to look at the player
		
		if(stopped) return;
		transform.up = targetPlayer.transform.position - transform.position;
	}

	private void FixedUpdate()
	{
		if(stopped) return;
		rb.AddForce(transform.up * acceleration);
	}

	private IEnumerator MoreSpeed()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(1);
			acceleration += speedIncremental;
		}
	}
	
	public override void StopMoving()
	{
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		stopped = true;
	}
}