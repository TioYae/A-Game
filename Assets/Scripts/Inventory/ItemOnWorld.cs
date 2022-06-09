using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
            Debug.Log("Item Destroyed");
        }
    }





    //将道具加入背包
    public void AddNewItem()
    {
        if(thisItem.itemHeld< thisItem.maxHeld)
        { 
        thisItem.itemHeld += 1;
        }
        if (!playerInventory.itemList.Contains(thisItem))
        {
                // playerInventory.itemList.Add(thisItem);
                thisItem.itemHeld =  1;
            //InventoryManager.CreateNewItem(thisItem);
         
                //在背包中找一个空位
            for (int i = 0; i < playerInventory.itemList.Count; i++)
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    break;
                }
      
        }
        InventoryManager.RefreshItem();
    }

}
