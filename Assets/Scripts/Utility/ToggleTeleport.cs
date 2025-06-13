using UnityEngine;
using UnityEngine.XR;

public class ToggleTeleport : MonoBehaviour
{
    [Header("어느 손 컨트롤러 입력을 사용할지")]
    public XRNode inputSource = XRNode.RightHand;

    [Header("XR Origin (또는 VR Rig)의 Transform")]
    public Transform xrOrigin;

    [Header("토글할 순간이동 지점 A")]
    public Transform teleportPointA;

    [Header("토글할 순간이동 지점 B")]
    public Transform teleportPointB;

    // 내부 상태 추적
    private InputDevice device;
    private bool lastPressed = false;
    private bool toggleState = false;  // false→A, true→B

    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    void Update()
    {
            // 1) XRDevice API로 Primary Button 체크
        bool xrPressed = device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed) && pressed;
    
        // 2) 에디터에서 B 키로도 토글
        bool simPressed = Input.GetKeyDown(KeyCode.B);
    
        if ((xrPressed || simPressed) && !lastPressed)
        {
            DoToggleTeleport();
        }
        lastPressed = xrPressed || simPressed;
    }

    private void DoToggleTeleport()
    {
        // 이동할 대상 결정
        Transform target = toggleState ? teleportPointA : teleportPointB;

        xrOrigin.position = target.position;

       
        toggleState = !toggleState;
    }
}
