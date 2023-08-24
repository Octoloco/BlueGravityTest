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

    void Start()
    {
        nameText.text = m_UsableName;
        HideName();
    }

    void Update()
    {
        
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
        Debug.Log("Open Shop");
    }
}
