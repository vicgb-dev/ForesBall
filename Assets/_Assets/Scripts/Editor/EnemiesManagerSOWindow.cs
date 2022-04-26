using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemiesManagerSOWindow : ExtendedEditorWindow
{
	public static void Open(EnemiesManagerSO enemiesManagerSO)
	{
		EnemiesManagerSOWindow window = GetWindow<EnemiesManagerSOWindow>("Enemies Editor");
		window.serializedObject = new SerializedObject(enemiesManagerSO);
	}

	public void OnGUI()
	{
		currentProperty = serializedObject.FindProperty("enemies");
		// DrawProperties(currentProperty, true);

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

		DrawSidebar(currentProperty);

		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

		if (selectedProperty != null)
		{
			DrawProperties(selectedProperty, true);
		}
		else
		{
			EditorGUILayout.LabelField("Select an item from the list");
		}

		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal();
	}
}
