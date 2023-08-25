using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Item item;

    public void PopulateItem(Item baseItem)
    {
        item = baseItem;
        sprite.sprite = item.sprite;
        nameText.text = item.itemName;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate () { Wear(); });
    }

    public void Wear()
    {
        if (item.familyIndex == 0)
        {
        
            PlayerActions.instance.inventory.wornTop = item;
        }
        else if (item.familyIndex == 1)
        {
            PlayerActions.instance.inventory.wornBottom = item;
        }
        else if (item.familyIndex == 2)
        {
            PlayerActions.instance.inventory.wornShoes = item;
        }

        PlayerActions.instance.UpdateClothes();
    }
}
