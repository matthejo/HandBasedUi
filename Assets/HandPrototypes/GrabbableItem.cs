using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    public BoxCollider Box;
    public GameObject ThumbnailContent;

    private float timeSinceGrab;

    private Vector3 finalContentScale;

    void Start()
    {
        finalContentScale = transform.localScale;
        GrabbableItemsManager.Instance.RegisterGrabbableItem(this);
    }

    void Update()
    {
        if(GrabbableItemsManager.Instance.GrabbedItem != this)
        {
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        timeSinceGrab += Time.deltaTime;
        timeSinceGrab = Mathf.Clamp(timeSinceGrab, 0, GrabbableItemsManager.Instance.GrabRestoreTime);
        float grabLerp = timeSinceGrab / GrabbableItemsManager.Instance.GrabRestoreTime;
        transform.localScale = Vector3.Lerp(transform.localScale, finalContentScale, grabLerp);
    }

    internal void EndGrab()
    {
        ThumbnailContent.SetActive(true);
        transform.parent = null;
        timeSinceGrab = 0;
    }

    internal void StarGrab()
    {
        transform.parent = ThumbnailContent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        ThumbnailContent.SetActive(false);
        transform.parent = GrabbableItemsManager.Instance.SmoothedGrabPoint;
    }

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }
}
