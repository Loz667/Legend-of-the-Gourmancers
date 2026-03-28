using System;
using UnityEngine;

public class InputEvents
{
    public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

    public void ChangeInputEventContext(InputEventContext newContext)
    {
        inputEventContext = newContext;
    }

    public event Action<InputEventContext> OnSubmitPressed;
    public void SubmitPressed()
    {
        if (OnSubmitPressed != null)
        {
            OnSubmitPressed(this.inputEventContext);
        }
    }
}
