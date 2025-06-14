using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using System.Linq;



[RequireComponent(typeof(XRBaseInteractable))]
public class Program : MonoBehaviour
{
    public BuildExecutor executor;
    public Transform BlockSpot;

    public Transform UserBlocks;
    public Transform SuccessBlocks;

    public Renderer _renderer;
    public Material startMaterial;
    public Material stopMaterial;

    private XRBaseInteractable interactable;
    private Coroutine runCoroutine;

    public bool IsRunning => runCoroutine != null;

    private string errorMessage;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable == null)
            Debug.LogError("Program :  XRBaseInteractable이 없습니다.");
        else
        {
            interactable.activated.AddListener(OnActivated);
        }
        if (_renderer != null && startMaterial != null)
            _renderer.material = startMaterial;
        errorMessage = "";
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (runCoroutine == null)
        {
            XRGrabInteractable[] blocks = BlockSpot.GetComponentsInChildren<XRGrabInteractable>();
            List<CodeBlock> codeList = new List<CodeBlock>();
            foreach (var comp in blocks)
            {
                if (comp.gameObject.layer == LayerMask.NameToLayer("Block"))
                    codeList.Add(new CodeBlock(comp.gameObject));
            }
            if (checkValidBlockList(codeList) == false)
            {
                ErrorNotifier.Instance.ShowError(errorMessage, 2);
                return;
            }

            executor.ResetCancellation();
            if (_renderer != null && stopMaterial != null)
                _renderer.material = stopMaterial;
            
            runCoroutine = StartCoroutine(RunCodeWithCallback(codeList));
        }
        else
        {
            executor.CancelExecution();
            StopCoroutine(runCoroutine);
            runCoroutine = null;
            StartCoroutine(DelayedUserBlockClear(10));
            if (_renderer != null && startMaterial != null)
                _renderer.material = startMaterial;
        }
    }

    private IEnumerator RunCodeWithCallback(List<CodeBlock> codeList)
    {
        yield return StartCoroutine(executor.RunCode(codeList));
        for (int i = 0; i < 180; ++i)
            yield return null;
        string check = Stage.Instance.isAnswer();
        if (check == "ok")
            Success();
        else
        {
            ToggleTeleport.Instance.TeleportOriginLoc();
            string errorMessage = "오답입니다! " + check;
            ErrorNotifier.Instance.ShowError(errorMessage, 5);
            StartCoroutine(DelayedUserBlockClear(5));
        }
        RestoreMaterial();
        runCoroutine = null;
    }

    private void Success()
    {
        // 성공시 코드블록 삭제
        var blocks = BlockSpot
                .GetComponentsInChildren<XRGrabInteractable>()
                .Where(comp => comp.gameObject.layer == LayerMask.NameToLayer("Block"))
                .ToList();
        foreach (var b in blocks)
            Destroy(b.gameObject);

        // 기존에 쌓은 성들 부모 옮기기.
        while (UserBlocks.childCount > 0)
            UserBlocks.GetChild(0).SetParent(SuccessBlocks);
        
        ToggleTeleport.Instance.TeleportOriginLoc();
        Notifier.Instance.ShowNoti("성공하셨습니다!! 다음 단계로 넘어갑니다.", 5);
        Stage.Instance.PassProblem();
    }
    private void RestoreMaterial()
    {
        if (_renderer != null && stopMaterial != null)
            _renderer.material = startMaterial;
    }

    private void OnDestroy()
    {
        // 리스너 해제
        if (interactable != null)
            interactable.activated.RemoveListener(OnActivated);
    }

    private IEnumerator DelayedUserBlockClear(int frame)
    {
        for (int i = 0; i < frame; i++)
            yield return null;

        foreach (Transform child in UserBlocks)
            Destroy(child.gameObject);
    }

    private bool checkValidBlockList(List<CodeBlock> codeList)
    {
        var stack = new Stack<CodeBlock>();

        for (int i = 0; i < codeList.Count; i++)
        {
            var block = codeList[i];

            switch (block.Type)
            {
                case CodeBlock.BlockType.Repeat:
                    stack.Push(block);
                    break;

                case CodeBlock.BlockType.RepeatEnd:
                    if (stack.Count == 0)
                    {
                        errorMessage = "반복 끝 블록에 짝이 없습니다.";
                        return false;
                    }
                    stack.Pop();
                    break;

                default:
                    errorMessage = block.checkValidBlock();
                    if (errorMessage != "정상")
                        return false;
                    break;
            }
        }

        if (stack.Count > 0)
        {
            errorMessage = $"반복 블록 {stack.Count}개가 \n닫히지 않았습니다.";
            // 스택에 남은 Repeat이 있다면 닫히지 않은 블록이 있는 것
            return false;
        }

        return true;
    }

}

