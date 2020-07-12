using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] workerList;

    [SerializeField]
    private static float _workProgress = 0;
    public float clearProgress = 0;
    public float waveNumber = 0;

    public float waveDuration = 10;
    private float _preGame = 5;
    private float _lastTime = 0;
    private float _currentTime = 0;
    private float _interval = 1.5f;
    [SerializeField]
    private float _gameTime = 0;

    private void Awake()
    {
        workerList = GameObject.FindGameObjectsWithTag("Worker");
    }

    private void Start()
    {
        _gameTime = _preGame + waveNumber * waveDuration;
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

        //Check win condition
        if (CheckWin())
        {
            Win();
        }
        else
        {
            _gameTime -= Time.deltaTime;
            if (_gameTime <= 0)
            {
                Lose();
            }
        }   
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

    private bool CheckWin()
    {
        if (_workProgress >= clearProgress)
        {
            //Win
            return true;
        }
        else
        {
            //Lose
            return false;
        }
    }

    private void Win()
    {
        Debug.Log("You won!");
    }

    private void Lose()
    {
        Debug.Log("You lost!");
    }
}
