using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeSwitcher : MonoBehaviour
{
    public PrototypeStyle Style;

    public HandPrototype PrototypeA;
    public HandPrototype PrototypeB;
    public HandPrototype PrototypeC;

    public MenuItemButton PrototypeAButton;
    public MenuItemButton PrototypeBButton;
    public MenuItemButton PrototypeCButton;

    public HandPrototype SelectedStyle { get; private set; }

    public GameObject LayoutButtons;

    public enum PrototypeStyle
    {
        StyleA,
        StyleB,
        StyleC,
        StyleD
    }

    private void Start()
    {
        PrototypeAButton.Released += PrototypeAButton_Released;
        PrototypeBButton.Released += PrototypeBButton_Released;
        PrototypeCButton.Released += PrototypeCButton_Released;
    }

    private void PrototypeCButton_Released(object sender, EventArgs e)
    {
        Style = PrototypeStyle.StyleC;
    }

    private void PrototypeAButton_Released(object sender, System.EventArgs e)
    {
        Style = PrototypeStyle.StyleA;
    }

    private void PrototypeBButton_Released(object sender, System.EventArgs e)
    {
        Style = PrototypeStyle.StyleB;
    }

    private void Update()
    {
        SelectedStyle = GetSelectedStyle();
        LayoutButtons.SetActive(!SelectedStyle.IsSummoned);

        PrototypeA.gameObject.SetActive(Style == PrototypeStyle.StyleA);
        PrototypeAButton.Toggled = Style == PrototypeStyle.StyleA;
        PrototypeB.gameObject.SetActive(Style == PrototypeStyle.StyleB);
        PrototypeBButton.Toggled = Style == PrototypeStyle.StyleB;
        PrototypeC.gameObject.SetActive(Style == PrototypeStyle.StyleC);
        PrototypeCButton.Toggled = Style == PrototypeStyle.StyleC;
    }

    private HandPrototype GetSelectedStyle()
    {
        switch (Style)
        {
            case PrototypeStyle.StyleA:
                return PrototypeA;
            case PrototypeStyle.StyleB:
                return PrototypeB;
            case PrototypeStyle.StyleC:
                return PrototypeC;
            case PrototypeStyle.StyleD:
            default:
                return null;
        }
    }
}
