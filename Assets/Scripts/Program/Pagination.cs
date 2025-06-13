using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class Pagination : MonoBehaviour
{
    XRSocketInteractor socket;
    public GameObject NextPage;
    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnSocketed);
    }
    void OnSocketed(SelectEnterEventArgs args)
    {
        NextPage.SetActive(true);
    }    

    void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnSocketed);
    }
}