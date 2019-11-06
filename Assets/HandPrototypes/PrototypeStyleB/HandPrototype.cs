using UnityEngine;

public abstract class HandPrototype : MonoBehaviour
{
    public float Smoothing;

    public abstract void OnAnyButtonRelease();
    public abstract void OnAnyButtonPress();
}
