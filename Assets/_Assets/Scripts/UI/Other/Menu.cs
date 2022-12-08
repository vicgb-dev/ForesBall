using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] protected GameObject panelMenu;
	[SerializeField] protected GameObject blockPanel;

	protected UIState childState;
	protected Direction panelDirection;

	private Vector3 panelLeftPosition;
	private Vector3 panelRightPosition;
	private Vector3 panelUpPosition;
	private Vector3 panelDownPosition;
	private Vector3 panelCenterPosition;

	protected enum Direction
	{
		Right,
		Left,
		Up,
		Down,
		Center
	}

	protected virtual void Awake()
	{
		RectTransform rT = panelMenu.GetComponent<RectTransform>();
		panelLeftPosition = new Vector3(rT.localPosition.x - rT.rect.width - 10, rT.localPosition.y, rT.localPosition.z);
		panelRightPosition = new Vector3(rT.localPosition.x + rT.rect.width + 10, rT.localPosition.y, rT.localPosition.z);
		panelUpPosition = new Vector3(rT.localPosition.x, rT.localPosition.y + rT.rect.height + 10, rT.localPosition.z);
		panelDownPosition = new Vector3(rT.localPosition.x, rT.localPosition.y - rT.rect.height - 10, rT.localPosition.z);
		panelCenterPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);
	}

	protected virtual void OnEnable()
	{
		Actions.onNewUIState += OnNewUIState;
	}

	protected virtual void OnNewUIState(UIState state)
	{
		if (state == childState)
		{
			MoveTo(Direction.Center);
			blockPanel.SetActive(false);
		}
		else
		{
			MoveTo(panelDirection);
			blockPanel.SetActive(true);
		}
	}

	protected void MoveTo(Direction direction)
	{
		Vector3 finalPosition;

		switch (direction)
		{
			case Direction.Right:
				finalPosition = panelRightPosition;
				break;
			case Direction.Left:
				finalPosition = panelLeftPosition;
				break;
			case Direction.Up:
				finalPosition = panelUpPosition;
				break;
			case Direction.Down:
				finalPosition = panelDownPosition;
				break;
			case Direction.Center:
			default:
				finalPosition = panelCenterPosition;
				break;
		}

		StopAllCoroutines();
		StartCoroutine(UIHelpers.Instance.MovePanel(panelMenu,
			panelMenu.transform.localPosition,
			finalPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove));
	}
}