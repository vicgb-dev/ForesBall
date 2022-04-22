using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
	protected Rigidbody2D rb;
	protected bool stopped = false;

	private void OnEnable() => Actions.onCleanLvl += DestroyThis;

	private void OnDisable() => Actions.onCleanLvl -= DestroyThis;

	private void DestroyThis() => Destroy(gameObject);
	
	protected IEnumerator ActivateEnemyTag(Tag tag, float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds);
		gameObject.tag = tag.ToString();
	}
}