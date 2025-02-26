using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public List<int> scoreList;
    private int score;

    private string dataPath;

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/leaderboard.json";
        scoreList = GetScoreListData();//游戏开始拿到分数记录

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPointEvent;

        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;

        EventHandler.GameOverEvent -= OnGameOverEvent;

    }

    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    private void OnGameOverEvent()
    {
        //TODO:分数添加在list中 排序
        if (!scoreList.Contains(score))
        {
            scoreList.Add(score);
        }

        //foreach (var item in scoreList)
        //{
        //    Debug.Log(item.ToString());
        //}

        scoreList.Sort();
        scoreList.Reverse();


        File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));//传入分数 数据序列化

    }

    /// <summary>
    /// 读取保存数据的记录
    /// </summary>
    /// <returns></returns>
    public List<int> GetScoreListData()
    {
        if (File.Exists(dataPath))
        {
            string jsonData = File.ReadAllText(dataPath);//拿到本地分数
            return JsonConvert.DeserializeObject<List<int>>(jsonData);//放回list中
        }

        return new List<int>();
    }
}
