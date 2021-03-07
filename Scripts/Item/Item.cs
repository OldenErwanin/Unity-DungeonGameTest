using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item", fileName = "New Item")]

public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Shield, Armor, Food, Drink, Junk }
    public ItemType itemType;
    public string itemName;
    [TextArea(5, 15)]
    public string itemDescription;
    public int itemStack;
    public Sprite itemInventoryIcon;
    public Sprite itemPickupIcon;
}
