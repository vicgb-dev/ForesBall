using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PowerUps", menuName = "PowerUps/New powerUp manager")]
public class PowerUpsManagerSO : ScriptableObject
{
	public List<PowerUpSO> powerUps;
}