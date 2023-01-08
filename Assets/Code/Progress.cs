using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : Singleton<Progress>
{
    public Dictionary<string, int> ApplesCollected = new();

    public List<ApplesOrder> CurrentOrders = new()
    {
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["red"] = 10
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["red"] = 10,
                ["green"] = 10,
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["green"] = 20,
                ["orange"] = 10,
            }
        }
    };

    public ApplesOrder CurrentOrder => CurrentOrders.Count > 0 ? CurrentOrders[0] : null;
    
    
    public void CollectApple(Apple apple)
    {
        if (!ApplesCollected.ContainsKey(apple.AppleType))
        {
            ApplesCollected.Add(apple.AppleType, 0);
        }

        ApplesCollected[apple.AppleType] += 1;
    }

    public int GetCollected(string key)
    {
        return ApplesCollected.ContainsKey(key) ? ApplesCollected[key] : 0;
    }

    public void ResetCollected()
    {
        ApplesCollected.Clear();
    }

    public bool IsOrderFulfilled()
    {
        if (CurrentOrder != null)
        {
            return CurrentOrder.IsFulfilled();
        }

        return false;
    }
    
    public void CompleteOrder()
    {
        if (CurrentOrders.Count > 0)
        {
            CurrentOrders.RemoveAt(0);
        }

        ResetCollected();
    }
}


public class ApplesOrder
{
    public Dictionary<string, int> Order;

    public bool IsFulfilled()
    {
        var collected = Progress.Instance.ApplesCollected;
        
        var fulfilled = true;
        
        foreach (var kv in Order)
        {
            if (!collected.ContainsKey(kv.Key) || collected[kv.Key] < kv.Value)
            {
                fulfilled = false;
                break;
            }
        }

        return fulfilled;
    }
}