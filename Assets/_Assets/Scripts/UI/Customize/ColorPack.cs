using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPack : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI colorName;
	[SerializeField] Image player;
	[SerializeField] Image straightEnemy;
	[SerializeField] Image followEnemy;
	[SerializeField] Image bigEnemy;
	[SerializeField] Image rayEnemy;
	[SerializeField] Image challenge;
	[SerializeField] Image powerUp;

	public void SetUp(ColorsSO colors)
	{
		colorName.text = colors.colorName;
		player.color = colors.playerColor;
		straightEnemy.color = colors.straightEnemyColor;
		followEnemy.color = colors.followEnemyColor;
		bigEnemy.color = colors.bigEnemyColor;
		rayEnemy.color = colors.rayEnemyColor;
		challenge.color = colors.challengesColor;
		powerUp.color = colors.powerUpColor;
	}
}
