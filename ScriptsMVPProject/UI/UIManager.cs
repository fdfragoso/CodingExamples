using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, inventory, crafting;
    private PlayerInputActions playerInputActions;
    private InputAction pauseAction, inventoryAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        pauseAction = playerInputActions.Player.OpenMenu;
        pauseAction.Enable();

        inventoryAction = playerInputActions.Player.Inventory;
        inventoryAction.Enable();
    }



    // Update is called once per frame
    void Update()
    {
        CheckForPause();
        CheckForInventory();

        if(Keyboard.current[Key.C].wasPressedThisFrame) //TODO temp, remove when NPC is Added
        {
            crafting.SetActive(true);
        }
    }

    private void CheckForInventory()
    {
        if (inventoryAction.WasPressedThisFrame())
        {
            inventory.SetActive(!inventory.activeSelf);
            //Time.timeScale = inventory.activeSelf ? 0 : 1; //Not sure if it pauses
        }
    }

    private void CheckForPause()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
        }
    }

    private void OnDisable()
    {
        pauseAction.Disable();
        inventoryAction.Disable();
    }
}
