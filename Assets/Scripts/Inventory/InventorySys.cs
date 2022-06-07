using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySys : MonoBehaviour
{
    public Item selectedItem;//记录当前选中的物品
    public GameObject Player;
    public Inventory myBag;
    public Item RevivePotion;


    public void useBtnClicked()
    {
        useItem(selectedItem);
    }

    public void throwBtnClicked()
    {
        throwItem(selectedItem);
    }

    public bool findRevivePotion()
    {
        for (int i = 0; i < myBag.itemList.Count; i++)
        {
            if (myBag.itemList[i] != null)

                if (myBag.itemList[i].itemName == "RevivePotion")
                    return true;
        }
        return false;
    }

    public void reviveBtnClicked()//死亡后按键复活
    {
        Debug.Log("reviveBtnclicked");

        useItem(RevivePotion);
        Player.GetComponent<PlayerController>().reburnUI.SetActive(false);
        return;
    }




    public void useItem(Item thisItem)
    {
        if (thisItem != null)
        {
            if (thisItem.itemName == "Potion")
            {
                if (thisItem.itemHeld >= 1)
                {
                    thisItem.itemHeld--;
                    Player.GetComponent<PlayerController>().BloodUp(10f);
                }
            }
            if (thisItem.itemName == "BigPotion")
            {
                if (thisItem.itemHeld >= 1)
                {
                    thisItem.itemHeld--;
                    Player.GetComponent<PlayerController>().BloodUp(50f);
                }
            }

            if (thisItem.itemName == "RevivePotion")
            {
                //调用
                thisItem.itemHeld--;
                Player.GetComponent<PlayerController>().BloodUp(20f);
                Player.GetComponent<PlayerController>().Reburn();

                Debug.Log("不能直接用复活药");
            }
            InventoryManager.RefreshItem();
        }
    }

    public void throwItem(Item thisItem)
    {
        if (thisItem != null)
        {
            thisItem.itemHeld--;
            InventoryManager.RefreshItem();
        }
    }








    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public int GetItemHeld(Item item)
    {
        return item.itemHeld;
    }

    public void SetItemHeld(Item item, int held)
    {
        item.itemHeld = held;
    }

    public List<Item> GetBagList(Inventory inventory)
    {
        return inventory.itemList;
    }

    public void SetBagList(Inventory inventory, int i, Item item)
    {
        inventory.itemList[i] = item;
    }





}
