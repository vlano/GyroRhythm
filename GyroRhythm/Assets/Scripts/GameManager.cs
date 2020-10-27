using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudioReactor _audioReactor;
    [SerializeField]
    private WallPuller _wallPuller;
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private TMP_Text _scoreDisplay;
    [SerializeField]
    private TMP_Text _multiplierDisplay;

    private GameManager _instance;
    public static GameManager Instance { get; private set; }

    public int wallScore;


    private long _currentScore;
    public long currentScore
    {
        get { return _currentScore; }
        set
        {
            _currentScore = value;
            _scoreDisplay.text = _currentScore.ToString();
            StartCoroutine(AnimateText(_scoreDisplay));
        }
    }

    private int _comboMultiplier;
    public int comboMultiplier
    {
        get { return _comboMultiplier; }
        set
        {
            _comboMultiplier = value;
            _multiplierDisplay.text = 'x'+_comboMultiplier.ToString();
        }
    }

    public Action<long,int,int> success;
    public Action fail;

    [SerializeField]
    private PlayerController _player;

    private bool _isPaused;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _player.collosionDetected += WallFailed;
        _player.obsticlePassed += WallCleared;
        _audioReactor.StartMusic();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }
    }

    public void PauseResume()
    {
        if (!_isPaused)
        {
            if (!_audioReactor.IsDelayedAudioStarted)
                return;

            Time.timeScale = 0;
            _menu.SetActive(true);
            _isPaused = true;
            _audioReactor.PauseAudio();
        }
        else
        {
            _menu.SetActive(false);
            Time.timeScale = 1;
            _audioReactor.ResumeAudio();
            _isPaused = false;
        }

    }

    public void WallCleared()
    {
        int addedScore = wallScore * comboMultiplier;
        currentScore += addedScore;
        comboMultiplier++;
        success?.Invoke(_currentScore, wallScore ,comboMultiplier);
    }

    public void WallFailed()
    {
        comboMultiplier = 1;
        fail?.Invoke();
    }

    private IEnumerator AnimateText(TMP_Text text)
    {
        for (int i = 0; i < 10; i++)
        {
            text.transform.localScale = new Vector2(text.transform.localScale.x, text.transform.localScale.y)*1.05f;
            yield return new WaitForEndOfFrame();
        }
        while(text.transform.localScale.x > 1)
        {
            text.transform.localScale = new Vector2(text.transform.localScale.x, text.transform.localScale.y) * 0.99f;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Restart()
    {
        _audioReactor.StartMusic();
        _wallPuller.StopAllCoroutines();
        foreach (Transform tr in _wallPuller.transform)
        {
            Destroy(tr.gameObject);
        }
        PauseResume();
        currentScore = 0;
        comboMultiplier = 0;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
