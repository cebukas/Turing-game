using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public List<Level> levels;
    [System.NonSerialized]
    public Level level;
    private void Awake()
    {
        level = levels[PlayerPrefs.GetInt("Level") - 1];
    }

}