using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/New enemy")]
public class EnemySO : ScriptableObject
{
	public GameObject enemyPrefab;
	[Header("Overrides")]
	public SpriteRendererSO spriteRendererSO;
	public TrailSO trailSO;
	public DeathParticlesSO deathParticlesSO;

	public void SetUpEnemy(GameObject enemy)
	{
		SpriteRenderer spriteGO = enemy.GetComponentInChildren<SpriteRenderer>();
		if (spriteGO != null && spriteRendererSO.sprite != null) spriteGO.sprite = spriteRendererSO.sprite;
		if (spriteGO != null && spriteRendererSO.color != null) spriteGO.color = spriteRendererSO.color;

		TrailRenderer trailGO = enemy.GetComponentInChildren<TrailRenderer>();
		if (trailGO != null && trailSO != null) trailSO.SetUpTrail(trailGO);

		if (deathParticlesSO != null) enemy.GetComponent<Enemy>().deathParticlesPrefab = deathParticlesSO.deathParticlesPrefab;
	}
}