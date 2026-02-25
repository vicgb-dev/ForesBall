using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Testing : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		Logger.Instance.Log("OnPointerDown");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Logger.Instance.Log("OnBeginDrag");
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Logger.Instance.Log("OnEndDrag");
	}

	public void OnDrag(PointerEventData eventData)
	{
		Logger.Instance.Log("OnDrag");
	}
}