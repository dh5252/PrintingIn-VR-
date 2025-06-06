using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CloneObject : MonoBehaviour
{
    public GameObject toClone;

    public Transform spawnPoint;

    private XRBaseInteractable _interactable;

    private Transform parent;
    private Vector3 localPos;
    private Quaternion localRot;
    private GameObject clone;

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        if (_interactable == null)
            Debug.LogError("CloneObject: XRBaseInteractable 컴포넌트가 필요합니다.");
        parent = transform.parent;
        localPos = transform.localPosition;
        localRot = transform.localRotation;
        clone = null;
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
        if (toClone == null)
        {
            Debug.LogError("CloneObject : 복사할 오브젝트가 설정되지 않았습니다.");
            return;
        }

        clone = Instantiate(toClone, localPos, localRot, parent);
        clone.name = $"{toClone.name}_Clone_{Time.frameCount}";
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // 복제본 바꾸기, 다른 슬롯에 장착된 경우만(다른 부모에 붙은 경우만)
        if (transform.parent != parent)
        {
            clone.transform.SetParent(transform.parent);
            clone.transform.localPosition = transform.localPosition;
            clone.transform.localRotation = transform.localRotation;
            clone.GetComponent<CloneObject>().enabled = false;
        }
        else // 올바른 슬롯에 장착되지 못했을때
            Destroy(clone);
        // 원본 정상화
        transform.SetParent(parent);
        transform.localPosition = localPos;
        transform.localRotation = localRot;
    }
    
}

