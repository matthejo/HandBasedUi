using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbablePanel : MonoBehaviour
{
    private Transform originalParent;
    public GrabbableItemsManager Manager;
    public BoxCollider Box;
    public GameObject ThumbnailContent;

    private float timeSinceGrab;

    private Vector3 finalContentScale;

    void Start()
    {
        originalParent = transform.parent;
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
        timeSinceGrab = Mathf.Clamp(timeSinceGrab, 0, Manager.GrabRestoreTime);
        float grabLerp = timeSinceGrab / Manager.GrabRestoreTime;
        transform.localScale = Vector3.Lerp(transform.localScale, finalContentScale, grabLerp);
    }

    internal void EndGrab()
    {
        ThumbnailContent.SetActive(true);
        transform.parent = originalParent;
        timeSinceGrab = 0;
    }

    internal void StarGrab()
    {
        transform.parent = ThumbnailContent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        ThumbnailContent.SetActive(false);
        transform.parent = Manager.SmoothedGrabPoint;
    }

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }
}
