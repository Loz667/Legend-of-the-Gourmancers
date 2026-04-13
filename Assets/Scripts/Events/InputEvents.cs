using System;
using LotG.Input;
using UnityEngine;

namespace LotG.Events
{
    public class InputEvents
    {
        public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

        public void ChangeInputEventContext(InputEventContext newContext)
        {
            inputEventContext = newContext;
        }

        public event Action<Vector2> OnMovePressed;
        public void MovePressed(Vector2 moveDir)
        {
            if (OnMovePressed != null)
            {
                OnMovePressed(moveDir);
            }
        }

        public event Action<InputEventContext> OnSubmitPressed;
        public void SubmitPressed()
        {
            if (OnSubmitPressed != null)
            {
                OnSubmitPressed(this.inputEventContext);
            }
        }

        public event Action OnQuestLogToggled;
        public void QuestLogToggled()
        {
            if (OnQuestLogToggled != null)
            {
                OnQuestLogToggled();
            }
        }
    }
}
