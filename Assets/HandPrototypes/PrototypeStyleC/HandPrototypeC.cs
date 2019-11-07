using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeC : StandardHandPrototype
{
    public GameObject ToolButtons;
    public GameObject VideoButtons;
    public MenuItemButton ToolsButton;
    public MenuItemButton VideoButton;

    private void Start()
    {
        ToolsButton.Released += ToolsButton_Released;
        VideoButton.Released += VideoButton_Released;
    }

    private void VideoButton_Released(object sender, System.EventArgs e)
    {
        if(VideoButton.Toggled)
        {
            ToolsButton.Toggled = false;
        }
    }

    private void ToolsButton_Released(object sender, System.EventArgs e)
    {
        if (ToolsButton.Toggled)
        {
            VideoButton.Toggled = false;
        }
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();

        ToolButtons.SetActive(ToolsButton.Toggled);
        VideoButtons.SetActive(VideoButton.Toggled);
    }

    private void OnEnabled()
    {
        ToolsButton.Toggled = false;
        VideoButton.Toggled = false;
    }
}
