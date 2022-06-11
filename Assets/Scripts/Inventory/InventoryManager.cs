using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour {
    static InventoryManager instance;
    public Inventory myBag;


    public GameObject slotGrid;
    public GameObject frontGrid;

    public Text itemInfomation;//��Ʒ����

    //��������Ʒ����ÿһ�����ӵ�prefab
    public GameObject emptySlot;
    public GameObject emptyFrontSlot;

    //��������Ʒ����ÿһ������
    public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> Frontslots = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake() {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    private void OnEnable() {
        RefreshItem();
        instance.itemInfomation.text = "";
    }
    // Update is called once per frame


    public static void UpdateItemInfo(string itemDescription) {
        instance.itemInfomation.text = itemDescription;
    }

    /*
    public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }
    */



    public static void RefreshItem() {
        //�����������Ʒ
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++) {
            if (instance.slotGrid.transform.childCount == 0) { break; }
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        //�����Ʒ������Ʒ
        for (int i = 0; i < instance.frontGrid.transform.childCount; i++) {
            if (instance.frontGrid.transform.childCount == 0) { break; }
            Destroy(instance.frontGrid.transform.GetChild(i).gameObject);
            instance.Frontslots.Clear();
        }


        // �������ɱ�����Ʒ
        for (int i = 0; i < instance.myBag.itemList.Count; i++) {
            if (instance.myBag.itemList[i] != null)
                if (instance.myBag.itemList[i].itemHeld <= 0)
                    instance.myBag.itemList[i] = null;

            //CreateNewItem(instance.myBag.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].transform.localScale = new Vector3(1, 1, 1);
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);

        }

        // ����������Ʒ����Ʒ
        for (int i = 0; i < 6; i++) {
            // CreateNewItem(instance.myBag.itemList[i]);
            instance.Frontslots.Add(Instantiate(instance.emptyFrontSlot));
            instance.Frontslots[i].transform.SetParent(instance.frontGrid.transform);
            instance.Frontslots[i].transform.localScale = new Vector3(1, 1, 1);

            instance.Frontslots[i].GetComponent<Slot>().slotID = i;
            instance.Frontslots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);
        }



    }













}
