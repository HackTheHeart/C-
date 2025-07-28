using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnAlInteractAction;
    public event EventHandler OnInventoryAction;
    public event EventHandler OnGardeningAction;
    public event EventHandler OnNPCEncounteringAction;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.AlInteract.performed += AlInteract_performed;
        playerInputActions.Player.Inventory.performed += Inventory_performed;
        playerInputActions.Player.Gardening.performed += Gardening_performed;
        playerInputActions.Player.NPCInteract.performed += NPCInteract_performed;
    }
    private void NPCInteract_performed(InputAction.CallbackContext obj)
    {
        OnNPCEncounteringAction?.Invoke(this, EventArgs.Empty);
    }
    private void Gardening_performed(InputAction.CallbackContext obj)
    {
        OnGardeningAction?.Invoke(this, EventArgs.Empty);
    }
    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        OnInventoryAction?.Invoke(this, EventArgs.Empty);
    }
    private void AlInteract_performed(InputAction.CallbackContext obj)
    {
        OnAlInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetvectorMovement()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
