using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject slotHolder;
    [Range(1, 30)]
    public int inventorySize;
    public List<GameObject> slots = new List<GameObject>();

    public Item testItem;
    public Item testItem2;

    [SerializeField] private GameObject inventoryCanvas;

    void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            var newSlot = Instantiate(slotPrefab, slotHolder.transform);
            slots.Add(newSlot);
        }

        inventoryCanvas.GetComponent<Canvas>().enabled = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            
            GiveItem(testItem, 1);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {

            GiveItem(testItem2, 1);
        }
    }


    public void ShowHideInventory()
    {
        inventoryCanvas.GetComponent<Canvas>().enabled = !inventoryCanvas.GetComponent<Canvas>().enabled;
    }




    public void GiveItem(Item _item, int count)
    {
        GameObject availableSlot = GetAvailableSlot(_item, count);
        if (availableSlot)
            availableSlot.GetComponent<Slot>().GiveItemToSlot(_item, count);
    }

    public GameObject GetAvailableSlot(Item _item, int count)
    {
        foreach (GameObject slot1 in slots)
        {
            Slot _slot = slot1.GetComponent<Slot>();
            if (_slot.slotItem == _item && _slot.IsItemFitInSlot(count))
            {
                return slot1;
            }
        }
        foreach (GameObject slot2 in slots)
        {
            Slot _slot = slot2.GetComponent<Slot>();
            if (_slot.slotItem == null)
            {
                return slot2;
            }
        }
        return null;
    }
}
