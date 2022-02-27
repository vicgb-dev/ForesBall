using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

	[SerializeField] private Canvas canvas;
	[SerializeField] private RectTransform rTConainer;

	private RectTransform rT;
	private PlayerMove playerMove;

	private float containerLimitUp;
	private float containerLimitDown;
	private float containerLimitRight;
	private float containerLimitLeft;

	private float offSetVertical;
	private float OffSetHorizontal;

	private float vertical;
	private float horizontal;

	public void Init(PlayerMove playerMove)
	{
		this.playerMove = playerMove;
	}

	private void Awake()
	{
		rT = GetComponent<RectTransform>();
	}

	private void Start()
	{
		Vector3[] containerCorners = new Vector3[4];
		rTConainer.GetWorldCorners(containerCorners);

		containerLimitUp = containerCorners[1].y;
		containerLimitDown = containerCorners[0].y;
		containerLimitRight = containerCorners[2].x;
		containerLimitLeft = containerCorners[0].x;

		Vector3[] joystickCorners = new Vector3[4];
		rT.GetWorldCorners(joystickCorners);

		offSetVertical = (joystickCorners[1].y - joystickCorners[0].y) / 2;
		OffSetHorizontal = (joystickCorners[2].x - joystickCorners[1].x) / 2;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("OnDrag");
		Vector2 nextPosition = Input.mousePosition;

		bool overLimitUp = nextPosition.y + offSetVertical > containerLimitUp;
		bool overLimitDown = nextPosition.y - offSetVertical < containerLimitDown;
		bool overLimitRight = nextPosition.x + OffSetHorizontal > containerLimitRight;
		bool overLimitLeft = nextPosition.x - OffSetHorizontal < containerLimitLeft;

		if (overLimitUp)
			nextPosition = new Vector2(nextPosition.x, containerLimitUp - offSetVertical);
		if (overLimitDown)
			nextPosition = new Vector2(nextPosition.x, containerLimitDown + offSetVertical);
		if (overLimitRight)
			nextPosition = new Vector2(containerLimitRight - OffSetHorizontal, nextPosition.y);
		if (overLimitLeft)
			nextPosition = new Vector2(containerLimitLeft + OffSetHorizontal, nextPosition.y);

		rT.position = nextPosition;

		vertical = Mathf.InverseLerp(containerLimitDown + offSetVertical, containerLimitUp - offSetVertical, rT.position.y);
		horizontal = Mathf.InverseLerp(containerLimitLeft + OffSetHorizontal, containerLimitRight - OffSetHorizontal, rT.position.x);

		Debug.Log("V: " + Mathf.Round(vertical * 100) + "% H: " + Mathf.Round(horizontal * 100) + "%");

		if (playerMove != null) playerMove.NewPosition(horizontal, vertical);
	}

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
}