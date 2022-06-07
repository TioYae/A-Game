using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//背包界面的每一个格子是一个Slot
public class Slot : MonoBehaviour
{
    public Item slotItem;//对应的道具
    public Image slotImage;//图片
    public Text slotNum;//数量
    public string slotInfo;//描述

    public int slotID;//在背包中的次序

    public GameObject itemInSlot;//slot里面那个Item对象，包含按钮和文字
    public GameObject InventorySys;//使用和丢弃按钮


    void Start()
    {
        InventorySys = GameObject.Find("InventorySys");
    }


    //背包中物品被点击
    public void itemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);//背包界面显示描述

        InventorySys.GetComponent<SecMenu>().selectedItem = slotItem;//选中当前格位，等待按丢弃或使用

       // Debug.Log(SecMenu.GetComponent<SecMenu>().item.itemHeld);
    }


    //物品栏中物品被点击，直接使用
    public void frontOnClicked()
        {
        InventorySys.GetComponent<SecMenu>().useItem(slotItem);
       
    }





    public void SetupSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        slotItem = item;
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfo;
    }
}
