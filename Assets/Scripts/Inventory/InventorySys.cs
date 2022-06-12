using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySys : MonoBehaviour
{
    public Item selectedItem;//记录当前选中的物品
    public GameObject Player;
    public Inventory myBag;
    public Item RevivePotion;
    public AudioClip PotionAudio;
    public AudioClip ReviveAudio;
    public AudioClip EnergyAudio;
    public AudioSource audioSource;
    public List<int> ItemsAmount = new List<int>(new int[18]);



    public void useBtnClicked()
    {
        useItem(selectedItem);
    }

    public void GetAmountofAllItems()
    {
        for (int i = 0; i < myBag.itemList.Count; i++)
        {
            if (myBag.itemList[i] != null)
                ItemsAmount[i] = myBag.itemList[i].itemHeld;
        }
    }

    public void SetAmountofAllItems(List<Item> packages, List<int> ItemsAmount)
    {
          for(int i = 0 ; i < packages.Count;i++ )
        {
            if (packages[i]!=null)
            packages[i].itemHeld = ItemsAmount[i];
        }
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

        UseRevivePotion();
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
                   // audioSource.clip = PotionAudio;
                 //   audioSource.Play();
                }
            }
            if (thisItem.itemName == "BigPotion")
            {
                if (thisItem.itemHeld >= 1)
                {
                    thisItem.itemHeld--;
                    Player.GetComponent<PlayerController>().BloodUp(Player.GetComponent<PlayerController>().bloodMax * 0.5f);
                  //  audioSource.clip = PotionAudio;
                  //  audioSource.Play();
                }
            }
            if (thisItem.itemName == "EnergyPotion")
            {
                if (thisItem.itemHeld >= 1)
                {
                    thisItem.itemHeld--;
                    Player.GetComponent<PlayerController>().EnergyUp(Player.GetComponent<PlayerController>().energyMax);
               //     audioSource.clip = EnergyAudio;
                //    audioSource.Play();
                }
            }
            

            InventoryManager.RefreshItem();
        }
    }
    public void UseRevivePotion()
    {

            //调用
       RevivePotion.itemHeld--;
        InventoryManager.RefreshItem();
        Player.GetComponent<PlayerController>().BloodUp(20f);
        Player.GetComponent<PlayerController>().Reburn();
        audioSource.clip = ReviveAudio;
        audioSource.Play();

        // Debug.Log("不能直接用复活药");

    }



    public void throwItem(Item thisItem)
    {
        if (thisItem != null)
        {
            thisItem.itemHeld--;
            if (thisItem.itemName == "Shoe")
                Player.GetComponent<PlayerController>().haveShoe = false;
            InventoryManager.RefreshItem();
        }


    }


    public void SetPackage(List<Item> packages) {
        myBag.SetPackage(packages);
    }

    public List<Item> GetPackages() {
        return myBag.GetPackage();
    }

    public void SetAmount(List<int> ItemsAmount)
    {
        this.ItemsAmount = ItemsAmount;
    }

    public List<int> GetAmount()
    {
        return ItemsAmount;
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
