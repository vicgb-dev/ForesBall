using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenuManager : Menu
{
	[SerializeField] protected GameObject button1;

	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Customize;
		panelDirection = Direction.Right;
	}

	private void Start()
	{
		button1.GetComponent<Button>().onClick.AddListener(() => ColorsManager.Instance.UpdateGlobalColors());
		button1.GetComponentInChildren<TextMeshProUGUI>().text = "Actualizar colores";
	}
}