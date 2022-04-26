using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteRenderer", menuName = "Enemy data/SpriteRenderer")]
public class SpriteRendererSO : ScriptableObject
{
    public Sprite sprite;
	public Color color;

	public void SetUpSprite(SpriteRenderer spriteRenderer)
	{
		spriteRenderer.sprite = sprite;
		spriteRenderer.color = color;
	}
}
