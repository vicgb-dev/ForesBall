using TMPro;
using UnityEngine;

public class ResolutionSlider : CustomSlider
{
	[SerializeField] private TextMeshProUGUI resolutionText;

	Resolution initialResolution;
	int resolutionOption = 0;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		Invoke("SetUp", 1.5f);
	}

	private void SetUp()
	{
		initialResolution = Screen.currentResolution;
		resolutionOption = PlayerPrefs.GetInt("resolutionOption", 3);
		Debug.Log($"get opcion {resolutionOption}");
		SwitchResolution(resolutionOption);
		slider.fillAmount = 0.25f + 0.25f * (float)resolutionOption;
		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));
		lastFillAmount = slider.fillAmount;
	}

	void Update()
	{
		if (!pointerIsOver) return;

		Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector3[] sliderCorners = new Vector3[4];
		thisRectT.GetWorldCorners(sliderCorners);
		float left = sliderCorners[0].x;
		float right = sliderCorners[3].x;
		float fill = Mathf.InverseLerp(left, right, touchWorldPos.x);

		if (fill > 0.99f) fill = 1;
		if (fill < 0.01f) fill = 0;

		resolutionOption = (int)Tools.Remap(fill, 0, 1, 0, 3.99f);
		slider.fillAmount = 0.25f + 0.25f * (float)resolutionOption;

		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));

		if (slider.fillAmount != lastFillAmount)
		{
			SwitchResolution(resolutionOption);
			Debug.Log($"set opcion {resolutionOption}");
			PlayerPrefs.SetInt("resolutionOption", resolutionOption);
			lastFillAmount = slider.fillAmount;
			Vibration.Vibrate(20);
		}
	}

	public void SwitchResolution(int option)
	{
		switch (option)
		{
			case 3:
				Screen.SetResolution(initialResolution.width, initialResolution.height, Screen.fullScreenMode);
				break;
			case 2:
				Screen.SetResolution((int)(initialResolution.width * 0.9f), (int)(initialResolution.height * 0.9f), Screen.fullScreenMode);
				break;
			case 1:
				Screen.SetResolution((int)(initialResolution.width * 0.7f), (int)(initialResolution.height * 0.7f), Screen.fullScreenMode);
				break;
			case 0:
				Screen.SetResolution((int)(initialResolution.width * 0.5f), (int)(initialResolution.height * 0.5f), Screen.fullScreenMode);
				break;
		}

		resolutionText.text = $"{Screen.currentResolution.width}x{Screen.currentResolution.height}";
	}
}
