using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecMenu : MonoBehaviour
{
    public Item item;//记录当前选中的物品

    public void useItem()
    {

        item.itemHeld--;
        InventoryManager.RefreshItem();
    }

    public void throwItem()
    {

        item.itemHeld--;
        InventoryManager.RefreshItem();
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
