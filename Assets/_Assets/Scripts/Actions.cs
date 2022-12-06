using System;
using UnityEngine;

public static class Actions
{
	public static Action<LevelSO> onLvlStart;
	public static Action<bool> onLvlEnd;
	public static Action onLvlFinished;
	public static Action onCleanLvl;
	public static Action<GameObject, float> powerUpShrink;

	public static Action<UIState> onNewUIState;
}