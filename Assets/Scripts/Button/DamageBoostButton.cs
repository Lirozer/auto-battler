using UnityEngine;

public class DamageBoostButton : AbilityButton
{
    private const float DAMAGE_BOOST_DELAY = 20f;

    protected override void OnStart()
    {
        cooldown = DAMAGE_BOOST_DELAY;
        base.OnStart();
    }

    public override void OnButtonClick()
    {
        if (!Player.CanUseDamageBoostMode)
        {
            return;
        }

        if (!IsButtonReady)
        {
            return;
        }

        base.OnButtonClick();
        Player.DamageBoostModeOn();
    }
}
