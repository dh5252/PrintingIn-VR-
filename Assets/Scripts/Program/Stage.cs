using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.UI;



public class Stage : MonoBehaviour
{
    static public Stage Instance { get; private set; }
    private List<Problem> problems;
    public TextMeshPro Description;
    public TextMeshPro Material1;
    public TextMeshPro Material2;
    public BuildExecutor buildExecutor;
    public TextMeshPro XText;
    public TextMeshPro ZText;
    public Image problemImage;
    public Transform userBlocks;

    public int StartLevel;

    public Sprite[] sprites;
    private int curIndex;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }


        problems = new List<Problem>(5);
        for (int i = 0; i < 5; ++i)
            problems.Add(new Problem());
        InitProblem1();
        InitProblem2();
        InitProblem3();
        InitProblem4();
        InitProblem5();
        curIndex = StartLevel - 1;
        SetProblemByIndex(curIndex);
    }

    public string isAnswer()
    {
        if (curIndex == 4)
            return "ok";
        List<ExpectedBlockData> answerCopy = problems[curIndex].Answer
            .Select(item => new ExpectedBlockData(item))
            .ToList();

        foreach (Transform block in userBlocks)
        {
            Vector2Int cmp = new Vector2Int((int)Math.Round(block.position.x), (int)Math.Round(block.position.z));
            var match = answerCopy.FirstOrDefault(item => cmp == item.Position);
            if (match != null)
                answerCopy.Remove(match);
    
            else
            {
                Debug.Log("Error Check : " + cmp);
                foreach (ExpectedBlockData c in answerCopy)
                {
                    Debug.Log(c.Position.x + " " + c.Position.y);
                }
                int errorXLoc = (cmp.x - (int)Math.Round(problems[curIndex].StartX)) / 5;
                int errorZLoc = (cmp.y - (int)Math.Round(problems[curIndex].StartZ)) / 5;
                return "가로 = " + errorXLoc.ToString() + ", " + "세로 = " + errorZLoc.ToString() + " 위치의 블록은 잘못됐어요.";
            }
        }
        if (answerCopy.Count > 0)
            return "놓은 블록 개수가 잘못되었어요.";

        return "ok";
    }

    public void PassProblem()
    {
        if (curIndex == 4)
            return;
        SetProblemByIndex(++curIndex);
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

        problemImage.sprite = problems[index].ProblemImage;

        // x, z 크기 설정
        XText.text = ((int)problems[index].x).ToString();
        ZText.text = ((int)problems[index].z).ToString();
        if (problems[index].x != 0)
            XText.transform.parent.gameObject.SetActive(true);
        else
            XText.transform.parent.gameObject.SetActive(false);
        if (problems[index].z != 0)
            ZText.transform.parent.gameObject.SetActive(true);
        else
            ZText.transform.parent.gameObject.SetActive(false);

    }

    private void InitProblem1()
    {
        problems[0].Description = "조선 사람들을 위해 콘크리트 성을 지어주자!\n먼저 오른쪽 성벽부터 지어주자!";
        problems[0].Material1 = "콘크리트";
        problems[0].Material2 = null;
        problems[0].StartX = 550;
        problems[0].StartZ = 785;
        problems[0].x = 0;
        problems[0].z = 23;
        problems[0].height = 2;
        problems[0].ProblemImage = sprites[0];
        problems[0].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[0].z; ++i)
            for (int j = 0; j < problems[0].height; ++j)
                problems[0].Answer.Add(new ExpectedBlockData(problems[0].Material1, new Vector3(problems[0].StartX, 0, problems[0].StartZ + i * 5)));
        problems[0].Description += "\n\n성의 높이 : 2";
    }
    private void InitProblem2()
    {
        problems[1].Description = "이번엔 성의 뒤 벽을 지어주자!";
        problems[1].Material1 = "콘크리트";
        problems[1].Material2 = null;
        problems[1].StartX = 550;
        problems[1].StartZ = 900;
        problems[1].x = 30;
        problems[1].z = 0;
        problems[1].height = 2;
        problems[1].ProblemImage = sprites[1];
        problems[1].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[1].x; ++i)
            for (int j = 0; j < problems[1].height; ++j)
                problems[1].Answer.Add(new ExpectedBlockData(problems[1].Material1, new Vector3(problems[1].StartX - i * 5, 0, problems[1].StartZ)));
        problems[1].Description += "\n\n성의 높이 : 2";
    }
    private void InitProblem3()
    {
        problems[2].Description = "성의 왼쪽 벽을 지어주자!";
        problems[2].Material1 = "콘크리트";
        problems[2].Material2 = null;
        problems[2].StartX = 400;
        problems[2].StartZ = 900;
        problems[2].x = 0;
        problems[2].z = 24;
        problems[2].height = 2;
        problems[2].ProblemImage = sprites[2];
        problems[2].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[2].z; ++i)
            for (int j = 0; j < problems[2].height; ++j)
                problems[2].Answer.Add(new ExpectedBlockData(problems[2].Material1, new Vector3(problems[2].StartX, 0, problems[2].StartZ - i * 5)));
        problems[2].Description += "\n\n성의 높이 : 2";
    }

    private void InitProblem4()
    {
        problems[3].Description = "가운데 성문 자리는 빼고, 성의 앞 벽을 지어주자!";
        problems[3].Material1 = "콘크리트";
        problems[3].Material2 = null;
        problems[3].StartX = 400;
        problems[3].StartZ = 780;
        problems[3].x = 14;
        problems[3].z = 3;
        problems[3].height = 2;
        problems[3].ProblemImage = sprites[3];
        problems[3].Answer = new List<ExpectedBlockData>();
        for (int i = 0; i < problems[3].x; ++i)
            for (int j = 0; j < problems[3].height; ++j)
                problems[3].Answer.Add(new ExpectedBlockData(problems[3].Material1, new Vector3(problems[3].StartX + i * 5, 0, problems[3].StartZ)));

        for (int i = 0; i < problems[3].x; ++i)
            for (int j = 0; j < problems[3].height; ++j)
                problems[3].Answer.Add(new ExpectedBlockData(problems[3].Material1, new Vector3(3 * 5 + problems[3].x * 5 + problems[3].StartX + i * 5, 0, problems[3].StartZ)));
        // 추가 정보
        problems[3].Description += "\n\n높이 : 2";
    }

    private void InitProblem5()
    {
        problems[4].Description = "성 안에서 자유롭게 원하는 것을 지어보자!";
        problems[4].Material1 = "콘크리트";
        problems[4].Material2 = null;
        problems[4].StartX = 430;
        problems[4].StartZ = 800;
        problems[4].x = 10;
        problems[4].z = 0;
        problems[4].height = 2;
        problems[4].ProblemImage = sprites[4];        
        // 추가 정보
    }

    private void OnDestroy()
    {
        // 에디터 모드 등에선 깔끔하게 정리
        if (Instance == this)
            Instance = null;
    }
}