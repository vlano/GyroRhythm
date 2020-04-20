using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _player.collosionDetected += WallFailed;
        _player.obsticlePassed += WallCleared;
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
}
