using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Header("Buttons config")]
	public float buttonSeconds;
	public float buttonScale;
	[Range(0, 1)]
	public float buttonBrightness;
	public AnimationCurve buttonCurve;
	public bool buttonVibration;


	[Header("Menus config")]
	[SerializeField] Scrollbar customizeScrollbar;

	public float secondsToMovePanels = 1;
	public AnimationCurve curveToMove;
	public float secondsToChangeAlpha = 0.5f;
	public AnimationCurve curveToOriginalColor;
	private int currentPanel = -1;

	private LevelsMenuManager levelsMenuManager;
	private MainMenuManager mainMenuManager;

	#region Singleton

	private static UIManager _instance;
	public static UIManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<UIManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<UIManager>();
			return _instance;
		}
	}

	private void Awake()
	{
		levelsMenuManager = GetComponentInChildren<LevelsMenuManager>();
		mainMenuManager = GetComponentInChildren<MainMenuManager>();

		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		_instance = this;
	}

	#endregion

	private void OnEnable()
	{
		Actions.onLvlEnd += (win) => levelsMenuManager.BlockGameView(win);
		Actions.onLvlStart += (lvl) => levelsMenuManager.CleanGameView(lvl);
	}

	private void Start()
	{
		Actions.onNewUIState?.Invoke(UIState.Main);
	}

	public void MainMenuButton1()
	{
		Actions.onNewUIState?.Invoke(UIState.Challenges);
	}

	public void MainMenuButton2()
	{
		Actions.onNewUIState?.Invoke(UIState.Customize);
		customizeScrollbar.value = 1;
	}

	public void MainMenuButton3()
	{
		Actions.onNewUIState?.Invoke(UIState.Settings);
	}

	public void MainMenuButton4()
	{
		Actions.onNewUIState?.Invoke(UIState.Levels);
	}

	public void BackButton()
	{
		Actions.onNewUIState?.Invoke(UIState.Main);
	}

	public bool PlayLevel()
	{
		if (LvlBuilder.Instance.GetLevels()[currentPanel].unlocked)
			Actions.onLvlStart?.Invoke(LvlBuilder.Instance.GetLevels()[currentPanel]);
		else
			Debug.Log($"El nivel {currentPanel + 1} no esta desbloqueado");

		return !LvlBuilder.Instance.GetLevels()[currentPanel].unlocked;
	}

	public void SetCurrentPanel(int newCurrentPanel)
	{
		if (currentPanel == newCurrentPanel) return;
		currentPanel = newCurrentPanel;

		SoundManager.Instance.PlayMusicPreview(LvlBuilder.Instance.GetLevels()[currentPanel]);

	}
}