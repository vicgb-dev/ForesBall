using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBounds : MonoBehaviour
{
	public RectTransform panelJuego;
	public RectTransform panelSafeArea;

	[Header("Bounds")]
	public float offSetHeight = 0;
	public GameObject GoUp;
	public GameObject GoDown;

	public float offSetWidth = 0;
	public GameObject GoRight;
	public GameObject GoLeft;

	// Calculation of bounds of map depending on screen size and aspect ratio using rays
	void Start()
	{
		List<float> limits = new List<float>();
		Camera cam = Camera.main;

		Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width * panelSafeArea.anchorMax.x * panelJuego.anchorMin.x, Screen.height / 2));
		GoLeft.transform.position = new Vector3(ray.origin.x - offSetWidth, ray.origin.y);
		limits.Add(ray.origin.x);

		ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height * panelSafeArea.anchorMax.y * panelJuego.anchorMax.y));
		GoUp.transform.position = new Vector3(ray.origin.x, ray.origin.y + offSetHeight);
		limits.Add(ray.origin.y);

		ray = cam.ScreenPointToRay(new Vector3(Screen.width * panelSafeArea.anchorMax.x * panelJuego.anchorMax.x, Screen.height / 2));
		GoRight.transform.position = new Vector3(ray.origin.x + offSetWidth, ray.origin.y);
		limits.Add(ray.origin.x);

		ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height * panelSafeArea.anchorMax.y * panelJuego.anchorMin.y));
		GoDown.transform.position = new Vector3(ray.origin.x, ray.origin.y - offSetHeight);
		limits.Add(ray.origin.y);

		GameManager.Instance.SetMapLimits(limits);
	}

}