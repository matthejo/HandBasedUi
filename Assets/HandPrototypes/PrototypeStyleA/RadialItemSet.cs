using System;
using System.Linq;
using UnityEngine;

public class RadialItemSet : MonoBehaviour
{
    public HandPrototypeA Prototype;

    private float currentSubSummonTime;
    private float subSummonedness;

    public bool ShowItems;
    
    public RadialItem[] Items;
    
    public float Radius;
    public float AngleSpread;
    public float AngleOffset;
    public float OriginOffset;
    

    private void Update()
    {
        UpdateSubSummonedness();
        UpdateTransform();

        float angleSum = Items.Sum(item => item.Angle);
        float currentAngle = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            DoItemUpdate(Items[i], currentAngle / angleSum);
            currentAngle += Items[i].Angle;
        }
        UpdateCanvasAlpha();
    }

    private void UpdateTransform()
    {
        Vector3 posTarget = (HandPrototypeProxies.Instance.LeftIndex.position
            + HandPrototypeProxies.Instance.LeftRing.position
            + HandPrototypeProxies.Instance.LeftMiddle.position
            + HandPrototypeProxies.Instance.LeftPinky.position) / 4;

        transform.position = Vector3.Lerp(transform.position, posTarget, Time.deltaTime * Prototype.Smoothing);

        Vector3 toCamera = transform.position - Camera.main.transform.position;
        Vector3 right = Vector3.Cross(toCamera, -Vector3.up);
        Vector3 ourUp = Vector3.Cross(toCamera, right);
        transform.LookAt(Camera.main.transform.position, ourUp);
        transform.Rotate(0, 180, 0);
    }

    private void UpdateSubSummonedness()
    {
        if (ShowItems)
        {
            currentSubSummonTime += Time.deltaTime;
        }
        else
        {
            currentSubSummonTime -= Time.deltaTime;
        }
        currentSubSummonTime = Mathf.Clamp(currentSubSummonTime, 0, Prototype.SummonTime);
        subSummonedness = currentSubSummonTime / Prototype.SummonTime;
    }

    private void DoItemUpdate(RadialItem item, float angle)
    {
        item.gameObject.SetActive(subSummonedness > Mathf.Epsilon);
        SetItemPosition(item, angle);
    }
    private void UpdateCanvasAlpha()
    {
        float alpha = Mathf.Pow(subSummonedness, 2);
        foreach (RadialItem item in Items)
        {
            item.UpdateAlpha(alpha);
        }
    }

    private void SetItemPosition(RadialItem item, float angle)
    {
        float effectiveAngle = angle * AngleSpread + AngleOffset;

        Vector3 basePosition = transform.right * Radius + transform.position;
        item.transform.position = basePosition;

        Vector3 rootPos = GetRootPos();

        item.transform.rotation = transform.rotation;
        item.transform.RotateAround(rootPos, transform.forward, effectiveAngle);

    }

    private Vector3 GetRootPos()
    {
        Vector3 basePos = transform.position;
        Vector3 offset = transform.right * OriginOffset;
        return basePos + offset;
    }
}