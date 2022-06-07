using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//项目菜单右键可创建
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]


//每一种武器或道具为一个Item
public class Item : ScriptableObject
{


    public string itemName;//物品名字
    public Sprite itemImage;//物品图片
    public int itemHeld; //物品数量
    public int maxHeld;

    [TextArea]
    public string itemInfo;//物品描述
    public bool equipt;//是否是装备类物品

}
