using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler//, IBeginDragHandler, IEndDragHandler
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private RectTransform rTConainer;

	[SerializeField] private float secondsToMorph = 3f;
	[SerializeField] private float maxScale = 3f;
	[SerializeField] private AnimationCurve curve;

	private RectTransform thisRT;
	private PlayerMove playerMove;

	private float containerLimitUp;
	private float containerLimitDown;
	private float containerLimitRight;
	private float containerLimitLeft;
	private Vector2 containerCenter;

	private float offSetVertical;
	private float OffSetHorizontal;

	private float vertical;
	private float horizontal;

	private bool isButton = false;
	public bool buttonEnabled = false;
	public bool isJoystick = true;
	private int currentLvl;

	public void Init(PlayerMove playerMove)
	{
		this.playerMove = playerMove;
		StartCoroutine(ToNothing());
	}

	private void Awake()
	{
		thisRT = GetComponent<RectTransform>();
		Vector3[] joystickCorners = new Vector3[4];
		thisRT.GetWorldCorners(joystickCorners);

		offSetVertical = (joystickCorners[1].y - joystickCorners[0].y) / 2;
		OffSetHorizontal = (joystickCorners[2].x - joystickCorners[1].x) / 2;
	}

	private void Start()
	{
		SetUpContainers();
	}

	public void SetUpContainers()
	{
		Vector3[] containerCorners = new Vector3[4];
		rTConainer.GetWorldCorners(containerCorners);

		containerLimitUp = containerCorners[1].y;
		containerLimitDown = containerCorners[0].y;
		containerLimitRight = containerCorners[2].x;
		containerLimitLeft = containerCorners[0].x;
		containerCenter = new Vector2(containerLimitRight - containerLimitLeft, containerLimitUp - containerLimitDown);

	}

	private void OnEnable()
	{
		Actions.colorsChange += PackSelected;
		Actions.onLvlStart += lvl =>
		{
			StopAllCoroutines();
			StartCoroutine(ToJoystick());
		};
		Actions.onLvlEnd += win =>
		{
			StopAllCoroutines();
			StartCoroutine(ToButton());
		};

		Actions.onCleanLvl += () => buttonEnabled = true;

		Actions.onNewUIState += OnNewUIState;
	}

	private void OnNewUIState(UIState state)
	{
		if (state == UIState.Levels)
			StartCoroutine(ToButton());
		else
			StartCoroutine(ToNothing());
	}

	private void PackSelected(ColorsSO newColor)
	{
		gameObject.GetComponent<Image>().color = newColor.playerColor;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!isJoystick) return;
		//Debug.Log("OnDrag");
		//Vector2 nextPosition = Input.GetTouch(0).position;
		Vector2 nextPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

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

		//thisRT.position = nextPosition;
		thisRT.position = new Vector3(nextPosition.x, nextPosition.y, thisRT.position.z);

		vertical = Mathf.InverseLerp(containerLimitDown + offSetVertical, containerLimitUp - offSetVertical, thisRT.position.y);
		horizontal = Mathf.InverseLerp(containerLimitLeft + OffSetHorizontal, containerLimitRight - OffSetHorizontal, thisRT.position.x);

		//Debug.Log("V: " + Mathf.Round(vertical * 100) + "% H: " + Mathf.Round(horizontal * 100) + "%");
		PostProcessingManager.Instance.ChangeLensDistorsion(horizontal, vertical);

		if (playerMove != null) playerMove.NewPosition(horizontal, vertical);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!isButton) return;
		if (!buttonEnabled) return;

		Vibration.Vibrate(30);
		buttonEnabled = UIManager.Instance.PlayLevel();
	}

	public IEnumerator ToButton()
	{
		isButton = true;
		isJoystick = false;

		float time = 0;
		Vector3 initialScale = thisRT.localScale;
		Vector3 finalScale = new Vector3(maxScale, maxScale, maxScale);

		Vector2 initialPosition = thisRT.position;
		Vector2 finalPosition = rTConainer.position;

		Image imagePlayButton = gameObject.transform.GetChild(0).GetComponent<Image>();
		Color initialColor = imagePlayButton.color;
		Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

		float elapsedTime = 0;
		while (elapsedTime < secondsToMorph)
		{
			elapsedTime += Time.deltaTime;
			time += Time.deltaTime / secondsToMorph;
			thisRT.localScale = Vector3.Lerp(initialScale, finalScale, curve.Evaluate(time));
			thisRT.position = Vector2.Lerp(initialPosition, finalPosition, curve.Evaluate(time));
			imagePlayButton.color = Color.Lerp(initialColor, finalColor, curve.Evaluate(time));
			yield return null;
		}
	}

	public IEnumerator ToJoystick()
	{
		float time = 0;
		gameObject.GetComponent<Image>().color = ColorsManager.Instance.GetPlayerColor();

		Vector3 initialScale = thisRT.localScale;
		Vector3 finalScale = Vector3.one;

		Image imagePlayButton = gameObject.transform.GetChild(0).GetComponent<Image>();
		Color initialColor = imagePlayButton.color;
		Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

		float elapsedTime = 0;
		while (elapsedTime < secondsToMorph)
		{
			elapsedTime += Time.deltaTime;
			time += Time.deltaTime / secondsToMorph;
			thisRT.localScale = Vector3.Lerp(initialScale, finalScale, curve.Evaluate(time));
			imagePlayButton.color = Color.Lerp(initialColor, finalColor, curve.Evaluate(time));
			yield return null;
		}

		isButton = false;
		isJoystick = true;
	}

	public IEnumerator ToNothing()
	{
		isButton = false;
		buttonEnabled = false;

		float time = 0;
		Vector3 initialScale = thisRT.localScale;
		Vector3 finalScale = Vector3.zero;

		Vector2 initialPosition = thisRT.position;
		Vector2 finalPosition = rTConainer.position;

		Image imagePlayButton = gameObject.transform.GetChild(0).GetComponent<Image>();
		Color initialColor = imagePlayButton.color;
		Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

		float elapsedTime = 0;
		while (elapsedTime < secondsToMorph)
		{
			elapsedTime += Time.deltaTime;
			time += Time.deltaTime / secondsToMorph;
			thisRT.localScale = Vector3.Lerp(initialScale, finalScale, curve.Evaluate(time));
			thisRT.position = Vector2.Lerp(initialPosition, finalPosition, curve.Evaluate(time));
			imagePlayButton.color = Color.Lerp(initialColor, finalColor, curve.Evaluate(time));
			yield return null;
		}
	}
}