using UnityEngine;

public class Hotspot : MonoBehaviour
{
	[Range(0.1f, 0.9f)]
	[SerializeField] private float percentOfLvlInHotspot;
	[SerializeField] private GameObject prefabDestroyParticles;
	[SerializeField] private float timeToCompleteHotspot;
	[SerializeField] private float timeInHotspot = 0;

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
		GetComponentInChildren<SpriteRenderer>().color = Color.gray;
		GetComponentInChildren<Collider2D>().enabled = false;
	}

	private void DestroyThis()
	{
		Destroy(this.gameObject);
	}

	private void Start()
	{
		timeToCompleteHotspot = LvlBuilder.Instance.GetMusicLength();
		timeToCompleteHotspot *= percentOfLvlInHotspot;
		transform.localScale = new Vector3(5, 5, 5);
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			timeInHotspot += Time.deltaTime;
			var lerpScale = Mathf.Lerp(5f, 0.5f, timeInHotspot / timeToCompleteHotspot);
			transform.localScale = new Vector3(lerpScale, lerpScale, lerpScale);

			// Set score of challenge
			LvlBuilder.Instance.SetHotSpotAchieve(timeInHotspot / timeToCompleteHotspot);

			if (timeInHotspot >= timeToCompleteHotspot)
			{
				LvlBuilder.Instance.SetHotSpotAchieve(1);
				Instantiate(prefabDestroyParticles, this.transform.position, this.transform.rotation);
				Debug.LogWarning("Desafio completado");
				Destroy(this.gameObject);
			}
		}
	}
}
