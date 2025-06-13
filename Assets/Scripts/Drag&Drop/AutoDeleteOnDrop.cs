using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;


public class AutoDeleteOnDrop : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectExited(SelectExitEventArgs args)
{
    if (transform.parent == null)
    {
        StartCoroutine(DestroySafely());
    }
}

    private IEnumerator DestroySafely()
    {
        yield return null; // 한 프레임 기다려서 시스템 처리 끝나고 삭제
        Destroy(gameObject);
    }
}