using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeC : StandardHandPrototype
{
    public GameObject ToolButtons;
    public GameObject VideoButtons;
    public MenuItemButton ToolsButton;
    public MenuItemButton VideoButton;

    private UiState state;

    private enum UiState
    {
        Ready,
        ToolsOpen,
        VideoControlsOpen
    }

    private void Start()
    {
        ToolsButton.Released += ToolsButton_Released;
        VideoButton.Released += VideoButton_Released;
    }

    private void VideoButton_Released(object sender, System.EventArgs e)
    {
        if(VideoButton.Toggled)
        {
            state = UiState.VideoControlsOpen;
        }
        else
        {
            state = UiState.Ready;
        }
    }

    private void ToolsButton_Released(object sender, System.EventArgs e)
    {
        if (ToolsButton.Toggled)
        {
            state = UiState.ToolsOpen;
        }
        else
        {
            state = UiState.Ready;
        }
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();

        ToolButtons.SetActive(state == UiState.ToolsOpen);
        VideoButtons.SetActive(state == UiState.VideoControlsOpen);

        VideoButton.Toggled = state == UiState.VideoControlsOpen;
        ToolsButton.Toggled = state == UiState.ToolsOpen;
    }
}
