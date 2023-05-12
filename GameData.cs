using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
[Serializable]
public class SaveData
{
    public List<int> addID = new List<int>();
    public List<int> inventoryItemsAmount = new List<int>();
    public List<string> inventoryItemsName = new List<string>();
}

public class GameData : MonoBehaviour
{
    public SaveData saveData;
    public static GameData instance;
    
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(instance.gameObject);           
            instance = this;
        }
        
        Debug.Log(Application.persistentDataPath);
        
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            Load();
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat",FileMode.Create);
        SaveData data = new SaveData();
        data = saveData;
        formatter.Serialize(file,data);
        file.Close();
    }
    
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat",FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
        }
    }
    
    public void ClearData()
    {
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            File.Delete(Application.persistentDataPath + "/player.dat");
        }
    }
    
    public void ClearAllDataList()//kayıtlı datanın üstüne ekstra inventory yazmasın diye önce siliyor sonra tekrar kaydediyoruz
    {
        saveData.addID.Clear();
        saveData.inventoryItemsName.Clear();
        saveData.inventoryItemsAmount.Clear();
        Save();
    }
}
