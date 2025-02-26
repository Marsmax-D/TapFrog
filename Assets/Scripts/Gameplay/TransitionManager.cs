using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    private CanvasGroup canvasGroup;
    public float scaler;

    private void Awake()
    {
        

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(Fade(0));
    }

    public void Transition(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return Fade(1);

        yield return SceneManager.LoadSceneAsync(sceneName);//“Ï≤Ωº”‘ÿ

        yield return Fade(0);

    }

    /// <summary>
    /// Ω•±‰
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator Fade(int amount)
    {
        canvasGroup.blocksRaycasts = true;

        while (canvasGroup.alpha != amount)
        {
            switch (amount)
            {
                case 1:
                    canvasGroup.alpha += Time.deltaTime * scaler;
                    break;
                case 0:
                    canvasGroup.alpha -= Time.deltaTime * scaler;

                    break;
            }
            yield return null;
        }

        canvasGroup.blocksRaycasts = false;
        
    }
}
