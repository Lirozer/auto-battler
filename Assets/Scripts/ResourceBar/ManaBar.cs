public class ManaBar : ResourceBar
{
    protected override void OnUpdate()
    {
        fill.fillAmount = owner.GetMana();
    }
}
