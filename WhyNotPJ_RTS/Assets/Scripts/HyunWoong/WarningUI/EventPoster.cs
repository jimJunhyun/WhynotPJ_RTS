using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventPoster : MonoBehaviour
{
    public EventReceiver receiver;

    private void Awake()
    {
        receiver = FindObjectOfType<EventReceiver>();
    }

    public void OnEnable()
    {
        if (receiver != null) receiver.OnReceiveHandle += Active;
    }

    public void Active()
    {
        WarningUI.Instance.NewCard();
    }

    private void OnDisable()
    {
        receiver.OnReceiveHandle -= Active;
    }
}
