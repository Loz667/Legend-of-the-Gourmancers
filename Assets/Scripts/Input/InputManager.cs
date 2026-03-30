using UnityEngine;
using UnityEngine.InputSystem;
using LotG.Events;

namespace LotG.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public void HandleSubmit(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameEventsManager.instance.inputEvents.SubmitPressed();
            }
        }
    }
}