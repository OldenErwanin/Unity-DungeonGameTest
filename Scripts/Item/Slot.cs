using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public int slotCount;

    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI slotCountText;

    void Awake()
    {
        //slotImage = GetComponentInChildren<Image>();
        //slotCountText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateSlotUI();
    }

    public void GiveItemToSlot(Item _item, int count)
    {
        slotItem = _item;
        slotCount += count;
        UpdateSlotUI();
    }

    public void TakeItemFromSlot(int count)
    {
        slotCount -= count;
        if (slotCount <= 0)
        {
            ClearSlot();
        }
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        if (slotItem == null)
        {
            slotImage.enabled = false;
            slotCountText.enabled = false;
        }
        else
        {
            slotImage.enabled = true;
            slotCountText.enabled = true;
            slotImage.sprite = slotItem.itemInventoryIcon;
            slotCountText.text = slotCount.ToString();
        }
    }


    public bool IsItemFitInSlot(int count)
    {
        if (slotCount + count > slotItem.itemStack)
        {
            return false;
        }
        return true;
    }

    public void ClearSlot()
    {
        slotItem = null;
        slotCount = 0;
        slotImage = null;
        slotCountText = null;
    }
}
