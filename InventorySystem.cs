using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public GameObject[] slots;
    //public GameObject[] backpack;
    bool isInstantiated;
    public ItemList itemList;
    TextMeshProUGUI amountText;
    public Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
    
    void Start()
    {
        Debug.Log("Start yapıldı");
        Debug.Log(itemList);
        if(itemList != null)
        {
            DataToInventory();
            Debug.Log("Data inv e çekildi");
        }
    }
    public void CheckSlotsAvailableity(GameObject itemToAdd,string itemName,int itemAmount)
    {
        isInstantiated = false;
        for(int i = 0; i<slots.Length ; i++)
        {
            if(slots[i].transform.childCount > 0)//Slot dolu
            {
                slots[i].GetComponent<Scots>().isUsed = true;
            }
            else if(!isInstantiated && !slots[i].GetComponent<Scots>().isUsed)//Kullanılabilir bi slot
            {
                if(!inventoryItems.ContainsKey(itemName))//Farklı bi item aldıysan çalışır
                {
                   GameObject item = Instantiate(itemToAdd, slots[i].transform.position, Quaternion.identity);
                   item.transform.SetParent(slots[i].transform,false);
                   item.transform.localPosition = new Vector3(0,0,0);
                   item.name = item.name.Replace("(Clone)","");
                   isInstantiated = true;
                   slots[i].GetComponent<Scots>().isUsed = true;
                   inventoryItems.Add(itemName,itemAmount);
                   
                   amountText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
                   amountText.text = itemAmount.ToString();
                   break;
                }
                else//Aynı isimli item e rastlarsan
                {
                    for(int j = 0; j< slots.Length ; j++)
                    {
                        if(slots[j].transform.GetChild(0).gameObject.name == itemName)
                        {
                            inventoryItems[itemName] += itemAmount;
                            amountText = slots[j].GetComponentInChildren<TextMeshProUGUI>();
                            amountText.text = inventoryItems[itemName].ToString();
                            break;
                        }
                        
                    }
                    break;
                }
            }
        }
    }

    public void UseInventoryItems(string itemName)
    {
        for(int i = 0; i < slots.Length ; i ++)
        {
            if(!slots[i].GetComponent<Scots>().isUsed)
            {
                continue;
            }
            
            if(slots[i].transform.GetChild(0).gameObject.name == itemName)
            {
                inventoryItems[itemName]--;
                amountText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
                amountText.text = inventoryItems[itemName].ToString();
                if(inventoryItems[itemName] <= 0)
                {
                    Destroy(slots[i].transform.GetChild(0).gameObject);
                    slots[i].GetComponent<Scots>().isUsed = false;
                    inventoryItems.Remove(itemName);
                    ReorganizedInv();
                    
                }
                break;
            }
        }
    }
    public void ReorganizedInv()
    {  
        for(int i = 0; i< slots.Length ; i++)
        {
          if(!slots[i].GetComponent<Scots>().isUsed)
            {
                for(int j= i+1; j<slots.Length; j++)
                {
                    if(slots[j].GetComponent<Scots>().isUsed)
                    {
                        Transform itemMove = slots[j].transform.GetChild(0).transform;
                        itemMove.transform.SetParent(slots[i].transform,false);
                        itemMove.transform.localPosition = new Vector3(0,0,0);
                        slots[i].GetComponent<Scots>().isUsed = true;
                        slots[j].GetComponent<Scots>().isUsed = false;
                        break;
                    }
                }
            }  
        }
    }

    public void DataToInventory()
    {
        for(int i = 0; i< GameData.instance.saveData.addID.Count ; i++)
        {
            for(int j =0 ; j < itemList.items.Count ; j++)
            {
                if(itemList.items[j].ID == GameData.instance.saveData.addID[i])
                {
                    CheckSlotsAvailableity(itemList.items[j].gameObject,GameData.instance.saveData.inventoryItemsName[i],GameData.instance.saveData.inventoryItemsAmount[i]);

                }
            }
        }
    }
    public void InventoryToData()
    {
        for(int i=0; i< slots.Length;i++)
        {
            if(slots[i].GetComponent<Scots>().isUsed)
            {
                if(!GameData.instance.saveData.addID.Contains(slots[i].GetComponentInChildren<ItemUse>().ID))
                {
                    GameData.instance.saveData.addID.Add(slots[i].GetComponentInChildren<ItemUse>().ID);
                    GameData.instance.saveData.inventoryItemsName.Add(slots[i].GetComponentInChildren<ItemUse>().name);
                    GameData.instance.saveData.inventoryItemsAmount.Add(inventoryItems[slots[i].GetComponentInChildren<ItemUse>().name]);
                }
            }
        }
    }

    public void EquipmentInInventory(ItemType type)
    {
        for(int i = 0; i< slots.Length ; i++)
        {
            if(slots[i].GetComponent<Scots>().isUsed)
            {
                if(slots[i].transform.GetComponentInChildren<ItemUse>().itemType != ItemType.USABLE)
                {
                    if(slots[i].transform.GetComponentInChildren<ItemUse>().itemType == type)
                    {
                        if(slots[i].transform.GetChild(0).GetChild(1).gameObject.activeSelf)
                        {
                            slots[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
