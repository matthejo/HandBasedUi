using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeA : HandPrototype
{
    public float HoverDist = .2f;
    public float FingerRadius = 0.03f;

    private HandPrototypeProxies proxies;

    public SummonDetector Summoning;
    public UiPositionCore PositionCore;

    public RadialItemSet MainSet;
    public RadialItemSet ToolsSet;
    public HorizontalItemSet PanelsSet;

    public MenuItemButton ToolsButton;
    public MenuItemButton PanelsButton;

    public AudioSource ButtonPressSound;
    public AudioSource ButtonReleaseSound;

    public float Radius;

    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }

    public UiState State { get; private set; }

    public override bool IsSummoned
    {
        get
        {
            return Summoning.IsSummoned;
        }
    }

    public enum UiState
    {
        Unsummoned,
        Summoned,
        BrowsingTools,
        BrowsingWindows
    }

    private void Start()
    {
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

    public override void OnAnyButtonPress()
    {
        ButtonPressSound.Play();
    }

    public override void OnAnyButtonRelease()
    {
        ButtonReleaseSound.Play();
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        
        MainSet.ShowItems = State != UiState.Unsummoned;
        ToolsSet.ShowItems = State == UiState.BrowsingTools;
        PanelsSet.ShowItems = State == UiState.BrowsingWindows;
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
        if(Summoning.IsSummoned)
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
}
