using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private float seconds;
	[SerializeField] private float scale;
	[SerializeField] private float brightness;
	[SerializeField] private AnimationCurve curve;
	[SerializeField] private bool vibration;
	public Color pressedColor;
	Color initialColor;

	Button button;
	Image image;
	Vector3 initialScale;

	private void Awake()
	{
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		initialColor = image.color;
		if (button == null)
		{
			Destroy(this);
			return;
		}
		initialScale = gameObject.transform.localScale;
	}

	private void Start()
	{
		//SetUp();
	}

	private void SetUp()
	{
		seconds = UIManager.Instance.buttonSeconds;
		scale = UIManager.Instance.buttonScale;
		brightness = UIManager.Instance.buttonBrightness;
		curve = UIManager.Instance.buttonCurve;
		vibration = UIManager.Instance.buttonVibration;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("Enter");
		StopAllCoroutines();
		StartCoroutine(PressAnimation());
		StartCoroutine(PressColor());
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Click");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("Exit");
		StopAllCoroutines();
		StartCoroutine(ReleaseAnimation());
		StartCoroutine(ReleaseColor());
	}

	private IEnumerator PressAnimation()
	{
		Vector3 smallScale = initialScale * scale;
		float elapsedTime = 0;

		while (gameObject.transform.localScale.x >= smallScale.x)
		{
			elapsedTime += Time.deltaTime;
			gameObject.transform.localScale = Vector3.Lerp(initialScale, smallScale, elapsedTime / seconds);
			yield return null;
		}

		gameObject.transform.localScale = smallScale;
	}

	private IEnumerator PressColor()
	{
		Color startColor = image.color;
		float elapsedTime = 0;

		while (elapsedTime < (seconds / 2))
		{
			elapsedTime += Time.deltaTime;
			image.color = Color.Lerp(startColor, pressedColor, elapsedTime / (seconds / 2));
			yield return null;
		}

		image.color = pressedColor;
	}

	private IEnumerator ReleaseAnimation()
	{
		Vector3 bigScale = initialScale;
		float elapsedTime = 0;

		while (gameObject.transform.localScale.x <= bigScale.x)
		{
			elapsedTime += Time.deltaTime;
			gameObject.transform.localScale = Vector3.Lerp(initialScale * scale, bigScale, elapsedTime / seconds);
			yield return null;
		}

		gameObject.transform.localScale = initialScale * scale;
	}

	private IEnumerator ReleaseColor()
	{
		Color startColor = image.color;
		float elapsedTime = 0;

		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			image.color = Color.Lerp(startColor, initialColor, elapsedTime / seconds);
			yield return null;
		}

		image.color = initialColor;
	}
}
