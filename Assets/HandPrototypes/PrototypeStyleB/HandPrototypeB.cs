using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeB : MonoBehaviour
{
    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }

    public SummonDetector Summoning;
    public UiPositionCore PositionCore;
    public enum UiState
    {
        Unsummoned,
        Summoned
    }

    public UiState State { get; private set; }

    private void Start()
    {
        transform.SetParent(PositionCore.CoreTransform, false);
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
    }

    private void UpdatePrimaryVisibility()
    {
        if (Summoning.IsSummoned)
        {
            if (State == UiState.Unsummoned)
            {
                State = UiState.Summoned;
            }
        }
        else
        {
            State = UiState.Unsummoned;
        }
    }
}
