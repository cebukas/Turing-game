using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameMan : MonoBehaviour
{
    public TMP_InputField binaryInput;
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
    private Level level;
    private int currentInputRow = 0;

    public GameObject errorPanel;

    public SaveData saveData;

    private bool isFastExectionRunning = false;
    private bool wasWinScreenShown = false;
    public TMP_Text popup;
    public TMP_Text popup2;
    private AudioManager audioMan;
    private bool isShown = false;
    private bool isShown2 = false;

    public int isFreeMode = 0;

    private bool isFirstRun = true;
    private int firstLevelHelps = 1;
    private int finalStateReachedCounter = 0;

    List<StateFunction> lastSavedFunctions = new List<StateFunction>();
    public void InstantiateInput()
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
       // this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1600, 900);
        audioMan = FindObjectOfType<AudioManager>();
        isFreeMode = PlayerPrefs.GetInt("isFreeMode");
        level = GameObject.Find("SceneLoader").GetComponent<SceneLoader>().level;
        if (isFreeMode == 0)
        {
            for (int i = 0; i < inputFields.Count; i++)
            {
                inputFields[i].text = level.exampleInputList[i];
            }
            onInputStringChange("0");
            FillSavedStateFunctions(saveData.getStateFunctions(level.level));

            lastSavedFunctions = saveData.getStateFunctions(level.level);
            HighlightCell(turingMachine.getPointer());


            var TmpObject = GameObject.Find("Task Description");
            var TMP = TmpObject.GetComponent<TMP_Text>();
            TMP.text = level.levelDescription;

            var TmpObjectTitle = GameObject.Find("Task Title");
            var TMPTitle = TmpObjectTitle.GetComponent<TMP_Text>();
            TMPTitle.text = "Level " + level.level + " Task";
        }
        else
        {
            lastSavedFunctions = saveData.getStateFunctions(16);
            FillSavedStateFunctions(saveData.getStateFunctions(16));
            onStartFreeMode();
        }
    }

    private void FillSavedStateFunctions(List<StateFunction> sfList)
    {
        for (int i = 0; i < sfList.Count; i++)
        {
            for (int j = 0; j < stateFields.Count; j++)
            {
                string gameObjectName = stateFields[j].name.ToString();

                if (Char.IsDigit(gameObjectName[1]))
                {

                    if (sfList[i].getTriggerValue() == gameObjectName[9] && sfList[i].getTriggerState() == int.Parse((gameObjectName[0] - '0').ToString() + (gameObjectName[1] - '0').ToString()))
                    {
                        stateFields[j].text = "q" + sfList[i].getOutputState().ToString() + ", " + sfList[i].getOutputValue().ToString() + ", " + sfList[i].getAction().ToString();
                    }
                }
                else
                {
                    if (sfList[i].getTriggerValue() == gameObjectName[8] && sfList[i].getTriggerState() == (gameObjectName[0] - '0'))
                    {
                        stateFields[j].text = "q" + sfList[i].getOutputState().ToString() + ", " + sfList[i].getOutputValue().ToString() + ", " + sfList[i].getAction().ToString();
                    }
                }

            }

        }

    }
    private string FormatOutput(string output)
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
        outputFields[currentInputRow].text = FormatOutput(new string(turingMachine.GetOutputList().ToArray()));
    }
    public void onInputStringChange(string inputNumber)
    {
        currentInputRow = int.Parse(inputNumber);
        inputString = "bb" + inputFields[currentInputRow].text + "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        inputList.Clear();
        inputList.AddRange(inputString);
        InstantiateInput();
        HighlightState(0);

        if (isFastExectionRunning)
        {
            StopCoroutine("allStepsWithDelay");
            isFastExectionRunning = false;
        }
        HighlightCell(turingMachine.getPointer());
        finalStateReachedCounter = 0;

    }
    public void HighlightState(int state)
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
    public void CheckOutput()
    {
        string output = FormatOutput(new string(turingMachine.GetOutputList().ToArray()));
        if (output == level.exampleOutputList[currentInputRow])
        {
            audioMan.Stop("Menu");
            audioMan.Stop("Final");
            audioMan.Stop("Step");
            audioMan.Play("Row");
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
                saveData.PassLevel(level.level);
                GetComponent<InGameMenu>().onWin();
            }
            if (firstLevelHelps > 0)
            {
                ShowPopup2("Good job on getting the right answer for your first input row. Now select a different input!");
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
        dataReader.SetStateFields(stateFields);
        dataReader.ReadStateFields();
        if (isFreeMode == 1)
            saveData.setStateFuctions(dataReader.GetStateFunctions(), 16);
        else
            saveData.setStateFuctions(dataReader.GetStateFunctions(), level.level);
        SceneManager.LoadScene(0);
    }
    public void onExitWithoutSave()
    {
        if (isFreeMode == 0)
        {
            saveData.setStateFuctions(lastSavedFunctions, level.level);
        }
        else
            saveData.setStateFuctions(lastSavedFunctions, 16);
        audioMan.Play("Menu");
        SceneManager.LoadScene(0);
    }

    private bool CheckDataErrors(List<int> errors)
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
                if(last[i].getAction() != current[i].getAction() ||
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
            if(results.Contains(true))
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


        if (!isIdentical && isFreeMode == 0)
            StateFunctionsChanged();

        if (CheckDataErrors(errors))
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
            if (isFreeMode == 0)
                onOutputChange();
            HighlightCell(turingMachine.getPointer());
            HighlightState(turingMachine.getState());

            if (isFreeMode == 1)
                saveData.setStateFuctions(dataReader.GetStateFunctions(), 16);
            else
                saveData.setStateFuctions(dataReader.GetStateFunctions(), level.level);
            if (machineResult == 1) //final state reached
            {
                finalStateReachedCounter++;

                audioMan.Stop("Step");
                audioMan.Play("Final");
                ShowPopup("Final state reached!");
                CheckOutput();
            }

            if(finalStateReachedCounter > 3)
            {
                ShowPopup2("Consider resetting the input by clicking on one of the input rows.");
                finalStateReachedCounter = 0;
            }

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
             
            if (!isIdentical && isFreeMode == 0)
                StateFunctionsChanged();

            if (CheckDataErrors(errors))
            {
                stateFunctions = dataReader.GetStateFunctions();

                turingMachine.SetOutputList(inputList);
                turingMachine.SetStateFunctions(stateFunctions);
                if (isFreeMode == 1)
                    saveData.setStateFuctions(dataReader.GetStateFunctions(), 16);
                else
                    saveData.setStateFuctions(dataReader.GetStateFunctions(), level.level);
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
                if (Input.GetKeyUp(KeyCode.Alpha1))  //step shortcut
                {
                    onStep();
                }
                if (Input.GetKeyUp(KeyCode.Alpha2))  //fast shortcut
                {
                    onFastSteps();
                }
                if (Input.GetKeyUp(KeyCode.Alpha3))  //skip shortcut
                {
                    onSkipSteps();
                }
            if (Input.GetKeyUp(KeyCode.Alpha4))  //skip shortcut
            {

                for(int i = 0; i< 8; i++)
                {
                    onInputStringChange(i.ToString());
                    onSkipSteps();
                }

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
            if (isFreeMode == 0)
                onOutputChange();
            if (returnValue == 1) //final state reached
            {
                audioMan.Stop("Menu");
                audioMan.Play("Final");
                ShowPopup("Final state reached!");
                CheckOutput();
                finalStateReachedCounter++;
                if (finalStateReachedCounter > 3)
                {
                    ShowPopup2("Consider resetting the input by clicking on one of the input rows.");
                    finalStateReachedCounter = 0;
                }
            }
            yield return new WaitForSeconds(0.8f);
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

        if (!isIdentical && isFreeMode == 0)
            StateFunctionsChanged();

        if (CheckDataErrors(errors))
        {
            stateFunctions = dataReader.GetStateFunctions();

            turingMachine.SetOutputList(inputList);
            turingMachine.SetStateFunctions(stateFunctions);
            if (isFastExectionRunning)
            {
                StopCoroutine("allStepsWithDelay");
                isFastExectionRunning = false;
            }
            if (isFreeMode == 1)
                saveData.setStateFuctions(dataReader.GetStateFunctions(), 16);
            else
                saveData.setStateFuctions(dataReader.GetStateFunctions(), level.level);
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
            if (isFreeMode == 0)
                onOutputChange();
            CheckOutput();
            HighlightCell(turingMachine.getPointer());
        }

    }
    public void onRestart()
    {
        inputString = binaryInput.text;
        inputString = dataReader.ReadBinaryInput(inputString);
            if (inputString == "")
            {
                ShowError("Non binary input!");
            }
        inputString = "bb" + inputString + "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        inputList.Clear();
        inputList.AddRange(inputString);
        InstantiateInput();
        HighlightCell(2);
        HighlightState(0);
    }
    private void onStartFreeMode()
    {
        inputString = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        inputList.Clear();
        inputList.AddRange(inputString);
        InstantiateInput();
        HighlightCell(2);
        HighlightState(0);
    }
}