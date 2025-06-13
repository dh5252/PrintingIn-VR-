using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class ErrorNotifier : MonoBehaviour
{
    public static ErrorNotifier Instance { get; private set; }

    [Header("World-Space Canvas")]
    public GameObject  errorPanel;    // 월드 스페이스 캔버스 (비활성화 상태)
    public TextMeshProUGUI errorText; // 패널 내 텍스트


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        errorPanel.SetActive(false);
    }
    public void ShowError(string message)
    {
        // 1) 패널 켜고 메시지 갱신
        errorText.text = message;
        errorPanel.SetActive(true);

        // 2) 오류 사운드 재생
        AudioManager.Instance.PlayErrorSound();


        // 5) 3초 후 자동으로 숨김
        StartCoroutine(HideAfterSeconds(2f));
    }

    IEnumerator HideAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        errorPanel.SetActive(false);
    }
}
