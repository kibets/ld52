using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private TraderUI traderUI;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero") && other.TryGetComponent<Hero>(out var hero))
        {
            hero.ShootingDisabled = true;
            
            traderUI.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hero") && other.TryGetComponent<Hero>(out var hero))
        {
            hero.ShootingDisabled = false;
            traderUI.Hide();
        }
    }
}
