using System.Runtime.InteropServices;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private enum Direction
    {
        None,
        가로플러스,
        가로마이너스,
        세로플러스,
        세로마이너스,
        높이플러스,
        높이마이너스
    }
    [SerializeField] private int distance
    {
        get => distance;
        set => distance = Mathf.Max(1, value);
    }
    [SerializeField] private Direction direction = Direction.None;

    public void SetDirection(string dir)
    {
        direction = dir switch
        {
            "가로+" => Direction.가로플러스,
            "가로-" => Direction.가로마이너스,
            "세로+" => Direction.세로플러스,
            "세로-" => Direction.세로마이너스,
            "높이+" => Direction.높이플러스,
            "높이-" => Direction.높이마이너스,
            _ => Direction.None
        };
    }

    public void IncreaseDistance() => distance++;
    public void DecreaseDistance() => distance = Mathf.Max(0, distance - 1);

    // 어떻게 움직일지 담은 Vector Return
    public Vector3 GetMoveVector()
    {
        return direction switch
        {
            Direction.가로플러스 => Vector3.right * distance,
            Direction.가로마이너스 => Vector3.left * distance,
            Direction.세로플러스 => Vector3.forward * distance,
            Direction.세로마이너스 => Vector3.back * distance,
            Direction.높이플러스 => Vector3.up * distance,
            Direction.높이마이너스 => Vector3.down * distance,
            _ => Vector3.zero
        };
    }
}
