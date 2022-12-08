using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "UI/Colors")]
public class ColorsSO : ScriptableObject
{
	[Header("Menus")]
	public Color mainMenuColor;
	public Color challengesMenuColor;
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
