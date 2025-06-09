using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CloneObject : MonoBehaviour
{
    public GameObject toClone;

    public Transform spawnPoint;

    private XRBaseInteractable _interactable;

    XRInteractionManager interactionManager;

    private Transform parent;
    private Vector3 localPos;
    private Quaternion localRot;

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        interactionManager = _interactable.interactionManager;
        if (_interactable == null)
            Debug.LogError("CloneObject: XRBaseInteractable 컴포넌트가 필요합니다.");
        parent = transform.parent;
        localPos = transform.localPosition;
        localRot = transform.localRotation;
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnSelectEntered);
        _interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnSelectEntered);
        _interactable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Grab Interactable hover 상태 해제
        IXRHoverInteractable hoverInteractable = _interactable;
        if (hoverInteractable != null)
            interactionManager.CancelInteractableHover(hoverInteractable);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // 원본을 다시 원래 위치로 되돌림. 올바르게 장착되었을때는 onSocketed에서 처리
        transform.SetParent(parent);
        transform.localPosition = localPos;
        transform.localRotation = localRot;
    }
}

