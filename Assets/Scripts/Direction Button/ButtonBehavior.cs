using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;


[RequireComponent(typeof(XRBaseInteractable))]
public class ButtonBehavior : MonoBehaviour
{
    [Header("머티리얼 설정")]
    [Tooltip("버튼의 비선택(기본) 상태 머티리얼")]
    public Material defaultMaterial;
    [Tooltip("버튼이 선택되었을 때 적용할 머티리얼")]
    public Material selectedMaterial;
    [Tooltip("버튼 위에 마우스(또는 Ray)가 hover되었을 때 일시 적용할 머티리얼")]
    public Material hoverMaterial;

    // 내부 참조
    private Renderer _renderer;
    private XRBaseInteractable _interactable;
    private ButtonsManager _blockManager;

    // 현재 이 버튼이 선택 상태인지 여부
    private bool _isSelected = false;
    // 현재 hover 중인지 여부
    private bool _isHovered = false;

    private void Awake()
    {
        // 1) Renderer 가져오기
        _renderer = GetComponentInChildren<Renderer>(includeInactive : true);
        if (_renderer == null)
        {
            Debug.LogError($"[{name}] ButtonBehavior: Renderer가 없습니다. 버튼 모델에 MeshRenderer를 붙이세요.");
        }

        // 2) XRBaseInteractable 가져오기
        _interactable = GetComponent<XRBaseInteractable>();
        if (_interactable == null)
        {
            Debug.LogError($"[{name}] ButtonBehavior: XRBaseInteractable 컴포넌트가 없습니다.");
        }
        else
        {
            // hover 이벤트 리스너 등록
            _interactable.hoverEntered.AddListener(OnHoverEntered);
            _interactable.hoverExited.AddListener(OnHoverExited);

            // **Activate 이벤트 리스너 등록** (selectEntered 대신 activated)
            _interactable.activated.AddListener(OnActivated);
        }

        // 3) 부모(블록)에서 ButtonsManager 찾아두기
        _blockManager = GetComponentInParent<ButtonsManager>();
        if (_blockManager == null)
        {
            Debug.LogError($"[{name}] ButtonBehavior: 부모에서 BlockButtonManager를 찾을 수 없습니다.");
        }
    }

    private void OnDestroy()
    {
        // 리스너 해제
        if (_interactable != null)
        {
            _interactable.hoverEntered.RemoveListener(OnHoverEntered);
            _interactable.hoverExited.RemoveListener(OnHoverExited);
            _interactable.activated.RemoveListener(OnActivated);
        }
    }

    private void Start()
    {
        // 초기 상태: 선택 안 된 상태이므로 기본 머티리얼 적용
        _isSelected = false;
        ApplyDefaultMaterial();
    }

    /// <summary>
    /// XR Simple Interactable의 Activate 이벤트 콜백
    /// </summary>
    private void OnActivated(ActivateEventArgs args)
    {
        if (_blockManager != null)
        {
            _blockManager.OnButtonClicked(this);
        }
    }

    /// <summary>
    /// XR Simple Interactable의 hoverEntered 이벤트 콜백
    /// </summary>
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        _isHovered = true;
        // 선택 상태가 아니면 hover 머티리얼 적용
        if (!_isSelected)
        {
            ApplyHoverMaterial();
        }
    }

    /// <summary>
    /// XR Simple Interactable의 hoverExited 이벤트 콜백
    /// </summary>
    private void OnHoverExited(HoverExitEventArgs args)
    {
        _isHovered = false;
        // hover가 끝날 때
        if (_isSelected)
        {
            // 선택 상태라면 selectedMaterial로 복원
            ApplySelectedMaterial();
        }
        else
        {
            // 선택되지 않은 상태라면 기본 머티리얼 복원
            ApplyDefaultMaterial();
        }
    }

    /// <summary>
    /// BlockButtonManager에서 호출: 이 버튼을 '선택 상태'로 변경
    /// </summary>
    public void SetSelected()
    {
        if (!_isSelected)
        {
            _isSelected = true;
            // 선택 상태일 때는 hover 여부와 상관없이 selectedMaterial 적용
            ApplySelectedMaterial();
        }
    }

    /// <summary>
    /// BlockButtonManager에서 호출: 이 버튼을 '비선택 상태'로 변경
    /// </summary>
    public void SetDeselected()
    {
        if (_isSelected)
        {
            _isSelected = false;
            // 비선택 상태라면, hover 중일 때는 hoverMaterial, 아니면 기본 머티리얼
            if (_isHovered)
                ApplyHoverMaterial();
            else
                ApplyDefaultMaterial();
        }
    }

    #region Material 적용 메서드

    private void ApplySelectedMaterial()
    {
        if (_renderer != null && selectedMaterial != null)
        {
            var mats = _renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = selectedMaterial;
            }
            _renderer.materials = mats;
        }
    }

    private void ApplyDefaultMaterial()
    {
        if (_renderer != null && defaultMaterial != null)
        {
            var mats = _renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = defaultMaterial;
            }
            _renderer.materials = mats;
        }
    }

    private void ApplyHoverMaterial()
    {
        if (_renderer != null && hoverMaterial != null)
        {
            var mats = _renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = hoverMaterial;
            }
            _renderer.materials = mats;
        }
    }

    #endregion
}
