using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private float seconds = 0.3f;
	[SerializeField] private float scale = 0.9f;
	[SerializeField] private float brightness;
	[SerializeField] private AnimationCurve curve;
	[SerializeField] private bool vibration;
	public Color pressedColor;
	Color initialColor;

	Button button;
	Image image;
	Vector3 initialScale;
	Transform childTransform;

	private void Awake()
	{
		childTransform = transform.GetChild(0).GetComponent<Transform>();
		initialScale = childTransform.localScale;
		button = GetComponent<Button>();
		image = transform.GetChild(0).GetComponent<Image>();
		if (image == null)
		{
			Destroy(this);
			return;
		}
		initialColor = image.color;

		if (button == null)
		{
			Destroy(this);
			return;
		}
	}

	private void Start()
	{
		SetUp();
	}

	[ContextMenu("SetUp variables")]
	public void SetUp()
	{
		seconds = UIManager.Instance.buttonSeconds;
		scale = UIManager.Instance.buttonScale;
		brightness = UIManager.Instance.buttonBrightness;
		curve = UIManager.Instance.buttonCurve;
		vibration = UIManager.Instance.buttonVibration;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerEnter != this.gameObject) return;
		SoundManager.Instance.PlaySinglePop();
		StopAllCoroutines();
		StartCoroutine(PressAnimation());
		StartCoroutine(PressColor());
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerEnter != this.gameObject) return;
		Debug.Log("Click");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerEnter != this.gameObject) return;
		StopAllCoroutines();
		StartCoroutine(ReleaseAnimation());
		StartCoroutine(ReleaseColor());
	}

	private IEnumerator PressAnimation()
	{
		Vector3 smallScale = initialScale * scale;
		float elapsedTime = 0;

		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			childTransform.localScale = Vector3.LerpUnclamped(initialScale, smallScale, curve.Evaluate(elapsedTime / seconds));
			yield return null;
		}

		childTransform.localScale = smallScale;
	}

	private IEnumerator PressColor()
	{
		Color startColor = image.color;
		float elapsedTime = 0;

		while (elapsedTime < (seconds / 2))
		{
			elapsedTime += Time.deltaTime;
			image.color = Color.Lerp(startColor, pressedColor, curve.Evaluate(elapsedTime / (seconds / 2)));
			yield return null;
		}

		image.color = pressedColor;
	}

	private IEnumerator ReleaseAnimation()
	{
		Vector3 bigScale = initialScale;
		float elapsedTime = 0;

		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			childTransform.localScale = Vector3.LerpUnclamped(initialScale * scale, bigScale, curve.Evaluate(elapsedTime / seconds));
			yield return null;
		}

		childTransform.localScale = bigScale;
	}

	private IEnumerator ReleaseColor()
	{
		Color startColor = image.color;
		float elapsedTime = 0;

		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			image.color = Color.Lerp(startColor, initialColor, curve.Evaluate(elapsedTime / seconds));
			yield return null;
		}

		image.color = initialColor;
	}
}
