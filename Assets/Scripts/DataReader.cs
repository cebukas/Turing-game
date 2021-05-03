using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System;

public class DataReader
{
    private List<TMP_InputField> stateFields = new List<TMP_InputField>();
    private List<StateFunction> stateFunctions = new List<StateFunction>();

    public void SetStateFields(List<TMP_InputField> stateFields)
    {
        this.stateFields = stateFields;
    }
    public List<StateFunction> GetStateFunctions()
    {
        return this.stateFunctions;
    }
    public List<int> ReadStateFields()
    {
        List<int> errors = new List<int>();
        stateFunctions.Clear();
        for (int i = 0; i < stateFields.Count; i++)
        {
            StateFunction stateFunction = new StateFunction();

            string gameObjectName = stateFields[i].name.ToString(); // tmp input name format is 'x_input_y' or 'xx_input_y'

            if (Char.IsDigit(gameObjectName[1]))
            {
                stateFunction.setTriggerValue(gameObjectName[9]);
                stateFunction.setTriggerState(int.Parse((gameObjectName[0] - '0').ToString() + (gameObjectName[1] - '0').ToString()));

            }
            else
            {
                stateFunction.setTriggerState(gameObjectName[0] - '0');
                stateFunction.setTriggerValue(gameObjectName[8]);
            }

            Regex regex = new Regex(@"q\d+(,)\s?[01b]{1}(,)\s?[RLN]");

            Match match = regex.Match(stateFields[i].text);

            if (match.Success)
            {
                string trim = stateFields[i].text.Replace(" ", "");
                string[] inputValues = trim.Split(','); // input format should be 'q0, 0, R' or 'q0,0,R' or 'q10, 0, R'

                if (inputValues[0].Length >= 3)
                {
                    stateFunction.setOutputState(int.Parse((inputValues[0][1] - '0').ToString() + (inputValues[0][2] - '0').ToString()));
                }
                else
                {
                    stateFunction.setOutputState(inputValues[0][1] - '0');
                }

                stateFunction.setOutputValue(inputValues[1][0]);
                stateFunction.setAction(inputValues[2][0]);

                stateFunctions.Add(stateFunction);
            }
            else if (stateFields[i].text != "")
            {
                errors.Add(i);
            }

        }
        return errors;
    }
}