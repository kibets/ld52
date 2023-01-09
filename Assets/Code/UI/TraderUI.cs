using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraderUI : MonoBehaviour
{
    [SerializeField] private Transform container;

    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;
    private Coroutine _acceptRoutine;

    private bool _accepting;
    private bool _shouldHide;

    private void Awake()
    {
        container.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        var currentOrder = Progress.Instance.CurrentOrder;

        if (currentOrder == null)
        {
            confirmButton.gameObject.SetActive(false);
            
            mainText.SetText("No more orders!");
        }
        else
        {
            if (currentOrder.IsFulfilled())
            {
                mainText.SetText("Did you get what I requested?");
                
                confirmButton.gameObject.SetActive(true);
            }
            else
            {
                var result = "I need apples:";

                foreach (var orderKV in currentOrder.Order)
                {
                    result += $"\n{orderKV.Key}: {Progress.Instance.GetCollected(orderKV.Key)}/{orderKV.Value}";
                }
        
                mainText.SetText(result);
                
                confirmButton.gameObject.SetActive(false);
            }
        }
    }
    
    
    public void Show()
    {
        container.gameObject.SetActive(true);

        if (!_accepting)
        {
            UpdateUI();
        }
        
        _shouldHide = false;
    }
    
    public void Hide()
    {
        if (!_accepting)
        {
            container.gameObject.SetActive(false);
        }

        _shouldHide = true;
    }

    public void OnConfirmClicked()
    {
        if (_accepting) return;
        
        confirmButton.gameObject.SetActive(false);
        
        _accepting = true;
        
        _acceptRoutine = StartCoroutine(AcceptRoutine());
    }

    private IEnumerator AcceptRoutine()
    {
        var rewardFn = Progress.Instance.CurrentOrder.RewardFn;

        Progress.Instance.CompleteOrder();

        mainText.SetText("Good....");

        yield return new WaitForSeconds(3f);

        if (rewardFn != null)
        {
            mainText.SetText("Your reward!");
            yield return new WaitForSeconds(1f);
            rewardFn?.Invoke();
            
            yield return new WaitForSeconds(2f);
        }

        if (Progress.Instance.HasMoreOrders())
        {
            mainText.SetText("I have new order!");
            yield return new WaitForSeconds(2f);
            
            UpdateUI();
            LevelUI.Instance.UpdateUI();
        }
        else
        {
            UpdateUI();
            LevelUI.Instance.UpdateUI();
        }

        yield return new WaitForSeconds(2f);

        _accepting = false;

        if (_shouldHide)
        {
            Hide();
        }
    }
}
