using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer useInputSprite;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnimator;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private PlayerInputactions playerInputActions;

    private int usableInRangeIndex;
    private List<IUsableActor> usablesInRangeList = new List<IUsableActor>();

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        playerInputActions = new PlayerInputactions();
        playerInputActions.Enable();
        playerInputActions.Player.Use.performed += Use;
        playerInputActions.Player.ScrollUsableSelection.performed += ScrollUse;
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
        if (usablesInRangeList.Count > 0)
        {
            usablesInRangeList[usableInRangeIndex].Action();
        }
    }

    private void ScrollUse(InputAction.CallbackContext context)
    {
        if (usablesInRangeList.Count > 1)
        {
            usableInRangeIndex++;

            if (usableInRangeIndex >= usablesInRangeList.Count)
            {
                usableInRangeIndex = 0;
            }
        }
        Debug.Log("This one selected: " + usablesInRangeList[usableInRangeIndex]);
        //Update who is selected
    }

    private void MoveCamera()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
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

    }


    void Update()
    {
        MoveCamera();
    }

    private void FixedUpdate()
    {
        Moved();
    }

}
