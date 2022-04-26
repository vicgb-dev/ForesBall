using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HorizontalScrollView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
	public LvlSwiper lvlSwiper;

	public void OnPointerDown(PointerEventData eventData)
	{
		lvlSwiper.StopAllCoroutines();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		lvlSwiper.ScrollbarMoveOrigin();
	}
}
