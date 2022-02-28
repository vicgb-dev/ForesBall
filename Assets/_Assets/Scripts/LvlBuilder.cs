using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlBuilder : MonoBehaviour
{
	// Componente que se ocupa de preparar enemigos, powerups y probabilidades
	private void OnEnable()
	{
		Actions.onLvlStart += StartLevel;
	}

	private void OnDisable()
	{
		Actions.onLvlStart -= StartLevel;
	}

	private void StartLevel(int lvlNum)
	{
		Debug.Log("Comienza el lvl " + lvlNum);
	}
}
