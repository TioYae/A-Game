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
        thisItem.itemHeld--;
        InventoryManager.RefreshItem();
            if(thisItem.itemName == "Potion")
            {
                Player.GetComponent<PlayerController>().usePotion();
            }
            if (thisItem.itemName == "bigPotion")
            {
                Player.GetComponent<PlayerController>().useBigPotion();
            }


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
