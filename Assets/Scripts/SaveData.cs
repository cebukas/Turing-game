using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    LevelDataList levelDataList;

    string path;
    private void Awake()
    {
        path = Application.dataPath + "/Save Data";

        levelDataList = new LevelDataList();

        if (!File.Exists(path))
            WriteToBinaryFile(path, levelDataList); // for new builds
        levelDataList = ReadFromBinaryFile<LevelDataList>(path);
    }
    public void PassLevel(int level)
    {
        levelDataList = ReadFromBinaryFile<LevelDataList>(path);

        levelDataList.levels[level - 1] = true;

        WriteToBinaryFile(path, levelDataList);
    }
    public List<bool> GetLevels()
    {
        levelDataList = ReadFromBinaryFile<LevelDataList>(path);

        return levelDataList.levels;
    }
    public void setStateFuctions(List<StateFunction> stateFunctions, int level)
    {
        levelDataList = ReadFromBinaryFile<LevelDataList>(path);

        levelDataList.levelDataList[level - 1].stateFunctions = stateFunctions;

        WriteToBinaryFile(path, levelDataList);
    }
    public List<StateFunction> getStateFunctions(int level)
    {
        levelDataList = ReadFromBinaryFile<LevelDataList>(path);
        return levelDataList.levelDataList[level - 1].stateFunctions;

    }

    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }

    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }
}

[System.Serializable]
public class LevelDataList
{
    public List<bool> levels = new List<bool>(new bool[15]);
    public List<LevelData> levelDataList = new List<LevelData>();

    public LevelDataList()
    {
        var data1 = new LevelData();
        var data2 = new LevelData();
        var data3 = new LevelData();
        var data4 = new LevelData();
        var data5 = new LevelData();
        var data6 = new LevelData();
        var data7 = new LevelData();
        var data8 = new LevelData();
        var data9 = new LevelData();
        var data10 = new LevelData();
        var data11 = new LevelData();
        var data12 = new LevelData();
        var data13 = new LevelData();
        var data14 = new LevelData();
        var data15 = new LevelData();
        var data16 = new LevelData();

        levelDataList.Add(data1);
        levelDataList.Add(data2);
        levelDataList.Add(data3);
        levelDataList.Add(data4);
        levelDataList.Add(data5);
        levelDataList.Add(data6);
        levelDataList.Add(data7);
        levelDataList.Add(data8);
        levelDataList.Add(data9);
        levelDataList.Add(data10);
        levelDataList.Add(data11);
        levelDataList.Add(data12);
        levelDataList.Add(data13);
        levelDataList.Add(data14);
        levelDataList.Add(data15);
        levelDataList.Add(data16);
    }
}

[System.Serializable]
public class LevelData
{
    public List<StateFunction> stateFunctions = new List<StateFunction>();
}