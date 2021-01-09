using System.Collections.Generic;
using TMPro;

public class DataReader
{
    private List<TMP_InputField> stateFields = new List<TMP_InputField>();
    private List<StateFunction> stateFunctions = new List<StateFunction>();

    public void SetStateFields(List<TMP_InputField> stateFields){
        this.stateFields = stateFields;
    }
    public List<StateFunction> GetStateFunctions(){
        return this.stateFunctions;
    }
    public void ReadStateFields(){
        stateFunctions.Clear();
        for (int i = 0; i < stateFields.Count; i++){
            StateFunction stateFunction = new StateFunction();

            string gameObjectName = stateFields[i].name.ToString();

            stateFunction.setTriggerState(gameObjectName[0] - '0'); // tmp input name format is 'x_input_y'
            stateFunction.setTriggerValue(gameObjectName[8]);
            

            if (stateFields[i].text != ""){
            string[] inputValues = stateFields[i].text.Split(','); // input format should be 'q0, 0, R'

            stateFunction.setOutputState(inputValues[0][1] - '0');
            stateFunction.setOutputValue(inputValues[1][1]);
            stateFunction.setAction(inputValues[2][1]);

            stateFunctions.Add(stateFunction);
            }
        }
    }
}
