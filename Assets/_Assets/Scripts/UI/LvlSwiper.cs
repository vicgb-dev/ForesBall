using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LvlSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
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
	}
}
