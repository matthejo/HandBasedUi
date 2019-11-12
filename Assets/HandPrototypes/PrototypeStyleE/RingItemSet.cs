using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingItemSet : MonoBehaviour
{
    public HandPrototypeE Mothership;

    public RingItem[] Items;
    public float AngleSpread;

    private void Update()
    {
        float weightSum = Items.Sum(item => item.Weight);
        float currentWeight = -weightSum / 2;
        for (int i = 0; i < Items.Length; i++)
        {
            float halfWeight = currentWeight + Items[i].Weight / 2;
            DoItemUpdate(Items[i], halfWeight / weightSum);
            currentWeight += Items[i].Weight;
        }
    }

    private void DoItemUpdate(RingItem ringItem, float param)
    {
        float effectiveAngle = param * AngleSpread;
        
        Vector3 rootPos = GetRootPos();

        ringItem.transform.localPosition = Vector3.zero;
        ringItem.transform.localRotation = Quaternion.identity;
        ringItem.transform.RotateAround(rootPos, transform.up, effectiveAngle);
        ringItem.transform.Rotate(0, 180, 0);
    }

    private Vector3 GetRootPos()
    {
        Vector3 basePos = transform.position;
        Vector3 userPos = Camera.main.transform.position;
        
        Vector3 offetDir = (basePos - userPos).normalized;
        return basePos + offetDir * Mothership.RingRadius;
    }
}
