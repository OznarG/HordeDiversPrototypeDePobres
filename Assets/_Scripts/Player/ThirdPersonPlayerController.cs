using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;


public class ThirdPersonPlayerController : MonoBehaviour, IDamage
{
    [Header("--- References ---")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform player;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Rigidbody rb;
    [SerializeField] CharacterController characterController;
    public Animator playerAnim;
    [SerializeField] ParticleSystem particleLvlUp;
    [Header("--- Combat Stats ---")]
    public float meleDamage;
    public float meleAttackSpeed;
    public bool attacking;
    public bool restrictedByAnimation;
    public bool rolling;
    [Header("--- Movement Stats ---")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float gravity;
    private Vector3 lastPosition;
    [Header("--- Player States ---")]
    public bool playerDead;
    public bool isgrouded;
    public bool canShoot;
    [SerializeField] bool canMove = true;
    [Header("----- Player Belt -----")]
    public int beltAmount;
    [SerializeField] GameObject belt;
    public GameObject[] playerBelt;
    public int currentBeltSelection;
    [Header("--- Player Weapons ---")]
    public GameObject currentWeapon;
    public GameObject[] playerWeapons;
    [Header("--- Player Use Stats")]
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float energy;
    [SerializeField] float maxEnergy;
    [SerializeField] float mana;
    [SerializeField] float maxMana;
    [SerializeField] float exp;
    [SerializeField] float maxExpNeeded;
    [SerializeField] int level;
    [SerializeField] int upgradePoints;
    [Header("--- Player Bars ---")]
    [SerializeField] Image HealthBar;
    [SerializeField] Image ManaBar;
    [SerializeField] Image EnergyBar;
    [SerializeField] Image ExpBar;

    private void Start()
    {
        health = maxHealth;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        lastPosition = transform.position;
        moveSpeed = walkSpeed;
        SetPlayerBelt();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            gameManager.instance.ToggleInventory();
        }

        if (canMove)
        {
            //Need to check if is expensive
            Vector3 velocity = rb.linearVelocity;
            HandleRunning();
            HandleMovement(velocity);
            HandleGravity(velocity);

            if (Input.GetButton("Fire1") && rolling == false)
            {
                //Need to change the Slot to an actual slot because GetComponent is expensive
                gameManager.instance.selectedSlot.GetComponent<Slot>().UseItem();
            }
            if(Input.GetButton("Jump") && !rolling)
            {
                rolling = true;
                
                playerAnim.SetTrigger("Roll");
                Debug.Log("Rollings");
            }
            //Debug.Log(characterController.velocity.magnitude);
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        //splayerAnim.SetTrigger("GotHit");
        if (health <= 0)
        {
            playerDead = true;
            health = 0;
        }
        UpdateHealthBar();
    }

    #region ---Movement and gravty---
    private void HandleMovement(Vector3 velocity)
    {     
        // Get input
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down
        if ((horizontalInput != 0 || verticalInput != 0) && restrictedByAnimation == false)
        {
            // Get camera's forward and right vectors
            Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;
            // Calculate movement direction relative to the camera
            Vector3 inputDir = cameraForward * verticalInput + cameraRight * horizontalInput;
            // Calculate velocity based on position change
            velocity = (transform.position - lastPosition) / Time.deltaTime;
            // Update last position for the next frame
            lastPosition = transform.position/*.normalized*/; //this was changed recently
            // Get speed (magnitude of velocity)
            float speed = velocity.magnitude;
            // Update the Animator's Speed parameter
            // Move the player
            if (inputDir != Vector3.zero)
            {
                // Rotate the player to face the movement direction
                playerObj.rotation = Quaternion.Slerp(playerObj.rotation, Quaternion.LookRotation(inputDir), Time.deltaTime * rotationSpeed);
                // Move the player
                characterController.Move(inputDir.normalized * moveSpeed * Time.deltaTime);
            }
            //Debug.Log(speed);
            playerAnim.SetFloat("Speed", characterController.velocity.magnitude);
        }
        else
        {
            playerAnim.SetFloat("Speed", 0);

        }
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
    private void HandleGravity(Vector3 velocity)
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;

        }
        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region ---Animation Functions---
    public void Attacking()
    {
        attacking = true;
    }
    public void NotAttacking()
    {
        attacking = false;
    }
    public void EndRestrictByAnimation()
    {
        restrictedByAnimation = false;
        Debug.Log("fALSE RESTRICTED");
    }
    public void RestrictByAnimation()
    {
        Debug.Log("True RESTRICTED");
        restrictedByAnimation = true;
    }
    public void Rolling()
    {
        rolling = true;
    }    
    public void DoneRolling()
    {
        rolling = false;
    }
    public void UsingTool()
    {
        canMove = false;
    }
    public void DoneUsingTool()
    {
        canMove = true;
    }
    #endregion

    #region --State Bars Functions--
    public void UpdateHealthBar()
    {
        HealthBar.fillAmount = health / maxHealth;
    }
    public void UpdateManaBar()
    {
        ManaBar.fillAmount = mana / maxMana;
    }
    public void UpdateEnergyBar()
    {
        EnergyBar.fillAmount = energy / maxEnergy;
    }
    public void UpdateExpyBar()
    {
        ExpBar.fillAmount = exp / maxExpNeeded;
    }
    #endregion

    #region Adder and Value Changers
    public void AddExp(float amount)
    {
        exp += amount;
        if(exp >= maxExpNeeded)
        {
            
            particleLvlUp.Play();
            float temp;
            temp = exp - maxExpNeeded;
            exp = 0;
            level++;
            upgradePoints++;
            maxExpNeeded = maxExpNeeded * 1.5f;   
            AddExp(temp);

        } 
        UpdateExpyBar();
    }
    #endregion

    #region --- Inventory Functions ---
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
        characterController = GetComponent<CharacterController>();
        //Open and close inventory to populate
        gameManager.instance.ToggleInventory();
        gameManager.instance.UnPauseGame();
        //Enable visuals of inventory of selcted slot
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
        gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
    }
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
        Slot playerBeltSlot;
        for (int i = 0; i < beltAmount; i++)
        {
            playerBeltSlot = playerBelt[i].GetComponent<Slot>();
            if (playerBeltSlot.GetItemStackAmount() == 0)
            {
                playerBeltSlot.IncrementStackBy(1);
                playerBeltSlot.AddItemToSlot(stats.ID, stats.type, stats.itemName, stats.description, stats.stackMax, stats.icon, stats.itemPrefab, stats.amountToAdd, stats.usable);
                playerBeltSlot.UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                return true;
            }
            else if (playerBeltSlot.GetID() == stats.ID && playerBeltSlot.GetItemStackAmount() < stats.stackMax)
            {
                //Possible Error if item are not added correctly check here ----------< >
                playerBeltSlot.IncrementStackBy(1);
                playerBeltSlot.UpdateSlot();
                gameManager.instance.inventoryAud.PlayOneShot(gameManager.instance.pickup);
                return true;
            }
            gameManager.instance.selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
        }
        return false;
    }
    #endregion
}
