using UnityEngine;

[CreateAssetMenu(fileName = "Trail", menuName = "Agent data/Trail")]
public class TrailSO : ScriptableObject
{
	public AnimationCurve curve;
	public float time = 0.5f;
	public float minVertexDistance = 0.1f;
	public Material material;
	public int cornerVertices;
	public int EndCapVertices;

	public void SetUpTrail(TrailRenderer trailRenderer, Color color)
	{
		trailRenderer.widthCurve = curve;
		trailRenderer.time = time;
		trailRenderer.minVertexDistance = minVertexDistance;
		trailRenderer.sharedMaterial = material;
		trailRenderer.numCornerVertices = cornerVertices;
		trailRenderer.numCapVertices = EndCapVertices;

		// Set new gradient
		Gradient gradient = new Gradient();
		GradientColorKey[] colorKey = new GradientColorKey[2];
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];

		colorKey[0].color = color;
		colorKey[0].time = 0.18f;
		colorKey[1].color = Color.white;
		colorKey[1].time = 1.0f;

		alphaKey[0].alpha = 1.0f;
		alphaKey[0].time = 0.18f;
		alphaKey[1].alpha = 0.0f;
		alphaKey[1].time = 1.0f;

		gradient.SetKeys(colorKey, alphaKey);
		trailRenderer.colorGradient = gradient;
	}
}