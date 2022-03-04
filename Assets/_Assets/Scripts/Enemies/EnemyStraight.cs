using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraight : MonoBehaviour, IEnemy
{
	[SerializeField] private float strength = 1;
	private Vector2 direction;
	private float newAngle;

	private void Start()
	{
		newAngle = Random.Range(0, 360);
		transform.Rotate(0, 0, newAngle);
		//Vector3 v3Force = strength * transform.up;
		//Rigidbody2D rb = GetComponent<Rigidbody2D>();
		//rb.AddForce(v3Force);
	}

	private void Update()
	{
		Move();
	}
	public void Move()
	{
		transform.Translate(0, transform.up.y * Time.deltaTime * strength, 0);
	}
	private void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Colision");
		newAngle = 180 - newAngle;
		transform.Rotate(0, 0, newAngle);
	}
}