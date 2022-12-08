using UnityEngine;

[CreateAssetMenu(fileName = "NewChallenge", menuName = "Challenges/Challenge")]
public class ChallengeSO : ScriptableObject
{
	public GameObject challengePrefab;
	[Header("Overrides")]
	public SpriteRendererSO spriteRendererSO;
	public DeathParticlesSO destroyParticlesSO;
	public ChallengeType challengeType;
	public enum ChallengeType
	{
		hotspot,
		collectible
	}

	public void SetUpChallenge(GameObject challenge, float timeToComplete = 0)
	{
		Color color = ColorsManager.Instance.GetChallengesColor();

		SpriteRenderer spriteGO = challenge.GetComponentInChildren<SpriteRenderer>();
		if (spriteGO != null && spriteRendererSO.sprite != null) spriteGO.sprite = spriteRendererSO.sprite;
		if (spriteGO != null) spriteGO.color = color;

		if (destroyParticlesSO != null)
		{
			challenge.GetComponent<Challenge>().prefabDestroyParticles = destroyParticlesSO.deathParticlesPrefab;
			ParticleSystem ps = destroyParticlesSO.deathParticlesPrefab.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule psmain = ps.main;
			psmain.startColor = color;
		}

		if (challengeType == ChallengeType.hotspot)
		{
			challenge.GetComponent<Hotspot>().timeToCompleteHotspot = timeToComplete;
		}
	}
}
