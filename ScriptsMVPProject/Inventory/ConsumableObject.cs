using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    public int RestoreHealthValue;

    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
