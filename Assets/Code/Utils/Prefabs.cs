using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Prefabs : Singleton<Prefabs>
{
    [SerializeField] private List<GameObject> prefabsList;

    private Dictionary<string, GameObject> _prefabs;

    protected virtual void Awake()
    {
        _prefabs = prefabsList.ToDictionary(x => x.name, x => x);
    }

    public TC Produce<TC>()
    {
        var prefabName = typeof(TC).Name;

        if (_prefabs.ContainsKey(prefabName))
        {
            var obj = Instantiate(_prefabs[prefabName]);
            return obj.GetComponent<TC>();
        }
        else
        {
            Debug.LogWarning($"{prefabName} not found in {gameObject.name}");
        }
        return default;
    }

    public TC Produce<TC>(string prefabName)
    {
        if (_prefabs.ContainsKey(prefabName))
        {
            var obj = Instantiate(_prefabs[prefabName]);
            return obj.GetComponent<TC>();
        }
        return default;
    }

    public GameObject Produce(string prefabName)
    {
        if (_prefabs.ContainsKey(prefabName))
        {
            return Instantiate(_prefabs[prefabName]);
        }
        Debug.LogWarning($"{prefabName} not found in {gameObject.name}");
        return null;
    }

}