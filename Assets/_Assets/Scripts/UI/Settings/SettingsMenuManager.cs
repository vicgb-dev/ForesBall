using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManager : Menu
{
	[SerializeField] protected GameObject button1;
	[SerializeField] protected GameObject button2;
	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Settings;
		panelDirection = Direction.Right;
	}

	private void Start()
	{
		button1.GetComponent<Button>().onClick.AddListener(() => LvlBuilder.Instance.ResetLvls());
		button1.GetComponentInChildren<TextMeshProUGUI>().text = "Resetear puntuacion de niveles";

		button2.GetComponent<Button>().onClick.AddListener(() => LvlBuilder.Instance.UnlockAllLvls());
		button2.GetComponentInChildren<TextMeshProUGUI>().text = "Desbloquear todos los niveles";
	}
}