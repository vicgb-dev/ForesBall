using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChallengesManager", menuName = "Challenges/ChallengesManager")]
public class ChallengesManagerSO : ScriptableObject
{
	public List<ChallengeSO> challenges;
}
