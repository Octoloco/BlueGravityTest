using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shopkeeper : MonoBehaviour, IUsableActor
{
    public string m_UsableName;
    public string usableName => m_UsableName;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject nameObject;

    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        nameText.text = m_UsableName;
        HideName();
    }

    public void ShowName()
    {
        nameObject.SetActive(true);
    }

    public void HideName()
    {
        nameObject.SetActive(false);
    }

    public void Action()
    {
        UIHandler.instance.OpenClothesShop(inventory.items, this);
    }

    public void RemoveItemFromStore(Item removedItem)
    {
        inventory.items.Remove(removedItem);
    }

    public void AddItemToStore(Item removedItem)
    {
        inventory.items.Add(removedItem);
    }
}
