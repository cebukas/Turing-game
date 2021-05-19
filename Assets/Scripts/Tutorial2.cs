using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour
{
    public List<TMP_InputField> stateFields = new List<TMP_InputField>();
    private List<StateFunction> stateFunctions = new List<StateFunction>();
    public List<TMP_Text> stateNameFields = new List<TMP_Text>();

    public List<TMP_Text> inputFields = new List<TMP_Text>();
    public List<TMP_Text> outputFields = new List<TMP_Text>();
    private List<bool> results = new List<bool>(new bool[8]);

    private List<char> inputList = new List<char>();

    private DataReader dataReader = new DataReader();
    private TuringMachine turingMachine = new TuringMachine();

    public GameObject inputCell;
    private List<GameObject> inputCellList = new List<GameObject>();

    private string inputString;
    private int currentInputRow = 0;

    public GameObject errorPanel;

    private bool isFastExectionRunning = false;
    private bool wasWinScreenShown = false;
    public TMP_Text popup;
    public TMP_Text popup2;
    private AudioManager audioMan;
    private bool isShown = false;
    private bool isShown2 = false;

    private bool isFirstRun = true;
    private int firstLevelHelps = 1;
    private int finalStateReachedCounter = 0;

    public List<Button> tutorialButtons = new List<Button>();
    public List<GameObject> infoPanels = new List<GameObject>();
    private int counter = 0;
    private int stepPressed = 4;

    public GameObject inputOutputTable;
    public GameObject rightMenu;
    public GameObject buttons;
    public GameObject stateFuncTable;
    public GameObject dataRow;

    public List<Animator> animatorHeaders = new List<Animator>();

    public GameObject nextButton;

    public void onNext()
    {
        audioMan.Play("Menu");
        counter++;
        if (counter == 1)
        {
            infoPanels[0].SetActive(false);
            infoPanels[1].SetActive(true);
            dataRow.SetActive(true);
        }
        if (counter == 2)
        {
            rightMenu.SetActive(true);
            infoPanels[1].SetActive(false);
            infoPanels[2].SetActive(true);
        }
        if (counter == 3)
        {
            inputOutputTable.SetActive(true);
            infoPanels[2].SetActive(false);
            infoPanels[3].SetActive(true);
        }
        if (counter == 4)
        {
            stateFuncTable.SetActive(true);
            infoPanels[3].SetActive(false);
            infoPanels[4].SetActive(true);
            foreach (var i in stateNameFields)
            {
                var a = i.GetComponentInChildren<Animator>();
                if (a != null)
                {
                    a.enabled = false;
                }
            }

            foreach (var i in animatorHeaders)
            {
                i.enabled = false;
            }
        }

        if (counter == 5)
        {
            stateFuncTable.SetActive(true);
            infoPanels[5].SetActive(true);

            foreach (var i in stateNameFields)
            {
                var a = i.GetComponentInChildren<Animator>();
                if (a != null)
                {
                    a.enabled = true;
                }
            }
        }
        if (counter == 6)
        {
            stateFuncTable.SetActive(true);
            infoPanels[6].SetActive(true);
            foreach (var i in stateNameFields)
            {
                var a = i.GetComponentInChildren<Animator>();
                if (a != null)
                {
                    a.enabled = false;
                }
            }
            foreach (var i in animatorHeaders)
            {
                i.enabled = true;
            }
        }
        if (counter == 7)
        {
            stateFuncTable.SetActive(true);
            infoPanels[4].SetActive(false);
            infoPanels[5].SetActive(false);
            infoPanels[6].SetActive(false);
            infoPanels[7].SetActive(true);

            foreach (var i in animatorHeaders)
            {
                i.enabled = false;
            }
        }
        if (counter == 8)
        {
            stateFields[0].text = "q0, 0, R";
            stateFields[1].text = "q0, 0, R";

            infoPanels[7].SetActive(false);
            infoPanels[8].SetActive(true);
            buttons.SetActive(true);
            tutorialButtons[1].interactable = false;
            tutorialButtons[2].interactable = false;
            nextButton.SetActive(false);
        }
        if (counter == 9)
        {
            disableButtons();
            infoPanels[8].SetActive(false);
            infoPanels[9].SetActive(true);
            nextButton.SetActive(true);
        }
        if (counter == 10)
        {
            infoPanels[9].SetActive(false);
            infoPanels[10].SetActive(true);
        }
        if (counter == 11)
        {
            stepPressed = 4;
            stateFields[2].text = "q1, b, L";
            stateFields[3].text = "q1, 0, L";
            nextButton.SetActive(false);
            tutorialButtons[0].interactable = true;

            infoPanels[10].SetActive(false);
            infoPanels[11].SetActive(true);
        }

        if (counter == 12)
        {
            nextButton.SetActive(true);
            tutorialButtons[0].interactable = false;

            infoPanels[11].SetActive(false);
            infoPanels[12].SetActive(true);
        }

        if (counter == 13)
        {
            stepPressed = 3;
            stateFields[5].text = "q2, b, R";
            infoPanels[12].SetActive(false);
            infoPanels[13].SetActive(true);

            nextButton.SetActive(false);
            tutorialButtons[0].interactable = true;

        }
        if (counter == 14)
        {
            tutorialButtons[0].interactable = false;
            infoPanels[13].SetActive(false);
            infoPanels[14].SetActive(true);

            nextButton.SetActive(true);

        }

        if (counter == 15)
        {
            nextButton.SetActive(false);

            inputFields[1].GetComponent<Button>().interactable = true;
            infoPanels[14].SetActive(false);
            infoPanels[15].SetActive(true);

        }

        if (counter == 16)
        {
            tutorialButtons[2].interactable = true;

            infoPanels[15].SetActive(false);
            infoPanels[16].SetActive(true);

        }

        if (counter == 17)
        {
            foreach (var i in inputFields)
            {
                i.GetComponent<Button>().interactable = true;
            }

            infoPanels[16].SetActive(false);
            infoPanels[17].SetActive(true);

        }
    }
    private void disableButtons()
    {
        foreach (var i in tutorialButtons)
        {
            i.interactable = false;
        }
    }
    public void instantiateInput()
    {

        foreach (var i in inputCellList)
        {
            Destroy(i);

        }
        inputCellList.Clear();

        for (int i = 0; i < inputList.Count; i++)
        {
            inputCell.GetComponentInChildren<TMP_Text>().text = inputList[i].ToString();
            var cell = Instantiate(inputCell, new Vector3(0, 0, 0), Quaternion.identity);
            cell.transform.SetParent(GameObject.Find("input").transform);
            cell.name = "cell (" + i + ")";

            inputCellList.Add(cell);
        }
        turingMachine.Restart();

    }
    public void Start()
    {
        foreach (var i in stateFields)
        {
            i.interactable = false;
        }
        foreach (var i in inputFields)
        {
            i.GetComponent<Button>().interactable = false;
        }
        // this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1600, 900);
        audioMan = FindObjectOfType<AudioManager>();

        for (int i = 0; i < inputFields.Count; i++)
        {
            inputFields[i].text = "1111";
        }
        onInputStringChange("0");
        HighlightCell(turingMachine.getPointer());

        var TmpObject = GameObject.Find("Task Description");
        var TMP = TmpObject.GetComponent<TMP_Text>();
        TMP.text = "Create a set of state functions that would replace all 1's and 0's in the data row to only 0's. The same functions have to work with every given input row!";

        var TmpObjectTitle = GameObject.Find("Task Title");
        var TMPTitle = TmpObjectTitle.GetComponent<TMP_Text>();
        TMPTitle.text = "Level " + 0 + " Task";

        inputOutputTable.SetActive(false);
        rightMenu.SetActive(false);
        buttons.SetActive(false);
        stateFuncTable.SetActive(false);
        dataRow.SetActive(false);

    }

    private string formatOutput(string output)
    {
        int pointer = turingMachine.getPointer();
        if (pointer < 0)
        {
            pointer = 0;
        }
        output = output.Remove(0, pointer);
        output = output.Substring(0, output.IndexOf("b"));
        if (output == "")
        {
            output = "-";
        }
        return output;
    }
    public void onOutputChange()
    {
        outputFields[currentInputRow].text = formatOutput(new string(turingMachine.GetOutputList().ToArray()));
    }
    public void onInputStringChange(string inputNumber)
    {
        currentInputRow = int.Parse(inputNumber);
        inputString = "bb" + inputFields[currentInputRow].text + "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        inputList.Clear();
        inputList.AddRange(inputString);
        instantiateInput();
        highlightState(0);

        if (isFastExectionRunning)
        {
            StopCoroutine("allStepsWithDelay");
            isFastExectionRunning = false;
        }
        HighlightCell(turingMachine.getPointer());
        finalStateReachedCounter = 0;

        if (counter == 15)
        {
            onNext();
        }

    }
    public void highlightState(int state)
    {
        if (state != -1 && state <= 16)
        {
            foreach (var i in stateNameFields)
            {
                i.fontStyle = FontStyles.Normal;
                i.fontSize = 24;
            }
            stateNameFields[state].fontStyle = FontStyles.Bold;
            stateNameFields[state].fontSize = stateNameFields[state].fontSize + 5;
        }
    }
    public void checkOutput()
    {

        string output = formatOutput(new string(turingMachine.GetOutputList().ToArray()));
        if (output == "0000")
        {
            results[currentInputRow] = true;
            inputFields[currentInputRow].fontStyle = FontStyles.Strikethrough;

            ColorBlock modifiedColors = inputFields[currentInputRow].GetComponent<Button>().colors;
            modifiedColors.normalColor = Color.white;
            inputFields[currentInputRow].GetComponent<Button>().colors = modifiedColors;

            // check if all rows are completed
            if (!results.Contains(false) && !wasWinScreenShown)
            {
                audioMan.Play("GG");
                wasWinScreenShown = true;
                GetComponent<InGameMenu>().onWin();
            }
            if (firstLevelHelps > 0)
            {
                ShowPopup2("Good job on getting the right answer for your first input row!");
                firstLevelHelps--;
            }
        }
    }

    private void ShowPopup(string msg)
    {
        if (!isShown)
        {
            popup.text = msg;
            popup.gameObject.SetActive(true);
            isShown = true;
            Invoke("disablePopup", 2.0f);
        }
    }
    private void disablePopup()
    {
        isShown = false;
        popup.gameObject.SetActive(false);
    }
    private void ShowPopup2(string msg)
    {
        if (!isShown2)
        {
            popup2.text = msg;
            popup2.gameObject.SetActive(true);
            isShown2 = true;
            Invoke("disablePopup2", 6f);
        }
    }
    private void disablePopup2()
    {
        isShown2 = false;
        popup2.gameObject.SetActive(false);
    }

    public void onSaveExit()
    {
        audioMan.Play("Menu");
        SceneManager.LoadScene(0);
    }
    public void onExitWithoutSave()
    {
        audioMan.Play("Menu");
        SceneManager.LoadScene(0);
    }

    private bool checkDataErrors(List<int> errors)
    {
        if (errors.Count != 0)
        {
            string errorList = "";
            foreach (var i in errors)
            {

                string gameObjectName = stateFields[i].name.ToString();
                if (Char.IsDigit(gameObjectName[1]))
                {
                    errorList += "q" + ((gameObjectName[0] - '0').ToString() + (gameObjectName[1] - '0').ToString()).ToString() + "-" + gameObjectName[9];
                }
                else
                {
                    errorList += "q" + (gameObjectName[0] - '0').ToString() + "-" + gameObjectName[8];

                }

                errorList += "   ";
            }
            ShowError("Incorrect function syntax in: " + errorList.ToString() + "\n Please follow the function format 'state (q0, q1, q2...), value (0, 1, b), action (L, R, N)'");
            return false;
        }
        else
            return true;
    }

    private bool CompareStateFunctionLists(List<StateFunction> last, List<StateFunction> current) // returns true if identical, false if changed
    {
        if (last.Count != current.Count)
            return false;
        else
        {
            for (int i = 0; i < last.Count; i++)
            {
                if (last[i].getAction() != current[i].getAction() ||
                  last[i].getOutputState() != current[i].getOutputState() ||
                  last[i].getOutputValue() != current[i].getOutputValue() ||
                  last[i].getTriggerState() != current[i].getTriggerState() ||
                  last[i].getTriggerValue() != current[i].getTriggerValue())
                {
                    return false;
                }
            }
            return true;
        }
    }
    private void StateFunctionsChanged()
    {
        if (!isFirstRun)
        {
            if (results.Contains(true))
                ShowPopup2("Passed input rows were cleared because state functions have changed!");
            for (int i = 0; i < results.Count; i++)
            {
                results[i] = false;

                inputFields[i].fontStyle = FontStyles.Normal;

                ColorBlock modifiedColors = inputFields[i].GetComponent<Button>().colors;
                modifiedColors.normalColor = new Color32(255, 255, 255, 128);
                inputFields[i].GetComponent<Button>().colors = modifiedColors;
            }
        }
        isFirstRun = false;

    }
    public void onStep()
    {
        audioMan.Play("Step");

        stepPressed--;
        var last = new List<StateFunction>();
        foreach (var i in dataReader.GetStateFunctions())
        {
            last.Add(i);
        }

        dataReader.SetStateFields(stateFields);
        var errors = dataReader.ReadStateFields();

        if (dataReader.GetStateFunctions().Count == 0)
        {
            ShowError("The state function table is empty!");
        }

        bool isIdentical = CompareStateFunctionLists(last, dataReader.GetStateFunctions());

        if (!isIdentical)
            StateFunctionsChanged();

        if (checkDataErrors(errors))
        {
            stateFunctions = dataReader.GetStateFunctions();

            turingMachine.SetOutputList(inputList);
            turingMachine.SetStateFunctions(stateFunctions);

            if (isFastExectionRunning)
            {
                StopCoroutine("allStepsWithDelay");
                isFastExectionRunning = false;
            }

            int machineResult = turingMachine.oneStep();

            if (machineResult == -1)
            {
                ShowError("You are moving the pointer out of the game bounds." +
                  " Execution halted.");
                return;
            }
            List<char> outputList = turingMachine.GetOutputList();
            for (int i = 0; i < inputCellList.Count; i++)
            {
                inputCellList[i].GetComponentInChildren<TMP_Text>().text = outputList[i].ToString();
            }
            onOutputChange();
            HighlightCell(turingMachine.getPointer());
            highlightState(turingMachine.getState());

            if (machineResult == 1) //final state reached
            {
                finalStateReachedCounter++;

                audioMan.Stop("Step");
                audioMan.Play("Final");
                ShowPopup("Final state reached!");
                checkOutput();
            }

            if (finalStateReachedCounter > 3)
            {
                ShowPopup2("Consider resetting the input by clicking on one of the input rows.");
                finalStateReachedCounter = 0;
            }

        }

        if (stepPressed == 0)
        {
            onNext();
        }

    }
    public void HighlightCell(int cell)
    {
        foreach (var i in inputCellList)
        {
            i.GetComponentInChildren<Image>().color = Color.black;
        }
        inputCellList[cell].GetComponentInChildren<Image>().color = new Color32(100, 100, 100, 255);
    }

    public void ShowError(string error)
    {

        audioMan.Stop("Menu");
        audioMan.Play("Error");
        errorPanel.SetActive(true);
        var TmpObject = GameObject.Find("Error Message");
        var TMP = TmpObject.GetComponent<TMP_Text>();
        TMP.text = error;
    }
    public void onExitError()
    {
        audioMan.Play("Menu");
        errorPanel.SetActive(false);
    }
    public void onFastSteps()
    {

        if (!isFastExectionRunning)
        {
            var last = new List<StateFunction>();
            foreach (var i in dataReader.GetStateFunctions())
            {
                last.Add(i);
            }
            dataReader.SetStateFields(stateFields);
            var errors = dataReader.ReadStateFields();

            if (dataReader.GetStateFunctions().Count == 0)
            {
                ShowError("The state function table is empty!");
            }

            bool isIdentical = CompareStateFunctionLists(last, dataReader.GetStateFunctions());

            if (!isIdentical)
                StateFunctionsChanged();

            if (checkDataErrors(errors))
            {
                stateFunctions = dataReader.GetStateFunctions();

                turingMachine.SetOutputList(inputList);
                turingMachine.SetStateFunctions(stateFunctions);
                StartCoroutine("allStepsWithDelay");
            }
        }
        else
        {
            isFastExectionRunning = false;
            audioMan.Play("Menu");
            StopCoroutine("allStepsWithDelay");
        }
    }
    private void Update()
    {
        if (isFastExectionRunning)
        {
            foreach (var i in stateFields)
            {
                i.readOnly = true;
            }
        }
        else
        {
            foreach (var i in stateFields)
            {
                i.readOnly = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyUp(KeyCode.Alpha1)) //step shortcut
            {
                onStep();
            }
            if (Input.GetKeyUp(KeyCode.Alpha2)) //fast shortcut
            {
                onFastSteps();
            }
            if (Input.GetKeyUp(KeyCode.Alpha3)) //skip shortcut
            {
                onSkipSteps();
            }
        }
    }
    public IEnumerator allStepsWithDelay()
    {
        isFastExectionRunning = true;
        int returnValue = 0;
        while (returnValue != 1)
        {
            returnValue = turingMachine.oneStep();
            audioMan.Play("Step");
            if (returnValue == -1)
            {
                ShowError("You are moving the pointer out of the game bounds." +
                  " Execution halted.");
                isFastExectionRunning = false;
                break;
            }
            List<char> outputList = turingMachine.GetOutputList();
            for (int i = 0; i < inputCellList.Count; i++)
            {
                inputCellList[i].GetComponentInChildren<TMP_Text>().text = outputList[i].ToString();
            }
            HighlightCell(turingMachine.getPointer());
            onOutputChange();
            if (returnValue == 1) //final state reached
            {
                audioMan.Stop("Menu");
                audioMan.Play("Final");
                ShowPopup("Final state reached!");
                checkOutput();
                finalStateReachedCounter++;
                if (finalStateReachedCounter > 3)
                {
                    ShowPopup2("Consider resetting the input by clicking on one of the input rows.");
                    finalStateReachedCounter = 0;
                }
            }
            yield
            return new WaitForSeconds(0.8f);
        }
        isFastExectionRunning = false;

    }

    public void onSkipSteps()
    {
        audioMan.Play("Step");

        var last = new List<StateFunction>();
        foreach (var i in dataReader.GetStateFunctions())
        {
            last.Add(i);
        }

        dataReader.SetStateFields(stateFields);
        var errors = dataReader.ReadStateFields();

        if (dataReader.GetStateFunctions().Count == 0)
        {
            ShowError("The state function table is empty!");
        }

        bool isIdentical = CompareStateFunctionLists(last, dataReader.GetStateFunctions());

        if (!isIdentical)
            StateFunctionsChanged();

        if (checkDataErrors(errors))
        {
            stateFunctions = dataReader.GetStateFunctions();

            turingMachine.SetOutputList(inputList);
            turingMachine.SetStateFunctions(stateFunctions);
            if (isFastExectionRunning)
            {
                StopCoroutine("allStepsWithDelay");
                isFastExectionRunning = false;
            }
            int machineResult = turingMachine.allSteps();
            audioMan.Play("Final");
            if (machineResult == -1)
            {
                audioMan.Stop("Final");
                ShowError("You are moving the pointer out of the game bounds." +
                  " Execution halted.");
            }
            if (machineResult == -2)
            {
                audioMan.Stop("Final");
                ShowError("Turing Machine didn't reach a final state after 1000 steps. Are you creating an infinite loop?");
            }

            ShowPopup("Final state reached!");
            finalStateReachedCounter++;
            if (finalStateReachedCounter > 3)
            {
                ShowPopup2("Consider resetting the input by clicking on one of the input rows.");
                finalStateReachedCounter = 0;
            }
            List<char> outputList = turingMachine.GetOutputList();
            for (int i = 0; i < inputCellList.Count; i++)
            {
                inputCellList[i].GetComponentInChildren<TMP_Text>().text = outputList[i].ToString();
            }
            onOutputChange();
            checkOutput();
            HighlightCell(turingMachine.getPointer());
        }
        if (counter == 16)
            onNext();
    }
}