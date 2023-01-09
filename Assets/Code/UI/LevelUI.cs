using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : Singleton<LevelUI>
{
    [SerializeField] private TextMeshProUGUI applesCount;
    [SerializeField] private Button submitButton;


    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        var currentOrder = Progress.Instance.CurrentOrder;

        if (currentOrder == null)
        {
            // submitButton.gameObject.SetActive(false);
            
            applesCount.SetText("");
        }
        else
        {
            // submitButton.interactable = currentOrder.IsFulfilled();
            
            var result = "Harvested:";

            foreach (var orderKV in currentOrder.Order)
            {
                result += $"\n{orderKV.Key}: {Progress.Instance.GetCollected(orderKV.Key)}/{orderKV.Value}";
            }
        
            applesCount.SetText(result);
        }
    }

    public void OnOrderSubmit()
    {
        Progress.Instance.CompleteOrder();
        
        UpdateUI();
    }
}
