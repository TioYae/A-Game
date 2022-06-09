using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ŀ�˵��Ҽ��ɴ���
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]


//ÿһ�����������Ϊһ��Item
public class Item : ScriptableObject
{


    public string itemName;//��Ʒ����
    public Sprite itemImage;//��ƷͼƬ
    public int itemHeld; //��Ʒ����
    public int maxHeld;

    [TextArea]
    public string itemInfo;//��Ʒ����
    public bool equipt;//�Ƿ���װ������Ʒ


    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemImage()
    {
        return itemImage;
    }

    public int GetItemHeld()
    {
        return itemHeld;
    }

    public int GetMaxHeld()
    {
        return maxHeld;
    }
    public string GetItemInfo()
    {
        return itemInfo;
    }
    public bool GetEquip()
    {
        return equipt;
    }


    public void SetItemName(string itemName)
    {
        this.itemName = itemName;
    }

    public void SetItemImage(Sprite itemImage)
    {
        this.itemImage = itemImage;
    }
    public void SetItemHeld(int itemHeld)
    {
        this.itemHeld = itemHeld;
    }
    public void SetMaxHeld(int maxHeld)
    {
        this.maxHeld = maxHeld;
    }
    public void SetItemInfo(string itemInfo)
    {
        this.itemInfo = itemInfo;
    }

    public void SetEquip(bool equipt)
    {
        this.equipt = equipt;
    }













}
