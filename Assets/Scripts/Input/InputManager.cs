using UnityEngine;
using UnityEngine.InputSystem;
using LotG.Events;

namespace LotG.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        bool cursorLocked = true;

        public void HandleMovePressed(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                Vector2 moveDir = context.ReadValue<Vector2>();
                GameEventsManager.instance.inputEvents.MovePressed(moveDir);
            }
        }

        public void HandleSubmit(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventsManager.instance.inputEvents.SubmitPressed();
            }
        }

        public void HandleQuestToggle(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventsManager.instance.inputEvents.QuestLogToggled();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}