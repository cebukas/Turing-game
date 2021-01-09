using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TMP_InputField binaryInput;
    public List<TMP_InputField> stateFields = new List<TMP_InputField>();
    public List<StateFunction> stateFunctions = new List<StateFunction>();

    private List<char> inputList = new List<char>();

    private DataReader dataReader = new DataReader();
    private TuringMachine turingMachine = new TuringMachine();

    public GameObject inputCell;
    private List<GameObject> inputCellList = new List<GameObject>();

    private string inputString;

    private bool isModified = true;


    public void instantiateInput(){
        foreach(var i in inputCellList){
            Destroy(i);
        }
        inputCellList.Clear();

        for (int i = 0; i < inputList.Count; i++){
            inputCell.GetComponentInChildren<TMP_Text>().text = inputList[i].ToString();
            var cell = Instantiate(inputCell, new Vector3(0, 0, 0), Quaternion.identity);
            cell.transform.SetParent(GameObject.Find("input").transform);
            cell.name = "cell (" + i + ")";

            inputCellList.Add(cell);
        }
        isModified = true;
        turingMachine.Restart();

    }
    public void Awake(){
        inputString = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        inputList.AddRange(inputString);
        instantiateInput();
    }
    public void onStep(){
        if (isModified){
            dataReader.SetStateFields(stateFields);
            dataReader.ReadStateFields();
            stateFunctions = dataReader.GetStateFunctions();

            turingMachine.SetOutputList(inputList);
            turingMachine.SetStateFunctions(stateFunctions);
            isModified = false;
        }
       
        turingMachine.oneStep();
        List<char> outputList = turingMachine.GetOutputList();
        for(int i = 0; i < inputCellList.Count; i++){
            inputCellList[i].GetComponentInChildren<TMP_Text>().text = outputList[i].ToString();
        }

    }

     public void onFastSteps(){

         if (isModified){
            dataReader.SetStateFields(stateFields);
            dataReader.ReadStateFields();
            stateFunctions = dataReader.GetStateFunctions();

            turingMachine.SetOutputList(inputList);
            turingMachine.SetStateFunctions(stateFunctions);
            isModified = false;
        }
          StartCoroutine("allStepsWithDelay");

    }
    public IEnumerator allStepsWithDelay() {
        int returnValue = 0;
        while(returnValue != 1){
            returnValue = turingMachine.oneStep();
                    List<char> outputList = turingMachine.GetOutputList();
            for(int i = 0; i < inputCellList.Count; i++){
            inputCellList[i].GetComponentInChildren<TMP_Text>().text = outputList[i].ToString();
        }
            yield return new WaitForSeconds(1f);
        }

    }

    public void onSkipSteps(){
        if (isModified){
            dataReader.SetStateFields(stateFields);
            dataReader.ReadStateFields();
            stateFunctions = dataReader.GetStateFunctions();

            for (int i = 0; i < stateFunctions.Count; i++){
                Debug.Log(stateFunctions[i].getOutputValue());
            }

            turingMachine.SetOutputList(inputList);
            turingMachine.SetStateFunctions(stateFunctions);
            isModified = false;
        }
       
        turingMachine.allSteps();
        List<char> outputList = turingMachine.GetOutputList();
        for(int i = 0; i < inputCellList.Count; i++){
            inputCellList[i].GetComponentInChildren<TMP_Text>().text = outputList[i].ToString();
        }

    }
    public void onRestart(){
        inputString = binaryInput.text;
        inputString = "bb" + inputString + "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        inputList.Clear();
        inputList.AddRange(inputString);
        instantiateInput();
    }
}
