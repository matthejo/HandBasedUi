using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableThumbnail : Grabbable
{
    private Transform originalParent;
    public GameObject ThumbnailContent;

    private float timeSinceGrab;

    private Vector3 finalContentScale;

    void Start()
    {
        originalParent = transform.parent;
        finalContentScale = transform.localScale;
    }

    void Update()
    {
        if(Manager.GrabbedItem != this)
        {
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        timeSinceGrab += Time.deltaTime;
        timeSinceGrab = Mathf.Clamp(timeSinceGrab, 0, Manager.GrabRestoreTime);
        float grabLerp = timeSinceGrab / Manager.GrabRestoreTime;
        transform.localScale = Vector3.Lerp(transform.localScale, finalContentScale, grabLerp);
    }

    public override void EndGrab()
    {
        ThumbnailContent.SetActive(true);
        transform.parent = originalParent;
        timeSinceGrab = 0;
    }

    public override void StartGrab()
    {
        transform.parent = ThumbnailContent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        ThumbnailContent.SetActive(false);
        transform.parent = Manager.SmoothedGrabPoint;
    }
}
