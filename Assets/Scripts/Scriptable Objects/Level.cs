using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public int level;

    public string levelDescription;
    public List<string> helpList;

    public string turingFact;

    public List<string> exampleInputList;
    public List<string> exampleOutputList;

}