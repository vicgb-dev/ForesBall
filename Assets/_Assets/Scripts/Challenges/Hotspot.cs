using UnityEngine;

public class Hotspot : MonoBehaviour
{
	[SerializeField] private GameObject prefabDestroyParticles;
	[SerializeField] private float timeToCompleteHotspot;
	[SerializeField] private float timeInHotspot = 0;
	private SpriteRenderer sprite;

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
		sprite.color = Color.gray;
		GetComponentInChildren<Collider2D>().enabled = false;
	}

	private void DestroyThis()
	{
		Destroy(this.gameObject);
	}

	public void SetUp(float timeToComplete)
	{
		// Set color challenge to sprite
		sprite = GetComponentInChildren<SpriteRenderer>();
		sprite.color = ColorsManager.Instance.GetChallengesColor();

		// Set color challenge to prefabDestroyParticles
		ParticleSystem ps = prefabDestroyParticles.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule psmain = ps.main;
		psmain.startColor = ColorsManager.Instance.GetChallengesColor();

		timeToCompleteHotspot = timeToComplete;
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
			Actions.updateChallenge?.Invoke(Actions.ChallengeType.hotspot, timeInHotspot / timeToCompleteHotspot);

			if (timeInHotspot >= timeToCompleteHotspot)
			{
				Actions.updateChallenge?.Invoke(Actions.ChallengeType.hotspot, 1);
				SoundManager.Instance.PlaySinglePop();
				Instantiate(prefabDestroyParticles, this.transform.position, this.transform.rotation);
				Destroy(this.gameObject);
			}
		}
	}
}
