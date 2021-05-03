using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private int counter = 0;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public GameObject panel5;
    public GameObject panel6;
    public GameObject panel7;
    public GameObject panel8;
    public GameObject panel9;
    public GameObject panel10;
    public GameObject panel11;
    public GameObject panel12;
    public GameObject panel13;
    public GameObject panel14;
    public GameObject panel15;
    public GameObject panel16;
    public GameObject panel17;
    public GameObject panel18;
    public GameObject panel19;
    public GameObject panel20;
    public GameObject panel21;
    public GameObject panel22;
    public GameObject panel23;
    public GameObject panel24;
    public GameObject panel25;
    public GameObject panel26;
    public GameObject panel27;
    public GameObject panel28;

    void Update()
    {
        if (counter == 1)
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            panel3.SetActive(true);
        }
        if (counter == 2)
        {
            panel2.SetActive(false);
            panel4.SetActive(true);
            panel5.SetActive(true);

        }
        if (counter == 3)
        {
            panel6.SetActive(false);
            panel7.SetActive(true);
            panel8.SetActive(true);
        }
        if (counter == 4)
        {
            panel9.SetActive(true);
            panel10.SetActive(true);
            panel8.SetActive(true);
        }
        if (counter == 5)
        {
            panel11.SetActive(true);
        }
        if (counter == 6)
        {
            panel12.SetActive(true);
        }
        if (counter == 7)
        {
            panel13.SetActive(true);
            panel12.SetActive(false);
            panel11.SetActive(false);
            panel10.SetActive(false);
            panel7.SetActive(false);
            panel5.SetActive(false);
        }
        if (counter == 8)
        {
            panel14.SetActive(true);
            panel15.SetActive(true);
            panel16.SetActive(true);
            panel13.SetActive(false);
            panel3.SetActive(false);
            panel5.SetActive(false);
        }
        if (counter == 9)
        {
            panel17.SetActive(true);
            panel14.SetActive(false);
            panel19.SetActive(true);
        }
        if (counter == 10)
        {
            panel17.SetActive(false);
            panel18.SetActive(true);
            panel24.SetActive(true);
            panel25.SetActive(true);
            panel26.SetActive(true);
        }
        if (counter == 11)
        {
            panel27.SetActive(true);
            panel20.SetActive(true);

            panel24.SetActive(false);
        }

        if (counter == 12)
        {
            panel20.SetActive(false);
            panel21.SetActive(true);
        }
        if (counter == 13)
        {
            panel20.SetActive(false);
            panel21.SetActive(false);
            panel22.SetActive(true);
        }
        if (counter == 14)
        {
            panel22.SetActive(false);
            panel23.SetActive(true);
        }
        if (counter == 15)
        {
            panel28.SetActive(true);
        }
        if (counter == 16)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void onNext()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        counter++;
    }
}