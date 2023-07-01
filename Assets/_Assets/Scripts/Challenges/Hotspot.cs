using UnityEngine;

public class Hotspot : Challenge
{
	public float timeToCompleteHotspot;
	[SerializeField] private float timeInHotspot = 0;
	[SerializeField] private Sprite spriteNootNoot;
	private SpriteRenderer sprite;

	private void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	private void Start()
	{
		Debug.Log("checking if level is lacrimosa");
		if (LvlBuilder.Instance.GetCurrentLevel().musicName.Contains("lacrimosa"))
		{
			Debug.Log("level is lacrimosa");
			// cambiar imagen de sprite por spriteNootNoot
			sprite.sprite = spriteNootNoot;
		}
		transform.localScale = new Vector3(5, 5, 5);
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
		}
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
