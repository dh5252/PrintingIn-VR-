using UnityEngine;
using UnityEngine.InputSystem;

public class CodingWindowToggle : MonoBehaviour
{
    [Tooltip("토글할 CodingPanel 오브젝트")]
    public GameObject codingPanel;
    
    [Tooltip("Inspector에서 할당할 Input Action Reference (예: XRI Left PrimaryButton)")]
    public InputActionReference toggleAction;


    private void OnEnable()
    {
        // 액션 활성화 및 콜백 등록
        toggleAction.action.Enable();
        toggleAction.action.performed += OnTogglePerformed;
    }

    private void OnDisable()
    {
        // 콜백 해제 및 액션 비활성화
        toggleAction.action.performed -= OnTogglePerformed;
        toggleAction.action.Disable();
    }

    private void OnTogglePerformed(InputAction.CallbackContext ctx)
    {
        // 버튼을 누를 때마다 패널 토글
        codingPanel.SetActive(!codingPanel.activeSelf);
    }
}
