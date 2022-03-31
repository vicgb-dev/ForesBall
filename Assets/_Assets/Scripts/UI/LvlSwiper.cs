using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LvlSwiper : MonoBehaviour
{
	public Scrollbar scrollbar;
	public float panelMaxScale = 0.68f;
	public float panelMinScale = 0.5f;
	public float transitionTime = 0.1f;

	private float scroll_pos = 0;
	private float[] positions;
	private bool lvlPanelsCreated = false;
	private float distance;

	public void Populated()
	{
		positions = new float[transform.childCount];
		distance = 1f / (positions.Length - 1f);
		lvlPanelsCreated = true;
	}

	public void Update()
	{
		if (!lvlPanelsCreated) return;

		for (int i = 0; i < positions.Length; i++)
			positions[i] = distance * i;

		if (Input.GetMouseButton(0))
			UpdateScrollPosition();
		else
			UpdatePanelsPosition();

		UpdatePanelsScale();
	}

	private void UpdateScrollPosition()
	{
		scroll_pos = scrollbar.value;
	}

	private void UpdatePanelsPosition()
	{
		for (int i = 0; i < positions.Length; i++)
			if (scroll_pos < positions[i] + (distance / 2) && scroll_pos > positions[i] - (distance / 2))
				scrollbar.value = Mathf.Lerp(scrollbar.value, positions[i], 0.1f);
	}

	public void UpdatePanelsScale()
	{
		for (int i = 0; i < positions.Length; i++)
			if (scroll_pos < positions[i] + (distance / 2) && scroll_pos > positions[i] - (distance / 2))
			{
				transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(panelMaxScale, panelMaxScale), transitionTime);
				for (int a = 0; a < positions.Length; a++)
					if (a != i)
						transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(panelMinScale, panelMinScale), transitionTime);
			}
	}
}


/*
private Vector3 origingPos;
public float percentThreshold = 0.2f;
public float easing = 0.5f;
// Definida al construir los paneles
public int totalPages = 3;
public int currentPage = 1;
private RectTransform rT;

private float distance;

private void Start()
{
	origingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	rT = GetComponent<RectTransform>();
	distance = rT.rect.width * percentThreshold;
}

public void OnDrag(PointerEventData data)
{
	float difference = data.pressPosition.x - data.position.x;
	transform.position = origingPos - new Vector3(difference, 0, 0);
}

public void OnEndDrag(PointerEventData data)
{
	// recorrer los transform de los hijos
	// cual es el que esta mas cerca del centro de la pantalla
	float closerDistance = 10000;
	Transform closestChild = null;
	foreach (Transform child in transform)
	{
		float distance = (child.position - transform.position).magnitude;
		if (distance < closerDistance)
		{
			closerDistance = distance;
			closestChild = child;
		}
	}
	StartCoroutine(SmoothMove(closestChild.position, transform.position, easing));
	closerDistance = 10000;


	// float difference = data.selectedObject.transform.position.x - origingPos.x;
	// if (Mathf.Abs(difference) > distance)
	// 	StartCoroutine(SmoothMove(transform.position, easing));

	// float percentage = (data.pressPosition.x - data.position.x) / rT.rect.width;
	// if (Mathf.Abs(percentage) >= percentThreshold)
	// {
	// 	Vector3 newLocation = origingPos;
	// 	if (percentage > 0 && currentPage < totalPages)
	// 	{
	// 		currentPage++;
	// 		newLocation += new Vector3(-rT.rect.width, 0, 0);
	// 	}
	// 	else if (percentage < 0 && currentPage > 1)
	// 	{
	// 		currentPage--;
	// 		newLocation += new Vector3(rT.rect.width, 0, 0);
	// 	}
	// 	StartCoroutine(SmoothMove(transform.position, easing));
	// }
	// else
	// {
	// 	StartCoroutine(SmoothMove(transform.position, easing));
	// }
}

private IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
{
	float t = 0;
	while (t <= 1)
	{
		t += Time.deltaTime / seconds;
		transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
		yield return null;
	}
}*/