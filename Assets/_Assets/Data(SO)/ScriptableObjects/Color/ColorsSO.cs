using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "UI/Colors")]
public class ColorsSO : ScriptableObject
{
	public int idColor;
	public string colorName;
	[Header("Player")]
	public Color playerColor;
	[Header("Menus")]
	public Color mainMenuColor = Color.white;
	public Color customizeMenuColor = Color.white;
	public Color settingsMenuColor = Color.white;
	public Color levelsMenuColor = Color.white;

	[Header("Interface")]
	public Color challengesColor = Color.white;
	public Color powerUpColor = Color.white;

	[Header("Enemies")]
	public Color straightEnemyColor = Color.white;
	public Color followEnemyColor = Color.white;
	public Color bigEnemyColor = Color.white;
	public Color rayEnemyColor = Color.white;
}
