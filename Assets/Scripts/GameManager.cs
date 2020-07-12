using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] workerList;
    

    private void Awake()
    {
        workerList = GameObject.FindGameObjectsWithTag("Worker");
    }

    private void Start()
    {
        Wave(2);
    }

    // i is the amount of slackers
    private void Wave(int i)
    {
        if (i <= workerList.Length)
        {
            List<int> target = new List<int>();
            while (target.Count < i)
            {
                int rnd = Random.Range(0, workerList.Length);
                if (!target.Contains(rnd))
                {
                    target.Add(rnd);
                }
            }
            
            for (int j = 0; j < target.Count; j++)
            {
                workerList[target[j]].GetComponent<WorkerControl>().Slack();
            }
        }
    }
}
