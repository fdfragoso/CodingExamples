using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(EventTrigger))]
public class ButtonTextUpdater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;
    private Color normal;
    [SerializeField] Color inactive = Color.grey,
        hover = Color.white;

    #region Private Properties
    bool tracking;
    bool inBounds;
    #endregion

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        normal = text.color;

        if(!GetComponent<Button>().interactable)
        {
            text.color = inactive;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
        {
            text.color = hover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
        {
            text.color = normal;
        }
    }
}
