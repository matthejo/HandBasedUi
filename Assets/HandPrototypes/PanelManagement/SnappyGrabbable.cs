using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappyGrabbable : MonoBehaviour
{
    private Timewarping timewarper;

    private Transform originalParent;

    public PanelManagementPrototype Manager;

    public BoxCollider Box { get; private set; }

    private void Start()
    {
        originalParent = transform.parent;
        Box = GetComponent<BoxCollider>();
        timewarper = new Timewarping(Manager.TimewarpFrames);
    }

    public void Update()
    {
        if(Manager.GrabbedItem == this)
        {
            transform.position = Manager.PreviewBox.transform.position;
            transform.rotation = GetSnappedRotation(Manager.PreviewBox.transform.rotation);

            timewarper.RegisterTransform(transform.position, transform.rotation);
        }
    }

    private Quaternion GetSnappedRotation(Quaternion baseRotation)
    {
        Vector3 eulers = baseRotation.eulerAngles;
        float snappedX = GetSnappedVal(eulers.x, Manager.XSnap);
        float snappedY = GetSnappedVal(eulers.y, Manager.YSnap);
        float snappedZ = GetSnappedVal(eulers.z, Manager.ZSnap);
        return Quaternion.Euler(snappedX, snappedY, snappedZ);
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
        timewarper.Reset(transform.position, transform.rotation);
    }

    public void EndGrab()
    {
        transform.position = timewarper.GetTimewarpPosition();
        transform.rotation = timewarper.GetTimewarpRotation();
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
