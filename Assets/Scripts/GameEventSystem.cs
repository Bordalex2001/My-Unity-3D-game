using System;
using System.Collections.Generic;

public class GameEventSystem
{
    private static readonly Dictionary<String, List<Action<String, Object>>> listeners = new();

    public static void AddListener(Action<String, Object> action, String type)
    {
        if (!listeners.ContainsKey(type))
        {
            listeners[type] = new();
        }
        listeners[type].Add(action);
    }

    public static void AddListenerAndInvoke(Action<String, Object> action, String type)
    {
        AddListener(action, type);
        action(type, null);
    }

    public static void RemoveListener(Action<String, Object> action, String type)
    {
        if (listeners.ContainsKey(type))
        {
            listeners[type].Remove(action);
        }
    }

    public static void EmitEvent(String type, object payload)
    {
        if (listeners.ContainsKey(type))
        {
            foreach (Action<String, Object> action in listeners[type])
            {
                action(type, payload);
            }
        }
    }
}