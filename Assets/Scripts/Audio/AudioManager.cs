using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip jumpClip;
    public AudioClip longjumpClip;
    public AudioClip deadClip;

    [Header("Audio Source")]
    public AudioSource bgmMusic;
    public AudioSource fx;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

        bgmMusic.clip = bgmClip;
        PlayMusic();
    }

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;

    }

    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;

    }

    private void OnGameOverEvent()
    {
        fx.clip = deadClip;
        fx.Play();
    }

    /// <summary>
    /// ������Ծ����Ч
    /// </summary>
    /// <param name="type">0С�� 1����</param>
    public void SetJumpClip(int type)
    {
        switch (type)
        {
            case 0:
                fx.clip = jumpClip;
                break;
            case 1:
                fx.clip = longjumpClip;

                break;
        }
    }

    public void PlayJumpFX()
    {
        fx.Play();
    }

    public void PlayMusic()
    {
        if (!bgmMusic.isPlaying)
        {
            bgmMusic.Play();
        }
    }
}
