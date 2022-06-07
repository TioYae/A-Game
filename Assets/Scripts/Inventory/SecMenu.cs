using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecMenu : MonoBehaviour
{
    public Item selectedItem;//记录当前选中的物品
    public GameObject Player;



    public void useBtnClicked()
    {
        useItem(selectedItem);
    }

    public void throwBtnClicked()
    {

        throwItem(selectedItem);
    }

    public void useItem(Item thisItem)
    {

        if(thisItem != null)
        {

     
            if(thisItem.itemName == "Potion")
            {
                if (thisItem.itemHeld >= 1)
                {
                    thisItem.itemHeld--;
                Player.GetComponent<PlayerController>().usePotion();

                }
            }
            if (thisItem.itemName == "BigPotion")
            {
                if (thisItem.itemHeld >= 1)
                {
                    thisItem.itemHeld--;
                    Player.GetComponent<PlayerController>().useBigPotion();
                }
            }

            if (thisItem.itemName == "RevivePotion")
            {
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

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
