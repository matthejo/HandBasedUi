using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemButton : MonoBehaviour
{
    public ButtonInteractionStyles InteractionStyle;
    public bool Toggled;

    private bool clickable;
    public event EventHandler Pressed;
    public event EventHandler Released;

    public HandPrototype PrototypeManager;

    private ButtonState state;

    public MeshCollider Backdrop;
    public MeshCollider BackwardsBackdrop;
    private float fingerDistance;

    public MeshRenderer QuadMesh;
    private Material quadMeshMat;

    public Transform ButtonContent;

    private enum ButtonState
    {
        Ready,
        Hovered,
        Pressing,
    }
    public enum ButtonInteractionStyles
    {
        ToggleButton,
        ClickButton
    }

    private void Start()
    {
        quadMeshMat = QuadMesh.material;
    }

    private void Update()
    {
        UpdateInteraction();
        UpdateMaterial();
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        float buttonContentTarget = state == ButtonState.Hovered ? -12f : (state == ButtonState.Pressing ? 0.5f :-6f);
        float newButtonZ = Mathf.Lerp(ButtonContent.localPosition.z, buttonContentTarget, Time.deltaTime * 50);
        ButtonContent.localPosition = new Vector3(ButtonContent.localPosition.x, ButtonContent.localPosition.y, newButtonZ);

        float backdropTarget = state == ButtonState.Pressing ? 16f : 0;
        float newBackdropZ = Mathf.Lerp(QuadMesh.transform.localPosition.z, backdropTarget, Time.deltaTime * 50);
        QuadMesh.transform.localPosition = new Vector3(QuadMesh.transform.localPosition.x, QuadMesh.transform.localPosition.y, newBackdropZ);
    }

    private Color currentColor;

    private void UpdateMaterial()
    {
        Color colorTarget = GetStateColor();
        currentColor = Color.Lerp(currentColor, colorTarget, Time.deltaTime * 15);
        quadMeshMat.SetColor("_Color", currentColor);
    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Toggled ? Color.gray : Color.black;
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
                OnRelease();
            }
        }
        if(state == ButtonState.Hovered)
        {
            if (IsHoveringUnder())
            {
                OnPress();
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

    private void OnPress()
    {
        state = ButtonState.Pressing;
        if (PrototypeManager != null)
        {
            PrototypeManager.OnAnyButtonPress();
        }
        Pressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnRelease()
    {
        Released?.Invoke(this, EventArgs.Empty);
        if (PrototypeManager != null)
        {
            PrototypeManager.OnAnyButtonRelease();
        }
        state = ButtonState.Ready;
        if(InteractionStyle == ButtonInteractionStyles.ToggleButton)
        {
            Toggled = !Toggled;
        }
    }
}
