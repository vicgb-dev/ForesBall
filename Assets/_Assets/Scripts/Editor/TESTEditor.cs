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

		UIGameViewManager uIGameViewManager = tests.uIGameViewManager;

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
			uIGameViewManager.BlockGameView(true);
		}

		if (GUILayout.Button("CleanGameView(lvl)"))
		{
			uIGameViewManager.CleanGameView(LvlBuilder.Instance.GetLevels()[0]);
		}
		GUILayout.EndHorizontal();
	}
}