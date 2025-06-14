using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ExpectedBlockData
{
    public string Material;   // Concrete, Wood 등
    public Vector2Int Position;   // x, z 좌표만 쓰도록 Vector2

    public ExpectedBlockData(string m, Vector3 v)
    {
        Material = m;
        Position.x = (int)Math.Round(v.x);
        Position.y = (int)Math.Round(v.z);
    }

    public ExpectedBlockData(ExpectedBlockData e)
    {
        this.Material = e.Material;
        this.Position = e.Position;
    }
}


// 문제 하나를 정의하는 클래스
[Serializable]
public class Problem
{
    public string Description;

    public string Material1 = null;
    public string Material2 = null;

    public float StartX;

    public float StartZ;

    public float x;
    public float height;
    public float z;

    public Sprite ProblemImage;

    public List<ExpectedBlockData> Answer;

}
