using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : Singleton<Trader>
{
    [SerializeField] private TraderUI traderUI;

    private List<string> helloWords = new List<string>()
    {
        "Greetings!",
        "Greetings!",
        "Greetings!",
        "It is you again!",
        "It is you again!",
        "It is you again!",
        "Welcome!",
        "Hi there!",
        "Nice to see you!",
    };

    
    private List<string> yellWords = new List<string>()
    {
        "Stop that!",
        "Ouch!",
        "Ugh!",
        "No!",
        "STOP!",
        "Stop!",
        "Please Stop",
        "Hey!",
        "...",
        "@#$%!",
        "Enough!",
    };

    private Rigidbody _rig;

    private bool _deadRoutine;
    
    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(HelloRoutine());
    }

    private IEnumerator HelloRoutine()
    {
        yield return new WaitForSeconds(1.13f);
        
        Yell(helloWords.PickRandom());
    }

    private void Update()
    {
        if (!_deadRoutine && Hero.Instance.Dead)
        {
            _deadRoutine = true;
            traderUI.Hide();

            StartCoroutine(DeadRoutine());
        }
    }

    private IEnumerator DeadRoutine()
    {
        Yell("Oh no!");

        yield return new WaitForSeconds(4f);
        Yell("Anyway...");
        
        yield return new WaitForSeconds(10f);
        Yell("Restart?");
        
        yield return new WaitForSeconds(4);
        Yell("...");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero") && other.TryGetComponent<Hero>(out var hero))
        {

            if (!hero.Dead)
            {
                hero.ShootingDisabled = true;
                traderUI.Show();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hero") && other.TryGetComponent<Hero>(out var hero))
        {
            if (!hero.Dead)
            {
                hero.ShootingDisabled = false;
                traderUI.Hide();
            }
        }
    }

    public void Yell(string text)
    {
        traderUI.Yell(text);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ArrowTip") && collision.gameObject.TryGetComponent<Arrow>(out var arrow))
        {
            if (collision.contactCount > 0 && !arrow.Reflected && collision.impulse.magnitude > 5f)
            {
                var point = collision.GetContact(0);
                
                var effect = Prefabs.Instance.Produce("AppleHitFx");
                effect.transform.SetParent(transform);
                effect.transform.position = point.point;
                effect.transform.rotation = arrow.transform.rotation;
            
                arrow.StickTo(_rig, 0f);
                arrow.ShrinkDestroy(15f);
                Yell(yellWords.PickRandom());
            }
        }
    }
}
