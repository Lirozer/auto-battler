using UnityEngine;
using UnityEngine.UI;

public abstract class ResourceBar : MonoBehaviour
{
    protected Image fill;
    protected Character owner;

    private void Update() => OnUpdate();

    private void Start()
    {
        fill = transform.Find("Fill").GetComponent<Image>();
        owner = transform.parent.GetComponent<Character>();
    }

    protected abstract void OnUpdate();
}
