using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsManager", menuName = "Levels/Levels Manager")]
public class LevelsManagerSO : ScriptableObject
{
	public List<LevelSO> levels;
}
