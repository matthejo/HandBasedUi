using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeF : HandPrototype
{
    private float currentHoverTime;
    public float HoverTime;

    public Transform HoverProgressBar;
    public GameObject CallControlButtons;

    public MeshCollider HoverZone;
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
        UpdateCallControlButtons();
    }

    private void UpdateCallControlButtons()
    {
        bool isHovered = GetIsHovered(HoverZone);
        currentHoverTime += isHovered ? Time.deltaTime : -Time.deltaTime;
        currentHoverTime = Mathf.Clamp(currentHoverTime, 0, HoverTime);
        float hoverness = currentHoverTime / HoverTime;
        HoverProgressBar.localScale = new Vector3(hoverness, 1, 1);

        bool showProgressbar = GetShouldShowProgressbar(hoverness);
        HoverProgressBar.gameObject.SetActive(showProgressbar);

        bool showCallControlButtons = GetShouldShowCallButtons(hoverness);
        CallControlButtons.SetActive(showCallControlButtons);
    }

    private bool GetShouldShowCallButtons(float hoverness)
    {
        return (hoverness > 0.99f);
    }

    private bool GetShouldShowProgressbar(float hoverness)
    {
        return hoverness > float.Epsilon && hoverness < 1;
    }

    private bool GetIsHovered(MeshCollider collider)
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position - HoverZone.transform.forward, HoverZone.transform.forward);
        return collider.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }
}
