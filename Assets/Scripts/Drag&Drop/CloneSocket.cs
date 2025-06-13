using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(XRSocketInteractor))]
public class CloneSocket : MonoBehaviour
{
    [Tooltip("클론을 붙일 슬롯 위치 (Attach Transform)")]
    public Transform attachTransform;
    [Tooltip("원본")]
    public Transform origin;
    private Transform originParent;

    private Transform numberParent;

    public Transform x, z;

    Vector3 originPos;
    Quaternion originRot;
    Vector3 originScale;

    Vector3 xPos, zPos;
    Vector3 xScale, zScale;
    Quaternion xRot, zRot;

    XRSocketInteractor socket;
    XRInteractionManager manager;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        manager = socket.interactionManager;

        originParent = origin.parent;
        // 원본이 처음 있던 상태 저장
        originPos = origin.localPosition;
        originRot = origin.localRotation;
        originScale = origin.localScale;

        numberParent = x.parent;

        xPos = x.localPosition;
        xRot = x.localRotation;
        xScale = x.localScale;

        zPos = z.localPosition;
        zRot = z.localRotation;
        zScale = z.localScale;

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
        origT.GetComponent<BlockHover>().enabled = true;
        if (origT.name == "XValue")
        {
            origT.SetParent(numberParent, false);
            origT.localPosition = xPos;
            origT.localRotation = xRot;
            origT.localScale = xScale;
        }
        else if (origT.name == "ZValue")
        {
            origT.SetParent(numberParent, false);
            origT.localPosition = zPos;
            origT.localRotation = zRot;
            origT.localScale = zScale;
        }
        else
        {
            origT.SetParent(originParent, false);
            origT.localPosition = originPos;
            origT.localRotation = originRot;
            origT.localScale = originScale;
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

    private IEnumerator DelayedSocketRefresh(int frame)
    {
        for (int i = 0; i < frame; i++)
            yield return null;

        bool flag = false;
        Transform attach = transform.Find("Attach");
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
