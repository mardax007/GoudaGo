using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    PlayerData_Storage data;

    public static SaveManager instance { get; private set; }

    // What we want to store

    public List<string> visitedLocations;
    public int points;

    public Text Score;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        Load();
        UpdateScore();

    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);

            visitedLocations = data.visitedLocations;
            points = data.points;

            file.Close();
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage();
        data.visitedLocations = visitedLocations;
        data.points = points;

        bf.Serialize(file, data);
        file.Close();
    }

    public void AddVistedLocations(string locationName)
    {
        visitedLocations.Add(locationName);
    }

    public void UpdateScore()
    {
        Score.text = points.ToString();
    }
}

[Serializable]
class PlayerData_Storage
{
    public List<string> visitedLocations;
    public int points;
}