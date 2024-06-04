using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField]private InventoryObject inventory;
    [SerializeField] private Transform[] inventorySlots; 
    public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
    //    CreateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            AddItemToSlot(i);
        }
    }

    private void AddItemToSlot(int index)
    {
        Transform freeSlot = GetFreeInventorySlot();
        var obj = Instantiate(inventory.Container[index].item.prefab, Vector2.zero, Quaternion.identity);
        obj.GetComponent<Image>().sprite = inventory.Container[index].item.image;
        obj.transform.SetParent(freeSlot);
        (obj.transform as RectTransform).localPosition = Vector2.zero;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[index].amount.ToString("n0");
        itemsDisplayed.Add(inventory.Container[index], obj);

    }

    private Transform GetFreeInventorySlot()
    {
        Transform freeSlot = null;
        for (int j = inventorySlots.Length; j-- > 0;)
        {
            if (inventorySlots[j].childCount == 0)
            {
                freeSlot = inventorySlots[j];
            }
        }

        return freeSlot;
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                AddItemToSlot(i);
            }
        }

        for (int i = 0; i < itemsDisplayed.Count; i++) // Delete Removed inventory Item
        {
            var item = itemsDisplayed.ElementAt(i);
            if (!inventory.Container.Contains(item.Key))
            {
                Destroy(item.Value);
                itemsDisplayed.Remove(item.Key);
            }
        }
    }
}
