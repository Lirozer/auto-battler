using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityButton : MyButton
{
    private const float TICK = 0.3f;
    private const float ROUNDER = 10f;

    protected float cooldown;
    protected bool IsButtonReady => (counter <= 0);

    private Text buttonText;
    private string abilityName;

    private float counter;

    private Timer tickTimer;

    protected override void OnStart()
    {
        base.OnStart();

        tickTimer = gameObject.AddComponent<Timer>();

        buttonText = GetComponentInChildren<Text>();
        abilityName = buttonText.text;
    }

    private void Update()
    {
        if (tickTimer.State == TimerState.Completed)
        {
            counter -= TICK;
            buttonText.text = (Mathf.Round(counter * ROUNDER) / ROUNDER).ToString();

            tickTimer.StartTimer(TICK);
        }

        if (IsButtonReady)
        {
            buttonText.text = abilityName;
            tickTimer.StopTimer();
        }
    }

    public override void OnButtonClick()
    {
        if (!IsButtonReady)
        {
            return;
        }

        counter = cooldown;
        buttonText.text = (Mathf.Round(counter * ROUNDER) / ROUNDER).ToString();

        tickTimer.StartTimer(TICK);
    }
}
