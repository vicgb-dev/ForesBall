using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
	[SerializeField] private float initialForce = 0.1f;
	[SerializeField] private float acceleration = 0.1f;
	[SerializeField] private float speedIncremental = 0.01f;
	[SerializeField] private float secsToActivateCollider = 1;

	private Vector2 direction;
	private Vector2 lastVelocity;
	private float newAngle;
	private Rigidbody2D rb;

	public GameObject target; //the enemy's target
	public float rotationSpeed = 5; //speed of turning

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		newAngle = Random.Range(0, 360);
		transform.Rotate(0, 0, newAngle);

		target = GameObject.FindGameObjectWithTag("Player");
	}

	private void Start()
	{
		rb.AddForce(transform.up * initialForce);
		StartCoroutine(ActivateEnemyTag());
		StartCoroutine(MoreSpeed());
	}

	private void OnEnable() => Actions.onCleanLvl += DestroyThis;

	private void OnDisable() => Actions.onCleanLvl -= DestroyThis;

	private void DestroyThis() => Destroy(gameObject);

	private void Update()
	{
		//rotate to look at the player
		transform.up = target.transform.position - transform.position;
	}

	private void FixedUpdate()
	{
		rb.AddForce(transform.up * acceleration);
	}

	private IEnumerator ActivateEnemyTag()
	{
		yield return new WaitForSecondsRealtime(secsToActivateCollider);
		gameObject.tag = Tag.EnemyStraight.ToString();
	}

	private IEnumerator MoreSpeed()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(1);
			acceleration += speedIncremental;
		}
	}
}