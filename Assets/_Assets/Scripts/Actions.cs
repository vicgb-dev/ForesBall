using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
	public static Action<Level> onLvlStart;
	public static Action<bool> onLvlEnd;
	public static Action onCleanLvl;
	public static Action<List<Level>> onLoadLevels;
	//El tiempo se para, se reproduce un sonido de findejuego
	//se quita el control del joystick
	//se espera x segundos y se cierra la cortinilla
	//despues de cerrarse sale un menu
}