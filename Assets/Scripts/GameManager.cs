using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] workerList;

    private static float _workProgress = 0;

    private float _lastTime = 0;
    private float _currentTime = 0;
    private float _interval = 1.5f;

    private void Awake()
    {
        workerList = GameObject.FindGameObjectsWithTag("Worker");
    }

    private void Start()
    {
        Wave(1);
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime - _lastTime >= _interval)
        {
            Produce();
            _lastTime = _currentTime;
        }
        //TODO Progress bar
        Debug.Log(_workProgress);
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

    public static void Work(float amount)
    {
        _workProgress += amount;
    }

    private void Produce()
    {
        for (int i = 0; i < workerList.Length; i++)
        {
            if (!workerList[i].GetComponent<WorkerControl>().IsSlacking())
            {
                Work(1);
            }
        }
    }
}
