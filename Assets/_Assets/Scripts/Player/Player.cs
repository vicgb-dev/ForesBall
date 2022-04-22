using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Contains("Enemy"))
		{
			Debug.Log("Player touched enemy");
			Debug.LogWarning("FIN DEL JUEGO");
			Actions.onLvlEnd?.Invoke(false);
		}
	}
}
