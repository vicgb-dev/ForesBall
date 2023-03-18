using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
	[SerializeField] private Canvas mainCanvas;
	[SerializeField] private Canvas canvas;
	[SerializeField] private float secondsToStart = 0.1f;
	[Header("Loading animation")]
	[SerializeField] private Image img;
	[SerializeField] private float secondsToFill;
	[SerializeField] private AnimationCurve fillCurve;

	[Header("Fade out animation")]
	[SerializeField] private Vector3 initialImgScale;
	[SerializeField] private Vector3 finalImgScale;
	[SerializeField] private float secondsToChangeScale;
	[SerializeField] private AnimationCurve scaleCurve;

	[SerializeField] private Image background;

	private void Awake()
	{
		StartCoroutine(LoadYourAsyncScene());
	}

	IEnumerator LoadYourAsyncScene()
	{
		// The Application loads the Scene in the background as the current Scene runs.
		// This is particularly good for creating loading screens.
		// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
		// a sceneBuildIndex of 1 as shown in Build Settings.

		yield return new WaitForSeconds(secondsToStart);

		float time = 0;
		float elapsedTime = 0;
		while (elapsedTime < secondsToFill)
		{
			elapsedTime += Time.unscaledDeltaTime;
			time += Time.unscaledDeltaTime / secondsToFill;
			img.fillAmount = Mathf.Lerp(0, 1, fillCurve.Evaluate(time));
			yield return null;
		}
		img.fillAmount = 1;

		StartCoroutine(ImgFadeAnimation());

		// img.transform.gameObject.SetActive(false);
		// background.transform.gameObject.SetActive(false);

		// time = 0;
		// elapsedTime = 0;
		// float alpha = 1;
		// while (elapsedTime < secondsToFill)
		// {
		// 	elapsedTime += Time.unscaledDeltaTime;
		// 	time += Time.unscaledDeltaTime / secondsToFill;
		// 	alpha = Mathf.Lerp(1, 0, time);
		// 	img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
		// 	background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
		// 	yield return null;
		// }
		// img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
		// background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
	}

	private IEnumerator ImgFadeAnimation()
	{
		float timeElapsed = 0f;
		Vector3 startScale = img.transform.localScale;

		// Scale to first target
		while (timeElapsed < secondsToChangeScale)
		{
			float t = timeElapsed / secondsToChangeScale;
			timeElapsed += Time.deltaTime;
			img.transform.localScale = Vector3.Lerp(startScale, initialImgScale, scaleCurve.Evaluate(t));

			yield return null;
		}

		StartCoroutine(BackgroundAnimation());
		canvas.GetComponent<GraphicRaycaster>().enabled = false;

		// Scale to second target
		float alpha = 1;
		timeElapsed = 0f;
		while (timeElapsed < secondsToChangeScale)
		{
			float t = timeElapsed / secondsToChangeScale;
			timeElapsed += Time.deltaTime;

			alpha = Mathf.Lerp(1, 0, scaleCurve.Evaluate(t));
			img.transform.localScale = Vector3.Lerp(initialImgScale, finalImgScale, scaleCurve.Evaluate(t));

			img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
			yield return null;
		}

		// Make sure final scale is exact
		img.transform.localScale = finalImgScale;
		img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

		Destroy(canvas.transform.gameObject);
	}

	private IEnumerator BackgroundAnimation()
	{
		float alpha = 1;
		float timeElapsed = 0f;
		while (timeElapsed < secondsToChangeScale)
		{
			float t = timeElapsed / secondsToChangeScale;
			timeElapsed += Time.deltaTime;

			alpha = Mathf.Lerp(1, 0, scaleCurve.Evaluate(t));
			background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
			yield return null;
		}

		background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
	}

	public void CanvasRenderTogle()
	{
		if (mainCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		else
			mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
	}
}
