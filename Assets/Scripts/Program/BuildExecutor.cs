using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BuildExecutor : MonoBehaviour
{

    public Transform Joint;
    public Transform Stretchable;
    public Transform Support;
    public GameObject ConcretePrefab;
    public Vector3 CodeStartLocation { get; set; }
    public Transform UserBlocks;

    private Quaternion targetRot;
    private float targetLength;
    private float targetX;
    private float targetZ;


    public bool IsCancelled { get; private set; } = false;
    public void CancelExecution() => IsCancelled = true;
    public void ResetCancellation() => IsCancelled = false;


    public IEnumerator RunCode(List<CodeBlock> blocks)
    {
        if (IsCancelled) yield break;
        AudioManager.Instance.PlaySimulationStartSound();
        yield return StartCoroutine(MoveCoroutine(CodeStartLocation));
        if (IsCancelled) yield break;
        yield return StartCoroutine(RunCodeRoutine(blocks));
        
    }

    private void CalculateRotation(float x, float z)
    {
        float dx = x - Joint.position.x;
        float dz = z - Joint.position.z;

        float rad = Mathf.Atan2(dx, dz);
        float angle = rad * Mathf.Rad2Deg;

        targetRot = Quaternion.Euler(0f, angle, 0f);
    }

    private void CalculateLength(float x, float z)
    {
        float dx = x - Joint.position.x;
        float dz = z - Joint.position.z;

        float distance = Mathf.Sqrt(dx * dx + dz * dz);

        targetLength = distance - 1;
        targetX = x;
        targetZ = z;
    }
    private IEnumerator RotateToAngle(float duration)
    {
        Quaternion startRot = Joint.rotation;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (IsCancelled) yield break;
            float t = elapsed / duration;
            Joint.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (IsCancelled) yield break;
        Joint.rotation = targetRot;
    }

    private IEnumerator ExtendSupportRoutine(float duration)
    {
        Vector3 startScale = Stretchable.localScale;
        Vector3 endScale = new Vector3(startScale.x, targetLength / 2, startScale.z);

        Vector3 startPosition = Support.position;
        Vector3 endPosition = new Vector3(targetX, startPosition.y, targetZ);

        Vector3 startPivot = Stretchable.position;
        Vector3 endPivot = new Vector3((endPosition.x + Joint.position.x) / 2, startPivot.y, (endPosition.z + Joint.position.z) / 2);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (IsCancelled) yield break;
            float t = elapsed / duration;  // 0 → 1
            Stretchable.localScale = Vector3.Lerp(startScale, endScale, t);
            Stretchable.position = Vector3.Lerp(startPivot, endPivot, t);
            Support.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
        if (IsCancelled) yield break;
        Stretchable.localScale = endScale;
        Support.position = endPosition;
    }

    private void PlaceTargetObject(GameObject block)
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y - 8, transform.position.z);
        AudioManager.Instance.PlayBlockDropSound();
        Instantiate(block, target, Quaternion.identity, UserBlocks);
    }

    private IEnumerator MoveCoroutine(Vector3 next)
    {
        if (IsCancelled) yield break;
        CalculateRotation(next.x, next.z);
        CalculateLength(next.x, next.z);
        yield return StartCoroutine(RotateToAngle(0.01f));
        if (IsCancelled) yield break;
        yield return StartCoroutine(ExtendSupportRoutine(0.5f));
    }

    private IEnumerator RunCodeRoutine(List<CodeBlock> blocks)
    {
        int i = 0;
        while (i < blocks.Count)
        {
            if (IsCancelled) yield break;
            var b = blocks[i];
            switch (b.Type)
            {
                case CodeBlock.BlockType.Move:
                    Vector3 next = b.Direction * 5 + transform.position;
                    yield return StartCoroutine(MoveCoroutine(next));
                    i++;
                    break;

                case CodeBlock.BlockType.Place:
                    // 머티리얼 문자열→Material
                    GameObject block = ParseMaterial(b.Material);
                    // 잠깐 기다리고 놓기기
                    yield return new WaitForSeconds(0.5f);
                    if (IsCancelled) yield break;
                    PlaceTargetObject(block);

                    i++;
                    break;

                case CodeBlock.BlockType.Repeat:
                    // 대응하는 RepeatEnd 인덱스 찾기
                    int end = FindMatchingRepeatEnd(blocks, i);
                    if (end < 0) { Debug.LogError("RepeatEnd 누락"); yield break; }

                    // 중간 블록들 서브리스트로 뽑아서
                    var sub = blocks.GetRange(i + 1, end - i - 1);
                    // 반복 횟수만큼 재귀 실행
                    for (int r = 0; r < b.RepeatCount; r++)
                    {
                        if (IsCancelled) yield break;
                        yield return StartCoroutine(RunCodeRoutine(sub));
                    }

                    // i를 RepeatEnd 다음으로 점프
                    i = end + 1;
                    break;

                case CodeBlock.BlockType.RepeatEnd:
                    // 그냥 넘어가기
                    i++;
                    break;
            }
        }
    }

    private GameObject ParseMaterial(string material)
    {
        if (material == "콘크리트")
            return ConcretePrefab;
        return ConcretePrefab;
    }

    private int FindMatchingRepeatEnd(List<CodeBlock> blocks, int startIndex)
    {
        int depth = 0;
        for (int j = startIndex; j < blocks.Count; j++)
        {
            if (blocks[j].Type == CodeBlock.BlockType.Repeat) depth++;
            else if (blocks[j].Type == CodeBlock.BlockType.RepeatEnd)
            {
                depth--;
                if (depth == 0) return j;
            }
        }
        return -1;
    }

}
