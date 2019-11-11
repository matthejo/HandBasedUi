using UnityEngine;

public class StandardHandPrototype : HandPrototype
{
    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }
    public SummonDetector Summoning;
    public UiPositionCore PositionCore;

    public Transform HandContent;
    public Transform HandRotationPivot;

    public override bool IsSummoned
    {
        get
        {
            return Summoning.IsSummoned;
        }
    }

    protected void UpdatePrimaryVisibility()
    {
        HandContent.gameObject.SetActive(Summoning.IsSummoned);
    }

    protected void UpdatePosition()
    {
        Vector3 positionTarget = Vector3.Lerp(HandContent.position, PositionCore.CoreTransform.position, Time.deltaTime * Smoothing);
        HandContent.position = positionTarget;
        HandContent.LookAt(Camera.main.transform.position, Vector3.up);
        HandRotationPivot.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
