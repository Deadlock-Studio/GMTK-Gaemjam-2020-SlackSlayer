using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelCompleteUI : MonoBehaviour
{
    public GameObject VictoryUI;

    public GameObject DefeatUI;

    public GameObject GameUI;

    public GameObject skipLevelButton;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    public void Victory() {
        GameUI.SetActive(false);
        VictoryUI.SetActive(true);
        DefeatUI.SetActive(false);
    }

    public void Defeat()
    {
        GameUI.SetActive(false);
        VictoryUI.SetActive(false);
        DefeatUI.SetActive(true);
    }

    public void ReplayLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SkipLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            skipLevelButton.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
