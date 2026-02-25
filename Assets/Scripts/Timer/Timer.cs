using UnityEngine;

public class Timer : MonoBehaviour
{
    public TimerState State { get; private set; }

    private float elapsedTime;
    private float targetTime;

    private void Update() => OnUpdate();

    protected virtual void OnUpdate()
    {
        if (State != TimerState.Running)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= targetTime)
        {
            State = TimerState.Completed;
        }
    }

    public virtual void StartTimer(float time)
    {
        ResetTimer();

        targetTime = time;
        State = TimerState.Running;
    }

    public virtual void StopTimer()
    {
        ResetTimer();
        State = TimerState.NotStarted;
    }

    private void ResetTimer()
    {
        elapsedTime = 0f;
        targetTime = 0f;
    }
}
