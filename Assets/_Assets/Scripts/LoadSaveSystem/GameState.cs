using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
	public int idColor;
	public List<SavedLevel> savedLevels;
	public Accomplishments accomplishments;
}