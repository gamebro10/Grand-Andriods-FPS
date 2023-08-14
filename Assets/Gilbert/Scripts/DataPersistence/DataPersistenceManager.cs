using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one DataPersistence Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        //persistent Data path points to different places per OS system
        //Windows: C:\Users<user>\AppData\LocalLow<company name>
        //Mac: ~/Library/Application Support/company name/product name
        //Linux: $XDG_CONFIG_HOME/unity3d or $HOME/.config/unity3d
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this .gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load any saved data from file to handler
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //Push loaded data to scripts that need it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame() 
    {
        //Pass data to scripts that need it for updates
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        //Save that data to a file
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType< IDataPersistence>(); //Scripts need to have mono behavior to be found with this

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
