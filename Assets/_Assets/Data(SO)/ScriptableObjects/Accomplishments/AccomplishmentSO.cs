using UnityEngine;

[CreateAssetMenu(fileName = "AccomplishmentSO", menuName = "Accomplishments")]
public class AccomplishmentSO : ScriptableObject
{
	public AccompProperty property;
	public float greaterThan;
	public string accomplishmentTitle;
	public string accomplishmentDescription;
	public int idColorUnlock;
}