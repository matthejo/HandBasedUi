using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeE : HandPrototype
{
    public float RingRadius;
    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }
    public SummonDetector Summoning;

    public Transform HandContent;
    public Transform HandRotationPivot;

    public override bool IsSummoned
    {
        get
        {
            return Summoning.IsSummoned;
        }
    }

    protected void UpdatePrimaryVisibility()
    {
        HandContent.gameObject.SetActive(Summoning.IsSummoned);
    }

    protected void UpdatePosition()
    {
        Vector3 positionBase = HandPrototypeProxies.Instance.LeftPalm.position;

        Vector3 positionTarget = Vector3.Lerp(HandContent.position, positionBase, Time.deltaTime * Smoothing);
        HandContent.position = positionTarget;
        HandRotationPivot.LookAt(Camera.main.transform.position, Vector3.up);
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
    }
}
