using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;


public class BlockHover : MonoBehaviour
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class HoverDarken : MonoBehaviour
    {
        [Range(0f, 1f), Tooltip("원본 색상 대비 어두워질 비율 (0.8 = 20% 어둡게)")]
        public float darkenFactor = 0.8f;

        XRBaseInteractable interactable;
        Renderer[] renderers;
        Color[][] originalColors;

        void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();

            // 이 오브젝트와 자식들의 모든 Renderer를 가져와서
            renderers = GetComponentsInChildren<Renderer>();
            originalColors = new Color[renderers.Length][];

            // 각 머티리얼의 원본 색상 저장
            for (int i = 0; i < renderers.Length; i++)
            {
                var mats = renderers[i].materials;
                originalColors[i] = new Color[mats.Length];
                for (int j = 0; j < mats.Length; j++)
                    originalColors[i][j] = mats[j].HasProperty("_Color")
                                          ? mats[j].color
                                          : Color.white;
            }
        }

        void OnEnable()
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
        }

        void OnDisable()
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            // Hover 시작: 모든 머티리얼 색을 어둡게
            for (int i = 0; i < renderers.Length; i++)
            {
                var mats = renderers[i].materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    if (mats[j].HasProperty("_Color"))
                        mats[j].color = originalColors[i][j] * darkenFactor;
                }
            }
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            // Hover 끝: 원본 색으로 복원
            for (int i = 0; i < renderers.Length; i++)
            {
                var mats = renderers[i].materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    if (mats[j].HasProperty("_Color"))
                        mats[j].color = originalColors[i][j];
                }
            }
        }
    }

}
