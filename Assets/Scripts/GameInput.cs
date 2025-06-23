using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteract;
    
    private PlayerInputActions _inputActions;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Interact.performed += InteractAction;
    }

    private void InteractAction(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormailzed()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        
        return inputVector;
    }
}
