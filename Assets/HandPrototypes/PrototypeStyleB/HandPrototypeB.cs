using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeB : HandPrototype
{
    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }

    public Transform HandContent;
    public Transform HandRotationPivot;
    
    public SummonDetector Summoning;
    public UiPositionCore PositionCore;

    public AudioSource ButtonPressSound;
    public AudioSource ButtonReleaseSound;

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 positionTarget = Vector3.Lerp(HandContent.position, PositionCore.CoreTransform.position, Time.deltaTime * Smoothing);
        HandContent.position = positionTarget;
        HandContent.LookAt(Camera.main.transform.position, Vector3.up);
        HandRotationPivot.LookAt(Camera.main.transform.position, Vector3.up);
    }

    private void UpdatePrimaryVisibility()
    {
        HandContent.gameObject.SetActive(Summoning.IsSummoned);
    }

    public override void OnAnyButtonRelease()
    {
        ButtonReleaseSound.Play();
    }

    public override void OnAnyButtonPress()
    {
        ButtonPressSound.Play();
    }
}
