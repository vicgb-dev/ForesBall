using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftHanded : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Image backgroundButton;
	[SerializeField] private Image icon;
	[SerializeField] private RectTransform miniMenuRect;
	[SerializeField] private RectTransform playButtonRect;
	[SerializeField] private Joystick joystick;
	// Referencias del panel donde esta el joysticj y del minimenu y cambiar sus anchoredpositions

	private bool isLeftHanded;

	private void Start()
	{
		isLeftHanded = PlayerPrefs.GetInt("leftHanded", 0) != 1;
		Debug.Log("isLeftHanded: " + isLeftHanded);
		ToggleLeftHanded();
		if (!isLeftHanded)
		{
			Vector2 tempRect = miniMenuRect.position;
			miniMenuRect.position = playButtonRect.position;
			playButtonRect.position = tempRect;
		}
	}

	public void ToggleLeftHanded()
	{
		isLeftHanded = !isLeftHanded;

		Debug.Log("toggled isLeftHanded: " + isLeftHanded);
		if (isLeftHanded)
		{
			icon.transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			icon.transform.localScale = new Vector3(1, 1, 1);
		}
		PlayerPrefs.SetInt("leftHanded", isLeftHanded ? 1 : 0);
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();
		//feedback.SetColorPackSelected(isLeftHanded);

		// cambiar posiciones
		Vector2 tempRect = miniMenuRect.position;
		miniMenuRect.position = playButtonRect.position;
		playButtonRect.position = tempRect;
		joystick.SetUpContainers();

		// if (isLeftHanded)
		// 	backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
	}

	public void UpdateColor()
	{
		ButtonFeedback feedback = GetComponent<ButtonFeedback>();

		// if (isLeftHanded)
		// 	backgroundButton.color = new Color(feedback.pressedColor.r, feedback.pressedColor.g, feedback.pressedColor.b, 1);
	}

	private bool isPressed = false;
	private float pressTime = 0f;
	private const float holdDuration = 10f; // segundos

	private void Update()
	{
		if (isPressed)
		{
			pressTime += Time.deltaTime;
			if (pressTime >= holdDuration)
			{
				pressTime = 0f;
				isPressed = false;

				// 👇 Aquí va tu código al mantener 10s pulsado
				Debug.Log("¡Botón mantenido 10 segundos!");
				UnlockAllLevels();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isPressed = true;
		pressTime = 0f;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPressed = false;
		pressTime = 0f;
	}

	private void UnlockAllLevels()
	{
		LoadSaveManager.Instance.UnlockAllLevels();
		NotificationsSystem.Instance.NewNotification("All levels unlocked!");
		Actions.onLvlEnd?.Invoke(false);
	}
}
