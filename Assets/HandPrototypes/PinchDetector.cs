using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetector : MonoBehaviour
{
    public float PinchDist = 0.03f;
    public float UnpinchDist = 0.09f;
    private HandPrototypeProxies proxies;
    public static PinchDetector Instance;

    public bool Pinching { get; private set; }

    public Transform PinchPoint { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        proxies = HandPrototypeProxies.Instance;
        PinchPoint = new GameObject("Pinch Point").transform;
    }

    private void Update()
    {
        float tipDistance = (proxies.RightIndex.transform.position - proxies.RightThumb.transform.position).magnitude;
        if(Pinching)
        {
            Pinching = tipDistance < UnpinchDist;
        }
        else
        {
            Pinching = tipDistance < PinchDist;
        }

        UpdateGrabPoint();
    }

    private void UpdateGrabPoint()
    {
        Vector3 grabPos = (proxies.RightThumb.transform.position + proxies.RightIndex.transform.position) / 2;

        PinchPoint.position = grabPos;
        PinchPoint.rotation = proxies.RightPalm.rotation;
    }
}
