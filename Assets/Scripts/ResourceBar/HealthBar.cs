public class HealthBar : ResourceBar
{
    protected override void OnUpdate()
    {
        fill.fillAmount = owner.GetHealth();
    }
}
