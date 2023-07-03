using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private GameObject pauseButton;
	[SerializeField] private Joystick joystick;
	[SerializeField] private LevelsMenuManager levelsMenuManager;

	private Vector3 panelLeftPosition;
	private Vector3 panelRightPosition;
	private Vector3 panelCenterPosition;

	private Vector3 buttonUpPosition;
	private Vector3 buttonCenterPosition;

	private Image pausePanelImage;
	public Color initialColor;
	public Color finalColor;

	private void Awake()
	{
		RectTransform rTButton = pauseButton.GetComponent<RectTransform>();
		buttonUpPosition = new Vector3(rTButton.localPosition.x, rTButton.localPosition.y + 500, rTButton.localPosition.z);
		buttonCenterPosition = new Vector3(rTButton.localPosition.x, rTButton.localPosition.y, rTButton.localPosition.z);

		RectTransform rT = pausePanel.GetComponent<RectTransform>();
		panelLeftPosition = new Vector3(rT.localPosition.x - rT.rect.width - 10, rT.localPosition.y, rT.localPosition.z);
		panelRightPosition = new Vector3(rT.localPosition.x + rT.rect.width + 10, rT.localPosition.y, rT.localPosition.z);
		panelCenterPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);

		pausePanelImage = pausePanel.GetComponent<Image>();
		initialColor = new Color(pausePanelImage.color.r, pausePanelImage.color.g, pausePanelImage.color.b, 0);
		finalColor = new Color(pausePanelImage.color.r, pausePanelImage.color.g, pausePanelImage.color.b, 0.9f);

		HidePauseButton();
	}

	private void OnEnable()
	{
		Actions.onLvlStart += ShowPauseButton;
		Actions.onLvlEnd += HidePauseButton;
		Actions.onLvlFinished += HidePauseButton;
	}

	private void ShowPauseButton(LevelSO obj = null)
	{
		pauseButton.GetComponent<Button>().enabled = true;
		StartCoroutine(UIHelpers.Instance.MovePanel(
			pauseButton,
			buttonUpPosition,
			buttonCenterPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove));
	}

	private void HidePauseButton()
	{
		HidePauseButton(true);
	}
	private void HidePauseButton(bool win)
	{
		Time.timeScale = 1;
		pausePanelImage.color = initialColor;
		pausePanel.transform.GetChild(0).localPosition = panelLeftPosition;
		//Debug.Log("escondiendo boton");
		pauseButton.GetComponent<Button>().enabled = false;
		StartCoroutine(UIHelpers.Instance.MovePanel(
			pauseButton,
			buttonCenterPosition,
			buttonUpPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove));
	}

	public void OnQuit()
	{
		RemovePausePanel();

		Unpause();
		AccomplishmentsSystem.Instance.losefromPause = true;
		Actions.onLvlEnd?.Invoke(false);
	}

	public void OnResume()
	{
		RemovePausePanel(Unpause);
	}

	private void RemovePausePanel(Action callback = null)
	{
		StopAllCoroutines();

		StartCoroutine(UIHelpers.Instance.ColorChange(
			pausePanelImage,
			finalColor,
			initialColor,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

		StartCoroutine(UIHelpers.Instance.MovePanel(
			pausePanel.transform.GetChild(0).gameObject,
			pausePanel.transform.localPosition,
			panelRightPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove,
			callback
		));
	}

	public void Pause()
	{
		pauseButton.GetComponent<Button>().enabled = false;
		pausePanel.SetActive(true);
		joystick.buttonEnabled = false;
		joystick.isJoystick = false;

		levelsMenuManager.UnblockGameview();
		Time.timeScale = 0;

		SoundManager.Instance.PauseLvlMusic();

		StartCoroutine(UIHelpers.Instance.ColorChange(
			pausePanelImage,
			initialColor,
			finalColor,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));

		StartCoroutine(UIHelpers.Instance.MovePanel(
			pausePanel.transform.GetChild(0).gameObject,
			panelLeftPosition,
			panelCenterPosition,
			UIManager.Instance.secondsToMovePanels,
			UIManager.Instance.curveToMove
		));
	}

	public void Unpause()
	{
		pauseButton.GetComponent<Button>().enabled = true;
		pausePanel.SetActive(false);
		joystick.buttonEnabled = true;
		joystick.isJoystick = true;
		Time.timeScale = 1;
		SoundManager.Instance.UnPauseLvlMusic();
	}
}
