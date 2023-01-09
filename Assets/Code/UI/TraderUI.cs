using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraderUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform yellContainer;

    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI yellText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;

    private bool _accepting;
    private bool _shouldHide;
    
    private Coroutine _yellRoutine;
    private Coroutine _acceptRoutine;
    
    private void Awake()
    {
        container.gameObject.SetActive(false);
        yellText.gameObject.SetActive(false);

        yellText.transform.DOLocalRotate(new Vector3(0, 0, 3f), 0.33f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void UpdateUI()
    {
        var currentOrder = Progress.Instance.CurrentOrder;

        if (currentOrder == null)
        {
            confirmButton.gameObject.SetActive(false);
            
            SetText("No more orders!");
            Sounds.Instance.PlayExact("snap_b_1");
        }
        else
        {
            if (currentOrder.IsFulfilled())
            {
                SetText("Did you get what I asked?");
                Sounds.Instance.PlayExact("snap_b_1");
                
                confirmButton.gameObject.SetActive(true);
            }
            else
            {
                var result = "I need apples:";

                foreach (var orderKV in currentOrder.Order)
                {
                    result += $"\n{orderKV.Key}: {Progress.Instance.GetCollected(orderKV.Key)}/{orderKV.Value}";
                }
        
                SetText(result);
                
                confirmButton.gameObject.SetActive(false);
            }
        }
    }

    private void SetText(string text)
    {
        mainText.SetText(text);

        if (!string.IsNullOrEmpty(text))
        {
            Sounds.Instance.PlayExact("snap_b_1");
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
        
        Sounds.Instance.PlayExact("ui_swing_1");
    }

    private IEnumerator AcceptRoutine()
    {
        var rewardFn = Progress.Instance.CurrentOrder.RewardFn;

        Progress.Instance.CompleteOrder();

        SetText("Good....");

        yield return new WaitForSeconds(3f);

        if (rewardFn != null)
        {
            SetText("Your reward...");
            yield return new WaitForSeconds(1f);
            rewardFn?.Invoke();
            yield return new WaitForSeconds(1.7f);
            SetText("");
            
            yield return new WaitForSeconds(3f);
        }

        if (Progress.Instance.HasMoreOrders())
        {
            SetText("New quest!");
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

    public void Yell(string text)
    {
        if (_yellRoutine != null)
        {
            StopCoroutine(_yellRoutine);
            _yellRoutine = null;
        }
        
        _yellRoutine = StartCoroutine(YellRoutine(text));
    }

    private IEnumerator YellRoutine(string text)
    {
        yellContainer.gameObject.SetActive(false);
        yellText.gameObject.SetActive(true);
        yellText.SetText(text);

        yield return new WaitForSeconds(2f);
        yellContainer.gameObject.SetActive(true);
        yellText.gameObject.SetActive(false);
    }
}
