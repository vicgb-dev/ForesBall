using UnityEngine;

public class Player : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Contains(Tag.Enemy.ToString()))
		{
			Debug.LogWarning("FIN DEL JUEGO");
			Actions.onLvlEnd?.Invoke(false);
		}
	}
	private void OnTriggerStay2D(Collider2D other)
	{

		if (other.gameObject.tag.Contains(Tag.Enemy.ToString()))
		{
			Debug.LogWarning("FIN DEL JUEGO");
			Actions.onLvlEnd?.Invoke(false);
		}
	}
}
