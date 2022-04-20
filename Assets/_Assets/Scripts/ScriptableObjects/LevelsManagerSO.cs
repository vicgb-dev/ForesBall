using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsManager", menuName = " Scriptable Objects/LevelsManager")]
public class LevelsManagerSO : ScriptableObject
{
    [Header("Levels")]
	public List<LevelSO> levels;
}
