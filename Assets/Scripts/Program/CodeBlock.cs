using System;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CodeBlock
{
    public enum BlockType { Move, Place, Repeat, RepeatEnd }
    public BlockType Type;
    public Vector3Int Direction;      // MoveBlock 용
    public string Material = "";      // PlaceBlock 용

    public int RepeatCount;        // RepeatBlock 용

    public CodeBlock(GameObject block)
    {
        if (block.name[0] == 'M')
        {
            this.Type = BlockType.Move;
            Direction = ParseDirection(block.GetComponentInChildren<ButtonsManager>().GetSelectedButtonName());
        }
        else if (block.name[0] == 'P')
        {
            this.Type = BlockType.Place;
            if (block.GetComponentInChildren<XRSocketInteractor>().gameObject.GetComponentInChildren<TextMeshPro>() != null)
                Material = block.GetComponentInChildren<XRSocketInteractor>().gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        else if (String.Compare(block.name, 0, "RepeatEndBlock", 0, 14, StringComparison.Ordinal) == 0)
        {
            this.Type = BlockType.RepeatEnd;
        }
        else
        {
            this.Type = BlockType.Repeat;
            RepeatCount = 0;
            if (block.GetComponentInChildren<XRSocketInteractor>().gameObject.GetComponentInChildren<TextMeshPro>() != null)
                RepeatCount += int.Parse(block.GetComponentInChildren<XRSocketInteractor>().gameObject.GetComponentInChildren<TextMeshPro>().text);

            RepeatCount += int.Parse(block.transform.Find("AdditionalNumber").GetComponent<TextMeshPro>().text);
        }

    }
    public string checkValidBlock()
    {
        if (this.Type == BlockType.Move && Direction == Vector3Int.zero)
            return "이동 블록에 방향이 \n설정되지 않았습니다.";
        if (this.Type == BlockType.Place && Material == "")
            return "놓기 블록에 자재가 \n끼워지지 않았습니다.";
        return "정상";
    }
    private Vector3Int ParseDirection(string dir)
    {
        if (dir == "x plus")
            return new Vector3Int(1, 0, 0);
        else if (dir == "x minus")
            return new Vector3Int(-1, 0, 0);
        else if (dir == "z plus")
            return new Vector3Int(0, 0, 1);
        else if (dir == "z minus")
            return new Vector3Int(0, 0, -1);
        return new Vector3Int(0, 0, 0);
    }

}