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

	private bool control = false;
	private SpriteRenderer sRenderer;
	private GameObject playerVisuals;
	private Dictionary<Limits, float> limits;

	private void Start()
	{
		sRenderer = GetComponentInChildren<SpriteRenderer>();
		playerVisuals = transform.GetChild(0).gameObject;
	}

	private void OnEnable()
	{
		Actions.onLvlStart += EnableControl;
		Actions.onLvlEnd += UnableControl;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= EnableControl;
		Actions.onLvlEnd -= UnableControl;
	}

	public void Init()
	{
		limits = GameManager.Instance.limits;

		transform.position = new Vector3(
			(limits[Limits.right] - limits[Limits.left]) / 2 + limits[Limits.left],
			(limits[Limits.up] - limits[Limits.bottom]) / 2 + limits[Limits.bottom],
			0);
	}

	private void EnableControl(LevelSO lvl)
	{
		control = true;
	}

	private void UnableControl(bool win)
	{
		control = false;
	}

	public void NewPosition(float horizontal, float vertical)
	{
		if (!control) return;

		Vector2 newPosition;
		newPosition.x = Mathf.Lerp(
			limits[Limits.left] + (sRenderer.size.x * playerVisuals.transform.localScale.x) / 2,
			limits[Limits.right] - (sRenderer.size.x * playerVisuals.transform.localScale.x) / 2,
			horizontal);

		newPosition.y = Mathf.Lerp(
			limits[Limits.bottom] + (sRenderer.size.y * playerVisuals.transform.localScale.y) / 2,
			limits[Limits.up] - (sRenderer.size.y * playerVisuals.transform.localScale.y) / 2,
			vertical);

		transform.position = newPosition;
	}
}
