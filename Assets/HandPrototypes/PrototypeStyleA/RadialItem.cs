using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialItem : MonoBehaviour
{
    public float Angle = 1;
    public CanvasGroup OptionalCanvas;
    public MeshRenderer OptionalPanelMeshRenderer;

    public void UpdateAlpha(float alpha)
    {
        if(OptionalCanvas != null)
        {
            OptionalCanvas.alpha = alpha;
        }
        if(OptionalPanelMeshRenderer != null)
        {
            OptionalPanelMeshRenderer.material.SetFloat("_Fade", alpha);
        }
    }
}
