using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps", menuName = "PowerUps/New powerUp")]
public class PowerUpSO : ScriptableObject
{
	public GameObject powerUpPrefab;
	public SpriteRendererSO spriteRendererSO;
	public DeathParticlesSO destroyedParticlesSO;

	public void SetUpPowerUp(GameObject powerUp)
	{
		SpriteRenderer spriteGO = powerUp.GetComponentInChildren<SpriteRenderer>();
		if (spriteGO != null && spriteRendererSO.sprite != null) spriteGO.sprite = spriteRendererSO.sprite;
		if (spriteGO != null && spriteRendererSO.color != null) spriteGO.color = spriteRendererSO.color;

		if (destroyedParticlesSO != null) powerUp.GetComponent<PowerUp>().deathParticlesPrefab = destroyedParticlesSO.deathParticlesPrefab;
	}
}