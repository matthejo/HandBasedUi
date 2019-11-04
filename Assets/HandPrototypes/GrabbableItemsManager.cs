using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItemsManager : MonoBehaviour
{
    public float GrabRestoreTime;
    public float GrabMargin = .1f;
    public static GrabbableItemsManager Instance;

    private List<GrabbableItem> items;

    private bool wasGrabbing;
    public GrabbableItem GrabbedItem { get; private set; }

    public Transform SmoothedGrabPoint { get; private set; }

    private void Awake()
    {
        Instance = this;
        items = new List<GrabbableItem>();
    }

    private void Start()
    {
        SmoothedGrabPoint = new GameObject("Smoothed Grab Point").transform;
    }

    public void RegisterGrabbableItem(GrabbableItem item)
    {
        items.Add(item);
    }

    public void Update()
    {
        UpdateSmoothedGrabPoint();
        if(GrabDetector.Instance.Grabbing && !wasGrabbing)
        {
            if(GrabbedItem == null)
            {
                HandleStartGrab();
            }
        }
        if(!GrabDetector.Instance.Grabbing && GrabbedItem != null)
        {
            HandleStopGrabbing();
        }
        wasGrabbing = GrabDetector.Instance.Grabbing;
    }

    private void UpdateSmoothedGrabPoint()
    {
        Vector3 positionTarget = GrabDetector.Instance.GrabPoint.position;
        Quaternion rotationTarget = GrabDetector.Instance.GrabPoint.rotation;
        SmoothedGrabPoint.position = Vector3.Lerp(positionTarget, SmoothedGrabPoint.position, HandPrototypeA.Instance.Smoothing * Time.deltaTime);
        SmoothedGrabPoint.rotation = Quaternion.Lerp(rotationTarget, SmoothedGrabPoint.rotation, HandPrototypeA.Instance.Smoothing * Time.deltaTime);
    }

    private void HandleStopGrabbing()
    {
        GrabbedItem.EndGrab();
        GrabbedItem = null;
    }

    private void HandleStartGrab()
    {
        GrabbedItem = GetGrabbable();
        if(GrabbedItem != null)
        {
            GrabbedItem.StarGrab();
        }
    }

    private GrabbableItem GetGrabbable()
    {
        float closestGrabDist = GrabMargin;
        GrabbableItem ret = null;
        foreach (GrabbableItem item in items)
        {
            if(item.ThumbnailContent.activeInHierarchy)
            {
                float grabDist = item.GetDistanceToGrab();
                if(grabDist < closestGrabDist)
                {
                    closestGrabDist = grabDist;
                    ret = item;
                }
            }
        }
        return ret;
    }
}
