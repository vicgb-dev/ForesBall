using System;
using UnityEngine;

public static class Actions
{
	public static Action<LevelSO> onLvlStart;
	// Cuando se acaba la animación de fin de nivel y todo debe volver a su sitio
	public static Action<bool> onLvlEnd;
	// Justo cuando acaba el nivel si ganas
	public static Action onLvlFinished;
	public static Action onCleanLvl;
	public static Action<GameObject, float> powerUpShrink;
	public static Action<ChallengeType, float> updateChallenge;
	public static Action<ColorsSO> colorsChange;
	public static Action<GameObject> enemyDestroyed;
	public static Action<AudioSource> onLvlMusicChange;
	public static Action<bool> onMute;

	public enum ChallengeType
	{
		time,
		hotspot,
		collectible
	}

	public static Action<UIState> onNewUIState;
}