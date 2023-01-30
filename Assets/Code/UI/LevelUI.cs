using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : Singleton<LevelUI>
{
    [SerializeField] private TextMeshProUGUI applesCount;
    [SerializeField] private Button submitButton;

    [SerializeField] private TextMeshProUGUI hpLabel;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        hpLabel.SetText("hp: " + Hero.Instance.Health);
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

            if (currentOrder.IsFulfilled())
            {
                applesCount.color = new Color(0.62f, 1f, 0.31f);
                
                applesCount.transform.DOKill();
                applesCount.transform.localScale = Vector3.one;
                applesCount.transform.DOScale(1.1f, 0.4f);
            }
            else
            {
                applesCount.color = Color.white;
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
