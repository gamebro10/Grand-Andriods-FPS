using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int CompletedLevels;

    public int FOVValue;
    public int MouseSensValue;
    public int SFXValue;
    public int MusicValue;

    public SerializableDictionary<string, string> ButtonBinds;

    //These values must equal default values
    public GameData() 
    { 
        this.CompletedLevels = 0;
        this. FOVValue = 60;
        this.MouseSensValue = 50;
        this.SFXValue = 100;
        this.MusicValue = 100;
        //ButtonBinds = new SerializableDictionary<string, string>();
    }

}
