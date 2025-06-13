using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;
using System.Linq;

[RequireComponent(typeof(XRBaseInteractable))]
public class CodeClear : MonoBehaviour
{
    public Transform BlockSpot;
    public Program program;
    private XRBaseInteractable interactable;
    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
            Debug.LogError("Program :  XRBaseInteractable이 없습니다.");
        else
        {
            interactable.activated.AddListener(OnActivated);
        }
    }

    private void OnActivated(ActivateEventArgs args)
    {
        // 실행중이 아닐때만 동작
        if (program.IsRunning == false)
        {
            var blocks = BlockSpot
                .GetComponentsInChildren<XRGrabInteractable>()
                .Where(comp => comp.gameObject.layer == LayerMask.NameToLayer("Block"))
                .ToList();

            foreach (var b in blocks)
                Destroy(b.gameObject);
        }

    }
    private void OnDestroy()
    {
        // 리스너 해제
        if (interactable != null)
            interactable.activated.RemoveListener(OnActivated);
    }

}