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
		ColorsManager uIColorsManager = tests.uIColorsManager;

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
			uIColorsManager.UpdateGlobalColors();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("NewNotification"))
		{
			NotificationsSystem.Instance.NewNotification("Nueva notificacion de desbloqueo de algo");
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("RESETEAR TODO"))
		{
			LoadSaveManager.Instance.Delete();
			LvlBuilder.Instance.ResetLvls();
			PlayerPrefs.DeleteAll();
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Cambiar color"))
		{
			GameObject.FindWithTag("Player").GetComponentInChildren<SpriteRenderer>()
				.sharedMaterial.SetColor("_Color", new Color(0.8f, 0.5f, 0.2f, 1) * 3);
		}
		GUILayout.EndHorizontal();
	}
}