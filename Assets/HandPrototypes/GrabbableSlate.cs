using UnityEngine;

public class GrabbableSlate : Grabbable
{
    private Transform originalParent;

    private void Start()
    {
        originalParent = transform.parent;
    }

    public override void StartGrab()
    {
        transform.parent = Manager.SmoothedGrabPoint;
    }

    public override void EndGrab()
    {
        transform.parent = originalParent;
    }
}
