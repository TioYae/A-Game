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

}
