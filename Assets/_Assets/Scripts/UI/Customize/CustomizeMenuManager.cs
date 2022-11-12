public class CustomizeMenuManager : Menu
{
	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Customize;
		panelDirection = Direction.Right;
	}
}