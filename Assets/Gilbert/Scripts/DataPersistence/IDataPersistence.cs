using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data); //Reads the data
    void SaveData(ref GameData data); //Modifies the data
}
