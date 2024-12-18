using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject creditsScreen;
    private bool creditsActive = false;
    public GameObject tutorialScreen;
    private bool tutorialActive = false;

    public GameObject pauseScreen;
    private bool pauseMenu = false;

    public Animator curtains;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (creditsActive)
            {
                Credits();
            }
            else if (tutorialActive)
            {
                Tutorial();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                PauseMenu();
            }
        }
    }

    public void PlayGame()
    {
        curtains.SetBool("CurtainWillClose", true);
        Invoke("LoadScene", 1);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackToMenu()
    {
        curtains.SetBool("CurtainWillClose", true);
        Invoke("LoadMenu", 1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseMenu()
    {
        if (pauseMenu)
        {
            pauseScreen.gameObject.SetActive(false);
            pauseMenu = false;
        }
        else
        {
            pauseScreen.gameObject.SetActive(true);
            pauseMenu = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("I QUIT THE GAME!");
    }

    public void Credits()
    {
        if (creditsActive)
        {
            creditsScreen.gameObject.SetActive(false);
            creditsActive = false;
        }
        else
        {
            creditsScreen.gameObject.SetActive(true);
            creditsActive = true;
        }
    }

    public void Tutorial()
    {
        if (tutorialActive)
        {
            tutorialScreen.gameObject.SetActive(false);
            tutorialActive = false;
        }
        else
        {
            tutorialScreen.gameObject.SetActive(true);
            tutorialActive = true;
        }
    }
}
