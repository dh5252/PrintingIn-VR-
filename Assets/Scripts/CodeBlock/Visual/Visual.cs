using UnityEngine;

public class Visual : MonoBehaviour
{
    private Renderer blockRenderer;
    [SerializeField] private Material grayscaleMaterial;

    void Awake()
    {
        blockRenderer = GetComponent<Renderer>();
    }

    public void SetGrayscale()
    {
        if (grayscaleMaterial != null)
            blockRenderer.material = grayscaleMaterial;
        else
            Debug.LogWarning("흑백 Material이 설정되지 않았습니다.");
    }
}
