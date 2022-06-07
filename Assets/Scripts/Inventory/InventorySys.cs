using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySys : MonoBehaviour
{
    public Item selectedItem;//记录当前选中的物品
    public GameObject Player;
    public Inventory myBag;



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
            if(myBag.itemList[i] != null)

            if (myBag.itemList[i].itemName == "RevivePotion")
                return true;
        }
        return false;
    }

    public void reviveBtnClicked()//死亡后按左键复活
    {
        Debug.Log("clicked");
        for (int i = 0; i < myBag.itemList.Count; i++)
        {
            if (myBag.itemList[i] != null)
                if (myBag.itemList[i].itemName == "RevivePotion")
                useItem(myBag.itemList[i]);
            return;
        }
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
            
                // Debug.Log("不能直接用复活药");
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
