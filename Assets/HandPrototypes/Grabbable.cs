using UnityEngine;

public abstract class Grabbable : MonoBehaviour
{
    public GrabbableItemsManager Manager;

    public BoxCollider Box;

    public abstract void StartGrab();
    public abstract void EndGrab();

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }
}
