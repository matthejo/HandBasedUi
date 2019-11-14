using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappyGrabbable : MonoBehaviour
{
    private Timewarping timewarper;

    private Transform originalParent;
    private Transform snappingHelper;

    public GrabbableItemsManager Manager;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public BoxCollider Box { get; private set; }

    private void Start()
    {
        snappingHelper = new GameObject("Snapping Helper").transform;
        originalParent = transform.parent;
        Box = GetComponent<BoxCollider>();
        timewarper = new Timewarping(Manager.TimewarpFrames);
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    public void Update()
    {
        if(Manager.GrabbedItem == this)
        {
            targetPosition = Manager.PreviewBox.transform.position;
            targetRotation = GetSnappedRotation();

            timewarper.RegisterTransform(transform.position, transform.rotation);
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Manager.PanelSmoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Manager.PanelSmoothing);
    }

    private Quaternion GetSnappedRotation()
    {
        snappingHelper.position = targetPosition;
        snappingHelper.LookAt(Camera.main.transform);


        float dot = Vector3.Dot(Manager.PreviewBox.transform.forward, snappingHelper.forward);
        if(dot > Manager.SnapThreshold)
        {
            return snappingHelper.rotation;
        }
        return Manager.PreviewBox.transform.rotation;
    }

    private float GetSnappedVal(float axis, float snap)
    {
        float val = axis / snap;
        val = Mathf.Round(val);
        return val * snap;
    }

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }

    public void StartGrab()
    {
        timewarper.Reset(targetPosition, targetRotation);
    }

    public void EndGrab()
    {
        targetPosition = timewarper.GetTimewarpPosition();
        targetRotation = timewarper.GetTimewarpRotation();
    }

    private class Timewarping
    {
        private int currentIndex;
        private readonly int timewarpMomements;

        private readonly Vector3[] timewarpPositions;
        private readonly Quaternion[] timewarpRotations;
        private readonly float timewarpDuration;

        public Timewarping(int timewarpFrames)
        {
            this.timewarpMomements = timewarpFrames;
            this.timewarpPositions = new Vector3[timewarpFrames];
            this.timewarpRotations = new Quaternion[timewarpFrames];
        }

        public void Reset(Vector3 position, Quaternion rotation)
        {
            for (int i = 0; i < timewarpMomements; i++)
            {
                timewarpPositions[i] = position;
                timewarpRotations[i] = rotation;
            }
        }

        public void RegisterTransform(Vector3 position, Quaternion rotation)
        {
            timewarpPositions[currentIndex] = position;
            timewarpRotations[currentIndex] = rotation;
            currentIndex = (currentIndex + 1) % timewarpMomements;
        }

        public Vector3 GetTimewarpPosition()
        {
            int oldestMoment = (currentIndex + 1) % timewarpMomements;
            return timewarpPositions[oldestMoment];
        }

        public Quaternion GetTimewarpRotation()
        {
            int oldestMoment = (currentIndex + 1) % timewarpMomements;
            return timewarpRotations[oldestMoment];
        }
    }
}
