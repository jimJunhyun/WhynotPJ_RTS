using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    public Action OnReceiveHandle = null;

    [ContextMenu("event")]
    public void EventReceive()
    {
        OnReceiveHandle?.Invoke();
    }
}
