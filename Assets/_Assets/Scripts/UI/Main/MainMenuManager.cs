public class MainMenuManager : Menu
{
	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Main;
		panelDirection = Direction.Left;
	}
}