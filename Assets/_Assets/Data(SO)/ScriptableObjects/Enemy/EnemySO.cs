using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/New enemy")]
public class EnemySO : ScriptableObject
{
	public GameObject enemyPrefab;
	[Header("Overrides")]
	public SpriteRendererSO spriteRendererSO;
	public TrailSO trailSO;
	public DeathParticlesSO deathParticlesSO;
	public EnemyType enemyType;
	public enum EnemyType
	{
		straight,
		follow,
		big,
		ray
	}

	public void SetUpEnemy(GameObject enemy)
	{
		Color color = ColorsManager.Instance.GetEnemyColor(enemyType);

		SpriteRenderer spriteGO = enemy.GetComponentInChildren<SpriteRenderer>();
		if (spriteGO != null && spriteRendererSO.sprite != null) spriteGO.sprite = spriteRendererSO.sprite;
		if (spriteGO != null) spriteGO.color = color;

		TrailRenderer trailGO = enemy.GetComponentInChildren<TrailRenderer>();
		if (trailGO != null && trailSO != null) trailSO.SetUpTrail(trailGO, color);

		if (deathParticlesSO != null)
		{
			enemy.GetComponent<Enemy>().deathParticlesPrefab = deathParticlesSO.deathParticlesPrefab;
			ParticleSystem ps = deathParticlesSO.deathParticlesPrefab.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule psmain = ps.main;
			psmain.startColor = color;
		}
	}
}