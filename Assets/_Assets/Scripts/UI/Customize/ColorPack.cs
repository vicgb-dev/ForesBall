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
	private ColorsSO colorsSO;

	RectTransform rectPlayer;
	RectTransform rectStraightEnemy;
	RectTransform rectFollowEnemy;
	RectTransform rotfollowEnemy;
	RectTransform rectBigEnemy;
	RectTransform rectRayEnemy;
	RectTransform rectChallenge;
	RectTransform rectPowerUp;

	private void Awake()
	{
		rectPlayer = player.gameObject.GetComponent<RectTransform>();
		rectStraightEnemy = straightEnemy.gameObject.GetComponent<RectTransform>();
		rectFollowEnemy = followEnemy.gameObject.GetComponent<RectTransform>();
		rotfollowEnemy = followEnemy.gameObject.GetComponent<RectTransform>();
		rectBigEnemy = bigEnemy.gameObject.GetComponent<RectTransform>();
		rectRayEnemy = rayEnemy.gameObject.GetComponent<RectTransform>();
		rectChallenge = challenge.gameObject.GetComponent<RectTransform>();
		rectPowerUp = powerUp.gameObject.GetComponent<RectTransform>();
	}

	public void SetUp(ColorsSO colors, bool selected)
	{
		//Debug.Log($"Color {colors.idColor} {selected}");
		colorName.text = colors.colorName;
		player.color = colors.playerColor;
		straightEnemy.color = colors.straightEnemyColor;
		followEnemy.color = colors.followEnemyColor;
		bigEnemy.color = colors.bigEnemyColor;
		rayEnemy.color = colors.rayEnemyColor;
		challenge.color = colors.challengesColor;
		powerUp.color = colors.powerUpColor;
		colorId = colors.idColor;
		colorsSO = colors;

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
			MoveElements();
		else
		{
			StopAllCoroutines();
			StartCoroutine(ResetPositions());
		}
	}

	private IEnumerator ResetPositions()
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			rectPlayer.anchoredPosition = Vector2.Lerp(rectPlayer.anchoredPosition, Vector2.zero, elapsedTime / duration);
			rectStraightEnemy.anchoredPosition = Vector2.Lerp(rectStraightEnemy.anchoredPosition, Vector2.zero, elapsedTime / duration);
			rectFollowEnemy.anchoredPosition = Vector2.Lerp(rectFollowEnemy.anchoredPosition, Vector2.zero, elapsedTime / duration);
			rotfollowEnemy.rotation = Quaternion.Lerp(rotfollowEnemy.rotation, Quaternion.Euler(0, 0, 0), elapsedTime / duration);
			rectBigEnemy.localScale = Vector3.Lerp(rectBigEnemy.localScale, Vector3.one, elapsedTime / duration);
			rectRayEnemy.anchoredPosition = Vector2.Lerp(rectRayEnemy.anchoredPosition, Vector2.zero, elapsedTime / duration);
			rectChallenge.anchoredPosition = Vector2.Lerp(rectChallenge.anchoredPosition, Vector2.zero, elapsedTime / duration);
			rectPowerUp.anchoredPosition = Vector2.Lerp(rectPowerUp.anchoredPosition, Vector2.zero, elapsedTime / duration);
			yield return null;
		}
		rectPlayer.anchoredPosition = Vector2.zero;
		rectStraightEnemy.anchoredPosition = Vector2.zero;
		rectFollowEnemy.anchoredPosition = Vector2.zero;
		rotfollowEnemy.anchoredPosition = Vector2.zero;
		rectBigEnemy.anchoredPosition = Vector2.zero;
		rectRayEnemy.anchoredPosition = Vector2.zero;
		rectChallenge.anchoredPosition = Vector2.zero;
		followEnemy.transform.up = Vector3.up;

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
				rectPlayer));
		}
	}

	private IEnumerator MoveChallenge()
	{
		float elapsedTime = 0;
		while (true)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Sin(elapsedTime * challengeSpeed) * 10;
			rectChallenge.anchoredPosition = new Vector2(0, t);
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
			rectPowerUp.anchoredPosition = new Vector2(0, t);
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
				rectStraightEnemy));

			yield return StartCoroutine(
				Move(new Vector2(40, 0),
				0.5f,
				rectStraightEnemy));

			yield return StartCoroutine(
				Move(new Vector2(0, -40),
				0.5f,
				rectStraightEnemy));

			yield return StartCoroutine(
				Move(new Vector2(-40, 0),
				0.5f,
				rectStraightEnemy));
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
			rectFollowEnemy.anchoredPosition = new Vector2(tX, tY);
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
				rectRayEnemy));

			yield return StartCoroutine(
				Move(new Vector2(-20, -20),
				0.5f,
				rectRayEnemy));
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

	public void SetBackgroundButton()
	{
		if (isSeleceted)
			backgroundButton.color = new Color(colorsSO.customizeMenuColor.r - 0.2f, colorsSO.customizeMenuColor.g - 0.2f, colorsSO.customizeMenuColor.b - 0.2f, 1);
	}
}