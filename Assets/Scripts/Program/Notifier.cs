using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class Notifier : MonoBehaviour
{
    public static Notifier Instance { get; private set; }

    [Header("World-Space Canvas")]
    public GameObject  Canvas;    // 월드 스페이스 캔버스 (비활성화 상태)
    public TextMeshProUGUI UText; // 패널 내 텍스트


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Create");
        }
        else Destroy(gameObject);

        Canvas.SetActive(false);
    }
    public void ShowNoti(string message, float time)
    {
        // 1) 패널 켜고 메시지 갱신
        UText.text = message;
        Canvas.SetActive(true);

        // 2) 오류 사운드 재생
        AudioManager.Instance.PlayErrorSound();

        // 5) 3초 후 자동으로 숨김
        StartCoroutine(HideAfterSeconds(time));
    }

    IEnumerator HideAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        Canvas.SetActive(false);
    }
}
