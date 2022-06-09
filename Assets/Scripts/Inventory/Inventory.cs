using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]

//ÿһ�ֱ���Ϊһ��Inventory�ࣨ��ұ������̵걳���ȣ�
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
    public void addItem(Item item)//�ڱ����м�����Ʒ
    {
        itemList.Add(item);
    }

    public void RefreshList()
    {
        itemList.Clear();
    }

    public void SetPackage(List<Item> itemList) {
        this.itemList = itemList;
    }

    public List<Item> GetPackage() {
        return itemList;
    }
}

