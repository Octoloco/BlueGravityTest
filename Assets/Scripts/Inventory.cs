using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int money;
    public Item wornTop;
    public Item wornBottom;
    public Item wornShoes;
    public List<Item> items = new List<Item>();
}
