using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationsSystem : MonoBehaviour
{
	[SerializeField] private GameObject snackBar;
	[SerializeField] private float secondsAnimation;
	[SerializeField] private float secondsShowingSnackBar;
	[SerializeField] private AnimationCurve curve;

	List<string> notifications = new List<string>();
	Vector3 panelDownPosition;
	Vector3 panelUpPosition;
	Image fill;
	bool notificationsEnabled = false;

	#region Singleton

	private static NotificationsSystem _instance;
	public static NotificationsSystem Instance
	{
		get
		{
			if (_instance != null) return _instance;
			// Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<NotificationsSystem>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<NotificationsSystem>();
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
	}

	#endregion

	private void Start()
	{
		StartCoroutine(SetAsLastSibling());
		StartCoroutine(ShowNotification());
	}

	private IEnumerator SetAsLastSibling()
	{
		yield return null;
		snackBar.transform.SetAsLastSibling();
		RectTransform rT = snackBar.GetComponent<RectTransform>();
		panelDownPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, rT.localPosition.z);
		panelUpPosition = new Vector3(rT.localPosition.x, rT.localPosition.y + 500, rT.localPosition.z);
		snackBar.GetComponent<RectTransform>().localPosition = panelUpPosition;

		fill = snackBar.transform.GetChild(0).GetComponent<Image>();
		fill.fillAmount = 0;

		yield return null;
		notificationsEnabled = true;
	}

	private IEnumerator ShowNotification()
	{
		while (true)
		{
			if (notifications.Count > 0)
			{
				string currentNotText = notifications[0];
				snackBar.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentNotText;

				// Animacion hacia abajo
				yield return UIHelpers.Instance.MovePanel(snackBar, panelUpPosition, panelDownPosition, secondsShowingSnackBar, curve);
				// Loading
				yield return StartCoroutine(AnimateLoading());
				// Animacion hacia arriba
				yield return UIHelpers.Instance.MovePanel(snackBar, panelDownPosition, panelUpPosition, secondsShowingSnackBar, curve);

				fill.fillAmount = 0;
				notifications.RemoveAt(0);
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator AnimateLoading()
	{
		fill.fillAmount = 0;

		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < secondsAnimation)
		{
			elapsedTime += Time.deltaTime;
			time += Time.deltaTime / secondsAnimation;
			fill.fillAmount = Mathf.LerpUnclamped(0, 1, time);
			yield return null;
		}
		fill.fillAmount = 1;
	}


	public void NewNotification(string notificationText)
	{
		if (!notificationsEnabled) return;
		notifications.Add(notificationText);
	}
}
