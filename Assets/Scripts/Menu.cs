using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    public GameObject campaignMenu;
    public GameObject turingFactsMenu;
    public GameObject turingFactPanel;
    public GameObject settingsPanel;
    public GameObject backFromSettings;
    public GameObject soundButton;
    public TMP_Text turingText;

    public GameObject backCampaign;
    public GameObject backFacts;

    public List<Level> levelListSO = new List<Level>();

    public List<GameObject> levelList;
    public List<GameObject> factList;

    private GameObject gameManager;
    private SaveData saveData;
    private List<bool> levels;

    private AudioManager audioMan;
    public void onTutorial()
    {
        audioMan.Play("Menu");
        SceneManager.LoadScene(2);
    }
    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
        audioMan = FindObjectOfType<AudioManager>();
        saveData = gameManager.GetComponent<SaveData>();
        levels = saveData.GetLevels();
    }
    public void onPlay(){
            audioMan.Play("Menu");
        playMenu.SetActive(true);
        mainMenu.SetActive(false);

    }
     public void onBack(){
            audioMan.Play("Menu");
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
        public void onSettings() {
            audioMan.Play("Menu");
        backFromSettings.SetActive(true);
        settingsPanel.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void onSettingsBack()
    {
            audioMan.Play("Menu");
        settingsPanel.SetActive(false);
        mainMenu.SetActive(true);
        backFromSettings.SetActive(false);
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
        public void onFacts(){
            audioMan.Play("Menu");
        levels = saveData.GetLevels();
        backFacts.SetActive(true);
        turingFactsMenu.SetActive(true);
        mainMenu.SetActive(false);


        for (int i = 0; i < 15; i++)
        {
            if (levels[i] == false)
            {
                Destroy(factList[i].GetComponent<HoverText>());
                ColorBlock modifiedColors = factList[i].GetComponentInChildren<Button>().colors;
                modifiedColors.highlightedColor = new Color32(140, 132, 132, 255);
                modifiedColors.selectedColor = new Color32(140, 132, 132, 255);
                modifiedColors.pressedColor = Color.red;
                factList[i].GetComponentInChildren<Button>().colors = modifiedColors;

                foreach (Transform eachChild in factList[i].transform)
                {
                    if (eachChild.name == "Image")
                    {
                        foreach (Transform eachChildDeeper in eachChild)
                        {
                            if (eachChildDeeper.name == "lock")
                            {
                                eachChildDeeper.gameObject.SetActive(true);
                            }
                        }
                    }
                }

            }

        }

    }
    public void onFactsBack()
    {
            audioMan.Play("Menu");
        backFacts.SetActive(false);
        turingFactsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
     public void onFreeMode(){
            audioMan.Play("Menu");
        PlayerPrefs.SetInt("isFreeMode", 1);
        SceneManager.LoadScene(3);
    }
        public void onExit(){
            audioMan.Play("Menu");
        Application.Quit();
    }
    public void onCampaign()
    {
            audioMan.Play("Menu");
        levels = saveData.GetLevels();
        backCampaign.SetActive(true);
        playMenu.SetActive(false);
        campaignMenu.SetActive(true);
       
        for(int i = 0; i < 15; i++)
        {
            if (levels[i] == false)
            {
                //does it need to be active?
                if (checkIfPlayable(i))
                {
                  continue;
                }
                Destroy(levelList[i].GetComponent<HoverText>());
                ColorBlock modifiedColors = levelList[i].GetComponentInChildren<Button>().colors;
                modifiedColors.highlightedColor = new Color32(140, 132, 132, 255);
                modifiedColors.selectedColor = new Color32(140, 132, 132, 255);
                modifiedColors.pressedColor = Color.red;
                levelList[i].GetComponentInChildren<Button>().colors = modifiedColors;

                foreach (Transform eachChild in levelList[i].transform)
                {
                    if (eachChild.name == "Image")
                    {
                        foreach (Transform eachChildDeeper in eachChild)
                        {
                            if (eachChildDeeper.name == "lock")
                            {
                                eachChildDeeper.gameObject.SetActive(true);
                            }
                        }
                    }
                }

            }
            if (levels[i] == true)
            {
                levelList[i].GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Strikethrough;
            }

        }
    }

    private bool checkIfPlayable(int level)
    {
        if (level == 0 || level == 1 || level == 2 || levels[level - 1] == true)
            return true;
        else return false;
    }
    public void onLoadLevel(string level)
    {
            audioMan.Play("Menu");
        if (checkIfPlayable(int.Parse(level) - 1))
        {
            PlayerPrefs.SetInt("isFreeMode", 0);
            PlayerPrefs.SetInt("Level", int.Parse(level));
            SceneManager.LoadScene(1);
        }
    }
    public void onLoadFact(string fact)
    {
            audioMan.Play("Menu");
        if (levels[int.Parse(fact) - 1] != false)
        {
            turingFactPanel.SetActive(true);
            turingText.text = levelListSO[int.Parse(fact) - 1].turingFact;

        }
    }
    public void onExitFact()
    {
            audioMan.Play("Menu");
        turingFactPanel.SetActive(false);
    }
    public void onBackFromCampaign()
    {
            audioMan.Play("Menu");
        backCampaign.SetActive(false);
        campaignMenu.SetActive(false);
        playMenu.SetActive(true);
    }

}
