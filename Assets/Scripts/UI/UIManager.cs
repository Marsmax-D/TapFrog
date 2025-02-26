using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Text scoreText;
    public GameObject gameoverObject;

    public GameObject leaderboardPanel;

    private void OnEnable()
    {
        Time.timeScale = 1;
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;

    }

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GameOverEvent -= OnGameOverEvent;

    }

    

    private void Start()
    {
        scoreText.text = "00";
    }


    private void OnGetPointEvent(int point)
    {
        scoreText.text = point.ToString();
    }

    private void OnGameOverEvent()
    {
        gameoverObject.SetActive(true);

        if (gameoverObject.activeInHierarchy)
        {
            Time.timeScale = 0;//暂停游戏
        }
    }


    #region 按钮添加方法

    public void RestartGame()
    {
        //SceneManager.LoadScene("Gameplay");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        gameoverObject.SetActive(false);

        TransitionManager.instance.Transition("Gameplay");
    }


    public void BackToMenu()
    {
        gameoverObject.SetActive(false);
        TransitionManager.instance.Transition("Start");
    }

    public void OpenLeaderBoard()
    {
        leaderboardPanel.SetActive(true);
    }

    #endregion
}
