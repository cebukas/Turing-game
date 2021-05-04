using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject taskPanel;
    public GameObject helpPanel;
    public GameObject exitPanel;
    public GameObject pauseMenu;
    public GameObject winPanel;
    public GameObject factPanel;

    private AudioManager audioMan;

    private Level level;
    private void Start()
    {
        level = GameObject.Find("SceneLoader").GetComponent<SceneLoader>().level;
        audioMan = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }

        if (Input.GetKey(KeyCode.LeftControl))  
        {
            if (Input.GetKeyUp(KeyCode.T))  //task shortcut
            {
                if (taskPanel.activeInHierarchy)
                {
                    onTaskClose();
                }
                else
                    onTask();
            }
            if (Input.GetKeyUp(KeyCode.H))  //task shortcut
            {
                if (helpPanel.activeInHierarchy)
                {
                    onHelpClose();
                }
                else
                    onHelp();
            }
        }
    }
    public void onResume()
    {
        audioMan.Play("Menu");
        pauseMenu.SetActive(false);
    }
    public void onExitFromPause()
    {
        audioMan.Play("Menu");
        pauseMenu.SetActive(false);
        exitPanel.SetActive(true);

    }
    public void onExitFromTutorial()
    {
        audioMan.Play("Menu");
        SceneManager.LoadScene(0);

    }
    public void onTask()
    {
        audioMan.Play("Menu");
        taskPanel.SetActive(true);

        var TmpObject = GameObject.Find("Task Description");
        var TMP = TmpObject.GetComponent<TMP_Text>();
        TMP.text = level.levelDescription;

        var TmpObjectTitle = GameObject.Find("Task Title");
        var TMPTitle = TmpObjectTitle.GetComponent<TMP_Text>();
        TMPTitle.text = "Level " + level.level + " Task";
    }
    public void onTaskClose()
    {
        audioMan.Play("Menu");
        taskPanel.SetActive(false);
    }
    public void onHelp()
    {
        audioMan.Play("Menu");
        helpPanel.SetActive(true);

        var TmpObject = GameObject.Find("Help 1 Text");
        var TMP = TmpObject.GetComponent<TMP_Text>();
        TMP.text = level.helpList[0];

        var TmpObject2 = GameObject.Find("Help 2 Text");
        var TMP2 = TmpObject2.GetComponent<TMP_Text>();
        TMP2.text = level.helpList[1];
    }
    public void onExit()
    {
        audioMan.Play("Menu");
        exitPanel.SetActive(true);
    }
    public void onHelpClose()
    {
        audioMan.Play("Menu");
        helpPanel.SetActive(false);
    }
    public void onExitClose()
    {
        audioMan.Play("Menu");
        exitPanel.SetActive(false);
    }

    public void onWin()
    {
        audioMan.Play("Menu");
        winPanel.SetActive(true);
    }
    public void OonWinExit()
    {
        audioMan.Play("Menu");
        SceneManager.LoadScene(0);
    }
    public void onWinStay()
    {
        audioMan.Play("Menu");
        winPanel.SetActive(false);
    }
    public void onWinFact()
    {
        audioMan.Play("Menu");
        factPanel.SetActive(true);
        var TmpObject = GameObject.Find("Turing Description");
        var TMP = TmpObject.GetComponent<TMP_Text>();
        TMP.text = level.turingFact;
    }
    public void onFactClose()
    {
        audioMan.Play("Menu");
        factPanel.SetActive(false);
    }
}