using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TESTS))]
public class TESTEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		TESTS tests = (TESTS)target;

		LevelsMenuManager levelsMenuManager = tests.levelsMenuManager;
		UIColorsManager uIColorsManager = tests.uIColorsManager;

		GUILayout.Label("Actions");
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("OnLvlEnd"))
		{
			Actions.onLvlEnd?.Invoke(true);
		}

		if (GUILayout.Button("OnLvlStart"))
		{
			Actions.onLvlStart?.Invoke(LvlBuilder.Instance.GetLevels()[0]);
		}

		GUILayout.EndHorizontal();
		GUILayout.Label("UIGameViewManager");
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("BlockGameView(true)"))
		{
			levelsMenuManager.BlockGameView(true);
		}

		if (GUILayout.Button("CleanGameView(lvl)"))
		{
			levelsMenuManager.CleanGameView(LvlBuilder.Instance.GetLevels()[0]);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("ChangeColors"))
		{
			uIColorsManager.ChangeGlobalColors();
		}
		GUILayout.EndHorizontal();
	}
}