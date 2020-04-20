using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private bool _isFailed;
    void OnTriggerEnter(Collider col)
    {
        GameManager.Instance.WallFailed();
        _isFailed = true;
        SetColor(Color.red);
    }

    private void SetColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("Color_19AB8876", color);
    }

    private void OnDestroy()
    {
        if(!_isFailed)
            GameManager.Instance.WallCleared();
    }
}
