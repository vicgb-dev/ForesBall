using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelpers : MonoBehaviour
{

	#region Singleton

	private static UIHelpers _instance;
	public static UIHelpers Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<UIHelpers>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<UIHelpers>();
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

		_instance = this;
	}

	#endregion

	public IEnumerator MovePanel(GameObject go, Vector3 initialPosition, Vector3 finalPosition, float seconds, AnimationCurve curve, Action callback = null)
	{
		//Debug.Log($"Moviendo {go.name} de {initialPosition} a {finalPosition} en {seconds} segundos.");
		RectTransform rT = go.GetComponent<RectTransform>();
		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < seconds)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / seconds;
			rT.localPosition = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time));
			yield return null;
		}
		rT.localPosition = finalPosition;
		callback?.Invoke();
	}

	public IEnumerator ColorChange(Image image, Color initialColor, Color finalColor, float seconds, AnimationCurve curve, Action callback = null)
	{
		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < seconds)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / seconds;
			image.color = Color.Lerp(initialColor, finalColor, curve.Evaluate(time));
			yield return null;
		}
		image.color = finalColor;
		callback?.Invoke();
	}
}