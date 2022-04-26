using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
	[OnOpenAsset()]
	public static bool OpenEditor(int instanceId, int line)
	{
		EnemiesManagerSO obj = EditorUtility.InstanceIDToObject(instanceId) as EnemiesManagerSO;
		if (obj != null)
		{
			EnemiesManagerSOWindow.Open(obj);
			return true;
		}
		return false;
	}
}

[CustomEditor(typeof(EnemiesManagerSO))]
public class EnemiesManagerSOEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Editor"))
		{
			EnemiesManagerSOWindow.Open((EnemiesManagerSO)target);
		}
	}
}
