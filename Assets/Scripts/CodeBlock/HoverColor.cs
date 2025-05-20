using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverColor : MonoBehaviour
{
    Graphic _graphic;
    Color _original;
    [Tooltip("호버 시 변경할 색상")]
    public Color hoverColor = Color.yellow;

    void Awake()
    {
        // UI 요소(Image, TextMeshProUGUI 등 Graphic을 상속한 컴포넌트를 가져옵니다.
        _graphic = GetComponent<Graphic>();
        if (_graphic == null)
        {
            Debug.LogError($"HoverColor requires a UI Graphic (e.g. Image, Text, TextMeshProUGUI) on '{gameObject.name}'.");
            enabled = false;
            return;
        }
        _original = _graphic.color;
    }

    // XR Grab Interactable 의 Hover Entered 이벤트에 바인딩
    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        _graphic.color = hoverColor;
    }

    // XR Grab Interactable 의 Hover Exited 이벤트에 바인딩
    public void OnHoverExited(HoverExitEventArgs args)
    {
        _graphic.color = _original;
    }
}
