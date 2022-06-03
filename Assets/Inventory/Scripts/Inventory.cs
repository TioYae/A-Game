using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]

//每一种背包为一个Inventory类（玩家背包、商店背包等）
public class Inventory : ScriptableObject
{

    public List<Item> itemList = new List<Item>();

}
