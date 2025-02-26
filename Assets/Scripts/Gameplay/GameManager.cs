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
        scoreList = GetScoreListData();//��Ϸ��ʼ�õ�������¼

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
        //TODO:���������list�� ����
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


        File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));//������� �������л�

    }

    /// <summary>
    /// ��ȡ�������ݵļ�¼
    /// </summary>
    /// <returns></returns>
    public List<int> GetScoreListData()
    {
        if (File.Exists(dataPath))
        {
            string jsonData = File.ReadAllText(dataPath);//�õ����ط���
            return JsonConvert.DeserializeObject<List<int>>(jsonData);//�Ż�list��
        }

        return new List<int>();
    }
}
