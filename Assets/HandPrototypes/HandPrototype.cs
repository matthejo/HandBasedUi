using UnityEngine;

public abstract class HandPrototype : MonoBehaviour
{
    public float Smoothing;

    public abstract bool IsSummoned { get; }
}
