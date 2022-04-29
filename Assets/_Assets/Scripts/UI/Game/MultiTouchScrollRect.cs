using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouchScrollRect : ScrollRect
{
	#region Fields
	private int minimumTouchCount = 1, maximumTouchCount = 2, initialTouchCount = 0;
	#endregion

	#region Properties
	public Vector2 MultiTouchPosition
	{
		get
		{
			Vector2 position = Vector2.zero;
			// for (int i = 0; i < Input.touchCount && i < maximumTouchCount; i++)
			// {
			// 	position += Input.touches[i].position;
			// }
			// position /= ((Input.touchCount <= maximumTouchCount) ? Input.touchCount : maximumTouchCount);

			position = Input.GetTouch(0).position;
			return position;
		}
	}
	#endregion

	#region Methods
	private void Update()
	{
#if UNITY_EDITOR
		if (Input.touchCount > 0)
		{
			if (initialTouchCount == 0)
			{
				initialTouchCount = Input.touchCount;
			}
		}
		else
		{
			initialTouchCount = 0;
		}
#endif
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			if (Input.touchCount > 0)
			{
				if (initialTouchCount == 0)
				{
					initialTouchCount = Input.touchCount;
				}
			}
			else
			{
				initialTouchCount = 0;
			}
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
#if UNITY_EDITOR
		if (Input.touchCount >= minimumTouchCount && Input.touchCount == initialTouchCount)
		{
			eventData.position = MultiTouchPosition;
			base.OnBeginDrag(eventData);
		}
#endif
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			if (Input.touchCount >= minimumTouchCount && Input.touchCount == initialTouchCount)
			{
				eventData.position = MultiTouchPosition;
				base.OnBeginDrag(eventData);
			}
		}
		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			base.OnBeginDrag(eventData);
		}
	}
	public override void OnDrag(PointerEventData eventData)
	{
#if UNITY_EDITOR
		if (Input.touchCount >= minimumTouchCount && Input.touchCount == initialTouchCount)
		{
			eventData.position = MultiTouchPosition;
			base.OnDrag(eventData);
		}
#endif
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			if (Input.touchCount >= minimumTouchCount && Input.touchCount == initialTouchCount)
			{
				eventData.position = MultiTouchPosition;
				base.OnDrag(eventData);
			}
		}
		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			base.OnDrag(eventData);
		}
	}
	public override void OnEndDrag(PointerEventData eventData)
	{
#if UNITY_EDITOR
		if (Input.touchCount >= minimumTouchCount)
		{
			eventData.position = MultiTouchPosition;
			base.OnEndDrag(eventData);
		}
#endif
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			if (Input.touchCount >= minimumTouchCount)
			{
				eventData.position = MultiTouchPosition;
				base.OnEndDrag(eventData);
			}
		}
		else if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			base.OnEndDrag(eventData);
		}
	}
	#endregion
}