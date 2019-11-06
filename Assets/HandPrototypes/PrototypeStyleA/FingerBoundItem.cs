using System;
using UnityEngine;

public class FingerBoundItem : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public MeshRenderer MeshRenderer;

    public void DoItemUpdate( 
        Transform fingerTipPos, 
        Transform knucklePos)
    {
        bool primaryVisibility = HandPrototypeA.Instance.State != HandPrototypeA.UiState.Unsummoned;
        Transform palm = HandPrototypeA.Instance.EffectivePalm;
        gameObject.SetActive(HandPrototypeA.Instance.Summonedness > Mathf.Epsilon);
        PositionSelf(palm, fingerTipPos, knucklePos.position);
        UpdateCanvasAlpha();
    }

    private void UpdateCanvasAlpha()
    {
        float alpha = Mathf.Pow(HandPrototypeA.Instance.Summonedness, 2);
        if(CanvasGroup != null)
        {
            CanvasGroup.alpha = alpha;
        }
        if(MeshRenderer != null)
        {
            MeshRenderer.material.SetFloat("_Fade", alpha);
        }
    }

    private void PositionSelf(Transform palm,  Transform fingerTipPos, Vector3 knucklePos)
    {
        Vector3 positionTarget = GetPositionTarget(palm, fingerTipPos);
        //positionTarget = Vector3.Lerp(knucklePos, positionTarget, HandPrototypeA.Instance.Summonedness);
        transform.position = Vector3.Lerp(positionTarget, transform.position, HandPrototypeA.Instance.Smoothing * Time.deltaTime);
        transform.rotation = GetRotation(palm, transform.position, knucklePos);
    }

    private Quaternion GetRotation(Transform palm, Vector3 itemPos, Vector3 knucklePos)
    {
        Vector3 toFinger = knucklePos - itemPos;
        Vector3 up = Vector3.Cross(toFinger, palm.up);
        return Quaternion.LookRotation(palm.up, up);
    }

    private Vector3 GetPositionTarget(Transform palm, Transform fingerPos)
    {
        float radius = HandPrototypeA.Instance.Radius;
        Plane palmPlane = new Plane(palm.up, palm.position);
        Vector3 closestPlanePoint = palmPlane.ClosestPointOnPlane(fingerPos.position);
        Vector3 toPalm = closestPlanePoint - palm.position;
        return toPalm.normalized * radius + palm.position;
    }
}