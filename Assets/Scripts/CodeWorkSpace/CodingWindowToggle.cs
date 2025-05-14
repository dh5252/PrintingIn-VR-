using UnityEngine;
using UnityEngine.InputSystem;

public class CodingWindowToggle : MonoBehaviour
{
    [Tooltip("토글할 CodingPanel 오브젝트")]
    public GameObject codingPanel;
    [Tooltip("Inspector에서 할당할 Input Action Reference" )]
    public InputActionReference toggleAction;
    [Tooltip("XR Origin 하위의 Camera를 연결")]
    public Camera xrCamera;
    [Tooltip("카메라 앞까지의 거리 (미터 단위)")]
    public float spawnDistance = 2f;

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
        bool willBeActive = !codingPanel.activeSelf;
        codingPanel.SetActive(willBeActive);

        if (willBeActive)
        {
            // 켜질 때만 한 번 위치·회전 설정
            Vector3 forward = xrCamera.transform.forward;
            Vector3 spawnPos = xrCamera.transform.position + forward * spawnDistance;

            codingPanel.transform.position = spawnPos;
            codingPanel.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
}
