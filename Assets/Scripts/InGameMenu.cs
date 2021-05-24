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
    public GameObject soundPanel;
    public GameObject soundButton;

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
            soundPanel.SetActive(false);
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
    public void onSettings()
    {
        audioMan.Play("Menu");
        pauseMenu.SetActive(false);
        soundPanel.SetActive(true);
    }

    public void onSound()
    {
        audioMan.Play("Menu");

        var tmpObject = soundButton.GetComponentInChildren<TMP_Text>();

        if (tmpObject.text == "SOUND : ON")
        {
            tmpObject.text = "SOUND : OFF";
            PlayerPrefs.SetInt("sound", -1);
        }

        else
        {
            tmpObject.text = "SOUND : ON";
            PlayerPrefs.SetInt("sound", 1);
        }
    }
    public void onCloseSettings()
    {
        audioMan.Play("Menu");
        pauseMenu.SetActive(true);
        soundPanel.SetActive(false);
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