using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail", menuName = "Enemy data/Trail")]
public class TrailSO : ScriptableObject
{
    public AnimationCurve curve;
	public float time = 0.5f;
	public float minVertexDistance = 0.1f;
	public Gradient colorGradient;
	public Material material;
	public int cornerVertices;
	public int EndCapVertices;

	public void SetUpTrail(TrailRenderer trailRenderer)
	{
		trailRenderer.widthCurve = curve;
		trailRenderer.time = time;
		trailRenderer.minVertexDistance = minVertexDistance;
		trailRenderer.colorGradient = colorGradient;
		trailRenderer.sharedMaterial = material;
		trailRenderer.numCornerVertices = cornerVertices;
		trailRenderer.numCapVertices = EndCapVertices;
	}
}