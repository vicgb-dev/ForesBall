using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Testing : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("OnPointerDown");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("OnBeginDrag");
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag");
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("OnDrag");
	}
}