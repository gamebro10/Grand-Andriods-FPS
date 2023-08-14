using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "GrandAndroids";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load the serialized data
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(stream)) 
                    {
                        dataToLoad = sr.ReadToEnd();
                    }
                }

                //Optional decryption
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                //Deserializing time(Json -> C# Object)
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data) 
    { 
        //Accounts for different OS's using the game
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //Creates a directory for the file if one doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the data into a Json
            string dataToStore = JsonUtility.ToJson(data, true);

            //Optional encryption
            if (useEncryption) 
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //write the actual file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                { 
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        //Data encryption setup using XOR encryption
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
