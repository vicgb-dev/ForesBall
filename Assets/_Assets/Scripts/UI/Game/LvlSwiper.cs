using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LvlSwiper : MonoBehaviour, IPointerDownHandler
{
	[Header("Swipe options")]
	public Scrollbar scrollbar;
	public float swipeRange;

	[Header("Scale panel options")]
	public float panelMaxScale = 0.68f;
	public float panelMinScale = 0.5f;
	public float transitionTime = 0.1f;

	private Vector2 startTouchPosition;
	private float[] positions;
	private bool lvlPanelsCreated = false;
	private float distance;
	private int currentPanel = 0;
	private int lastPanel = 0;
	private bool pointerInSwipeLvl = false;

	public void Populate()
	{
		positions = new float[transform.childCount];
		distance = 1f / (positions.Length - 1f);
		lvlPanelsCreated = true;
		for (int i = 0; i < positions.Length; i++)
			positions[i] = distance * i;
	}

	public void Update()
	{
		if (!lvlPanelsCreated) return;

		UpdatePanelsScale();

		if (!pointerInSwipeLvl) return;

		if (Input.touchCount > 0) Swipe();

	}

	private void Swipe()
	{
		if (Input.GetTouch(0).phase == TouchPhase.Began)
		{
			lastPanel = currentPanel;
			startTouchPosition = Input.GetTouch(0).position;
			StopAllCoroutines();
		}

		if (Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			if (currentPanel == lastPanel)
			{
				pointerInSwipeLvl = false;
				Vector2 endTouchPosition = Input.GetTouch(0).position;
				Vector2 range = endTouchPosition - startTouchPosition;
				if (range.x < -swipeRange && currentPanel < positions.Length - 1)
				{
					StopAllCoroutines();
					StartCoroutine(MoveLeft());
					return;
				}
				if (range.x > swipeRange && currentPanel > 0)
				{
					StopAllCoroutines();
					StartCoroutine(MoveRight());
					return;
				}
			}

			StopAllCoroutines();
			StartCoroutine(MoveOrigin());
		}
	}

	public IEnumerator MoveOrigin()
	{
		float nextPosition = positions[currentPanel];
		while (Mathf.Abs(nextPosition - scrollbar.value) > 0.001f)
		{
			yield return null;
			scrollbar.value = Mathf.Lerp(scrollbar.value, nextPosition, 0.1f);
		}
	}

	private IEnumerator MoveLeft()
	{
		currentPanel++;
		float nextPosition = positions[currentPanel];
		while (nextPosition - scrollbar.value > 0.001f)
		{
			yield return null;
			scrollbar.value = Mathf.Lerp(scrollbar.value, nextPosition, 0.1f);
		}
	}

	private IEnumerator MoveRight()
	{
		currentPanel--;
		float nextPosition = positions[currentPanel];
		while (scrollbar.value - nextPosition > 0.001f)
		{
			yield return null;
			scrollbar.value = Mathf.Lerp(scrollbar.value, nextPosition, 0.1f);
		}
	}

	public void UpdatePanelsScale()
	{
		for (int i = 0; i < positions.Length; i++)
			if (scrollbar.value < positions[i] + (distance / 2) && scrollbar.value > positions[i] - (distance / 2))
			{
				currentPanel = i;
				UIManager.Instance.SetCurrentPanel(currentPanel);
				transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(panelMaxScale, panelMaxScale), transitionTime);
				for (int a = 0; a < positions.Length; a++)
					if (a != i)
						transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(panelMinScale, panelMinScale), transitionTime);
			}
	}

	public void ScrollbarMoveOrigin()
	{
		StopAllCoroutines();
		StartCoroutine(MoveOrigin());
	}

	public void OnPointerDown(PointerEventData eventData) => pointerInSwipeLvl = true;

}