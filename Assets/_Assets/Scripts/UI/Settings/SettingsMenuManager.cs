public class SettingsMenuManager : Menu
{
	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Settings;
		panelDirection = Direction.Right;
	}
}