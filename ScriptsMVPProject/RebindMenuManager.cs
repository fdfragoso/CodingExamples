using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    public InputActionReference[] inputRefs;

    private void OnEnable()
    {
        foreach(InputActionReference reference in inputRefs)
        {
            reference.action.Disable();
        }
    }

    private void OnDisable()
    {
        foreach (InputActionReference reference in inputRefs)
        {
            reference.action.Enable();
        }
    }
}
