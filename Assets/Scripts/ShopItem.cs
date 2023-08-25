using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Item item;

    private Shopkeeper shopKeeper;

    public void BuyItem()
    {
        if (PlayerActions.instance.CanBuy(item.buyPrice))
        {
            PlayerActions.instance.SubtractMoney(item.buyPrice);
            PlayerActions.instance.AddToInventory(item);
            shopKeeper.RemoveItemFromStore(item);
            Destroy(gameObject);
        }
    }

    public void SellItem()
    {
        PlayerActions.instance.AddMoney(item.sellPrice);
        PlayerActions.instance.RemoveFromInventory(item);
        shopKeeper.AddItemToStore(item);
        Destroy(gameObject);
    }

    public void PopulateItemBuy(Item baseItem, Shopkeeper shopKeeperRef)
    {
        item = baseItem;
        shopKeeper = shopKeeperRef;
        sprite.sprite = item.sprite;
        nameText.text = item.itemName;
        priceText.text = "$ " + item.buyPrice.ToString();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate() { BuyItem(); });
    }

    public void PopulateItemSell(Item baseItem, Shopkeeper shopKeeperRef)
    {
        item = baseItem;
        shopKeeper = shopKeeperRef;
        sprite.sprite = item.sprite;
        nameText.text = item.itemName;
        priceText.text = "$ " + item.sellPrice.ToString();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate () { SellItem(); });
    }
}
