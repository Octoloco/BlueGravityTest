using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    static public UIHandler instance;

    [SerializeField] private GameObject clothesShopCanvas;
    [SerializeField] private GameObject InstructionsCanvas;
    [SerializeField] private GameObject clothesShopContent;
    [SerializeField] private Button clothesShopBuyButton;
    [SerializeField] private Button clothesShopSellButton;
    [SerializeField] private GameObject shopItemGeneric;
    [SerializeField] private GameObject inventoryItemGeneric;
    [SerializeField] private GameObject changingRoomCanvas;
    [SerializeField] private GameObject changingRoomContentTop;
    [SerializeField] private GameObject changingRoomContentBottom;
    [SerializeField] private GameObject changingRoomContentShoes;
    [SerializeField] private GameObject wearableGeneric;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CloseAllWindows();
        OpenInstructions();
    }

    public void OpenInstructions()
    {
        PlayerActions.instance.PlayerIsInMenu(true);
        InstructionsCanvas.SetActive(true);
    }

    public void HideInstructions()
    {
        PlayerActions.instance.PlayerIsInMenu(false);
        InstructionsCanvas.SetActive(false);
    }

    public void OpenClothesShop(List<Item> itemsList, Shopkeeper shopKeeper)
    {
        PlayerActions.instance.PlayerIsInMenu(true);
        clothesShopCanvas.SetActive(true);
        PopulateClothesShopBuy(itemsList, shopKeeper);
        clothesShopBuyButton.onClick.AddListener(delegate () { PopulateClothesShopBuy(itemsList, shopKeeper); });
        clothesShopSellButton.onClick.AddListener(delegate () { PopulateClothesShopSell(shopKeeper); });
    }

    public void HideClothesShop()
    {
        PlayerActions.instance.PlayerIsInMenu(false);
        ClearClothesShop();
        clothesShopCanvas.SetActive(false);
        clothesShopBuyButton.onClick.RemoveAllListeners();
        clothesShopSellButton.onClick.RemoveAllListeners();
    }

    public void ClearClothesShop()
    {
        foreach (Transform shopItem in clothesShopContent.transform.GetComponentInChildren<Transform>())
        {
            Destroy(shopItem.gameObject);
        }
    }

    public void ClearChangingRoomTop()
    {
        foreach (Transform item in changingRoomContentTop.transform.GetComponentInChildren<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    public void ClearChangingRoomBottom()
    {
        foreach (Transform item in changingRoomContentBottom.transform.GetComponentInChildren<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    public void ClearChangingRoomShoes()
    {
        foreach (Transform item in changingRoomContentShoes.transform.GetComponentInChildren<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    public void OpenChangingRoom()
    {
        PlayerActions.instance.PlayerIsInMenu(true);
        ClearChangingRoomBottom();
        ClearChangingRoomTop();
        ClearChangingRoomShoes();
        changingRoomCanvas.SetActive(true);
        PopulateChangingRoomTop();
        PopulateChangingRoomBottom();
        PopulateChangingRoomShoes();
    }

    public void HideChangingRoom()
    {
        PlayerActions.instance.PlayerIsInMenu(false);
        ClearChangingRoomBottom();
        ClearChangingRoomTop();
        ClearChangingRoomShoes();
        changingRoomCanvas.SetActive(false);
    }

    public void CloseAllWindows()
    {
        HideChangingRoom();
        HideClothesShop();
        HideInstructions();
    }

    public void PopulateChangingRoomTop()
    {
        ClearChangingRoomTop();
        foreach (Item item in PlayerActions.instance.inventory.items)
        {
            if (item.isWearable && item.familyIndex == 0)
            {
                GameObject newItem = Instantiate(inventoryItemGeneric, changingRoomContentTop.transform);
                newItem.GetComponent<InventoryItem>().PopulateItem(item);
            }
        }
    }

    public void PopulateChangingRoomBottom()
    {
        ClearChangingRoomBottom();
        foreach (Item item in PlayerActions.instance.inventory.items)
        {
            if (item.isWearable && item.familyIndex == 1)
            {
                GameObject newItem = Instantiate(inventoryItemGeneric, changingRoomContentBottom.transform);
                newItem.GetComponent<InventoryItem>().PopulateItem(item);
            }
        }
    }

    public void PopulateChangingRoomShoes()
    {
        ClearChangingRoomShoes();
        foreach (Item item in PlayerActions.instance.inventory.items)
        {
            if (item.isWearable && item.familyIndex == 2)
            {
                GameObject newItem = Instantiate(inventoryItemGeneric, changingRoomContentShoes.transform);
                newItem.GetComponent<InventoryItem>().PopulateItem(item);
            }
        }
    }

    public void PopulateClothesShopBuy(List<Item> itemsList, Shopkeeper shopKeeper)
    {
        ClearClothesShop();
        clothesShopBuyButton.interactable = false;
        clothesShopSellButton.interactable = true;
        foreach (Item item in itemsList)
        {
            GameObject newItem = Instantiate(shopItemGeneric, clothesShopContent.transform);
            newItem.GetComponent<ShopItem>().PopulateItemBuy(item, shopKeeper);
        }
    }

    public void PopulateClothesShopSell(Shopkeeper shopKeeper)
    {
        ClearClothesShop();
        clothesShopBuyButton.interactable = true;
        clothesShopSellButton.interactable = false;
        foreach (Item item in PlayerActions.instance.inventory.items)
        {
            if (item.isWearable && PlayerActions.instance.inventory.wornBottom != item && PlayerActions.instance.inventory.wornTop != item && PlayerActions.instance.inventory.wornShoes != item)
            {
                GameObject newItem = Instantiate(shopItemGeneric, clothesShopContent.transform);
                newItem.GetComponent<ShopItem>().PopulateItemSell(item, shopKeeper);
            }
        }
    }



}
