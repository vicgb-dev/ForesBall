using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
	GameObject player;

	private void Update()
	{
		if (player == null)
			player = GameObject.Find("Player(Clone)");

		if (player != null)
		{
			transform.position = player.transform.position;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals(Tag.EnemyFollow.ToString()))
		{
			AccomplishmentsSystem.Instance.AddTimeCloseToEnemyFollow(Time.deltaTime);
		}
		if (other.gameObject.tag.Equals(Tag.EnemyRay.ToString()))
		{
			AccomplishmentsSystem.Instance.AddTimeCloseToEnemyRay(Time.deltaTime);
		}
	}
}
