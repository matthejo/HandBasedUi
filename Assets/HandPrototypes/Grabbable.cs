using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private Vector3 fullContentScale;

    public GrabbableItemsManager Manager;
    private float timeSinceGrab;

    public BoxCollider Box { get; private set; }

    public GameObject ThumbnailContent;
    public BoxCollider ThumbnailBox;

    private bool thumbnailWasGrabbed;
    private Timewarping timewarper;

    private Transform originalParent;
    private Transform snappingHelper;


    private Vector3 targetPosition;
    private Quaternion targetRotation;


    private void Start()
    {
        Box = GetComponent<BoxCollider>();
        fullContentScale = transform.localScale;
        snappingHelper = new GameObject("Snapping Helper").transform;
        originalParent = transform.parent;
        Box = GetComponent<BoxCollider>();
        timewarper = new Timewarping(Manager.TimewarpFrames);
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }


    private void Update()
    {
        if (Manager.GrabbedItem == this)
        {
            targetPosition = Manager.PreviewBox.transform.position;
            targetRotation = GetSnappedRotation();

            timewarper.RegisterTransform(transform.position, transform.rotation);
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Manager.PanelSmoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Manager.PanelSmoothing);


        if (Manager.GrabbedItem != this)
        {
            ThumbnailContent.SetActive(true);
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        timeSinceGrab += Time.deltaTime;
        timeSinceGrab = Mathf.Clamp(timeSinceGrab, 0, Manager.GrabRestoreTime);
        float grabLerp = timeSinceGrab / Manager.GrabRestoreTime;
        transform.localScale = Vector3.Lerp(transform.localScale, fullContentScale, grabLerp);
    }

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }

    public float GetDistanceToThumbnailGrab()
    {
        Vector3 grabPoint = GrabDetector.Instance.GrabPoint.position;
        Vector3 closestPoint = ThumbnailBox.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }

    public void StartGrab()
    {
        timewarper.Reset(targetPosition, targetRotation);
    }

    public void StartThumbGrab()
    {
        ThumbnailContent.SetActive(false);
        thumbnailWasGrabbed = true;

        Transform originalParent = transform.parent;
        transform.parent = ThumbnailContent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        transform.parent = originalParent;

        targetPosition = transform.position;
        targetRotation = transform.rotation;

        timewarper.Reset(targetPosition, targetRotation);
    }

    public void EndGrab()
    {
        targetPosition = timewarper.GetTimewarpPosition();
        targetRotation = timewarper.GetTimewarpRotation();
        timeSinceGrab = 0;
    }

    private Quaternion GetSnappedRotation()
    {
        snappingHelper.position = targetPosition;
        snappingHelper.LookAt(Camera.main.transform);
        snappingHelper.Rotate(0, 180, 0, Space.Self);


        float dot = Vector3.Dot(Manager.PreviewBox.transform.forward, snappingHelper.forward);
        if (dot > Manager.SnapThreshold)
        {
            return snappingHelper.rotation;
        }
        return Manager.PreviewBox.transform.rotation;
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
