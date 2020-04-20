using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPuller : MonoBehaviour
{
    public List<GameObject> prefabs;
    private bool nextWall = true;
    private void Start()
    {
        //StartCoroutine(GenerateWall(1));
    }

    public void GenerateWall(int waitTime)
    {
        if (!nextWall)
            return;

        //for (int i = 0; i < 60; i++)
        //{
            int nextWallNumber = UnityEngine.Random.Range(0, 4);
            GameObject wall = Instantiate(prefabs[nextWallNumber]);
            wall.transform.position = Vector3.forward * 40; 
            StartCoroutine(PullWall(wall));

        nextWall = false;
            StartCoroutine(WaitForNextWall(0.5f));
            //yield return new WaitForSecondsRealtime(waitTime);
        //}
    }

    private IEnumerator PullWall(GameObject gameObject)
    {
        while(gameObject.transform.position.z > -1)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z-0.4f);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    private IEnumerator WaitForNextWall(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        nextWall = true;
    }

}
