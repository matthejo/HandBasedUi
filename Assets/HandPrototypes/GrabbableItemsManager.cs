using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItemsManager : MonoBehaviour
{
    public HandPrototype Prototype;
    public float GrabRestoreTime;
    public float GrabMargin = .1f;
    public float GrabSnapThreshold;

    public Grabbable[] Items;

    private bool wasGrabbing;
    public Grabbable GrabbedItem { get; private set; }

    public Transform SmoothedGrabPoint { get; private set; }

    private void Start()
    {
        SmoothedGrabPoint = new GameObject("Smoothed Grab Point").transform;
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
        //rotationTarget = GetSnappedGrabTarget(rotationTarget);
        SmoothedGrabPoint.position = Vector3.Lerp(positionTarget, SmoothedGrabPoint.position, Prototype.Smoothing * Time.deltaTime);
        SmoothedGrabPoint.rotation = Quaternion.Lerp(rotationTarget, SmoothedGrabPoint.rotation, Prototype.Smoothing * Time.deltaTime);
    }

    // TODO: Figure out the snapping stuff eventually
    //private Quaternion GetSnappedGrabTarget(Quaternion rotationTarget)
    //{
    //    Vector3 eulers = rotationTarget.eulerAngles;
    //    float distToAngle = Mathf.Abs((eulers.z % 45) - 45);
    //    if(distToAngle < GrabSnapThreshold)
    //    {
    //        Vector3 newEuler = new Vector3(eulers.x, eulers.y, eulers.z - (eulers.z % 45));
    //        return Quaternion.Euler(newEuler);
    //    }
    //    return rotationTarget;
    //}

    private void HandleStopGrabbing()
    {
        GrabbedItem.EndGrab();
        GrabbedItem = null;
    }

    private void HandleStartGrab()
    {
        GrabbedItem = GetGrabbableThumbnail();
        if(GrabbedItem != null)
        {
            GrabbedItem.StartGrab();
        }
    }

    private Grabbable GetGrabbableThumbnail()
    {
        float closestGrabDist = GrabMargin;
        Grabbable ret = null;
        foreach (Grabbable item in Items)
        {
            if(item.gameObject.activeInHierarchy)
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
