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
	[SerializeField] private float followSpeed = 3;
	[SerializeField] private Image bigEnemy;
	[SerializeField] private float bigscale = 1;
	[SerializeField] private Image rayEnemy;
	[SerializeField] private Image challenge;
	[SerializeField] private float challengeSpeed = 1;
	[SerializeField] private Image powerUp;
	[SerializeField] private float duration = 0.2f;


	private int colorId;
	private ButtonFeedback feedback;
	private bool isSeleceted;

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
		isSeleceted = selected;
		if (selected)
		{
			backgroundButton.color = new Color(colors.customizeMenuColor.r - 0.2f, colors.customizeMenuColor.g - 0.2f, colors.customizeMenuColor.b - 0.2f, 1);
		}
	}

	private void OnEnable()
	{
		Actions.colorsChange += PackSelected;
		Actions.onNewUIState += UIStateChanged;
	}

	private void OnDisable()
	{
		Actions.colorsChange -= PackSelected;
		Actions.onNewUIState -= UIStateChanged;
	}

	private void UIStateChanged(UIState state)
	{
		if (state != UIState.Customize || !isSeleceted)
		{
			StopAllCoroutines();
			StartCoroutine(ResetPositions());
			return;
		}

		MoveElements();
	}

	private void PackSelected(ColorsSO newColor)
	{
		bool selected = colorId == newColor.idColor;
		feedback.SetColorPackSelected(selected);

		isSeleceted = selected;
		if (selected)
		{

			// animar los 7 elementos
			MoveElements();
		}
		else
		{
			StopAllCoroutines();
			Debug.LogWarning("ResetPositions");
			StartCoroutine(ResetPositions());
		}
	}

	private IEnumerator ResetPositions()
	{
		float elapsedTime = 0f;

		Vector2 currentPosplayer = player.gameObject.GetComponent<RectTransform>().anchoredPosition;
		Vector2 currentPosstraightEnemy = straightEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition;
		Vector2 currentPosfollowEnemy = followEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition;
		Quaternion currentRotfollowEnemy = followEnemy.gameObject.GetComponent<RectTransform>().rotation;
		Vector3 currentScalebigEnemy = bigEnemy.gameObject.GetComponent<RectTransform>().localScale;
		Vector2 currentPosrayEnemy = rayEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition;
		Vector2 currentPoschallenge = challenge.gameObject.GetComponent<RectTransform>().anchoredPosition;
		Vector2 currentPospowerUp = powerUp.gameObject.GetComponent<RectTransform>().anchoredPosition;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			player.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(currentPosplayer, Vector2.zero, elapsedTime / duration);
			straightEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(currentPosstraightEnemy, Vector2.zero, elapsedTime / duration);
			followEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(currentPosfollowEnemy, Vector2.zero, elapsedTime / duration);
			followEnemy.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Lerp(currentRotfollowEnemy, Quaternion.Euler(0, 0, 0), elapsedTime / duration);
			bigEnemy.gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(currentScalebigEnemy, Vector3.one, elapsedTime / duration);
			rayEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(currentPosrayEnemy, Vector2.zero, elapsedTime / duration);
			challenge.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(currentPoschallenge, Vector2.zero, elapsedTime / duration);
			powerUp.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(currentPospowerUp, Vector2.zero, elapsedTime / duration);
			yield return null;
		}
		player.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		straightEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		followEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		followEnemy.transform.up = Vector3.up;
		bigEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		rayEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		challenge.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		powerUp.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

		yield return null;
	}

	private void MoveElements()
	{
		StartCoroutine(MovePlayer());
		StartCoroutine(MoveChallenge());
		StartCoroutine(MovePowerup());
		StartCoroutine(MoveStraight());
		StartCoroutine(MoveFollow());
		StartCoroutine(MoveBig());
		StartCoroutine(MoveRay());
	}

	private IEnumerator MovePlayer()
	{
		while (true)
		{
			yield return StartCoroutine(
				Move(new Vector2(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100)),
				UnityEngine.Random.Range(0.3f, 0.7f),
				player.gameObject.GetComponent<RectTransform>()));
		}
	}

	private IEnumerator MoveChallenge()
	{
		float elapsedTime = 0;
		while (true)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Sin(elapsedTime * challengeSpeed) * 10;
			challenge.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, t);
			yield return null;
		}
	}

	private IEnumerator MovePowerup()
	{
		yield return new WaitForSeconds(0.2f);
		float elapsedTime = 0;
		while (true)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Sin(elapsedTime * challengeSpeed) * 10;
			powerUp.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, t);
			yield return null;
		}
	}

	private IEnumerator MoveStraight()
	{
		while (true)
		{
			yield return StartCoroutine(
				Move(new Vector2(0, 40),
				0.5f,
				straightEnemy.gameObject.GetComponent<RectTransform>()));

			yield return StartCoroutine(
				Move(new Vector2(40, 0),
				0.5f,
				straightEnemy.gameObject.GetComponent<RectTransform>()));

			yield return StartCoroutine(
				Move(new Vector2(0, -40),
				0.5f,
				straightEnemy.gameObject.GetComponent<RectTransform>()));

			yield return StartCoroutine(
				Move(new Vector2(-40, 0),
				0.5f,
				straightEnemy.gameObject.GetComponent<RectTransform>()));
		}
	}

	private IEnumerator MoveFollow()
	{
		float elapsedTime = 0;
		while (true)
		{
			elapsedTime += Time.deltaTime;
			float tY = Mathf.Sin(elapsedTime * followSpeed) * 20;
			float tX = Mathf.Cos(elapsedTime * followSpeed) * 20;
			followEnemy.transform.up = followEnemy.transform.parent.position - followEnemy.transform.position;
			followEnemy.transform.Rotate(0, 0, -90);
			followEnemy.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(tX, tY);
			yield return null;
		}
	}

	private IEnumerator MoveBig()
	{
		Vector3 initialScale = bigEnemy.transform.localScale;
		float elapsedTime = 0;
		while (true)
		{
			elapsedTime += Time.deltaTime;
			float scale = Mathf.Sin(elapsedTime * challengeSpeed * 0.2f) * bigscale;
			bigEnemy.transform.localScale = new Vector3(scale, scale, scale);
			yield return null;
		}
	}

	private IEnumerator MoveRay()
	{
		while (true)
		{
			yield return StartCoroutine(
				Move(new Vector2(20, 20),
				0.5f,
				rayEnemy.gameObject.GetComponent<RectTransform>()));

			yield return StartCoroutine(
				Move(new Vector2(-20, -20),
				0.5f,
				rayEnemy.gameObject.GetComponent<RectTransform>()));
		}
	}
	IEnumerator Move(Vector2 destination, float duration, RectTransform rectTransform)
	{
		Vector2 startPos = rectTransform.anchoredPosition;
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			rectTransform.anchoredPosition = Vector2.Lerp(startPos, destination, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		rectTransform.anchoredPosition = destination;
	}

	float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

}
