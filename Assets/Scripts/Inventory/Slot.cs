using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//���������ÿһ��������һ��Slot
public class Slot : MonoBehaviour
{
    public Item slotItem;//��Ӧ�ĵ���
    public Image slotImage;//ͼƬ
    public Text slotNum;//����
    public string slotInfo;//����

    public int slotID;//�ڱ����еĴ���

    public GameObject itemInSlot;//slot�����Ǹ�Item���󣬰�����ť������
    public GameObject SecMenu;//ʹ�úͶ�����ť


    void Start()
    {
        SecMenu = GameObject.Find("SecMenu");
    }


    //��������Ʒ�����
    public void itemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);//����������ʾ����

        SecMenu.GetComponent<SecMenu>().selectedItem = slotItem;//ѡ�е�ǰ��λ���ȴ���������ʹ��

       // Debug.Log(SecMenu.GetComponent<SecMenu>().item.itemHeld);
    }


    //��Ʒ������Ʒ�������ֱ��ʹ��
    public void frontOnClicked()
        {
        SecMenu.GetComponent<SecMenu>().useItem(slotItem);
       
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
