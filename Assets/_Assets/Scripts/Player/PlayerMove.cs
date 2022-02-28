using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	private float leftLimit;
	private float upLimit;
	private float rightLimit;
	private float bottomLimit;

	private bool init = false;
	private SpriteRenderer sRenderer;
	private GameObject playerVisuals;

	private void Start()
	{
		sRenderer = GetComponentInChildren<SpriteRenderer>();
		playerVisuals = transform.GetChild(0).gameObject;
	}

	public void Init(List<float> limits)
	{
		leftLimit = limits[0];
		upLimit = limits[1];
		rightLimit = limits[2];
		bottomLimit = limits[3];

		transform.position = new Vector3((rightLimit - leftLimit) / 2 + leftLimit, (upLimit - bottomLimit) / 2 + bottomLimit, 0);

		init = true;
	}

	public void NewPosition(float horizontal, float vertical)
	{
		Vector2 newPosition;
		// newPosition.x = ((rightLimit - leftLimit) * horizontal) + leftLimit;
		newPosition.x = Mathf.Lerp(leftLimit + (sRenderer.size.x * playerVisuals.transform.localScale.x) / 2, rightLimit - (sRenderer.size.x * playerVisuals.transform.localScale.x) / 2, horizontal);
		newPosition.y = Mathf.Lerp(bottomLimit + (sRenderer.size.y * playerVisuals.transform.localScale.y) / 2, upLimit - (sRenderer.size.y * playerVisuals.transform.localScale.y) / 2, vertical);
		transform.position = newPosition;
	}
}
