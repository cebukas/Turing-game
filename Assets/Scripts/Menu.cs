using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    public void onPlay(){
        playMenu.SetActive(true);
        mainMenu.SetActive(false);

    }
     public void onBack(){
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
        public void onSettings(){
    }
        public void onFacts(){
    }
     public void onFreeMode(){
        SceneManager.LoadScene(1);
    }
        public void onExit(){
            Application.Quit();
             Debug.Log("Game is exiting");
    }
}
