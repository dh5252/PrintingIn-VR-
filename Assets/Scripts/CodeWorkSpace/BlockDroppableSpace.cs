using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDroppableSpace : MonoBehaviour, IDropHandler
{
    [Tooltip("드롭된 블록의 부모로 삼을 RectTransform")]
    public RectTransform dropParent;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppeedObj = eventData.pointerDrag;

        droppeedObj.GetComponent<DraggableBlock>();
    }

    private bool IsValidDrop()
    {
        

        return true;
    }
}
