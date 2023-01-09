using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Progress : Singleton<Progress>
{
    public float AppleFirstStageMod = 1f;
    
    public float AppleSpawnSpeedMod = 1f;
    public float AppleRipeSpeedMod = 1f;
    
    public Dictionary<string, int> ApplesCollected = new();

    public List<ApplesOrder> CurrentOrders = new()
    {
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["green"] = 1,
                
                // ["pink"] = 6
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyGreen");
                Progress.Instance.AppleSpawnSpeedMod *= 2f;

                Progress.Instance.AppleFirstStageMod = 1f;
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["green"] = 1,

                // ["pink"] = 10,
                // ["green"] = 10,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddBow("BowSilver");
                Progress.Instance.AppleSpawnSpeedMod *= 2f;
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["green"] = 1,
                
                // ["pink"] = 10,
                // ["red"] = 10,
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
                ["purple"] = 1,
                
                // ["purple"] = 10,
                // ["green"] = 20,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddBow("BowGolden");
            }
        },
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["purple"] = 10,
                ["green"] = 20,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyPurple");
            }
        },
    };

    public ApplesOrder CurrentOrder => CurrentOrders.Count > 0 ? CurrentOrders[0] : null;

    private void Start()
    {
        Progress.Instance.AppleFirstStageMod = 0.2f;
    }

    public void CollectApple(Apple apple)
    {
        if (!ApplesCollected.ContainsKey(apple.Stage.Name))
        {
            ApplesCollected.Add(apple.Stage.Name, 0);
        }

        ApplesCollected[apple.Stage.Name] += 1;
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