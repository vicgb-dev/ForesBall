using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
	// [Header("Choose output")]
	// [SerializeField] private bool console;
	// [SerializeField] private bool panel;

	// [Header("Panel options")]
	// [SerializeField] private GameObject panelGO;
	// [SerializeField] private Sprite logIcon;
	// [SerializeField] private Sprite warningIcon;
	// [SerializeField] private Sprite errorIcon;
	// [SerializeField] private float timeToHide;

	[Header("Choose source logs")]
	[SerializeField] private bool swipeMenu;

	private enum LogType
	{
		Log,
		Warning,
		Error
	}

	private int logsCounter = 0;
	private int warningsCounter = 0;
	private int errorsCounter = 0;
	private TextMeshProUGUI logMessage;
	private TextMeshProUGUI logSummary;
	private GameObject logInScene;

	private static Logger _instance;

	public static Logger Instance
	{
		get { return _instance; }
	}

	/// <summary>
	/// Si se quiere agregar una nueva fuente debemos añadirla al enum LogSource,
	/// al switch del método SourceEnabled y un bool en el inspector para controlar su activación
	/// </summary>
	private void Awake() => _instance = this;

	/// <summary>
	/// Tendrá un icono de Log.
	/// Mostrará por consola, por escena o por ambos el msg dependiendo de la configuración del Loggeren el Inspector.
	/// <param name="msg">Mensaje</param>
	/// <param name="source">Desde donde viene el msg para agrupar los logs</param>
	/// </summary>
	public void Log(string msg, LogSource source)
	{
		if (!SourceEnabled(source)) return;
		Debug.Log(msg);
		// if (console) Debug.Log(msg);
		// if (panel) PanelLog(msg, LogType.Log);
	}

	/// <summary>
	/// Tendrá un icono de Warning.
	/// Mostrará por consola, por escena o por ambos el msg dependiendo de la configuración del Loggeren el Inspector.
	/// <param name="msg">Mensaje</param>
	/// <param name="source">Desde donde viene el msg para agrupar los logs</param>
	/// </summary>
	public void LogWarning(string msg, LogSource source)
	{
		if (!SourceEnabled(source)) return;
		Debug.LogWarning(msg);
		// if (console) Debug.LogWarning(msg);
		// if (panel) PanelLog(msg, LogType.Warning);
	}

	/// <summary>
	/// Tendrá un icono de Error.
	/// Mostrará por consola, por escena o por ambos el msg dependiendo de la configuración del Loggeren el Inspector.
	/// <param name="msg">Mensaje</param>
	/// <param name="source">Desde donde viene el msg para agrupar los logs</param>
	/// </summary>
	public void LogError(string msg, LogSource source)
	{
		if (!SourceEnabled(source)) return;
		Debug.LogError(msg);
		// if (console) Debug.LogError(msg);
		// if (panel) PanelLog(msg, LogType.Error);
	}

	private bool SourceEnabled(LogSource source)
	{
		switch (source)
		{
			case LogSource.SwipeMenu:
				return swipeMenu;
		}
		return false;
	}

	// private void PanelLog(string msg, LogType logType)
	// {
	// 	if (logInScene != null)
	// 		logInScene.SetActive(true);
	// 	else
	// 	{
	// 		logInScene = GameObject.Find(panelGO.name + "(Clone)");
	// 		if (logInScene == null)
	// 		{
	// 			logInScene = Instantiate(panelGO, Camera.main.transform);
	// 			logSummary = logInScene.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
	// 			logMessage = logInScene.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
	// 		}
	// 	}

	// 	logMessage.text = logMessage.text.Replace("Last log", " ");
	// 	logMessage.text = $"[{System.DateTime.Now.TimeOfDay}] Last log\n{msg}\n-----\n{logMessage.text}";

	// 	switch (logType)
	// 	{
	// 		case LogType.Log:
	// 			logsCounter++;
	// 			logInScene.transform.GetChild(2).GetComponent<Image>().sprite = logIcon;
	// 			break;
	// 		case LogType.Warning:
	// 			warningsCounter++;
	// 			logInScene.transform.GetChild(2).GetComponent<Image>().sprite = warningIcon;
	// 			break;
	// 		case LogType.Error:
	// 			errorsCounter++;
	// 			logInScene.transform.GetChild(2).GetComponent<Image>().sprite = errorIcon;
	// 			break;
	// 	}

	// 	logSummary.text = $"Logs: {logsCounter}\nWarnings: {warningsCounter}\nErrors: {errorsCounter}";

	// 	CancelInvoke();
	// 	Invoke("HideLogger", timeToHide);
	// }

	// private void HideLogger()
	// {
	// 	logInScene.SetActive(false);
	// }
}