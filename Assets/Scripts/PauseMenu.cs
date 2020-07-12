using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;
    public static bool isTutorial = false;
    public GameObject pauseMenuUI;
    public GameObject skipLevelButton;
    public GameObject GameUI;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ) {
            if (!isTutorial) {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }
    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        GameUI.SetActive(true);

    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        GameUI.SetActive(false);
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Freeze()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        isTutorial = true;
    }

    public void Unfreeze()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        isTutorial = false;

    }

    public void SkipLevel() {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            skipLevelButton.SetActive(false);
        }

    }
    public void BackToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
