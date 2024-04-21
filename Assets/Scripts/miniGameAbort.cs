using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniGameAbort : MonoBehaviour
{
    public void abortMiniGame()
    {
        EventBus.Publish<miniGameAbortEvent>(new miniGameAbortEvent());
    }
}
