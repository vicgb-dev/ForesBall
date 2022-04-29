using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
	public static Action<LevelSO> onLvlStart;
	public static Action<bool> onLvlEnd;
	public static Action<int> onNewActiveLvlPanel;
	public static Action onCleanLvl;

	public static Action<UIState> onNewUIState;
}