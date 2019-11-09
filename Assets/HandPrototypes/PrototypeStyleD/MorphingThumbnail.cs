using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphingThumbnail : MonoBehaviour
{
    private Transform originalLocation;
    public Transform ExpandedTarget;

    private void Start()
    {
        CreateOriginalLocation();
    }

    public void Morph(float param)
    {
        transform.localPosition = Vector3.Lerp(originalLocation.localPosition, ExpandedTarget.localPosition, param);
        transform.localRotation = Quaternion.Lerp(originalLocation.localRotation, ExpandedTarget.localRotation, param);
        transform.localScale = Vector3.Lerp(originalLocation.localScale, ExpandedTarget.localScale, param);
    }

    private void CreateOriginalLocation()
    {
        originalLocation = new GameObject("Morph source").transform;
        originalLocation.parent = this.transform.parent;
        originalLocation.localPosition = transform.localPosition;
        originalLocation.localRotation = transform.localRotation;
        originalLocation.localScale = transform.localScale;
    }
}
