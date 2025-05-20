using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    private Vector3 originalPosition;
    CanvasGroup canvasGroup;
    CanvasGroup cloneCanvasGroup;
    public GameObject blockPrefab; // 미리 연결된 blockPrefab
    private GameObject draggingBlock; // 드래그할때 미리 연결된 blockPrefab 복사하기
    public bool IsInWorkspace{ get; set; }

    void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        IsInWorkspace = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 아직 블록 리스트에 있을때 복사하기
        if (IsInWorkspace == false)
        {
            draggingBlock = Instantiate(blockPrefab, transform.root);
            cloneCanvasGroup = draggingBlock.GetComponent<CanvasGroup>();
            cloneCanvasGroup.blocksRaycasts = false;
        }
        // 워크스페이스에 있는 경우 -> 다시 짜야함.
        else
        {
            originalPosition = transform.localPosition;
            originalParent = transform.parent;
            canvasGroup.blocksRaycasts = false;
            transform.SetParent(transform.root);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsInWorkspace == false)
            draggingBlock.transform.position += (Vector3)eventData.delta;
        else
            transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 복사인 경우.
        if (IsInWorkspace == false)
        {
            // 드롭 실패
            if (draggingBlock.transform.parent == transform.root)
                Destroy(draggingBlock);
            else // 드롭 성공
                cloneCanvasGroup.blocksRaycasts = true;
        }
        else // 워크스페이스 내의 이동인 경우
        {
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
            // 드롭 실패
            if (transform.parent == transform.root)
                transform.localPosition = originalPosition;
        }
    }
}
