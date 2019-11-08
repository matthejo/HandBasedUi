using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeD : StandardHandPrototype
{
    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
    }
}
