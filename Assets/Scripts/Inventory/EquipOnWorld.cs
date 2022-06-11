using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipOnWorld : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //如果有该装备就不在地图上生成
      //  gameObject.SetActive(!gameObject.transform.GetComponent<ItemOnWorld>().playerInventory.itemList.
      //      Contains(gameObject.transform.GetComponent<ItemOnWorld>().thisItem));
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
