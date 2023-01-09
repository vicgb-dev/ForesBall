using UnityEngine;

public class Hotspot : Challenge
{
	public float timeToCompleteHotspot;
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

	private void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	private void Start()
	{
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
				AccomplishmentsSystem.Instance.AddHotspot();
				Vibration.Vibrate(30);
				Actions.updateChallenge?.Invoke(Actions.ChallengeType.hotspot, 1);
				SoundManager.Instance.PlaySinglePop();
				Instantiate(prefabDestroyParticles, this.transform.position, this.transform.rotation);
				Destroy(this.gameObject);
			}
		}
	}
}
