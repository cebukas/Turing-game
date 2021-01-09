using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuringMachine
{ 
    private int pointer = 2;
    private int currentState = 0;
    private List<char> outputList = new List<char>();
    private List<StateFunction> stateFunctions = new List<StateFunction>();

    public void Restart(){
        pointer = 2;
        currentState = 0;
    }
    public void SetStateFunctions(List<StateFunction> stateFunctions){
        this.stateFunctions = stateFunctions;
    }
    public void SetOutputList(List<char> inputList){
        if (outputList.Count == 0){
            this.outputList = inputList;
        }
    }
     public List<char> GetOutputList(){
        return this.outputList;
    }
    public void allSteps(){
        int returnValue = 0;
        while(returnValue != 1)
            returnValue = oneStep();
    }
    public int oneStep(){               // return 1 if final state was reached, 0 if not
        char input = 'b';
        if(pointer != outputList.Count){
            input = outputList[pointer];
        }
        else
            Debug.Log("inputPointer out of bounds");

         StateFunction stateFunction = findStateFunction(input, currentState);
         currentState = stateFunction.getOutputState();

         if(currentState != -1){
            outputList[pointer] = stateFunction.getOutputValue();

            if (stateFunction.getAction() == 'L'){
                pointer --;
            }
            
            if (stateFunction.getAction() == 'R'){
                pointer ++;
            }
         }
         else{
               Debug.Log("Final state reached!");
               return 1;
         }
        return 0;

    }
        private StateFunction findStateFunction(char triggerValue, int triggerState){
        StateFunction stateFunction = new StateFunction();
        stateFunction.setOutputState(-1);

        foreach(StateFunction sf in stateFunctions){
            if (sf.getTriggerState() == triggerState && sf.getTriggerValue() == triggerValue){
                return sf;
            }
        }
        return stateFunction;
    }

}
