using Mono.Cecil;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

[System.Serializable]
public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Transform transform)
    {
        position = transform.localPosition;
        rotation = transform.localRotation;
        scale = transform.localScale;
    }
}


[RequireComponent(typeof(XRSocketInteractor))]
public class BlockSocket : MonoBehaviour
{
    [Tooltip("클론을 붙일 슬롯 위치 (Attach Transform)")]
    public Transform attachTransform;
    [Tooltip("원본")]

    public Transform moveBlock;
    public Transform placeBlock;
    public Transform repeatBlock;
    public Transform repeatEndBlock;

    private TransformData moveData;
    private TransformData placeData;
    private TransformData repeatData;
    private TransformData repeatEndData;

    private Transform originParent;
    XRSocketInteractor socket;
    XRInteractionManager manager;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        manager = socket.interactionManager;

        moveData = new TransformData(moveBlock);
        placeData = new TransformData(placeBlock);
        repeatData = new TransformData(repeatBlock);
        repeatEndData = new TransformData(repeatEndBlock);
        originParent = moveBlock.parent;

        // 소켓에 SelectEnter 이벤트 붙이기
        socket.selectEntered.AddListener(OnSocketed);
    }


    void OnSocketed(SelectEnterEventArgs args)
    {
        // 1) 원본 오브젝트 컴포넌트 & Transform
        var origComp = args.interactableObject as Component;
        var origT = origComp.transform;

        // 클론 일때
        if (!origComp.gameObject.CompareTag("Origin"))
        {
            origComp.transform.SetParent(attachTransform, false);
            origComp.transform.localPosition = new Vector3(0, 0, 0);
            origComp.transform.localRotation = Quaternion.identity;
            origComp.transform.localScale = Vector3.one;
            origComp.GetComponent<BlockHover>().enabled = false;
            socket.socketScaleMode = SocketScaleMode.Fixed;
            socket.fixedScale = new Vector3(
                1f / origComp.transform.lossyScale.x,
                1f / origComp.transform.lossyScale.y,
                1f / origComp.transform.lossyScale.z
            );
            AudioManager.Instance.PlayEffectSound();
            return;
        }

        // 2) 원본을 소켓에서 분리 (SelectExit 호출)
        manager.SelectExit(socket, args.interactableObject);

        // 3) 원본을 원위치로 복원
        if (origT.name == "MoveBlock")
        {
            origT.SetParent(originParent, false);
            SetOriginTransform(origT, moveData);
        }
        else if (origT.name == "PlaceBlock")
        {
            origT.SetParent(originParent, false);
            SetOriginTransform(origT, placeData);
        }
        else if (origT.name == "RepeatBlock")
        {
            origT.SetParent(originParent, false);
            SetOriginTransform(origT, repeatData);
        }
        else if (origT.name == "RepeatEndBlock")
        {
            origT.SetParent(originParent, false);
            SetOriginTransform(origT, repeatEndData);
        }


        // 4) 슬롯(attachTransform) 밑에 클론 생성
        var clone = Instantiate(origComp.gameObject);
        clone.transform.SetParent(attachTransform, false);
        clone.name = origT.name + Time.frameCount;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localRotation = Quaternion.identity;
        clone.transform.localScale = Vector3.one;
        clone.tag = "Untagged";

        StartCoroutine(DelayedSocketRefresh(60));
    }

    void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnSocketed);
    }

    private void SetOriginTransform(Transform origin, TransformData s)
    {
        origin.localPosition = s.position;
        origin.localRotation = s.rotation;
        origin.localScale = s.scale;
    }

    private IEnumerator DelayedSocketRefresh(int frame)
    {
        for (int i = 0; i < frame; i++)
            yield return null;

        bool flag = false;
        Transform attach = transform.Find("attach");
        foreach (Transform child in attach)
        {
            if (!flag)
            {
                flag = true;
                continue;
            }
            Destroy(child.gameObject);
        }
    }

}
