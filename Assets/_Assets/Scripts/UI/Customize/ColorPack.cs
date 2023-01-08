using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPack : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI colorName;
	[SerializeField] private Image backgroundButton;
	[SerializeField] private Image player;
	[SerializeField] private Image straightEnemy;
	[SerializeField] private Image followEnemy;
	[SerializeField] private Image bigEnemy;
	[SerializeField] private Image rayEnemy;
	[SerializeField] private Image challenge;
	[SerializeField] private Image powerUp;
	private int colorId;
	private ButtonFeedback feedback;

	public float moveDistance = 50f; // distance to move the UI image up and down
	public float moveDuration = 1f; // duration of the movement up and down
	public float delay = 1f; // delay between each movement

	public void SetUp(ColorsSO colors, bool selected)
	{
		colorName.text = colors.colorName;
		player.color = colors.playerColor;
		straightEnemy.color = colors.straightEnemyColor;
		followEnemy.color = colors.followEnemyColor;
		bigEnemy.color = colors.bigEnemyColor;
		rayEnemy.color = colors.rayEnemyColor;
		challenge.color = colors.challengesColor;
		powerUp.color = colors.powerUpColor;
		colorId = colors.idColor;

		feedback = this.gameObject.GetComponent<ButtonFeedback>();
		if (feedback != null)
		{
			feedback.pressedColor = new Color(colors.customizeMenuColor.r - 0.2f, colors.customizeMenuColor.g - 0.2f, colors.customizeMenuColor.b - 0.2f, 1);
			feedback.SetColorPackSelected(selected);
		}
		else
		{
			Debug.LogError("Los botones de customizar colores deben tener un ButtonFeedback");
		}
		if (selected)
		{
			backgroundButton.color = new Color(colors.customizeMenuColor.r - 0.2f, colors.customizeMenuColor.g - 0.2f, colors.customizeMenuColor.b - 0.2f, 1);
		}
	}

	private void OnEnable()
	{
		Actions.colorsChange += PackSelected;
	}

	private void OnDisable()
	{
		Actions.colorsChange -= PackSelected;
	}

	private void PackSelected(ColorsSO newColor)
	{
		bool selected = colorId == newColor.idColor;
		feedback.SetColorPackSelected(selected);

		if (selected)
		{

			// animar los 7 elementos

		}
		else
		{
			// parar las animaciones y posicionarlos en origen
		}
	}


	IEnumerator MoveUpAndDown(Image img)
	{
		RectTransform rectTransform = img.gameObject.GetComponent<RectTransform>();
		Vector2 originalPosition = rectTransform.anchoredPosition;
		while (true)
		{
			// move the UI image up
			yield return StartCoroutine(Move(originalPosition + Vector2.up * moveDistance, moveDuration, rectTransform));

			// wait for the specified delay
			yield return new WaitForSeconds(delay);

			// move the UI image down
			yield return StartCoroutine(Move(originalPosition, moveDuration, rectTransform));

			// wait for the specified delay
			yield return new WaitForSeconds(delay);
		}
	}

	IEnumerator Move(Vector2 destination, float duration, RectTransform rectTransform)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, destination, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		rectTransform.anchoredPosition = destination;
	}
}
