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
    }

    private void OnDestroy()
    {
        if(!_isFailed)
            GameManager.Instance.WallCleared();
    }
}
