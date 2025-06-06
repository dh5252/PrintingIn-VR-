using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlusButton : MonoBehaviour
{
    [Tooltip("버튼의 비선택(기본) 상태 머티리얼")]
    public Material defaultMaterial;
    [Tooltip("버튼 위에 마우스(또는 Ray)가 hover되었을 때 일시 적용할 머티리얼")]
    public Material hoverMaterial;
    private XRBaseInteractable _interactable;
    private AdditionalNumber additionalNumber;

    private Renderer _renderer;
    private bool _isHovered = false;

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        if (_interactable == null)
            Debug.LogError($"[{name}] PlusButton: XRBaseInteractable 컴포넌트가 없습니다.");
        else
        {
            _interactable.hoverEntered.AddListener(OnHoverEntered);
            _interactable.hoverExited.AddListener(OnHoverExited);
            _interactable.activated.AddListener(OnActivated);
        }

        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
            Debug.LogError($"[{name}] PlusButton: Renderer가 없습니다. 버튼 모델에 MeshRenderer를 붙이세요.");
        

        additionalNumber = transform.parent.Find("AdditionalNumber").GetComponent<AdditionalNumber>();
        if (additionalNumber == null)
            Debug.LogError($"[{name}] PlusButton: additionalNumber를 붙이세요.");
        
    }

    private void Start()
    {
        ApplyDefaultMaterial();
    }

    private void OnDestroy()
    {
        if (_interactable != null)
        {
            _interactable.hoverEntered.RemoveListener(OnHoverEntered);
            _interactable.hoverExited.RemoveListener(OnHoverExited);
            _interactable.activated.RemoveListener(OnActivated);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        _isHovered = true;
        ApplyHoverMaterial();
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        _isHovered = false;
        ApplyDefaultMaterial();
    }

    private void OnActivated(ActivateEventArgs args)
    {
        additionalNumber.PlusNumber();
    }

    #region Material 적용 메서드


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
