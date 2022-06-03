using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//令装备栏中的条目可用鼠标拖动

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public Inventory mybag;
    private int currentItemID;


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);

        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        { 
            //如果拖到非空槽位就交换物品位置
            if (eventData.pointerCurrentRaycast.gameObject.name == "ItemImage")
            {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
            var temp = mybag.itemList[currentItemID];

            mybag.itemList[currentItemID] = mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
            mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
                InventoryManager.RefreshItem();
            return;
             }


            //如果拖到空槽位就将原来的位置置空
            if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)" || eventData.pointerCurrentRaycast.gameObject.name == "FrontSlot(Clone)")
            {

             transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
             transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
             mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = mybag.itemList[currentItemID];
             if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID != currentItemID)
                    mybag.itemList[currentItemID] = null;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                InventoryManager.RefreshItem();
                return;

            }
            //其他任何位置归位物品
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;

        }
        else
        {
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

    }
    // Start is called before the first frame update

}
