using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private bool pointerIsOver; // flag to track if the pointer is over the target game object
	[SerializeField] private SlicedFilledImage slider;

	private Color color;
	private float lastFillAmount;
	private RectTransform thisRectT;

	private void Awake()
	{
		thisRectT = this.transform.GetComponent<RectTransform>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerEnter != this.gameObject) return;
		pointerIsOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		pointerIsOver = false;
	}

	private void Start()
	{
		lastFillAmount = slider.fillAmount;
		slider.fillAmount = SoundManager.Instance.GetLinearVolume();
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Remap(slider.fillAmount, 0, 1, 0.3f, 1));
	}

	void Update()
	{
		if (!pointerIsOver) return;

		// get the screen point of the pointer (mouse or touch input)
		Vector2 pointerScreenPoint = Input.mousePosition;

		// convert the screen point to a point in local space within the target rectangle
		Vector2 pointerLocalPoint;
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRectT, pointerScreenPoint, null, out pointerLocalPoint))
			return;

		// calculate a value from 0 to 1 representing the pointer position within the target rectangle
		slider.fillAmount = 0.5f + pointerLocalPoint.x / thisRectT.rect.width;
		SoundManager.Instance.SetVolume(slider.fillAmount);

		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Remap(slider.fillAmount, 0, 1, 0.3f, 1));

		if (slider.fillAmount != lastFillAmount)
		{
			lastFillAmount = slider.fillAmount;
			if (Mathf.RoundToInt(lastFillAmount * 100) % 10 == 0)
			{
				Vibration.Vibrate(20);
			}
		}

		if (slider.fillAmount < 0.0001f)
			slider.fillAmount = 0.0001f;
	}

	public void SetColor(Color newColor)
	{
		color = newColor;
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Remap(slider.fillAmount, 0, 1, 0.3f, 1));
	}

	float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
