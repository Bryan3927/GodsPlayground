using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject card;

    public void OnPointerEnter(PointerEventData eventData)
    {
        card.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
