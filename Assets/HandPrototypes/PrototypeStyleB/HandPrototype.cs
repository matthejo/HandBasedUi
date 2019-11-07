using UnityEngine;

public abstract class HandPrototype : MonoBehaviour
{
    public float Smoothing;

    public abstract bool IsSummoned { get; }

    public abstract void OnAnyButtonRelease();
    public abstract void OnAnyButtonPress();
}
