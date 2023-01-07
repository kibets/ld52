using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : Singleton<MousePointer>
{
    private int _mask;
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        
        _mask = LayerMask.GetMask("MouseCatcher");
    }

    // Update is called once per frame
    void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        var raycast = Physics.Raycast(ray, out var hit, 250, _mask);
        
        if (raycast)
        {
            transform.position = hit.point;
        }
    }
}
