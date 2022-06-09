using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory MyBag;//±³°ü
    public Inventory InventorySave;//±³°ü´æµµ
    //public Inventory All;

    public Item Potion;//Ð¡Ò©Ë®
    public Item BigPotion;//Ð¡Ò©Ë®´æµµ
    public Item RevivePotion;


    public Item PotionSave;
    public Item BigPotionSave;
    public Item RevivePotionSave;
 

    //´æµµ
    public void Save()
    {
        PotionSave.itemHeld = Potion.itemHeld;
        BigPotionSave.itemHeld = BigPotion.itemHeld;
        RevivePotionSave.itemHeld = RevivePotion.itemHeld;
        InventorySave.itemList = MyBag.itemList;
    }

    // ¶ÁÈ¡´æµµ
    public void Read()
    {
        Potion.itemHeld = PotionSave.itemHeld;
        BigPotion.itemHeld = BigPotionSave.itemHeld;
        RevivePotion.itemHeld = RevivePotionSave.itemHeld;
        MyBag.itemList = InventorySave.itemList;
        InventoryManager.RefreshItem();
    }

    //É¾³ý´æµµ
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
