using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeA : MonoBehaviour
{
    public static HandPrototypeA Instance;

    public float HoverDist = .2f;
    public float FingerRadius = 0.03f;

    private HandPrototypeProxies proxies;
    [Range(0, 1)]
    public float PalmSummonThreshold;
    [Range(0, 1)]
    public float PalmDismissThreshold;

    public RadialItemSet MainSet;
    public RadialItemSet ToolsSet;
    public HorizontalItemSet PanelsSet;

    public MenuItemButton ToolsButton;
    public MenuItemButton PanelsButton;

    public AudioSource ButtonPressSound;
    public AudioSource ButtonReleaseSound;

    public float Radius;
    public float Smoothing;

    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }

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
        ToolsButton.Released += OnToolsButtonReleased;
        PanelsButton.Released += OnPanelsButtonReleased;
    }

    private void OnPanelsButtonReleased(object sender, EventArgs e)
    {
        if (State == UiState.BrowsingWindows)
        {
            State = UiState.Summoned;
        }
        else
        {
            State = UiState.BrowsingWindows;
        }
    }

    private void OnToolsButtonReleased(object sender, EventArgs e)
    {
        if (State == UiState.BrowsingTools)
        {
            State = UiState.Summoned;
        }
        else
        {
            State = UiState.BrowsingTools;
        }
    }

    internal void OnAnyButtonPress()
    {
        ButtonPressSound.Play();
    }

    internal void OnAnyButtonRelease()
    {
        ButtonReleaseSound.Play();
    }

    private void Update()
    {
        UpdatePsuedoPalm();
        UpdatePrimaryVisibility();
        
        MainSet.ShowItems = State != UiState.Unsummoned;
        ToolsSet.ShowItems = State == UiState.BrowsingTools;
        PanelsSet.ShowItems = State == UiState.BrowsingWindows;
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
