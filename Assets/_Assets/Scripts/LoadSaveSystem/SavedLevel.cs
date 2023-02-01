using System;

[Serializable]
public class SavedLevel
{
	public string lvlName;
	public float timeChallenge;
	public float hotspot;
	public float collectibles;

	public SavedLevel(string lvlName, float timeChallenge, float hotspot, float collectibles)
	{
		this.lvlName = lvlName;
		this.timeChallenge = timeChallenge;
		this.hotspot = hotspot;
		this.collectibles = collectibles;
	}

	override
	public string ToString()
	{
		return $"lvlName: {lvlName} timeChallenge:{timeChallenge} hotspot:{hotspot} collectibles:{collectibles}";
	}
}
