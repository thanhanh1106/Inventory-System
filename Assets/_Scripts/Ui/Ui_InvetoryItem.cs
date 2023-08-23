using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ui_InvetoryItem : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,
    IEndDragHandler,IDropHandler,IDragHandler
{
    [HideInInspector] public Image itemImage;
    [SerializeField] TMP_Text quanityText;

    [SerializeField] Image borderImage;

    public event Action<Ui_InvetoryItem> OnItemClicked;
    public event Action<Ui_InvetoryItem> OnItemDropOn;
    public event Action<Ui_InvetoryItem> OnItemBeginDrag;
    public event Action<Ui_InvetoryItem> OnItemEndDrag;
    public event Action<Ui_InvetoryItem> OnRightMouseBtnClick;

    bool isEmpty = false;

    private void Awake()
    {
        ResetData();
        Deselect();
    }
    public void ResetData()
    {
        itemImage.gameObject.SetActive(false);
        isEmpty = true;
    }
    public void Deselect() => borderImage.enabled = false;

    public void SetData(Sprite sprite, int quantity)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        quanityText.text = quantity.ToString();
        isEmpty = false;
    }
    public void Select() => borderImage.enabled = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            OnRightMouseBtnClick?.Invoke(this);
        else
            OnItemClicked?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty) return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
