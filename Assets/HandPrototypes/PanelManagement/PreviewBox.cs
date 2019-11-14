using UnityEngine;

public class PreviewBox : MonoBehaviour
{
    private Transform originalParent;

    public PanelManagementPrototype Manager;

    public BoxCollider Box { get; private set; }

    private void Start()
    {
        originalParent = transform.parent;
        Box = GetComponent<BoxCollider>();
    }

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }

    public void StartGrab()
    {
        transform.parent = Manager.SmoothedGrabPoint;

        Transform grabbedTransform = Manager.GrabbedItem.transform;
        transform.localScale = new Vector3(grabbedTransform.lossyScale.x + Manager.PreviewBoxPadding,
            grabbedTransform.lossyScale.y + Manager.PreviewBoxPadding,
            grabbedTransform.lossyScale.z + Manager.PreviewBoxPadding);
        transform.position = grabbedTransform.position;
        transform.rotation = grabbedTransform.rotation;
    }

    public void EndGrab()
    {
        transform.parent = originalParent;
    }
}