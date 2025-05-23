using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugTest : MonoBehaviour
{
    public InputAction clickAction;

    void OnEnable()
    {
        clickAction.Enable();
        clickAction.performed += ctx => Debug.Log("[Input] Click Action performed!");
    }

    void OnDisable()
    {
        clickAction.performed -= ctx => Debug.Log("Click performed");
        clickAction.Disable();
    }
}