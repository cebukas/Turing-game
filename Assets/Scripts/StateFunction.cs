[System.Serializable]
public struct StateFunction
{
    private int outputState;
    private char triggerValue;
    private int triggerState;
    private char outputValue;
    private char action;
    public int getOutputState()
    {
        return this.outputState;
    }
    public char getTriggerValue()
    {
        return this.triggerValue;
    }
    public int getTriggerState()
    {
        return this.triggerState;
    }
    public char getOutputValue()
    {
        return this.outputValue;
    }
    public char getAction()
    {
        return this.action;
    }
    public void setOutputState(int outputState)
    {
        this.outputState = outputState;
    }
    public void setTriggerValue(char triggerValue)
    {
        this.triggerValue = triggerValue;
    }
    public void setTriggerState(int triggerState)
    {
        this.triggerState = triggerState;
    }
    public void setOutputValue(char outputValue)
    {
        this.outputValue = outputValue;
    }
    public void setAction(char action)
    {
        this.action = action;
    }
}