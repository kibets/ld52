using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailScreenUI : Singleton<FailScreenUI>
{
    [SerializeField] private RectTransform container;

    [SerializeField] private Image overlay;
    [SerializeField] private TextMeshProUGUI primaryLabel;
    [SerializeField] private TextMeshProUGUI secondaryLabel;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        container.gameObject.SetActive(false);
        
        overlay.gameObject.SetActive(false);
        primaryLabel.gameObject.SetActive(false);
        secondaryLabel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public void Show()
    {
        container.gameObject.SetActive(true);
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        overlay.color = Color.clear;
        overlay.gameObject.SetActive(true);
        overlay.DOColor(new Color(0, 0, 0, 0.5f), 0.7f);
        
        yield return new WaitForSeconds(0.7f);
        primaryLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.13f);
        secondaryLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.11f);
        restartButton.gameObject.SetActive(true);
    }

    public void OnRestartClicked()
    {
        Game.Instance.Restart();
    }
}
