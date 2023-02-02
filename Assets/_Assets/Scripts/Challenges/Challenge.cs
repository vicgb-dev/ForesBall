using UnityEngine;

public class Challenge : MonoBehaviour
{
	public GameObject prefabDestroyParticles;

	private void OnEnable()
	{
		Actions.onCleanLvl += DestroyThis;
		Actions.onLvlFinished += Disable;
	}

	private void OnDisable()
	{
		Actions.onCleanLvl -= DestroyThis;
		Actions.onLvlFinished -= Disable;
	}

	private void Disable()
	{
		StopAllCoroutines();
		GetComponentInChildren<SpriteRenderer>().color = Color.gray;
		GetComponent<Collider2D>().enabled = false;
	}

	private void DestroyThis()
	{
		Destroy(this.gameObject);
	}
}
