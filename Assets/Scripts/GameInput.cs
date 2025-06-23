using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlternate;
    
    private PlayerInputActions _inputActions;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Interact.performed += InteractAction;
        _inputActions.Player.InteractAlternate.performed += InteractAlternateAction;
    }

    private void InteractAction(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }
    
    private void InteractAlternateAction(InputAction.CallbackContext context)
    {
        OnInteractAlternate?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormailzed()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        
        return inputVector;
    }
}
