using UnityEngine;

public class VolumeSlider : CustomSlider
{
	protected override void Start()
	{
		lastFillAmount = slider.fillAmount;
		slider.fillAmount = SoundManager.Instance.GetLinearVolume();
		base.Start();
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
		slider.fillAmount = fill;

		//SoundManager.Instance.SetVolume(slider.fillAmount);

		slider.color = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f, Tools.Remap(slider.fillAmount, 0, 1, 0.3f, 1));

		if (slider.fillAmount != lastFillAmount)
		{
			lastFillAmount = slider.fillAmount;
			if (Mathf.RoundToInt(lastFillAmount * 100) % 10 == 0)
			{
				Vibration.Vibrate(20);
			}
		}
	}
}