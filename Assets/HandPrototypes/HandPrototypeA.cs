using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeA : MonoBehaviour
{
    public static HandPrototypeA Instance;

    private HandPrototypeProxies proxies;
    [Range(0, 1)]
    public float PalmSummonThreshold;
    [Range(0, 1)]
    public float PalmDismissThreshold;

    public float Radius;
    public float Smoothing;

    public FingerBoundItem VideoItem;
    public FingerBoundItem CallControlsItem;
    public FingerBoundItem ToolsItem;
    public FingerBoundItem WindowsItem;

    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }

    private FingerBoundItem[] items;

    private Transform psuedoPalm;
    public Transform EffectivePalm { get; private set; }

    public UiState State { get; private set; }
    
    public enum UiState
    {
        Unsummoned,
        Summoned,
        BrowsingTools,
        BrowsingWindows
    }

    public void OnToolsPressed()
    {
        if(State == UiState.BrowsingTools)
        {
            State = UiState.Summoned;
        }
        else
        {
            State = UiState.BrowsingTools;
        }
    }

    public void OnWindowsPressed()
    {
        State = UiState.BrowsingWindows;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        psuedoPalm = new GameObject("PsuedoPalm").transform;
        EffectivePalm = psuedoPalm;
        proxies = HandPrototypeProxies.Instance;
        items = new FingerBoundItem[] { VideoItem, CallControlsItem, ToolsItem, WindowsItem };
    }

    private void Update()
    {
        UpdatePsuedoPalm();
        UpdatePrimaryVisibility();
        UpdateSummonness();
        VideoItem.DoItemUpdate(proxies.LeftIndex, proxies.LeftIndexKnuckle);
        CallControlsItem.DoItemUpdate(proxies.LeftMiddle, proxies.LeftMiddleKnuckle);
        WindowsItem.DoItemUpdate(proxies.LeftRing, proxies.LeftRingKnuckle);
        ToolsItem.DoItemUpdate(proxies.LeftPinky, proxies.LeftPinkyKnuckle);
    }

    private void UpdateSummonness()
    {
        if(State == UiState.Unsummoned)
        {
            currentSummonTime -= Time.deltaTime;
        }else
        {
            currentSummonTime += Time.deltaTime;
        }
        currentSummonTime = Mathf.Clamp(currentSummonTime, 0, SummonTime);
        Summonedness = currentSummonTime / SummonTime;
    }

    private void UpdatePsuedoPalm()
    {
        psuedoPalm.position = proxies.LeftPalm.position;
        Vector3 psuedoPalmForward = GetPsuedoPalmForward();
        Vector3 psuedoPalmUp = GetPsuedoPalmUp(psuedoPalmForward);
        psuedoPalm.rotation = Quaternion.LookRotation(psuedoPalmUp, -psuedoPalmForward);
    }

    private Vector3 GetPsuedoPalmUp(Vector3 forward)
    {
        Vector3 fingerTipAverage = (proxies.LeftIndex.position +
            proxies.LeftMiddle.position +
            proxies.LeftPinky.position +
            proxies.LeftRing.position) / 4;
        Vector3 toPalm = proxies.LeftPalm.position - fingerTipAverage;
        return Vector3.Cross(toPalm, forward);
    }

    private Vector3 GetPsuedoPalmForward()
    {
        Vector3 pointA = (proxies.LeftIndex.position + proxies.LeftMiddle.position) / 2;
        Vector3 pointB = (proxies.LeftRing.position + proxies.LeftPinky.position) / 2;

        Vector3 toPalmA = pointA - proxies.LeftPalm.position;
        Vector3 toPalmB = pointB - proxies.LeftPalm.position;
        return Vector3.Cross(toPalmA, toPalmB);
    }

    private Quaternion GetItemRotation()
    {
        Vector3 toCamera = Camera.main.transform.forward;
        Vector3 skyTangent = Vector3.Cross(toCamera, Vector3.up);
        Vector3 personalUp = Vector3.Cross(toCamera, skyTangent);
        return Quaternion.LookRotation(proxies.LeftPalm.up, -personalUp);
    }

    private void UpdatePrimaryVisibility()
    {
        bool primaryVisibility = GetPrimaryVisibility();
        if(primaryVisibility)
        {
            if(State == UiState.Unsummoned)
            {
                State = UiState.Summoned;
            }
        }
        else
        {
            State = UiState.Unsummoned;
        }
    }

    private bool GetPrimaryVisibility()
    {
        Vector3 toCamera = (proxies.LeftPalm.position - Camera.main.transform.position).normalized;
        float palmDot = Vector3.Dot(toCamera, EffectivePalm.up);
        if(State == UiState.Unsummoned)
        {
            return palmDot > PalmSummonThreshold;
        }
        return palmDot > PalmDismissThreshold;
    }
}
