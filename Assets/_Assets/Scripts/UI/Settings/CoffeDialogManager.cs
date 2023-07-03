using UnityEngine;
using UnityEngine.UI;

public class CoffeDialogManager : MonoBehaviour
{
	[SerializeField] private GameObject coffeDialogPanel;
	[SerializeField] private GameObject blockSettingsPanel;
	[SerializeField] private GameObject blockBackButtonPanel;

	private Vector3 panelLeftPosition;
	private Vector3 panelRightPosition;
	private Vector3 panelCenterPosition;
	private Image coffePanelImage;
	public Color initialColor;
	public Color finalColor;

	private void Awake()
	{
		RectTransform rT = coffeDialogPanel.GetComponent<RectTransform>();
		panelLeftPosition = new Vector3(rT.localPosition.x - rT.rect.width - 10, rT.localPosition.y, rT.localPosition.z);
		panelRightPosition = new Vector3(rT.localPosition.x + rT.rect.width + 10, rT.localPosition.y, rT.localPosition.z);
		panelCenterPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);

		coffePanelImage = coffeDialogPanel.GetComponent<Image>();
		initialColor = new Color(coffePanelImage.color.r, coffePanelImage.color.g, coffePanelImage.color.b, 0);
		finalColor = new Color(coffePanelImage.color.r, coffePanelImage.color.g, coffePanelImage.color.b, 0.9f);

		HideDialog();
	}

	private void HideDialog()
	{

		coffePanelImage.color = initialColor;
		coffeDialogPanel.transform.GetChild(0).localPosition = panelLeftPosition;
		coffeDialogPanel.SetActive(false);

	}

	public void OpenCoffeDialog()
	{
		// Activar paneles de bloqueo de toques
		// Mover panel de dialogo a la posicion central
		//blockSettingsPanel.SetActive(true);
		//blockBackButtonPanel.SetActive(true);


		coffeDialogPanel.SetActive(true);

		StartCoroutine(UIHelpers.Instance.ColorChange(
			coffePanelImage,
			initialColor,
			finalColor,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

		Debug.Log("moviendo el panel a la derecha");

		StartCoroutine(UIHelpers.Instance.MovePanel(
			coffeDialogPanel.transform.GetChild(0).gameObject,
			panelLeftPosition,
			panelCenterPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

	}

	public void CloseCoffeDialog()
	{
		// Mover panel de dialogo a la izquierda
		//blockBackButtonPanel.SetActive(false);
		//blockSettingsPanel.SetActive(false);

		StartCoroutine(UIHelpers.Instance.ColorChange(
			coffePanelImage,
			finalColor,
			initialColor,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

		Debug.Log("moviendo el panel a la derecha");
		StartCoroutine(UIHelpers.Instance.MovePanel(
			coffeDialogPanel.transform.GetChild(0).gameObject,
			coffeDialogPanel.transform.localPosition,
			panelRightPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove,
			() => coffeDialogPanel.SetActive(false)
		));
	}

	public void AcceptCoffeDialog()
	{
		Application.OpenURL("https://www.buymeacoffee.com/VdeVic");
	}
}
