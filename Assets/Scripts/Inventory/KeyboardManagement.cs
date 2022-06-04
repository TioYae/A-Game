using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//按I开关背包

public class KeyboardManagement : MonoBehaviour
{
    public GameObject Bag;
    public Inventory mybag;

    public GameObject SecMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OpenMyBag();
        useItemByKeyboard();
    }
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Bag.SetActive(!Bag.activeSelf);
        }

    }
    public void useItemByKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SecMenu.GetComponent<SecMenu>().useItem(mybag.itemList[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SecMenu.GetComponent<SecMenu>().useItem(mybag.itemList[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SecMenu.GetComponent<SecMenu>().useItem(mybag.itemList[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SecMenu.GetComponent<SecMenu>().useItem(mybag.itemList[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SecMenu.GetComponent<SecMenu>().useItem(mybag.itemList[4]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SecMenu.GetComponent<SecMenu>().useItem(mybag.itemList[5]);
        }

    }
}