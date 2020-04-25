using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioVisualizer : MonoBehaviour
{
    private enum ScreenSide
    {
        Left,
        Right
    }
    private GameObject[] _objects = new GameObject[64];
    [SerializeField]
    private ScreenSide screenSide;
    [SerializeField]
    private float _heightMultiplier;

    public GameObject prefab;
    public AudioReactor audioReactor;

    void Start()
    {
        audioReactor.sampleReceived += SetSize;

        Vector3 edgePosition = Vector3.zero;
        if(screenSide == ScreenSide.Left)
            edgePosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height/2, 10));
        else
            edgePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 10));
        
        transform.position = edgePosition;

        for (int i = 0; i < _objects.Length; i++)
        {
            GameObject instance = Instantiate(prefab,transform);
            instance.transform.localPosition = new Vector3(0,0,i);
            _objects[i] = instance;
        }
    }

    internal void SetSize(int index, float height)
    {
        float multiplier = _heightMultiplier;
        if (index > 0)
        { multiplier *= index;
                }

        _objects[index].transform.localScale = new Vector3(1, height * multiplier, 1);
       
    }
}
