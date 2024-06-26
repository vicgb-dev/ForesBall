using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "UI/Colors")]
public class ColorsSO : ScriptableObject
{
	public int idColor;
	public string colorName;
	[Header("Player")]
	public Color playerColor;
	[Header("Menus")]
	public Color mainMenuColor;
	public Color customizeMenuColor;
	public Color settingsMenuColor;
	public Color levelsMenuColor;

	[Header("Interface")]
	public Color challengesColor;
	public Color powerUpColor;

	[Header("Enemies")]
	public Color straightEnemyColor;
	public Color followEnemyColor;
	public Color bigEnemyColor;
	public Color rayEnemyColor;
}
