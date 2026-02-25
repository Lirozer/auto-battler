public class AttackButton : AbilityButton
{
    private const float ATTACK_DELAY = 1.3f;

    protected override void OnStart()
    {
        cooldown = ATTACK_DELAY;
        base.OnStart();
    }

    public override void OnButtonClick()
    {
        if (!Player.CanUseActiveAttack)
        {
            return;
        }

        if (!IsButtonReady)
        {
            return;
        }

        base.OnButtonClick();
        Player.ActiveAttack();
    }
}
