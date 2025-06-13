using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;



public class Stage : MonoBehaviour
{
    private List<Problem> problems;
    public TextMeshPro Description;
    public TextMeshPro Material1;
    public TextMeshPro Material2;
    public BuildExecutor buildExecutor;
    public TextMeshPro XText;
    public TextMeshPro ZText;

    //public Canvas ImageCanvas;


    void Awake()
    {
        problems = new List<Problem>(5);
        InitProblem0();
        InitProblem1();
        InitProblem2();
        InitProblem3();
    }

    public void SetProblemByIndex(int index)
    {
        // 설명, 머티리얼 설정
        Description.text = problems[index].Description;
        Material1.text = problems[index].Material1;
        if (problems[index].Material2 == null)
            Material2.transform.parent.gameObject.SetActive(false);
        else
        {
            Material2.transform.parent.gameObject.SetActive(true);
            Material2.text = problems[index].Material2;
        }

        // 시작위치 설정
        buildExecutor.CodeStartLocation = new Vector3(problems[index].StartX, 0, problems[index].StartZ);

        // 이미지 작업하기
        //ImageCanvas

        // x, z 크기 설정정
        if (problems[index].x != 0)
        {
            XText.transform.parent.gameObject.SetActive(true);
            XText.text = ((int)problems[index].x).ToString();
        }
        else
            XText.transform.parent.gameObject.SetActive(false);

        if (problems[index].z != 0)
        {
            ZText.text = ((int)problems[index].z).ToString();
            ZText.text = ((int)problems[index].z).ToString();
        }
        else
            ZText.transform.parent.gameObject.SetActive(false);

    }

    private void InitProblem0()
    {
        problems[0].Description = "";
        problems[0].Material1 = "콘크리트";
        problems[0].Material2 = "나무";
        problems[0].StartX = 400;
        problems[0].StartZ = 780;
        problems[0].x = 28; // (550 - 400) / 5
        problems[0].z = 3;
        problems[0].height = 2;
        problems[0].ProblemImage = null;
        problems[0].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[0].x; ++i)
            for (int j = 0; j < problems[0].height; ++j)
                problems[0].Answer.Add(new ExpectedBlockData(problems[0].Material1, new Vector3(problems[0].StartX + i * 5, 0, problems[0].StartZ)));

        // 추가 정보
        problems[0].Description += "\n\n가로블록길이 : 30\n높이 : 2";
    }

    private void InitProblem1()
    {
        problems[1].Description = "";
        problems[1].Material1 = "콘크리트";
        problems[1].Material2 = null;
        problems[1].StartX = 550;
        problems[1].StartZ = 785;
        problems[1].x = 0;
        problems[1].z = 23;
        problems[1].height = 2;
        problems[1].ProblemImage = null;
        problems[1].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[1].z; ++i)
            for (int j = 0; j < problems[1].height; ++j)
                problems[1].Answer.Add(new ExpectedBlockData(problems[1].Material1, new Vector3(problems[1].StartX, 0, problems[1].StartZ + i * 5)));
        problems[1].Description += "\n\n세로블록길이 : 23\n높이 : 2";
    }

    private void InitProblem2()
    {
        problems[2].Description = "";
        problems[2].Material1 = "콘크리트";
        problems[2].Material2 = null;
        problems[2].StartX = 550;
        problems[2].StartZ = 900;
        problems[2].x = 30;
        problems[2].z = 0;
        problems[2].height = 2;
        problems[2].ProblemImage = null;
        problems[2].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[2].x; ++i)
            for (int j = 0; j < problems[2].height; ++j)
                problems[2].Answer.Add(new ExpectedBlockData(problems[2].Material1, new Vector3(problems[2].StartX - i * 5, 0, problems[2].StartZ)));
        problems[2].Description += "\n\n가로블록길이 : 30개 \n높이 : 2";
    }
    private void InitProblem3()
    {
        problems[3].Description = "";
        problems[3].Material1 = "콘크리트";
        problems[3].Material2 = null;
        problems[3].StartX = 400;
        problems[3].StartZ = 900;
        problems[3].x = 0;
        problems[3].z = 24;
        problems[3].height = 2;
        problems[3].ProblemImage = null;
        problems[3].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[3].z; ++i)
            for (int j = 0; j < problems[3].height; ++j)
                problems[3].Answer.Add(new ExpectedBlockData(problems[3].Material1, new Vector3(problems[3].StartX, 0, problems[3].StartZ - i * 5)));
        problems[3].Description += "\n\n세로블록길이 : 24개 \n높이 : 2";
    }
}