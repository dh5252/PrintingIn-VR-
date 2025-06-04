using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;


public class BlockHover : MonoBehaviour
{
    [Header("Hover 시 커질 크기 (원본 대비 배율)")]
    [Tooltip("Hover되었을 때 적용할 목표 로컬 스케일 비율")]
    public Vector3 hoverScale = new Vector3(3.5f, 1.2f, 0.1f);

    [Header("스케일 애니메이션 속도")]
    [Tooltip("클수록 빠르게 변환됩니다.")]
    [Range(1f, 20f)]
    public float scaleSpeed = 8f;

    // 내부 참조
    private XRBaseInteractable _interactable;

    // 원본(기본) 스케일을 저장
    private Vector3 _originalScale;

    // 현재 실행 중인 스케일 코루틴 참조 (겹침 방지)
    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        // 1) XRBaseInteractable 가져와서 Hover 이벤트 구독
        _interactable = GetComponent<XRBaseInteractable>();
        if (_interactable == null)
        {
            Debug.LogError($"[{name}] BlockHoverScale: XRBaseInteractable 컴포넌트가 필요합니다.");
            return;
        }
        _interactable.hoverEntered.AddListener(OnHoverEnter);
        _interactable.hoverExited.AddListener(OnHoverExit);

        // 2) 현재 오브젝트의 기본 스케일 저장
        _originalScale = transform.localScale;
    }

    private void OnDestroy()
    {
        if (_interactable != null)
        {
            _interactable.hoverEntered.RemoveListener(OnHoverEnter);
            _interactable.hoverExited.RemoveListener(OnHoverExit);
        }
    }

    /// <summary>
    /// Hover Entered 이벤트
    /// 부드럽게 hoverScale로 스케일을 변경
    /// </summary>
    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        StartScaleAnimation(hoverScale);
    }

    /// <summary>
    /// Hover Exited 이벤트
    /// 부드럽게 원본(_originalScale)으로 스케일을 변경
    /// </summary>
    private void OnHoverExit(HoverExitEventArgs args)
    {
        StartScaleAnimation(_originalScale);
    }

    /// <summary>
    /// 새로운 목표 스케일(target)로 가는 코루틴 시작
    /// </summary>
    private void StartScaleAnimation(Vector3 target)
    {
        // 이미 코루틴이 돌고 있으면 중단
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);

        _scaleCoroutine = StartCoroutine(ScaleCoroutine(target));
    }

    /// <summary>
    /// 현재 transform.localScale에서 targetScale로 부드럽게 Lerp
    /// </summary>
    private IEnumerator ScaleCoroutine(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float t = 0f;

        // t가 1이 될 때까지 보간
        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            // SmoothStep을 써서 처음과 끝이 부드럽게 이어지도록
            float s = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, s);
            yield return null;
        }

        // 정확히 목표 스케일로 마무리
        transform.localScale = targetScale;
        _scaleCoroutine = null;
    }

}
