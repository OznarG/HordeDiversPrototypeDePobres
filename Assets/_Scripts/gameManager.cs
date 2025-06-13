using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("Player References")]
    public GameObject player;
    public HudPlayer playerHUD2;
    public PlayerStats playerStats;
    public PlayerController playerController;
    public ThirdPersonPlayerController thirdPersonPlayerController;
    public Inventory playerInventoryScript;
    public Camera playerCamera;

    public GameObject farmingLandHolder;
    public FarmingLand[] farmingCubes;

    [Header("----- Menus -----")]
    public bool isPaused;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject looseMenu;
    public GameObject playerInventory;
    public GameObject CharacterStatsMenu;
    public GameObject CraftingMenu;
    public GameObject skillsMenu;
    public GameObject CollectiblesMenu;
    public GameObject infoMenu;
    public GameObject settingsMenu;
    public ButtonsFunctions buttonsMenus;


    [Header("----- Inventory Management -----")]
    public GameObject previuslySelectedSlot = null;
    public GameObject selectedSlot = null;
    public Color backgroundColor;
    public AudioSource inventoryAud;
    public AudioClip pickup;

    public Image crossHair;

    [Header("--- Chat Window ---")]
    public GameObject charWindow;
    public Image profilePicture;
    public Image textBackground;
    public TMP_Text chatText;
    public string[] textsToPlay;
    public Button nextButton;
    public int textIndex;

    [Header("--- Actions Buttons ---")]
    public GameObject ActionHolder;
    public GameObject ActionOne;
    public TMP_Text ActionOnetxt;
    public GameObject ActionTwo;
    public TMP_Text ActionTwotxt;
    public GameObject ActionThree;
    public TMP_Text ActionThreetxt;

    public BasecNPCTest[] test;
    public TimeManager timer;

    private void Awake()
    {
        instance = this;
        playerHUD2 = player.GetComponent<HudPlayer>();
        playerStats = player.GetComponent<PlayerStats>();
        playerController = player.GetComponent<PlayerController>();

        playerInventoryScript = playerInventory.GetComponent<Inventory>();
        SetFarmingLand();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerController.playerDead)
        {
            isPaused = !isPaused;
            if (isPaused && activeMenu == null)
            {
                activeMenu = pauseMenu;
                activeMenu.gameObject.SetActive(isPaused);
                PauseGame();
            }
            else
            {
                UnPauseGame();

            }
        }
    }

    public void PauseGame(bool cursorOn = true)
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = cursorOn;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void UnPauseGame()
    {
        if (activeMenu == playerInventoryScript.isOpen)
        {
            //Need replaced with belt
            selectedSlot.GetComponentInParent<SlotBackground>().selected = false;
            selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();
            selectedSlot = previuslySelectedSlot;
            selectedSlot.GetComponentInParent<SlotBackground>().selected = true;
            selectedSlot.GetComponentInParent<SlotBackground>().UpdateSelection();

            playerInventoryScript.isOpen = false;
            //instance.CharacterStatsMenu.SetActive(false);
            instance.playerInventory.SetActive(false);
            //***IMPLEMENT WHEN I ADD WEAPON ****
            //if (selectedSlot.GetComponentInParent<SlotBackground>().GetComponentInChildren<Slot>().GetItemType() == 10)
            //{
            //    playerScript.currentWeapon.SetActive(true);
            //}
        }

        Time.timeScale = 1;
        isPaused = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        if(activeMenu != null)
        {
          activeMenu.SetActive(false);
        }

        activeMenu = buttonsMenus.falseMenu;
        Cursor.visible = false;
    }

    public void ToggleInventory()
    {
        if (!instance.isPaused)
        {
            //open Inventory and set as main
            instance.CharacterStatsMenu.SetActive(true);
            instance.playerInventory.SetActive(true);
            instance.activeMenu = instance.CharacterStatsMenu;
            instance.isPaused = true;
            instance.playerInventoryScript.isOpen = true;
            //NEED TO IMPLEMENT THIS IF WE ADD A BACK BUTTON ON THE MENU  ---------<===>
            //instance.playerinventoryScrpt.backButton.SetActive(false);
            previuslySelectedSlot = selectedSlot;
            //if(playerScript.currentWeapon != null)
            //{
            //  playerScript.currentWeapon.SetActive(false);
            //}           
            instance.PauseGame();
        }
        else if (instance.activeMenu == instance.CharacterStatsMenu)
        {
            instance.UnPauseGame();
        }
    }

    private void SetFarmingLand()
    {
        int land;
        land = farmingLandHolder.transform.childCount;
        farmingCubes = new FarmingLand[land];
        for (int i = 0; i < land; i++)
        {
            farmingCubes[i] = farmingLandHolder.transform.GetChild(i).GetComponent<FarmingLand>();
            
        }
    }

    public void ResetTextWindow()
    {
        profilePicture.sprite = null;
        chatText.text = " ";
        charWindow.SetActive(false);
    }
}
