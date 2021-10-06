using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActions : MonoBehaviour
{
    public Dictionary<GameManager.Objects, UnityEvent> ueAction;
    void Start()
    {
        if (ueAction == null)
            ueAction = new Dictionary<GameManager.Objects, UnityEvent>();
    }

    //Add Listener
    public void AddListenerToKey(GameManager.Objects key, DoAction myAction)
    {
        ueAction.TryGetValue(key, out UnityEvent value);
        value.AddListener(myAction.StartAction);
    }

    //Add Listener And Key to the Dictionnary
    public void AddListenerAndKey(GameManager.Objects key, DoAction myAction)
    {
        ueAction.Add(key, new UnityEvent());
        AddListenerToKey(key, myAction);
    }

    //Remove Listener
    public void RemoveListenerToKey(GameManager.Objects key, DoAction myAction)
    {
        ueAction.TryGetValue(key, out UnityEvent value);
        value.RemoveListener(myAction.StartAction);
    }

    //Remove Listener And Key to the Dictionnary
    public void RemoveListenerAndKey(GameManager.Objects key, DoAction myAction)
    {
        RemoveListenerToKey(key, myAction);
        ueAction.Remove(key);
    }
    public void StartEventWithKey(GameManager.Objects key)
    {
        ueAction.TryGetValue(key, out UnityEvent value);
        value.Invoke();
    }
}
