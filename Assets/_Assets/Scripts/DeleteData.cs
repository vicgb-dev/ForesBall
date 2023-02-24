using UnityEngine;

public class DeleteData : MonoBehaviour
{
	public void DeleteAllData()
	{
		LoadSaveManager.Instance.Delete();
		LvlBuilder.Instance.ResetLvls();
		PlayerPrefs.DeleteAll();
	}
}
