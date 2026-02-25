public class SwitchTargetButton : MyButton
{
    protected Enemy target;

    protected override void OnStart()
    {
        base.OnStart();
        target = transform.GetComponentInParent<Enemy>();
    }

    public override void OnButtonClick()
    {
        Player.SetTarget(target);
    }
}
