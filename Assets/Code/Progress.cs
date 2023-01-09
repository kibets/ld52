using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Progress : Singleton<Progress>
{
    public float AppleSpawnSpeedMod = 1f;
    public float AppleRipeSpeedMod = 1f;
    
    public Dictionary<string, int> ApplesCollected = new();

    public List<ApplesOrder> CurrentOrders = new()
    {
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["green"] = 2
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyGreen");
                Progress.Instance.AppleSpawnSpeedMod *= 2f;
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["red"] = 2,
                ["green"] = 2,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyBlue");
                Progress.Instance.AppleSpawnSpeedMod *= 2f;
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["green"] = 2,
                ["orange"] = 2,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyPurple");
                Progress.Instance.AppleSpawnSpeedMod *= 2f;
            }
        },
    };

    public ApplesOrder CurrentOrder => CurrentOrders.Count > 0 ? CurrentOrders[0] : null;
    
    
    public void CollectApple(Apple apple)
    {
        if (!ApplesCollected.ContainsKey(apple.AppleAge))
        {
            ApplesCollected.Add(apple.AppleAge, 0);
        }

        ApplesCollected[apple.AppleAge] += 1;
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

    public bool HasMoreOrders()
    {
        return CurrentOrders.Count > 0;
    }
}


public class ApplesOrder
{
    public Dictionary<string, int> Order;

    public Action RewardFn;
    
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