using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengesMenuManager : Menu
{
	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Challenges;
		panelDirection = Direction.Right;
	}
}