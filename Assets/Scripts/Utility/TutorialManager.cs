using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    public GameObject Panel;      // 튜토리얼 Canvas GameObject
    public TextMeshProUGUI tutorialText;   // 표시할 텍스트

    [Header("Tutorial Steps")]
    [TextArea]
    public List<string> steps = new List<string>();

    private int currentStep = -1;           // 현재 표시된 스텝 인덱스
    private InputDevice controllerDevice;    // XR 기기 입력

    void Start()
    {
        // 처음에 비활성화
        Panel.SetActive(false);
        // 오른손 XR 컨트롤러 검색
        controllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        // XR 컨트롤러가 유효하지 않으면 재검색
        // if (!controllerDevice.isValid)
        //     controllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool pressed = false;
        // XR B 버튼 입력 체크
        if (controllerDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool xrPressed) && xrPressed)
            pressed = true;

        if (Input.GetKeyDown(KeyCode.N))
        {
            pressed = true;
        }

        if (pressed)
            HandleButtonPress();
    }

    private void HandleButtonPress()
    {
        if (currentStep < 0)
        {
            // 튜토리얼 시작: 캔버스 활성화
            Panel.SetActive(true);
            currentStep = 0;
            tutorialText.text = steps[currentStep];
        }
        else
        {
            currentStep++;
            if (currentStep < steps.Count)
            {
                // 다음 스텝 텍스트
                tutorialText.text = steps[currentStep];
            }
            else
            {
                // 모든 스텝 완료: 캔버스 비활성화
                Panel.SetActive(false);
                currentStep = -1;
            }
        }
    }
}
