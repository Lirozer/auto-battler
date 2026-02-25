using UnityEngine;
using UnityEngine.UI;

public abstract class MyButton : MonoBehaviour
{
    private Button button;

    private void Start() => OnStart();

    protected virtual void OnStart()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public abstract void OnButtonClick();
}
