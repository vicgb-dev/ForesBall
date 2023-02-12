using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PauseManager))]
public class PauseManagerEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		PauseManager pause = (PauseManager)target;

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Pause"))
		{
			pause.Pause();
		}

		if (GUILayout.Button("Unpause"))
		{
			pause.Unpause();
		}
		GUILayout.EndHorizontal();
	}
}
