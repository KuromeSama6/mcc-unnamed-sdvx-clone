using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System;

public class Dispatcher : MonoBehaviour
{
    public static Dispatcher Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private ConcurrentQueue<System.Action> actions = new ConcurrentQueue<System.Action>();
    
    private void Update()
    {
        if (actions.Count == 0) return;
        if (actions.TryDequeue(out System.Action action))
        {
            action.Invoke();
        }
    }

    public void RunInMainThread(Action action)
    {
        actions.Enqueue(action);
    }
}
