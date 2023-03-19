using UnityEngine;
using UnityEngine.EventSystems;

public class CustomSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] protected SlicedFilledImage slider;

	protected Color color;
	protected bool pointerIsOver; // flag to track if the pointer is over the target game object
	protected RectTransform thisRectT;
	protected float lastFillAmount;

	protected virtual void Start()
	{
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));
	}

	protected virtual void Awake()
	{
		thisRectT = this.transform.GetComponent<RectTransform>();
	}

	public virtual void SetColor(Color newColor)
	{
		color = newColor;
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));
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
}
