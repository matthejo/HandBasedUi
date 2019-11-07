using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeSwitcher : MonoBehaviour
{
    public PrototypeStyle Style;

    public HandPrototype PrototypeA;
    public HandPrototype PrototypeB;

    public MenuItemButton PrototypeAButton;
    public MenuItemButton PrototypeBButton;

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
        PrototypeB.gameObject.SetActive(Style == PrototypeStyle.StyleB);
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
                return null;
            case PrototypeStyle.StyleD:
            default:
                return null;
        }
    }
}
