using System.Collections;
using System.Collections.Generic;
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
}
