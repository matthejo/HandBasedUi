﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemButton : MonoBehaviour
{
    private bool clickable;
    public event EventHandler Pressed;
    public event EventHandler Released;

    private ButtonState state;

    public MeshCollider Backdrop;
    public MeshCollider BackwardsBackdrop;
    private float fingerDistance;

    public MeshRenderer QuadMesh;
    private Material quadMeshMat;

    private enum ButtonState
    {
        Ready,
        Hovered,
        Pressing,
    }

    private void Start()
    {
        quadMeshMat = QuadMesh.material;
    }

    private void Update()
    {
        UpdateInteraction();
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        Color color = GetStateColor();
        quadMeshMat.SetColor("_Color", color);
    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Color.gray;
            case ButtonState.Hovered:
                return Color.blue;
            case ButtonState.Pressing:
            default:
                return Color.cyan;
        }
    }

    private bool IsHoveringOver()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position, transform.forward);
        Debug.DrawRay(HandPrototypeProxies.Instance.RightIndex.position, transform.forward, Color.blue);
        return Backdrop.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }

    private bool IsHoveringUnder()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position, -transform.forward);
        Debug.DrawRay(HandPrototypeProxies.Instance.RightIndex.position, -transform.forward, Color.green);
        return BackwardsBackdrop.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }

    private void UpdateInteraction()
    {
        if(state == ButtonState.Pressing)
        {
            if(!IsHoveringUnder())
            {
                Released?.Invoke(this, EventArgs.Empty);
                HandPrototypeA.Instance.OnAnyButtonRelease();
                state = ButtonState.Ready;
            }
        }
        if(state == ButtonState.Hovered)
        {
            if (IsHoveringUnder())
            {
                state = ButtonState.Pressing;
                HandPrototypeA.Instance.OnAnyButtonPress();
                Pressed?.Invoke(this, EventArgs.Empty);
            }
            else if (!IsHoveringOver())
            {
                state = ButtonState.Ready;
            }
        }
        if(state == ButtonState.Ready)
        {
            if(IsHoveringOver())
            {
                state = ButtonState.Hovered;
            }
        }
    }
}
