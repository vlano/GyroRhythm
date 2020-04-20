using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    internal Action collosionDetected;
    internal Action obsticlePassed;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hole")
            obsticlePassed?.Invoke();
        else if (col.gameObject.tag == "Wall")
            collosionDetected?.Invoke();
    }
}
