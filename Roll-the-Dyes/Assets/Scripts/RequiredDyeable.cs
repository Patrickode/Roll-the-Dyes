using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredDyeable : Dyeable
{
    public static System.Action<RequiredDyeable> RequiredComponentDyed;

    private void OnEnable()
    {
        GetDyedCall += OnGetDyedCall;
    }

    private void OnDisable()
    {
        GetDyedCall -= OnGetDyedCall;
    }

    protected void OnGetDyedCall(bool success)
    {
        if (success)
        {
            RequiredComponentDyed?.Invoke(this);
        }
    }
}