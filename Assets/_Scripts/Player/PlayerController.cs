using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-- Player Movement Stats --")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpsCurrent;
    [SerializeField] private int jumpsLimit;

    [SerializeField] private float health;

    private Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;

    [Header("-- Player Gravity Stats --")]
    [SerializeField] private float gravity;
    private Vector3 velocity = Vector3.zero;

    [Header("-- Plater States --")]
    public bool playerDead;
    public bool isgrouded;
    public bool canShoot;

    [Header("----- Player Belt -----")]
    public int beltAmount;
    [SerializeField] GameObject belt;
    public GameObject[] playerBelt;
    public List<int> keys = new List<int>();
    public int currentBeltSelection;

    [Header("--- Player Weapons ---")]
    public GameObject currentWeapon;
    public GameObject[] playerWeapons;
    private bool isShooting;
    public float shootRate;
    public float shootDamage;
    public float shootDist;

    [Header("--- References ---")]
    public Animator animator;

    private void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;

        SetPlayerBelt();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            gameManager.instance.ToggleInventory();
        }
        HandleJumping(); //is bugged. when it hits ground it will not reset. May be do to the Handle Gravity?
        HandleGravity();
        HandleRunning();
        HandleMovement();

        
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        if (Input.GetButton("Fire1"))
        {
            //Need to change the Slot to an actual slot because GetComponent is expensive
            gameManager.instance.selectedSlot.GetComponent<Slot>().UseItem();
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleRunning()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpsCurrent = 0;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsCurrent < jumpsLimit)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpsCurrent++;
        }
    }

    //Inventory Functions
    public bool PlayerBeltHaveSpace(Item stats)
    {
        for (int i = 0; i < beltAmount; i++)
        {
            //Check if the item can be fit or not
            if (playerBelt[i].GetComponentInChildren<Slot>().GetID() == 0 || playerBelt[i].GetComponentInChildren<Slot>().GetID() == stats.ID && playerBelt[i].GetComponent<Slot>().GetItemStackAmount() < stats.stackMax)
            {
                return true;
            }
        }
        return false;
    }

    public bool AddItem(Item stats)
    {
        for (int i = 0; i < beltAmount; i++)
        {
            if (playerBelt[i].GetComponent<Slot>().GetItemStackAmount() == 0)
            {
                playerBelt[i].GetComponent<Slot>().IncrementStackBy(1);
                playerBelt[i].GetComponent<Slot>().AddItemToSlot(stats.ID, stats.type, stats.itemName, stats.description, stats.stackMax, stats.icon, stats.itemPrefab, stats.amountToAdd, stats.usable, stats.slotType, stats.weaponLevel, stats.damage, stats.strength, stats.speed, stats._name, stats.index);
                playerBelt[i].GetComponent<Slot>().UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                return true;
            }
            else if (playerBelt[i].transform.GetComponent<Slot>().GetID() == stats.ID && playerBelt[i].GetComponent<Slot>().GetItemStackAmount() < stats.stackMax)
            {
                //Possible Error if item are not added correctly check here ----------< >
                playerBelt[i].GetComponent<Slot>().IncrementStackBy(1);
                playerBelt[i].GetComponent<Slot>().UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                return true;
            }
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        }
        return false;
    }
    private void SetPlayerBelt()
    {
        //Get slots and add them to an array, set player belt slot ID to 200 - 203
        beltAmount = belt.transform.childCount;
        playerBelt = new GameObject[beltAmount];
        for (int i = 0; i < beltAmount; i++)
        {
            playerBelt[i] = belt.transform.GetChild(i).GetChild(0).GetChild(0).gameObject;
            playerBelt[i].GetComponentInParent<SlotBackground>().SlotID = i + 200;
        }
        //Set First Slot of belt as Selected
        gameManager.instance.selectedSlot = playerBelt[0];
        gameManager.instance.previuslySelectedSlot = playerBelt[0];
        controller = GetComponent<CharacterController>();
        //Open and close inventory to populate
        gameManager.instance.ToggleInventory();
        gameManager.instance.UnPauseGame();
        //Enable visuals of inventory of selcted slot
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
