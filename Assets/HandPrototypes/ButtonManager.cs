using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public Color ReadyColor = Color.black;
    public Color ReadyToggledColor = Color.gray;
    public Color HoverColor = Color.blue;
    public Color PressingColor = Color.cyan;

    public AudioSource ButtonPressSound;
    public AudioSource ButtonReleaseSound;


    public void OnAnyButtonPress()
    {
        ButtonPressSound.Play();
    }

    public void OnAnyButtonRelease()
    {
        ButtonReleaseSound.Play();
    }
}
