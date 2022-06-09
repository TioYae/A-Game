using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory MyBag;//����
    public Inventory InventorySave;//�����浵
    //public Inventory All;

    public Item Potion;//Сҩˮ
    public Item BigPotion;//Сҩˮ�浵
    public Item RevivePotion;


    public Item PotionSave;
    public Item BigPotionSave;
    public Item RevivePotionSave;
 

    //�浵
    public void Save()
    {
        PotionSave.itemHeld = Potion.itemHeld;
        BigPotionSave.itemHeld = BigPotion.itemHeld;
        RevivePotionSave.itemHeld = RevivePotion.itemHeld;
        InventorySave.itemList = MyBag.itemList;
    }

    // ��ȡ�浵
    public void Read()
    {
        Potion.itemHeld = PotionSave.itemHeld;
        BigPotion.itemHeld = BigPotionSave.itemHeld;
        RevivePotion.itemHeld = RevivePotionSave.itemHeld;
        MyBag.itemList = InventorySave.itemList;
        InventoryManager.RefreshItem();
    }

    //ɾ���浵
    public void DeleteInventorySave()
    {
        PotionSave.itemHeld = 0;
        BigPotionSave.itemHeld = 0;
        RevivePotionSave.itemHeld = 0;
        InventorySave.itemList.Clear();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
