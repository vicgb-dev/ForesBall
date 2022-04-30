using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Menu
{
	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Main;
		panelDirection = Direction.Left;
	}
}