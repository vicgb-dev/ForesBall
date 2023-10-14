using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoffeDialogManager : MonoBehaviour
{
	[SerializeField] private GameObject coffeDialogPanel;
	[SerializeField] private Image leftArrow;
	[SerializeField] private Image rightArrow;
	[SerializeField] private AnimationCurve arrowsCurveIn;
	[SerializeField] private AnimationCurve arrowsCurveOut;
	[SerializeField] private float secondsToMoveArrows = 0.5f;

	private Vector3 panelLeftPosition;
	private Vector3 panelRightPosition;
	private Vector3 panelCenterPosition;
	private Image coffePanelImage;
	private Color initialColor;
	private Color finalColor;
	private Coroutine moveRightArrowsCoroutine;
	private Coroutine moveLeftArrowsCoroutine;
	private Vector3 rightArrowInitPosition;
	private RectTransform rightArrowRectTransform;
	private Vector3 leftArrowInitPosition;
	private RectTransform leftArrowRectTransform;
	private bool isMovingArrows = false;


	private void Awake()
	{
		RectTransform rT = coffeDialogPanel.GetComponent<RectTransform>();
		panelLeftPosition = new Vector3(rT.localPosition.x - rT.rect.width - 10, rT.localPosition.y, rT.localPosition.z);
		panelRightPosition = new Vector3(rT.localPosition.x + rT.rect.width + 10, rT.localPosition.y, rT.localPosition.z);
		panelCenterPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);

		coffePanelImage = coffeDialogPanel.GetComponent<Image>();
		initialColor = new Color(coffePanelImage.color.r, coffePanelImage.color.g, coffePanelImage.color.b, 0);
		finalColor = new Color(coffePanelImage.color.r, coffePanelImage.color.g, coffePanelImage.color.b, 1f);

		rightArrowRectTransform = rightArrow.GetComponent<RectTransform>();
		rightArrowInitPosition = rightArrowRectTransform.anchoredPosition;
		leftArrowRectTransform = leftArrow.GetComponent<RectTransform>();
		leftArrowInitPosition = leftArrowRectTransform.anchoredPosition;

		HideDialog();
	}

	private void HideDialog()
	{
		coffePanelImage.color = initialColor;
		coffeDialogPanel.transform.GetChild(0).localPosition = panelLeftPosition;
		coffeDialogPanel.SetActive(false);
		isMovingArrows = false;
	}

	public void OpenCoffeDialog()
	{
		// Activar paneles de bloqueo de toques
		// Mover panel de dialogo a la posicion central
		coffeDialogPanel.SetActive(true);
		isMovingArrows = true;

		StartCoroutine(UIHelpers.Instance.ColorChange(
			coffePanelImage,
			initialColor,
			finalColor,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

		StartCoroutine(UIHelpers.Instance.MovePanel(
			coffeDialogPanel.transform.GetChild(0).gameObject,
			panelLeftPosition,
			panelCenterPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

		Vector3 finalPosRight = new Vector3(rightArrowInitPosition.x - 50, rightArrowInitPosition.y, rightArrowInitPosition.z);
		moveRightArrowsCoroutine = StartCoroutine(MoveArrow(
			rightArrowRectTransform,
			rightArrowInitPosition,
			finalPosRight
		));
		Vector3 finalPosLeft = new Vector3(leftArrowInitPosition.x + 50, leftArrowInitPosition.y, leftArrowInitPosition.z);
		moveLeftArrowsCoroutine = StartCoroutine(MoveArrow(
			leftArrowRectTransform,
			leftArrowInitPosition,
			finalPosLeft
		));
	}

	private IEnumerator MoveArrow(RectTransform rT, Vector3 initPosition, Vector3 finalPosition)
	{
		while (true)
		{
			float time = 0;
			float elapsedTime = 0;
			while (elapsedTime < secondsToMoveArrows)
			{
				elapsedTime += Time.unscaledDeltaTime;
				time += Time.unscaledDeltaTime / secondsToMoveArrows;
				rT.anchoredPosition = Vector3.LerpUnclamped(initPosition, finalPosition, arrowsCurveIn.Evaluate(time));
				yield return null;
			}
			rT.anchoredPosition = finalPosition;


			time = 0;
			elapsedTime = 0;
			while (elapsedTime < secondsToMoveArrows)
			{
				elapsedTime += Time.unscaledDeltaTime;
				time += Time.unscaledDeltaTime / secondsToMoveArrows;
				rT.anchoredPosition = Vector3.LerpUnclamped(finalPosition, initPosition, arrowsCurveOut.Evaluate(time));
				yield return null;
			}
			rT.anchoredPosition = initPosition;
		}
	}

	public void CloseCoffeDialog()
	{
		// Mover panel de dialogo a la izquierda
		StartCoroutine(UIHelpers.Instance.ColorChange(
			coffePanelImage,
			finalColor,
			initialColor,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove,
			() =>
			{
				if (moveRightArrowsCoroutine != null) StopCoroutine(moveRightArrowsCoroutine);
				rightArrow.transform.localPosition = rightArrowInitPosition;
				if (moveLeftArrowsCoroutine != null) StopCoroutine(moveLeftArrowsCoroutine);
			}
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
		isMovingArrows = false;
	}

	public void AcceptCoffeDialog()
	{
		Application.OpenURL("https://www.buymeacoffee.com/VdeVic");
	}
}
