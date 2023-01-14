using System;
using System.Collections;
using System.Collections.Generic;

public class Progress : Singleton<Progress>
{
    public float AppleFirstStageMod = 1f;
    
    public float AppleSpawnSpeedMod = 1f;
    public float AppleRipeSpeedMod = 1f;
    public double ApplePurpleGlobalChance = 0f;
    public double AppleBombGlobalChance = 0f;
    
    public Dictionary<string, int> ApplesCollected = new();

    public List<ApplesOrder> CurrentOrders = new()
    {
        ///// Quest #0
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                // ["green"] = 1,
                
                ["pink"] = 3
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyGreen");
                
                Progress.Instance.AppleSpawnSpeedMod *= 1.5f;
                Progress.Instance.AppleFirstStageMod = 1f;
            }
        },
        ///// Quest #1 
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                // ["green"] = 1,

                ["green"] = 6,
                ["red"] = 3,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddBow("BowSilver");
                Progress.Instance.AppleSpawnSpeedMod *= 1.5f;
                
                foreach (var b in Registry.Instance.GetBranches(2))
                {
                    b.SpawnCodes.Add("AppleJointPurple");
                }
            }
        },
        ///// Quest #2
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                // ["green"] = 1,
                
                ["pink"] = 5,
                ["purple"] = 5,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyBlue");
                Progress.Instance.AppleSpawnSpeedMod *= 1.5f;
                
                // foreach (var b in Registry.Instance.GetBranches(2))
                // {
                //     b.SpawnCodes.Add("AppleJointBomb");
                // }
            }
        },
        ///// Quest #3
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                // ["purple"] = 1,
                
                ["golden"] = 3,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddBow("BowGolden");
                
                foreach (var b in Registry.Instance.Branches)
                {
                    b.SpawnCodes.Add("AppleJointBomb");
                }
            }
        },
        
        ///// Quest #4
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["purple"] = 8,
                ["bomb"] = 8,
            },
            RewardFn = () =>
            {
                Trader.Instance.Yell("Your last quest!");
                
                foreach (var b in Registry.Instance.Branches)
                {
                    b.SpawnCodes.Add("AppleJointPurple");
                }
            }
        },
        
        ///// Quest #4
        new ApplesOrder
        {
            Order = new Dictionary<string, int>()
            {
                ["golden"] = 7,
            },
            RewardFn = () =>
            {
                Hero.Instance.AddKey("KeyPurple");
                Progress.Instance.AppleSpawnSpeedMod *= 1.5f;
                
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