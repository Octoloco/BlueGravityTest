using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerActions : MonoBehaviour
{
    static public PlayerActions instance;

    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer useInputSprite;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer topSprite;
    [SerializeField] private SpriteRenderer bottomSprite;
    [SerializeField] private SpriteRenderer shoesSprite;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private PlayerInputactions playerInputActions;
    private int usableInRangeIndex;
    private List<IUsableActor> usablesInRangeList = new List<IUsableActor>();
    private bool isInMenu;

    public Inventory inventory;

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

        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        playerInputActions = new PlayerInputactions();
        playerInputActions.Enable();
        playerInputActions.Player.Use.performed += Use;
        playerInputActions.Player.ScrollUsableSelection.performed += ScrollUse;
        playerInputActions.Player.ExitGame.performed += ExitGame;
    }

    public void ExitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public void UpdateClothes()
    {
        playerAnimator.SetInteger("TopColor", inventory.wornTop.itemIndex);
        playerAnimator.SetInteger("BottomColor", inventory.wornBottom.itemIndex);
        playerAnimator.SetInteger("ShoesColor", inventory.wornShoes.itemIndex);
        playerAnimator.SetTrigger("Change");
    }

    public void SubtractMoney(int price)
    {
        inventory.money -= price;
    }

    public void AddMoney(int price)
    {
        inventory.money += price;
    }

    public bool CanBuy(int price)
    {
        if (inventory.money - price >= 0)
        {
            return true;
        }
        
        return false;
    }

    public void AddToInventory(Item newItem)
    {
        inventory.items.Add(newItem);
    }

    public void RemoveFromInventory(Item soldItem)
    {
        inventory.items.Remove(soldItem);
    }

    public void PlayerIsInMenu(bool inMenu)
    {
        isInMenu = inMenu;
    }

    private void Moved()
    {
        Vector2 dir = playerInputActions.Player.Move.ReadValue<Vector2>();
        dir = dir.normalized;
        Animate(dir);
        rb.AddForce(dir * speed, ForceMode2D.Force);
    }

    private void Animate(Vector2 dir)
    {
        if (dir.magnitude > 0)
        {

            playerAnimator.SetBool("walking", true);
            


            if (dir.x > 0)
            {
                SpriteRenderer[] sprites = playerSprite.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.flipX = false;
                }

            }
            else if (dir.x < 0)
            {
                SpriteRenderer[] sprites = playerSprite.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.flipX = true;
                }
            }
        }
        else
        {
            playerAnimator.SetBool("walking", false);
        }
    }

    private void Use(InputAction.CallbackContext context)
    {
        if (!isInMenu)
        {
            if (usablesInRangeList.Count > 0)
            {
                usablesInRangeList[usableInRangeIndex].Action();
            }
        }
        else
        {
            UIHandler.instance.CloseAllWindows();
        }
    }

    private void ScrollUse(InputAction.CallbackContext context)
    {
        if (usablesInRangeList.Count > 1)
        {
            usablesInRangeList[usableInRangeIndex].HideName();
            usableInRangeIndex++;

            if (usableInRangeIndex >= usablesInRangeList.Count)
            {
                usableInRangeIndex = 0;
            }
        }
        else
        {
            return;
        }

        usablesInRangeList[usableInRangeIndex].ShowName();
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        usablesInRangeList.Add(collision.GetComponent<IUsableActor>());
        if (usableInRangeIndex >= usablesInRangeList.Count)
        {
            usableInRangeIndex = 0;
        }
        usablesInRangeList[usableInRangeIndex].ShowName();
        useInputSprite.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        usablesInRangeList.Remove(collision.GetComponent<IUsableActor>());
        if (usablesInRangeList.Count <= 0)
        {
            useInputSprite.enabled = false;
            usableInRangeIndex = 0;
            collision.GetComponent<IUsableActor>().HideName();
        }
        else if (usableInRangeIndex >= usablesInRangeList.Count)
        {
            usableInRangeIndex = usablesInRangeList.Count - 1;
            usablesInRangeList[usableInRangeIndex].ShowName();
        }
    }

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }


    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!isInMenu)
        {
            Moved();

        }
    }

}
