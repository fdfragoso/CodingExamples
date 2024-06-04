using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemObject item;

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = item.image;
    }
}
