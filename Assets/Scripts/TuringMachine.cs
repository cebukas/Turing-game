using System.Collections.Generic;
public class TuringMachine
{
    private int pointer = 2;
    private int currentState = 0;
    private List<char> outputList = new List<char>();
    private List<StateFunction> stateFunctions = new List<StateFunction>();

    public void Restart()
    {
        pointer = 2;
        currentState = 0;
    }
    public void SetStateFunctions(List<StateFunction> stateFunctions)
    {
        this.stateFunctions = stateFunctions;
    }
    public List<StateFunction> getStateFunctions()
    {
        return this.stateFunctions;
    }
    public void SetOutputList(List<char> inputList)
    {
        if (outputList.Count == 0)
        {
            this.outputList = inputList;
        }
    }
    public List<char> GetOutputList()
    {
        return this.outputList;
    }
    public int getPointer()
    {
        return pointer;
    }
    public int getState()
    {
        return currentState;
    }
    public int allSteps()
    { // return -2 if it looks like infinite loop
        int returnValue = 0;
        int maxStepsAllowed = 1000;
        while (returnValue != 1 && returnValue != -1)
        {
            returnValue = oneStep();
            maxStepsAllowed--;
            if (maxStepsAllowed < 0)
                return -2;

        }

        return returnValue;
    }
    public int oneStep()
    { // return 1 if final state was reached, 0 if not, -1 if pointer out of bounds
        char input = 'b';
        if (pointer != outputList.Count)
        {
            input = outputList[pointer];
        }

        StateFunction stateFunction = findStateFunction(input, currentState);
        currentState = stateFunction.getOutputState();

        if (pointer == 39 && stateFunction.getAction() == 'R')
            return -1;
        if (pointer == 0 && stateFunction.getAction() == 'L')
            return -1;

        if (currentState != -1)
        {
            outputList[pointer] = stateFunction.getOutputValue();

            if (stateFunction.getAction() == 'L')
            {
                pointer--;
            }

            if (stateFunction.getAction() == 'R')
            {
                pointer++;
            }
        }
        else
        {
            return 1;
        }
        return 0;

    }

    private StateFunction findStateFunction(char triggerValue, int triggerState)
    {
        StateFunction stateFunction = new StateFunction();
        stateFunction.setOutputState(-1);

        foreach (StateFunction sf in stateFunctions)
        {
            if (sf.getTriggerState() == triggerState && sf.getTriggerValue() == triggerValue)
            {
                return sf;
            }
        }
        return stateFunction;
    }

}