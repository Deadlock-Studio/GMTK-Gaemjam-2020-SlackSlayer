using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] workerList;
    public PlayerControl player;

    public Text slackTime;
    public Text slackCount;
    public Slider progressBar;
    private bool gameEnded = false;

    [SerializeField]
    private float _workProgress = 0;
    public float clearProgress = 0;
    public int waveNumber = 0;
    public int waveAffect = 0;
    private int _waveCount = 0;

    private float _preGame = 5;

    private float _lastTime = 0;
    private float _currentTime = 0;
    public float interval = 1.5f;

    private float _lastWaveTime = 0;
    public float waveDuration = 10;

    public LevelCompleteUI lvlUI;

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
        if (gameEnded)
            return;

        _currentTime += Time.deltaTime;
        //Announcement
        if (_waveCount < waveNumber)
        {
            if (_waveCount == 0)
            {
                slackTime.text = "Next wave: " + (_preGame - _currentTime).ToString("0");
                slackCount.text = "Slack Count: " + waveAffect;
            }
            else
            {
                slackTime.text = "Next wave: " + (waveDuration - (_currentTime - _lastWaveTime)).ToString("0");
                slackCount.text = "Slack Count: " + waveAffect;
            }
        }
        else
        {
            slackTime.text = "Final wave";
            slackCount.text = "";
        }

        //Wave
        player.SetControl(true);
        if (_waveCount == 0 && _currentTime > _preGame || _currentTime - _lastWaveTime >= waveDuration)
        {
            if (_waveCount < waveNumber)
            {
                Wave(waveAffect);
                IncreaseWave();
                _lastWaveTime = _currentTime;
            }
        }

        if (_currentTime - _lastTime >= interval)
        {
            Produce();
            _lastTime = _currentTime;
        }

        progressBar.value = _workProgress / clearProgress;

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

    private void IncreaseWave()
    {
        int nextWave = waveAffect * 2;
        if (nextWave > workerList.Length)
        {
            waveAffect = workerList.Length;
        }
        else waveAffect = nextWave;
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
        _waveCount++;
    }

    public void Work(float amount)
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
        gameEnded = true;
        lvlUI.Victory();
        Debug.Log("You won!");
    }

    private void Lose()
    {
        gameEnded = true;
        lvlUI.Defeat();
        Debug.Log("You lost!");
    }
}
