using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps", menuName = "PowerUps/New powerUp")]
public class PowerUpSO : ScriptableObject
{
	public GameObject powerUpPrefab;
	public SpriteRendererSO spriteRendererSO;
	public DeathParticlesSO destroyedParticlesSO;
	[Range(3f, 20f)]
	public float secsPowerUpEffect = 8f;

	public void SetUpPowerUp(GameObject powerUp)
	{
		Color color = ColorsManager.Instance.GetPowerUpColor();
		SpriteRenderer spriteGO = powerUp.GetComponentInChildren<SpriteRenderer>();
		if (spriteGO != null && spriteRendererSO.sprite != null) spriteGO.sprite = spriteRendererSO.sprite;
		if (spriteGO != null) spriteGO.color = color;
		powerUp.GetComponent<PowerUp>().secsPowerUpEffect = secsPowerUpEffect;

		if (destroyedParticlesSO != null)
		{
			powerUp.GetComponent<PowerUp>().deathParticlesPrefab = destroyedParticlesSO.deathParticlesPrefab;
			ParticleSystem ps = destroyedParticlesSO.deathParticlesPrefab.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule psmain = ps.main;
			psmain.startColor = color;
		}
	}
}